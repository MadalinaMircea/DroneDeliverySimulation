﻿using DroneDeliverySystem.DisplayUtils;
using DroneDeliverySystem.Global;
using DroneDeliverySystem.Messaging;
using DroneDeliverySystem.MoveUtils;
using DroneDeliverySystem.Utils;
using DroneDeliverySystem.Utils.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDeliverySystem.Agents
{
    public class Drone : Agent
    {
        public Position Position { get; set; }
        public bool IsAvailable { get; set; }

        private Thread movingThread;

        private Thread messageHandlerThread;

        public Position newPosition;

        public bool isMoving = false;

        private Movement movement;

        public Package package = null;

        //private List<PackageRequest> requests = new List<PackageRequest>();
        private PriorityQueue<PackageRequest> requests = new PriorityQueue<PackageRequest>();

        private List<MovingObject> observers = new List<MovingObject>();

        //Method gets called when the agent receives a message
        public override void OnMessageReceived(AgentMessage message)
        {
            //writes the information to the screen
            GlobalInformation.WriteToConsole($"{Name} got message {message.Content}");
            //The message is of the form "<type>-<message>"
            string[] msg = message.Content.Split('-');
            string type = msg[0].Trim();

            switch (type)
            {
                case "1":
                    //A message of the form "1-<<newP>-<posX, posY>-<prodId>-<packId>" is
                    //new information about a package
                    string newPackage = msg[1].Trim();
                    int receivedProducerId = Convert.ToInt32(msg[3].Trim());
                    int receivedPackageId = Convert.ToInt32(msg[4].Trim());
                    Position pos = new Position(msg[2]);

                    switch (newPackage)
                    {
                        case "0":
                            //if newPackage is 0, then the package represented by the received
                            //IDs has been picked up by another drone
                            RemoveRequest(receivedProducerId, receivedPackageId);
                            if (package == null)
                            {
                                if (requests.Count() == 0)
                                {
                                    //if the drone has no more requests, it stops moving
                                    StopMoving();
                                }
                                else
                                {
                                    //if it has requests, it moves towards the first request in the list
                                    Move(requests.Peek().Element.position);
                                }
                            }
                            break;
                        case "1":
                            //if newPackage is 1, a new package was created
                            //so the drone adds the information to the requests list
                            AddRequest(new PackageRequest(receivedProducerId, receivedPackageId, pos));
                            //requests.Add();
                            if (IsAvailable)
                            {
                                //if the drone is not currently delivering a package
                                //it starts moving towards the appropriate request
                                Move(requests.Peek().Element.position);
                            }
                            break;
                    }
                    break;
                case "2":
                    //A message of the form "2-<message>" means the drones are communicating with each other
                    break;
            }
        }

        private int GetPriority(PackageRequest req)
        {
            //computes the priority of a package given its position
            int deltaX = Position.X - req.position.X;
            int deltaY = Position.Y - req.position.Y;

            //computes the distance between the positions
            double dist = Math.Sqrt(deltaX * deltaX + deltaY + deltaY);

            //if the package is close, the priority is high
            if (dist <= 50)
            {
                return 3;
            }

            //if the package is a little further, the priority is medium
            if (dist <= 100)
            {
                return 2;
            }

            //otherwise, the priority is low
            //this will, however, grow the longer the package waits to be delived
            return 1;
        }

        private void AddRequest(PackageRequest req)
        {
            //Adds a new request to the list
            Monitor.Enter(requests);
            requests.Add(new PriorityPair<PackageRequest>(req, GetPriority(req)));
            Monitor.Exit(requests);
        }

        public void AddObserver(MovingObject obs)
        {
            //Adds an observer in the observer list (used for Icons and Titles)
            observers.Add(obs);
        }

        private void RemoveRequest(int producerId, int packageId)
        {
            //Removes a request from the list
            Monitor.Enter(requests);
            //for(int i = 0; i < requests.Count(); i++)
            //{
            //    if(requests[i].ProducerID == producerId && requests[i].PackageID == packageId)
            //    {
            //        requests.RemoveAt(i);
            //        break;
            //    }
            //}
            requests.Remove(
                new PriorityPair<PackageRequest>(new PackageRequest(producerId, packageId, new Position(0, 0)), 0));
            Monitor.Exit(requests);
        }

        private void StopMoving()
        {
            //Notifies the mover thread to start moving
            isMoving = false;

            //Notifies the observers that the drone status has changed
            Changed(true);

            GlobalInformation.WriteToConsole($"{Name} stopped moving");
        }

        public void Changed(bool changeColor = false, bool toFront = false)
        {
            //Notifies each observer that the drone status has changed
            foreach (MovingObject obs in observers)
            {
                obs.Notify(this, changeColor, toFront);
            }
        }

        public void MoveDrone()
        {
            //mover thread that is continuously checking if it should move
            while (true)
            {
                if (isMoving)
                {
                    //Checks if the new position is within the space reachable by the drone
                    if(CurrentEnvironment.IsPositionWithinLimits(newPosition))
                    //if (newPosition.X >= GlobalInformation.GetMinX() && newPosition.X <= GlobalInformation.GetMaxX() &&
                    //    newPosition.Y >= GlobalInformation.GetMinY() && newPosition.Y <= GlobalInformation.GetMaxY())
                    {
                        GlobalInformation.WriteToConsole($"{Name} moving to {newPosition}");

                        //Notifies the observers
                        Changed(true);

                        //Moves according to the moving strategy
                        movement.Move(this);

                        //If the flag "isMoving" is still true at this point
                        //then the desired position has been reached
                        if (isMoving)
                        {
                            GlobalInformation.WriteToConsole($"{Name} reached {newPosition}");

                            if (package != null)
                            {
                                //if the drone is carrying a package
                                //release the package
                                ReleasePackage();

                                if (requests.Count() != 0)
                                {
                                    //if there are more requests to be fulfilled
                                    //move towards the next one
                                    Move(requests.Peek().Element.position);
                                }
                                else
                                {
                                    //otherwise, stop moving
                                    StopMoving();
                                }
                            }
                            else
                            {
                                //if it is not carrying a parcel, but it has reached a position

                                if (requests.Count() != 0)
                                {
                                    //if the requests list is not epty
                                    //then this drone is the first one to pick up the package
                                    //at the current position
                                    int producerId = requests.Peek().Element.ProducerID;
                                    int packageId = requests.Peek().Element.PackageID;
                                    //picks up the package
                                    PickUpPackage();
                                    SendBroadcast(producerId, packageId);


                                    ////Sends a message to all the other drones that this drone
                                    ////has reached the package
                                    //Thread t = new Thread(() => SendBroadcast(producerId, packageId));
                                    //t.Start();

                                    //t.Join();
                                }
                                else
                                {
                                    //otherwise, stop moving
                                    StopMoving();
                                }
                            }
                        }
                    }
                    else
                    {
                        GlobalInformation.WriteToConsole($"{newPosition} outside the drone scope!");
                    }
                }
            }
        }

        public void SetMovement(Movement m)
        {
            movement = m;
        }

        private void SendBroadcast(int producerId, int packageId)
        {
            //Sends a message to all of the drones that this drone
            //reached a specific parcel
            Broadcast(ACLPerformative.INFORM, $"1-0-0,0-{producerId}-{packageId}");
        }
        public void Move(Position newPosition)
        {
            //notifies the mover thread about the new desired position
            this.newPosition = newPosition;
            isMoving = true;
        }

        private void PickUpPackage()
        {
            if (package == null)
            {
                //if the drone is not currently carrying a package
                if (requests.Count() != 0)
                {
                    //if the drone has requests
                    Monitor.Enter(requests);
                    //gets the appropriate package from the global information
                    package = GlobalInformation.GetPackage(requests.Peek().Element.ProducerID,
                        requests.Peek().Element.PackageID);
                    //requests.RemoveAt(0);

                    //removes the request from the list
                    requests.PopFront();
                    Monitor.Exit(requests);

                    if (package != null)
                    {
                        //if the package is not null, pick up the package
                        GlobalInformation.WriteToConsole($"{Name} picked up package");
                        //notify the mover thread about the new desired position
                        newPosition = package.ReceiverPosition;
                        isMoving = true;
                        IsAvailable = false;
                        //notify the observers about the change
                        Changed(false, true);
                    }
                    else
                    {
                        //if the package is null, stop moving
                        GlobalInformation.WriteToConsole($"Package is null");
                        StopMoving();
                    }
                }
                else
                {
                    //if the drone has no requests, stop moving
                    GlobalInformation.WriteToConsole($"IDs are null");
                    StopMoving();
                }
            }
        }

        private void MessageHandler()
        {
            while(true)
            {
                AgentMessage msg = CurrentEnvironment.GetNextMessage(ID);
                if (msg != null)
                {
                    OnMessageReceived(msg);
                }
            }
        }

        private void ReleasePackage()
        {
            //releases the package
            package.Release();
            package = null;
            IsAvailable = true;
            GlobalInformation.WriteToConsole($"{Name} released package");
        }
        public override void Stop()
        {
            //stops the agent
            movingThread.Join();
            messageHandlerThread.Join();
            base.Stop();
        }

        public override void Start()
        {
            //starts the agent's mover thread
            movingThread.Start();
            messageHandlerThread.Start();
            base.Start();
        }

        public override string ToString()
        {
            return $"Drone {ID}";
        }
        public Drone(int id, string name, Position position, Movement movement)
        {
            ID = id;
            Name = name;

            Position = position;
            IsAvailable = true;
            newPosition = position;
            this.movement = movement;

            GlobalInformation.WriteToConsole($"Created {ToString()} at position {Position}");

            movingThread = new Thread(new ThreadStart(MoveDrone));

            messageHandlerThread = new Thread(new ThreadStart(MessageHandler));
        }
    }
}

using DroneDeliverySystem.Agents;
using DroneDeliverySystem.DisplayUtils;
using DroneDeliverySystem.MoveUtils;
using DroneDeliverySystem.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace DroneDeliverySystem.Global
{
    public static class GlobalInformation
    {
        private static object monitorObj = new object();

        private static int droneNumber = 7;
        private static int producerNumber = 3;

        //private static int globalId = 1;

        private static int minX = 15;
        private static int maxX = 550;
        private static int minY = 10;
        private static int maxY = 340;

        private static DisplayConsole displayConsole;
        private static ChangingLabel winnerLabel;
        //private static Random globalRandom = new Random();

        public static Dictionary<int, List<Package>> producerPackages = new Dictionary<int, List<Package>>();

        private static AgentEnvironment Environment;
        private static List<Producer> producers;

        //public static int GetMinX()
        //{
        //    return minX;
        //}

        public static int GetProducerNumber()
        {
            return producerNumber;
        }

        public static void SetConsole(DisplayConsole console)
        {
            displayConsole = console;
        }

        public static void CreateEnvironment()
        {
            Environment = new AgentEnvironment(minX, maxX, minY, maxY);
        }

        //public static int GetMinY()
        //{
        //    return minY;
        //}

        //public static int GetMaxX()
        //{
        //    return maxX;
        //}

        //public static int GetMaxY()
        //{
        //    return maxY;
        //}

        //public static int GetRandomInt(int min, int max)
        //{
        //    return globalRandom.Next(min, max);
        //}

        //public static int GetGlobalId()
        //{
        //    Monitor.Enter(monitorObj);
        //    int id = globalId;
        //    globalId++;
        //    Monitor.Exit(monitorObj);
        //    return id;
        //}
        //public static int GetRandomX()
        //{
        //    return globalRandom.Next(minX, maxX);
        //}

        public static void WriteToConsole(string text)
        {
            Monitor.Enter(displayConsole);
            displayConsole.Add(text);
            Monitor.Exit(displayConsole);
        }

        //public static int GetDroneNumber()
        //{
        //    return droneNumber;
        //}

        //public static int GetRandomY()
        //{
        //    return globalRandom.Next(minY, maxY);
        //}

        //public static Position GetRandomPosition()
        //{
        //    return new Position(GetRandomX(), GetRandomY());
        //}

        public static void AddPackage(Producer producer, Package package)
        {
            Monitor.Enter(producerPackages);
            AddProducer(producer);
            producerPackages[producer.GetID()].Add(package);
            Monitor.Exit(producerPackages);
        }

        private static void AddProducer(Producer producer)
        {
            if (!producerPackages.ContainsKey(producer.GetID()))
            {
                producerPackages[producer.GetID()] = new List<Package>();
            }
        }

        //private static Producer GetProducerById(int producerId)
        //{
        //    foreach (int prod in producerPackages.Keys)
        //    {
        //        if (prod.ID == producerId)
        //        {
        //            return prod;
        //        }
        //    }
        //    return null;
        //}

        public static Package GetPackage(int producerId, int packageId)
        {
            Monitor.Enter(producerPackages);
            //Producer prod = GetProducerById(producerId);
            Package pack = null;

            for (int i = 0; i < producerPackages[producerId].Count && pack == null; i++)
            {
                if (producerPackages[producerId][i].ID == packageId)
                {
                    pack = producerPackages[producerId][i];
                    producerPackages[producerId].RemoveAt(i);
                    break;
                }
            }

            Monitor.Exit(producerPackages);
            return pack;
        }

        public static List<Producer> CreateProducers(Form1 form)
        {
            producers = new List<Producer>();

            for (int i = 0; i < GetProducerNumber(); i++)
            {
                //Producer p = new Producer(GetGlobalId(), GetRandomPosition(), form);
                //Environment.Add(p, $"P{p.ID}P");
                //producers.Add(p);

                Producer p = (Producer)Environment.Add(AgentType.PRODUCER);
                p.SetForm(form);

                producers.Add(p);
            }

            return producers;
        }

        public static List<Drone> CreateDrones()
        {
            List<Drone> drones = new List<Drone>();

            for (int i = 0; i < droneNumber; i++)
            {
                int move = Environment.GetRandomInt(0, 100);
                Movement movement = new MixedMovement();
                if (move <= 25)
                {
                    movement = new ManhattanMovement();
                }
                Drone d = (Drone)Environment.Add(AgentType.DRONE);
                d.SetMovement(movement);

                drones.Add(d);
            }

            return drones;
        }

        //public static void CreateAndStartAgents(Form1 form)
        //{
        //    List<Drone> drones = CreateDrones();

        //    List<Producer> producers = CreateProducers(form);

        //    AddDroneObservables(drones, producers);
        //}

        //public static void CreateRandomPackage()
        //{
        //    int rnd = GetRandomInt(0, producerNumber);
        //    producers[rnd].CreatePackage();
        //}

        //public static void StartAgents(List<Drone> agents)
        //{
        //    foreach (Agent p in agents)
        //    {
        //        p.Start();
        //    }
        //}

        //public static void StartAgents(List<Producer> agents)
        //{
        //    foreach (Agent p in agents)
        //    {
        //        p.Start();
        //    }
        //}

        public static void AddDroneObservables(List<Drone> drones, List<Producer> producers)
        {
            int rnd;

            foreach (Producer p in producers)
            {
                foreach (Drone d in drones)
                {
                    rnd = Environment.GetRandomInt(0, 100);
                    if (rnd <= 80)
                    {
                        p.Add(d);
                    }
                }

                if (p.HasNoObservers())
                {
                    rnd = Environment.GetRandomInt(0, drones.Count);
                    p.Add(drones[rnd]);
                }
            }
        }

        public static void StartAll()
        {
            Environment.StartAll();
        }

        //public static void PauseAll()
        //{
        //    Environment.PauseAll();
        //}

        public static void StopAll()
        {
            Environment.StopAll();
        }

        public static void SetWinner(string name)
        {
            winnerLabel.SetName(name);
        }
        
        public static void SetWinnerLabel(Label label)
        {
            winnerLabel = new ChangingLabel(label);
        }
    }
}


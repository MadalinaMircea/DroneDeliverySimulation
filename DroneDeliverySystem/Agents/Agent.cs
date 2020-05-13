﻿using DroneDeliverySystem.Agents;
using DroneDeliverySystem.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem
{
    public abstract class Agent
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public AgentEnvironment CurrentEnvironment { get; set; }

        public AgentType Type { get; set; }

        public void Broadcast(ACLPerformative performative, string content)
        {
            foreach(Agent a in CurrentEnvironment.GetAgents())
            {
                Send(performative, ID, a.GetID(), content);
            }
        }

        public int GetID()
        {
            return ID;
        }

        public virtual void OnMessageReceived(AgentMessage message)
        {

        }

        public virtual void Stop()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Send(AgentMessage message)
        {
            CurrentEnvironment.AddMessage(message);
        }

        public virtual void Send(ACLPerformative performative, int from, int to, string content)
        {
            CurrentEnvironment.AddMessage(new AgentMessage(performative, content, from, to));
        }

        public virtual void Pause()
        {

        }

        public virtual void Resume()
        {

        }
    }
}

using DroneDeliverySystem.Agents;
using DroneDeliverySystem.Messaging;
using System.Collections.Generic;
using System.Diagnostics;

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

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Agent))
            {
                Debug.WriteLine("Not agent");
                return false;
            }

            Agent a = (Agent)obj;

            return a.ID == ID;
        }

        public override int GetHashCode()
        {
            int hashCode = -259032398;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<AgentEnvironment>.Default.GetHashCode(CurrentEnvironment);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }
    }
}

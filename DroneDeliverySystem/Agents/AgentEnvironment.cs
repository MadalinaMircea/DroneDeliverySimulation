using DroneDeliverySystem.Messaging;
using DroneDeliverySystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDeliverySystem.Agents
{
    public class AgentEnvironment
    {
        List<Agent> agents = new List<Agent>();
        int globalAgentId = 1;
        AgentFactory agentFactory = new AgentFactory();
        EnvironmentLimits limits;
        EnvironmentLimitsFactory environmentLimitsFactory = new EnvironmentLimitsFactory();
        Random rnd = new Random();
        MessageBlackboard blackboard = new MessageBlackboard();

        public AgentEnvironment(int minX = -1, int maxX = -1, int minY = -1, int maxY = -1, int minZ = -1, int maxZ = -1)
        {
            limits = environmentLimitsFactory.CreateEnvironmentLimits(minX, maxX, minY, maxY, minZ, maxZ);
        }

        public void AddMessage(AgentMessage msg)
        {
            Monitor.Enter(blackboard);
            blackboard.AddMessage(msg);
            
            Monitor.Exit(blackboard);
        }

        public AgentMessage GetNextMessage(int agentId)
        {
            Monitor.Enter(blackboard);
            AgentMessage msg = null;
            for (int i = 0; i < blackboard.Unread.Count; i++)
            {
                if(blackboard.Unread[i].Receiver == agentId)
                {
                    msg = blackboard.Unread[i];
                    blackboard.ReadMessage(i);
                    break;
                }
            }
            Monitor.Exit(blackboard);
            return msg;
        }

        public bool IsPositionWithinLimits(Position position)
        {
            return limits.Contains(position);
        }

        public Position GetRandomPosition()
        {
            return limits.GeneratePosition(rnd);
        }

        public int GetNextId()
        {
            int id = globalAgentId;
            globalAgentId++;
            return id;
        }

        public int GetRandomInt(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public Agent Add(AgentType agentType, string name = "")
        {
            string n = name != "" ? name : globalAgentId.ToString();

            Agent a = agentFactory.CreateAgent(agentType, GetNextId(), n, limits.GeneratePosition(rnd), rnd);
            a.CurrentEnvironment = this;
            agents.Add(a);
            return a;
        }

        public void StartAll()
        {
            foreach(Agent a in agents)
            {
                a.Start();
            }
        }

        public void PauseAll()
        {
            foreach(Agent a in agents)
            {
                a.Pause();
            }
        }

        public void StopAll()
        {
            foreach(Agent a in agents)
            {
                a.Stop();
            }
        }

        public List<Agent> GetAgents()
        {
            return agents;
        }
    }
}

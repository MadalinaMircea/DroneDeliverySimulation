using DroneDeliverySystem.Utils.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.Behaviour
{
    class Competition
    {
        public List<PriorityPair<Agent>> AgentPoints { get; set; }
        public int NrOfWinners { get; set; }

        public Competition()
        {
            AgentPoints = new List<PriorityPair<Agent>>();
            NrOfWinners = 2;
        }

        public void AddAgent(Agent a)
        {
            if (!Contains(a))
            {
                AgentPoints.Add(new PriorityPair<Agent>(a, 0));
            }
        }

        private bool Contains(Agent a)
        {
            for(int i = 0; i < AgentPoints.Count; i++)
            {
                if(AgentPoints[i].Element.Equals(a))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddPoints(Agent a, int p)
        {
            AddAgent(a);

            for (int i = 0; i < AgentPoints.Count; i++)
            {
                if (AgentPoints[i].Element.Equals(a))
                {
                    AgentPoints[i].Priority += p;
                    break;
                }
            }
        }

        public string GetWinners()
        {
            StringBuilder result = new StringBuilder();

            PriorityQueue<Agent> winners = new PriorityQueue<Agent>();

            for(int i = 0; i < AgentPoints.Count; i++)
            {
                winners.Add(new PriorityPair<Agent>(AgentPoints[i].Element, AgentPoints[i].Priority));
            }

            int k = 0;
            while(k < NrOfWinners && winners.Count() > 0)
            {
                PriorityPair<Agent> pair = winners.PopFront();
                result.Append(pair.Element.ID);
                result.Append(" - ");
                result.Append(pair.Priority);
                result.Append('\n');

                k++;
            }

            return result.ToString();
        }

        //public Agent GetWinner()
        //{
        //    return AgentPoints.Peek().Element;
        //}
    }
}

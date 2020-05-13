using DroneDeliverySystem.MoveUtils;
using DroneDeliverySystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.Agents
{
    public class AgentFactory
    {
        Random rnd = new Random();

        public Agent CreateAgent(AgentType agentType, int id, string name, Position position)
        {
            switch(agentType)
            {
                case AgentType.DRONE:
                    return CreateDrone(id, name, position);
                case AgentType.PRODUCER:
                    return CreateProducer(id, name, position);
            }

            return null;
        }

        private Agent CreateDrone(int id, string name, Position position)
        {
            int move = rnd.Next(0, 100);
            Movement movement = new MixedMovement();
            if (move <= 25)
            {
                movement = new ManhattanMovement();
            }
            return new Drone(id, name, position, movement);
        }

        private Agent CreateProducer(int id, string name, Position position)
        {
            Producer p = new Producer(id, name, position);
            return p;
        }
    }
}

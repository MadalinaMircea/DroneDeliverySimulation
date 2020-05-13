using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.Messaging
{
    public class AgentMessage
    {
        public ACLPerformative Performative { get; set; }
        public string Content { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }

        public AgentMessage(ACLPerformative performative, string content, int sender, int receiver)
        {
            Performative = performative;
            Content = content;
            Sender = sender;
            Receiver = receiver;
        }
    }
}

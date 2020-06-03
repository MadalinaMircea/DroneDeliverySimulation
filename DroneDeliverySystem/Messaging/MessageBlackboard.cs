using System.Collections.Generic;
using System.Threading;

namespace DroneDeliverySystem.Messaging
{
    class MessageBlackboard
    {
        public List<AgentMessage> Read { get; set; }
        public List<AgentMessage> Unread { get; set; }

        public MessageBlackboard()
        {
            Unread = new List<AgentMessage>();
            Read = new List<AgentMessage>();
        }

        public void ReadMessage(int unreadIndex)
        {
            Monitor.Enter(Unread);
            Monitor.Enter(Read);
            AgentMessage msg = Unread[unreadIndex];
            Unread.RemoveAt(unreadIndex);
            Read.Add(msg);
            Monitor.Exit(Read);
            Monitor.Exit(Unread);
        }

        public void AddMessage(AgentMessage message)
        {
            Monitor.Enter(Unread);
            Unread.Add(message);
            Monitor.Exit(Unread);
        }
    }
}

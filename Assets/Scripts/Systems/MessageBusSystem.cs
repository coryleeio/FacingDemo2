using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class MessageBusSystem : IMessageBusSystem
    {
        private List<Message> UnsentMessages = new List<Message>();
        public ITokenSystem TokenSystem { get; set; }

        public MessageBusSystem()
        {
        }

        public void Process()
        {

        }

        public void Deliver(Message m)
        {
            if(m.Delay == 0.0f || m.TimeSent + m.Delay > Time.time)
            {
                DeliverImmediately(m);
            }
            else
            {
                UnsentMessages.Add(m);
            }
        }

        private void DeliverImmediately(Message m)
        {
            var token = TokenSystem.GetTokenById(m.Receiver);
            token.HandleMessage(m);
        }
    }
}
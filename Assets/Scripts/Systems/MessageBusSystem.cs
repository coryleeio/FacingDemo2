using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class MessageBusSystem : IMessageBusSystem
    {
        private List<Message> UnsentMessages = new List<Message>();
        private ITokenSystem _tokenSystem;

        public MessageBusSystem(ITokenSystem tokenSystem)
        {
            _tokenSystem = tokenSystem;
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
            var token = _tokenSystem.GetTokenById(m.Receiver);
            token.HandleMessage(m);
        }
    }
}
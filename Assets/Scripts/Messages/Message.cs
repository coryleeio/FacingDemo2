using System.Collections.Generic;

namespace Gamepackage
{
    public class Message
    {
        public Message()
        {
        }

        public double Delay;

        public double TimeSent;

        public int Sender;

        public int Receiver;

        public int MessageType;

        public Dictionary<string, string> ExtraInfo = new Dictionary<string, string>();
    }
}
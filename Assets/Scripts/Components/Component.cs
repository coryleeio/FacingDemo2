using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Component : IHandleMessage
    {
        [JsonIgnore]
        public Token Owner;

        public void HandleMessage(Message messageToHandle)
        {

        }
    }
}

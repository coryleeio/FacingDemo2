using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Component<TPrototype> : IHandleMessage, IResolvableReferences where TPrototype : IResource
    {
        [JsonIgnore]
        public Token Owner;

        public PrototypeReference<TPrototype> PrototypeReference { get; set; }

        [JsonIgnore]
        public TPrototype Prototype
        {
            get
            {
                return PrototypeReference.Prototype;
            }
            set
            {
                PrototypeReference.Prototype = value;
            }
        }

        public void HandleMessage(Message messageToHandle)
        {

        }

        public virtual void Resolve(IResourceManager resourceManager)
        {
            PrototypeReference.Resolve(resourceManager);
        }
    }
}

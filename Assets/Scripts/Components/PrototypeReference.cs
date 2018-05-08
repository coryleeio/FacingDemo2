using Newtonsoft.Json;
using TinyIoC;

namespace Gamepackage
{
    public class PrototypeReference<TPrototype> : IResolvableReferences where TPrototype : IResource
    {
        public PrototypeReference() {}
        public string PrototypeUniqueIdentifier;

        [JsonIgnore]
        private TPrototype _prototype;

        [JsonIgnore]
        public TPrototype Prototype
        {
            get
            {
                return _prototype;
            }
            set
            {
                _prototype = value;
                PrototypeUniqueIdentifier = value.UniqueIdentifier;
            }
        }

        public virtual void Resolve(TinyIoCContainer container)
        {
            IResourceManager resourceManager = container.Resolve<IResourceManager>();
            _prototype = resourceManager.GetPrototypeByUniqueIdentifier<TPrototype>(PrototypeUniqueIdentifier);
        }
    }
}

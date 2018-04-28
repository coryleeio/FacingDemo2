using Newtonsoft.Json;

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

        public void Resolve(IResourceManager resourceManager)
        {
            _prototype = resourceManager.GetPrototypeByUniqueIdentifier<TPrototype>(PrototypeUniqueIdentifier);
        }
    }
}

using Newtonsoft.Json;
using UnityEngine;

namespace Gamepackage
{
    public class ViewComponent : Component
    {
        public ViewType ViewType;
        public UniqueIdentifier ViewPrototypeUniqueIdentifier;
        public bool IsVisible;

        [JsonIgnore]
        public GameObject View;

        public ViewComponent()
        {

        }

        public ViewComponent(ViewComponent other)
        {
            ViewType = other.ViewType;
            ViewPrototypeUniqueIdentifier = other.ViewPrototypeUniqueIdentifier;
            IsVisible = other.IsVisible;
        }

        public override void InjectContext(ApplicationContext context)
        {
            base.InjectContext(context);
        }
    }
}

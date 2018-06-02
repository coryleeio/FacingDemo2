using Newtonsoft.Json;
using UnityEngine;

namespace Gamepackage
{
    public class View : Component
    {
        public ViewType ViewType;
        public UniqueIdentifier ViewPrototypeUniqueIdentifier;
        public bool IsVisible;

        [JsonIgnore]
        public GameObject ViewGameObject;

        public View() {}

        public View(View other)
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

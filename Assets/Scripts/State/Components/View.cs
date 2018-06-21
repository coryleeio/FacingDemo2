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
    }
}

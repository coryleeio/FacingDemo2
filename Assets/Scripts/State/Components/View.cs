using Newtonsoft.Json;
using Spine.Unity;
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

        [JsonIgnore]
        public SkeletonAnimation SkeletonAnimation
        {
            get
            {
                if (ViewGameObject != null)
                {
                    return ViewGameObject.GetComponentInChildren<SkeletonAnimation>();
                }
                return null;
            }
        }

        public View() { }
    }
}

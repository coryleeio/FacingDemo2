using Newtonsoft.Json;
using Spine.Unity;
using UnityEngine;

namespace Gamepackage
{
    public class View
    {
        public ViewType ViewType;
        public UniqueIdentifier ViewPrototypeUniqueIdentifier;
        public bool IsVisible;

        [JsonIgnore]
        public GameObject ViewGameObject;

        [JsonIgnore]
        public HealthBar HealthBar;

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

        [JsonIgnore]
        public Sortable Sortable
        {
            get
            {
                if (ViewGameObject != null)
                {
                    return ViewGameObject.GetComponentInChildren<Sortable>();
                }
                return null;
            }
        }
    }
}

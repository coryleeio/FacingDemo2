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
        private SkeletonAnimation _skeletonAnimation;

        [JsonIgnore]
        public SkeletonAnimation SkeletonAnimation
        {
            get
            {
                if(_skeletonAnimation == null)
                {
                    if(ViewGameObject != null)
                    {
                        _skeletonAnimation = ViewGameObject.GetComponentInChildren<SkeletonAnimation>();
                    }
                }
                return _skeletonAnimation;
            }
        }

        public View() {}
    }
}

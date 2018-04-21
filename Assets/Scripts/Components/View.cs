using UnityEngine;

namespace Gamepackage
{
    public abstract class View : Component
    {
        protected GameObject gameObject;
        public View()
        {

        }

        public virtual void Initialize()
        {
            gameObject = new GameObject();
            gameObject.name = Owner.PrototypeUniqueIdentifier.ToString();
            var utr = gameObject.AddComponent<UnityTokenReference>();
            utr.Owner = Owner;
        }
    }
}
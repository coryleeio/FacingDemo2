using UnityEngine;

namespace Gamepackage
{
    public abstract class TokenView : Component<TokenViewPrototype>
    {
        protected GameObject gameObject;
        public TokenView()
        {

        }

        public virtual void BuildView()
        {
            gameObject = new GameObject();
            gameObject.name = Owner.PrototypeUniqueIdentifier.ToString();
            var utr = gameObject.AddComponent<UnityTokenReference>();
            utr.Owner = Owner;
        }
    }
}
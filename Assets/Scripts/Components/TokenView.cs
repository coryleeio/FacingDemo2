using Newtonsoft.Json;
using UnityEngine;

namespace Gamepackage
{
    public abstract class TokenView : Component<TokenViewPrototype>
    {
        [JsonIgnore]
        public GameObject GameObject;

        private bool _isVisible;
        protected bool IsVisible
        {
            set
            {
                _isVisible = value;
                if(GameObject != null)
                {
                    GameObject.SetActive(value);
                }
            }
            get
            {
                return _isVisible;
            }
        }

        public virtual int SortOrder
        {
            set
            {
                // overwrite this to deal with relative sort orders, etc.
            }
        }

        // handle setting of a world position
        public Vector3 WorldPosition
        {
            set
            {
                if(GameObject != null)
                {
                    GameObject.transform.position = value;
                }
            }
        }

        // handle setting of a map position
        public Point MapPosition
        {
            set
            {
                WorldPosition = MathUtil.MapToWorld(value);
            }
        }

        // handle setting of a map position
        public Pointf LerpPosition
        {
            set
            {
                WorldPosition = MathUtil.MapToWorld(value);
            }
        }

        public TokenView()
        {

        }

        public virtual void BuildView()
        {
            GameObject = new GameObject();
            GameObject.name = Owner.PrototypeUniqueIdentifier.ToString();
            var utr = GameObject.AddComponent<UnityTokenReference>();
            utr.Owner = Owner;
            MapPosition = Owner.Position;
        }
    }
}
using UnityEngine;

namespace Gamepackage
{
    public class StaticSpriteView : TokenView
    {
        private SpriteRenderer _spriteRenderer;

        public StaticSpriteView()
        {

        }

        public override void BuildView()
        {
            base.BuildView();
            if(GameObject != null)
            {
                _spriteRenderer = GameObject.AddComponent<SpriteRenderer>();
                _spriteRenderer.sprite = Prototype.Sprite;
                _spriteRenderer.material = Prototype.Material;
            }
        }

        public override int SortOrder
        {
            set
            {
                base.SortOrder = value;
                _spriteRenderer.sortingOrder = value;
            }
        }
    }
}
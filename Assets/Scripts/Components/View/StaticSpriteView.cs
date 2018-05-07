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
            if(gameObject != null)
            {
                _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                _spriteRenderer.sprite = Prototype.Sprite;
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
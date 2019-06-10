using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public enum ChangeMethodType
    {
        NotSet,
        Linear,
        Random,
    }

    public class SpriteChanger : MonoBehaviour
    {
        public List<Sprite> Sprites = new List<Sprite>();
        public SpriteRenderer Renderer;
        public ChangeMethodType ChangeMethod;
        public float timePerSprite = 0.0f;
        private float TimeElapsed = 0.0f;

        public void Start()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        public void FindNextSprite()
        {
            if(Sprites.Count == 0 && Renderer != null)
            {
                return;
            }

            if(ChangeMethod == ChangeMethodType.Random)
            {
                Renderer.sprite = MathUtil.ChooseRandomElement<Sprite>(Sprites);
            }

            else if (ChangeMethod == ChangeMethodType.Linear)
            {
                var currentIndex = Sprites.IndexOf(Renderer.sprite);
                if (currentIndex  == Sprites.Count - 1)
                {
                    Renderer.sprite = Sprites[0];
                }
                else
                {
                    Renderer.sprite = Sprites[currentIndex + 1];
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Update()
        {
            TimeElapsed += Time.deltaTime;
            if(TimeElapsed >= timePerSprite)
            {
                TimeElapsed = 0.0f;
                FindNextSprite();
            }
        }
    }
}

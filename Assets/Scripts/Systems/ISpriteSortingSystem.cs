using UnityEngine;

namespace Gamepackage
{
    public interface ISpriteSortingSystem
    {
        void Init();
        void RegisterTile(SpriteRenderer tileSpriteRenderer, Point position);
        void Sort();
    }
}

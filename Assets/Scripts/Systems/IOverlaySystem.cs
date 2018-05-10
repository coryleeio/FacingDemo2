using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public interface IOverlaySystem
    {
        void Init(int mapWidth, int mapHeight);
        void Activate(Overlay overlay);
        void Deactivate(Overlay overlay);
        void Clear();
        void Process();
        List<SpriteRenderer> GetTilesInPosition(int x, int y);
    }
}

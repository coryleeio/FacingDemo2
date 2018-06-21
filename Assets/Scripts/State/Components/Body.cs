using System.Collections.Generic;

namespace Gamepackage
{
    public class Body : Component
    {
        public int CurrentHealth;
        public int MaxHealth;
        public bool IsDead = false;
        public float ElapsedTimeDead = 0.0f;

        public Body() {}
    }
}

using System.Collections.Generic;
using TinyIoC;

namespace Gamepackage
{
    public class Dungeon : IResolvableReferences
    {
        public Dungeon()
        {
            
        }

        public Level[] Levels;

        public void Resolve(TinyIoCContainer container)
        {
            foreach(var level in Levels)
            {
                if(level != null)
                {
                    level.Resolve(container);
                }
            }
        }
    }
}
using System.Collections.Generic;
using TinyIoC;

namespace Gamepackage
{
    public class Dungeon : IHasApplicationContext
    {
        public Dungeon()
        {
            
        }

        public Level[] Levels;

        private ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
            foreach(var level in Levels)
            {
                if(level != null)
                {
                    level.InjectContext(context);
                }
            }
        }
    }
}
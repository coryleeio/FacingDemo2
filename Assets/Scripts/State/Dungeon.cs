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

        public ApplicationContext ApplicationContext
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        private ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
            foreach(var level in Levels)
            {
                level.InjectContext(context);
            }
        }
    }
}
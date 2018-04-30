using System.Collections.Generic;

namespace Gamepackage
{
    public class Dungeon : IResolvableReferences
    {
        public Dungeon()
        {
            
        }

        public Level[] Levels;

        public void Resolve(IResourceManager resourceManager)
        {
            foreach(var level in Levels)
            {
                level.Resolve(resourceManager);
            }
        }
    }
}
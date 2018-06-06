namespace Gamepackage
{
    public class Dungeon
    {
        public Dungeon() {}

        public Level[] Levels;

        public void InjectContext()
        {
            foreach(var level in Levels)
            {
                if(level != null)
                {
                    level.InjectContext();
                }
            }
        }
    }
}
namespace Gamepackage
{
    public class Dungeon
    {
        public Dungeon() {}

        public Level[] Levels;

        public void Rereferece()
        {
            foreach(var level in Levels)
            {
                level.Rereference();
            }
        }
    }
}
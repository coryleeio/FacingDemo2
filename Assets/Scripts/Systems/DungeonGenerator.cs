namespace Gamepackage
{
    public class DungeonGenerator : IDungeonGenerator
    {
        IGameStateSystem _gameStateSystem;

        public DungeonGenerator(IGameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;
        }

        public void GenerateDungeon()
        {
            
        }
    }
}

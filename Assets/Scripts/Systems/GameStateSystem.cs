namespace Gamepackage
{
    public class GameStateSystem : IGameStateSystem
    {
        private Game Game { get; set; }
        private ITokenSystem _tokenSystem { get; set; }
        private ILogSystem _logSystem { get; set; }

        public GameStateSystem(ITokenSystem tokenSystem, ILogSystem logSystem)
        {
            _tokenSystem = tokenSystem;
            _logSystem = logSystem;
        }

        public void NewGame()
        {
            _logSystem.Log("Clearing game state");
            _tokenSystem.Clear();
            Game = new Game
            {
                FurthestLevelReached = 1,
                CurrentLevel = 1,
                MonstersKilled = 0,
                Time = 0
            };
            Game.Dungeon = new Dungeon();
        }
    }
}
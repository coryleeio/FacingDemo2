namespace Gamepackage
{
    public class GameStateSystem : IGameStateSystem
    {
        private Game Game { get; set; }
        private ITokenSystem _tokenSystem { get; set; } 

        public GameStateSystem(ITokenSystem tokenSystem)
        {
            _tokenSystem = tokenSystem;
        }

        public void NewGame()
        {
            Game = new Game();
            _tokenSystem.Clear();
        }
    }
}
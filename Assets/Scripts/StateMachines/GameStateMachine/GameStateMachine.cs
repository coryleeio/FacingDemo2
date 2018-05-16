namespace Gamepackage
{
    public class GameStateMachine : StateMachine
    {
        public GamePlayState GamePlayState { get; set; }
        public MainMenuState MainMenuState { get; set; }
        public LoadingResourcesState LoadingResourcesState { get; set; }
    }
}
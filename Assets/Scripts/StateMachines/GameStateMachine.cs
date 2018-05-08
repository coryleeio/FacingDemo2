namespace Gamepackage
{
    public class GameStateMachine : StateMachine<Root>
    {
        public GamePlayState GamePlayState { get; set; }
        public MainMenuState MainMenuState { get; set; }
        public LoadingResourcesState LoadingResourcesState { get; set; }

        public GameStateMachine(
            Root owner
        ) : base(owner)
        {

        }
    }
}
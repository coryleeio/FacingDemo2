namespace Gamepackage
{
    public class GameStateMachine : StateMachine<Root>
    {
        public GamePlayState GamePlayState;
        public MainMenuState MainMenuState;
        public LoadingResourcesState LoadingResourcesState;

        public GameStateMachine(
            Root owner,
            GamePlayState gamePlayState,
            MainMenuState mainMenuState,
            LoadingResourcesState loadingState
        ) : base(owner)
        {
            GamePlayState = gamePlayState;
            MainMenuState = mainMenuState;
            LoadingResourcesState = loadingState;
            ChangeState(MainMenuState);
        }
    }
}
namespace Gamepackage
{
    public class GameStateMachine : StateMachine<Root>
    {
        public GamePlayState GamePlayState;
        public MainMenuState MainMenuState;

        public GameStateMachine(
            Root owner,
            GamePlayState gamePlayState,
            MainMenuState mainMenuState
        ) : base(owner)
        {
            GamePlayState = gamePlayState;
            MainMenuState = mainMenuState;
            ChangeState(MainMenuState);
        }
    }
}
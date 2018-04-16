namespace Gamepackage
{
    public interface IGameStateSystem
    {
        void NewGame();
        void SaveGame();
        void LoadGame();
        void Clear();

        Game Game
        {
            get;
        }
    }
}

namespace Gamepackage
{
    public interface ILogSystem
    {
        void Log(string message);
        void Warn(string message);
        void Error(string message);
    }
}
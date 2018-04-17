namespace Gamepackage
{
    public interface IVisibilitySystem
    {
        void Init();
        void UpdateVisibility(bool[,] newVisibility);
        void UpdateVisibility();
        void Clear();
    }
}
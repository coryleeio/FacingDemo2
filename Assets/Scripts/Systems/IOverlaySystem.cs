namespace Gamepackage
{
    public interface IOverlaySystem
    {
        void Init();
        void Activate(Overlay overlay);
        void Deactivate(Overlay overlay);
        void Clear();
        void Process();
    }
}

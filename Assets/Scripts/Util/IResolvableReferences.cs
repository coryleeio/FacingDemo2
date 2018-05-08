using TinyIoC;

namespace Gamepackage
{
    interface IResolvableReferences
    {
        void Resolve(TinyIoCContainer resourceManager);
    }
}

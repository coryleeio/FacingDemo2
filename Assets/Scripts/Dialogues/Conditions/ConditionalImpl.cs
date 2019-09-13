using System.Collections.Generic;

namespace Gamepackage
{
    public interface ConditionalImpl
    {
        bool Satisfied(Game game, Dialog state, Dictionary<string, string> parameters);
    }
}

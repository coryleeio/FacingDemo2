using System.Collections.Generic;

namespace Gamepackage
{
    public class NotEquals : ConditionalImpl
    {
        public bool Satisfied(Game game, Dialog state, Dictionary<string, string> parameters)
        {
            return !Gamepackage.Equals.SatisfiedShared(game, state, parameters);
        }
    }
}

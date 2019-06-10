using System.Collections.Generic;

namespace Gamepackage
{
    public class TriggerTemplate
    {
        public string Identifier;
        public string TriggerableActionClassName;
        public TriggerShape TriggerShape;
        public TriggerMode TriggerMode;
        public CombatActionParameters CombatActionParameters;
        public string PressInputHint;
    }
}

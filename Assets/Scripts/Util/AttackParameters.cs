using System.Collections.Generic;

namespace Gamepackage
{
    public class AttackParameters
    {
        public int DyeSize;
        public int DyeNumber;
        public int Bonus;
        public DamageTypes DamageType;
        public List<Effect> AttackSpecificEffects = new List<Effect>();
        public string AttackMessage = null;

        public AttackParameters() { }

        public AttackParameters(AttackParameters input)
        {
            this.DyeSize = input.DyeSize;
            this.DyeNumber = input.DyeNumber;
            this.Bonus = input.Bonus;
            this.DamageType = input.DamageType;
            this.AttackMessage = input.AttackMessage;
        }
    }
}

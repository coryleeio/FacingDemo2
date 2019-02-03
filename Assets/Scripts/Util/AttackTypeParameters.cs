using System.Collections.Generic;

namespace Gamepackage
{
    // Contains the values that are common across all attacks of a particular type.
    // range, number of targets pierced etc should not differ across different swings
    // of the same type, with the same weapon.

    // Also contains a list of attack parameters which encompasses all attacks of the given type with this weapon/body etc.
    public class AttackTypeParameters
    {
        public List<AttackParameters> AttackParameters = new List<AttackParameters>();
        public int Range;
        public int NumberOfTargetsToPierce;
        public AttackTargetingType AttackTargetingType = AttackTargetingType.Line;
    }
}

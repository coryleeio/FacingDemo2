using System.Collections.Generic;

namespace Gamepackage
{
    public class Tuple<TFIRST, TSECOND>
    {
        public TFIRST Key;
        public TSECOND Value;
    }

    public static class StringUtil
    {
        public static string MeleeDamageKey = "Damage";
        public static string DamageTypeKey = "Type";

        public static string Capitalize(this string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }

        public static string DamageTypeToDisplayString(DamageTypes damageType)
        {
            if (damageType == DamageTypes.FIRE)
            {
                return "fire";
            }
            else if (damageType == DamageTypes.COLD)
            {
                return "cold";
            }
            else if (damageType == DamageTypes.LIGHTNING)
            {
                return "lightning";
            }
            else if (damageType == DamageTypes.SLASHING)
            {
                return "slashing";
            }
            else if (damageType == DamageTypes.BLUDGEONING)
            {
                return "bludgeoning";
            }
            else if (damageType == DamageTypes.PIERCING)
            {
                return "piercing";
            }
            else if (damageType == DamageTypes.ARCANE)
            {
                return "arcane";
            }
            else if (damageType == DamageTypes.NEGATIVE)
            {
                return "negative";
            }
            else if (damageType == DamageTypes.HOLY)
            {
                return "holy";
            }
            else
            {
                throw new NotImplementedException("Need to add a display string for this damage type: " + damageType);
            }
        }

        public static string DisplayValueForAttackParameters(AttackParameters param, bool isLast)
        {
            return string.Format("{0}-{1}{2}", param.DyeNumber + param.Bonus, param.DyeNumber * param.DyeSize + param.Bonus, isLast ? "" : ",");
        }

        public static List<Tuple<string, string>> GetDisplayAttributesForItem(Item item)
        {
            var retVal = new List<Tuple<string, string>>();
            if (item.AttackParameters.Count > 0)
            {
                for (var i = 0; i < item.AttackParameters.Count; i++)
                {
                    var attackParameter = item.AttackParameters[i];
                    var isLast = i == item.AttackParameters.Count - 1;
                    retVal.Add(new Tuple<string, string>()
                    {
                        Key = MeleeDamageKey,
                        Value = DisplayValueForAttackParameters(attackParameter, isLast)
                    });
                }
                for (var i = 0; i < item.AttackParameters.Count; i++)
                {
                    var attackParameter = item.AttackParameters[i];
                    var isLast = i == item.AttackParameters.Count - 1;
                    retVal.Add(new Tuple<string, string>()
                    {
                        Key = DamageTypeKey,
                        Value = string.Format("{0}{1}", Capitalize(DamageTypeToDisplayString(attackParameter.DamageType)), isLast ? "" : ","),
                    });
                }
            }
            foreach (var pair in item.Attributes)
            {
                retVal.Add(new Tuple<string, string>()
                {
                    Key = pair.Key.ToString(),
                    Value = pair.Value.ToString()
                });
            }
            return retVal;
        }

        public static List<string> GetDisplayAbilitiesForItem(Item item)
        {
            var retVal = new List<string>();
            foreach (var ability in item.Abilities)
            {
                retVal.Add(string.Format("{0} - {1}", ability.DisplayName, ability.Description));
            }
            return retVal;
        }
    }
}

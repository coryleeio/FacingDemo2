using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class Tuple<TFIRST, TSECOND>
    {
        public TFIRST Key;
        public TSECOND Value;
    }

    public static class DisplayUtil
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
                return "damage.type.fire".Localize();
            }
            else if (damageType == DamageTypes.COLD)
            {
                return "damage.type.cold".Localize();
            }
            else if (damageType == DamageTypes.LIGHTNING)
            {
                return "damage.type.lightning".Localize();
            }
            else if (damageType == DamageTypes.SLASHING)
            {
                return "damage.type.slashing".Localize();
            }
            else if (damageType == DamageTypes.BLUDGEONING)
            {
                return "damage.type.bludgeoning".Localize();
            }
            else if (damageType == DamageTypes.PIERCING)
            {
                return "damage.type.piercing".Localize();
            }
            else if (damageType == DamageTypes.ARCANE)
            {
                return "damage.type.arcane".Localize();
            }
            else if (damageType == DamageTypes.NEGATIVE)
            {
                return "damage.type.negative".Localize();
            }
            else if (damageType == DamageTypes.HOLY)
            {
                return "damage.type.holy".Localize();
            }
            else if (damageType == DamageTypes.POISON)
            {
                return "damage.type.poison".Localize();
            }
            else if (damageType == DamageTypes.HEALING)
            {
                return "damage.type.healing".Localize();
            }
            else
            {
                throw new NotImplementedException("Need to add a display string for this damage type: " + damageType);
            }
        }

        public static string DisplayValueForSlot(ItemSlot slot)
        {
            if (slot == ItemSlot.None)
            {
                return "item.slot.display.none".Localize();
            }
            else if (slot == ItemSlot.Chest)
            {
                return "item.slot.display.chest".Localize();
            }
            else if (slot == ItemSlot.Cloak)
            {
                return "item.slot.display.cloak".Localize();
            }
            else if (slot == ItemSlot.Helmet)
            {
                return "item.slot.display.helmet".Localize();
            }
            else if (slot == ItemSlot.Shoes)
            {
                return "item.slot.display.shoes".Localize();
            }
            else if (slot == ItemSlot.MainHand)
            {
                return "item.slot.display.mainhand".Localize();
            }
            else if (slot == ItemSlot.OffHand)
            {
                return "item.slot.display.offhand".Localize();
            }
            else if (slot == ItemSlot.Ammo)
            {
                return "item.slot.display.ammo".Localize();
            }
            else if (slot == ItemSlot.Ring)
            {
                return "item.slot.display.ring".Localize();
            }
            else if (slot == ItemSlot.Neck)
            {
                return "item.slot.display.neck".Localize();
            }
            else
            {
                throw new NotImplementedException("Need to add a display string for this slot type: " + slot);
            }
        }

        public static string TooltipFor(string key)
        {
            if (key == MeleeDamageKey)
            {
                return "";
            }
            else
            {
                return "";
            }
        }

        public static string DisplayValueForAttackParameters(AttackParameters param, bool isLast)
        {
            return string.Format("{0}-{1}{2}", param.DyeNumber + param.Bonus, param.DyeNumber * param.DyeSize + param.Bonus, isLast ? "" : ",");
        }

        public static string DisplayValueForAttribute(Attributes attr)
        {
            if (attr == Attributes.MAX_HEALTH)
            {
                return "attribute.max.health".Localize();
            }
            else
            {
                return attr.ToString();
            }
        }

        public static Color DamageDisplayColor(bool isPlayer, bool isHostile)
        {
            Color healthChangeColor = Color.black;

            if (isPlayer)
            {
                if (isHostile)
                {
                    // Damage to player
                    healthChangeColor = Color.red;
                }
                else
                {
                    // Healing to player
                    healthChangeColor = Color.green;
                }
            }
            else
            {
                if (isHostile)
                {
                    // Damage to NPC
                    healthChangeColor = Color.magenta;
                }
                else
                {
                    // Healing to NPC
                    healthChangeColor = Color.blue;
                }
            }
            return healthChangeColor;
        }

        public static List<Tuple<string, string>> GetDisplayAttributesForPlayer(Entity player)
        {
            var retVal = new List<Tuple<string, string>>();

            retVal.Add(new Tuple<string, string>()
            {
                Key = DisplayValueForAttribute(Attributes.MAX_HEALTH),
                Value = string.Format("{0}/{1}", player.Body.CurrentHealth, player.CalculateValueOfAttribute(Attributes.MAX_HEALTH)),
            });
            foreach (var enumVal in Enum.GetValues(typeof(Attributes)))
            {
                var castVal = (Attributes)enumVal;
                if (castVal == Attributes.MAX_HEALTH)
                {
                    continue;
                }
                retVal.Add(new Tuple<string, string>()
                {
                    Key = DisplayValueForAttribute(castVal),
                    Value = player.CalculateValueOfAttribute(castVal).ToString(),
                });
            }
            return retVal;
        }

        public static List<Tuple<string, string>> GetDisplayAttributesForItem(Item item)
        {
            var retVal = new List<Tuple<string, string>>();
            var meleeTypeParams = item.AttackTypeParameters[AttackType.Melee];
            var meleeParameters = meleeTypeParams.AttackParameters;
            if (meleeParameters.Count > 0)
            {
                for (var i = 0; i < meleeParameters.Count; i++)
                {
                    var attackParameter = meleeParameters[i];
                    var isLast = i == meleeParameters.Count - 1;
                    retVal.Add(new Tuple<string, string>()
                    {
                        Key = MeleeDamageKey,
                        Value = DisplayValueForAttackParameters(attackParameter, isLast)
                    });
                }
                for (var i = 0; i < meleeParameters.Count; i++)
                {
                    var attackParameter = meleeParameters[i];
                    var isLast = i == meleeParameters.Count - 1;
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
            foreach (var effect in item.EffectsGlobal)
            {
                retVal.Add(string.Format("{0} - {1}", effect.DisplayName.Localize(), effect.Description.Localize()));
            }
            return retVal;
        }

        public static string GetAnimationNameForDirection(Animations animation, Direction direction)
        {
            var output = animation.ToString();
            var directionComponent = "";

            if (direction == Direction.SouthEast)
            {
                directionComponent += "SE";
            }
            else if (direction == Direction.NorthEast)
            {
                directionComponent += "NE";
            }
            else if (direction == Direction.NorthWest)
            {
                directionComponent += "NW";
            }
            else if (direction == Direction.SouthWest)
            {
                directionComponent += "SW";
            }
            else
            {
                directionComponent += "SE";
            }
            return output += directionComponent;
        }
    }
}

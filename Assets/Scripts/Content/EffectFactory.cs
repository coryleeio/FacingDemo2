using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class EffectFactory
    {
        public static Effect Build(UniqueIdentifier uniqueIdentifier, List<AttackType> validCombatContexts = null)
        {
            Effect retVal = null;
            if (uniqueIdentifier == UniqueIdentifier.EFFECT_TRAVERSE_STAIRCASE)
            {
                retVal = new TraverseStaircase()
                {
                    LocalizationName = "traverse.staircase",
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE)
            {
                retVal = new LuckyCoinLifeSave()
                {
                    LocalizationName = "lucky.coin.lifesave",
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_WEAK_POISON,
                    LocalizationName = "weak.poison",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_MADNESS)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_MADNESS,
                    LocalizationName = "madness",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_CHARM)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_CHARM,
                    LocalizationName = "charm",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_DOMINATION)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_DOMINATION,
                    LocalizationName = "domination",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_STRONG_POISON)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_STRONG_POISON,
                    LocalizationName = "strong.poison",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_POISON_IMMUNITY)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_POISON_IMMUNITY,
                    LocalizationName = "poison.immunity",
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_WEAK_POISON)
            {
                retVal = new Poison()
                {
                    LocalizationName = "weak.poison",
                    PoisonAmount = 1,
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 3
                    },
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_MADNESS)
            {
                retVal = new Madness()
                {
                    LocalizationName = "madness",
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 3
                    },
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_CHARM)
            {
                retVal = new Charm()
                {
                    LocalizationName = "charm",
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 3
                    },
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_DOMINATION)
            {
                retVal = new Domination()
                {
                    LocalizationName = "domination",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_STRONG_POISON)
            {
                retVal = new Poison()
                {
                    LocalizationName = "strong.poison",
                    PoisonAmount = 3,
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 3
                    },
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_POISON_IMMUNITY)
            {
                retVal = new PoisonImmunity()
                {
                    LocalizationName = "poison.immunity",
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 20
                    },
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_WEAK_REGENERATION)
            {
                retVal = new Regeneration()
                {
                    LocalizationName = "weak.regeneration",
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 20
                    },
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }

            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_WEAK_REGENERATION)
            {
                retVal = new AppliedEffect()
                {
                    LocalizationName = "weak.regeneration",
                    EffectAppliedId = UniqueIdentifier.EFFECT_WEAK_REGENERATION,
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }

            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_STRENGTH_OF_GIANTS)
            {
                retVal = new StrengthOfGiants()
                {
                    LocalizationName = "strength.of.giants",
                    Attributes = new Dictionary<Attributes, int>()
                    {
                        {  Attributes.MAX_HEALTH, 100 },
                    },
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }

            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE)
            {
                retVal = new LuckyCoinLifeSave()
                {
                    LocalizationName = "lucky.coin.lifesave",
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }

            if (retVal is AppliedEffect)
            {
                var castVal = retVal as AppliedEffect;
                if (validCombatContexts == null || validCombatContexts.Count == 0)
                {
                    throw new NotImplementedException("An applied effect with no combat contexts will never be applied: " + uniqueIdentifier);
                }
                castVal.ValidCombatContextsForApplication.AddRange(validCombatContexts);
            }

            if (retVal == null)
            {
                throw new NotImplementedException();
            }
            retVal.Identifier = uniqueIdentifier;
            if (retVal.Attributes == null)
            {
                retVal.Attributes = new Dictionary<Attributes, int>();
            }
            Assert.IsNotNull(retVal.LocalizationName, "Localization not defined for " + retVal.Identifier);
            Assert.AreNotEqual("", retVal.LocalizationName, "Localization not defined for " + retVal.Identifier);
            Assert.IsTrue(retVal.Hostility != AttackHostility.NOT_SET, "Must specify hostility for all effects, not set for: " + retVal.Identifier);
            return retVal;
        }
    }
}

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
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE)
            {
                retVal = new LuckyCoinLifeSave()
                {
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_WEAK_POISON,
                    AppliedDisplayName = "effect.applied.weak.poison.name",
                    AppliedDisplayDescription = "effect.applied.weak.poison.description",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_MADNESS)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_MADNESS,
                    AppliedDisplayName = "effect.applied.madness.name",
                    AppliedDisplayDescription = "effect.applied.madness.description",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_CHARM)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_CHARM,
                    AppliedDisplayName = "effect.applied.charm.name",
                    AppliedDisplayDescription = "effect.applied.charm.description",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_DOMINATION)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_DOMINATION,
                    AppliedDisplayName = "effect.applied.domination.name",
                    AppliedDisplayDescription = "effect.applied.domination.description",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_STRONG_POISON)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_STRONG_POISON,
                    AppliedDisplayName = "effect.applied.strong.poison.name",
                    AppliedDisplayDescription = "effect.applied.strong.poison.description",
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_POISON_IMMUNITY)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_POISON_IMMUNITY,
                    AppliedDisplayName = "effect.applied.poison.immunity.name",
                    AppliedDisplayDescription = "effect.applied.poison.immunity.description",
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_WEAK_POISON)
            {
                retVal = new Poison()
                {
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
                    Hostility = AttackHostility.HOSTILE,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_STRONG_POISON)
            {
                retVal = new Poison()
                {
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
                    EffectAppliedId = UniqueIdentifier.EFFECT_WEAK_REGENERATION,
                    AppliedDisplayName = "effect.applied.regen.name".Localize(),
                    AppliedDisplayDescription = "effect.applied.regen.description".Localize(),
                    Hostility = AttackHostility.NOT_HOSTILE,
                };
            }

            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_STRENGTH_OF_GIANTS)
            {
                retVal = new StrengthOfGiants()
                {
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

            Assert.IsTrue(retVal.Hostility != AttackHostility.NOT_SET, "Must specify hostility for all effects, not set for: " + retVal.Identifier);
            return retVal;
        }
    }
}

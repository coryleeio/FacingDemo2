using System.Collections.Generic;

namespace Gamepackage
{
    public static class EffectFactory
    {
        public static Effect Build(UniqueIdentifier uniqueIdentifier, List<CombatContext> validCombatContexts = null)
        {
            Effect retVal = null;
            if (uniqueIdentifier == UniqueIdentifier.EFFECT_TRAVERSE_STAIRCASE)
            {
                retVal = new TraverseStaircase();
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE)
            {
                retVal = new LuckyCoinLifeSave();
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_WEAK_POISON,
                    AppliedDisplayName = "effect.applied.weak.poison.name".Localize(),
                    AppliedDisplayDescription = "effect.applied.weak.poison.description".Localize(),
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_MADNESS)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_MADNESS,
                    AppliedDisplayName = "effect.applied.madness.name".Localize(),
                    AppliedDisplayDescription = "effect.applied.madness.description".Localize(),
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_STRONG_POISON)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_STRONG_POISON,
                    AppliedDisplayName = "effect.applied.strong.poison.name".Localize(),
                    AppliedDisplayDescription = "effect.applied.strong.poison.description".Localize(),
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_POISON_IMMUNITY)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_POISON_IMMUNITY,
                    AppliedDisplayName = "effect.applied.poison.immunity.name".Localize(),
                    AppliedDisplayDescription = "effect.applied.poison.immunity.description".Localize(),
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
                    }
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_MADNESS)
            {
                retVal = new Madness()
                {
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 3
                    }
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
                    }
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_POISON_IMMUNITY)
            {
                retVal = new PoisonImmunity()
                {
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 20
                    }
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_WEAK_REGENERATION)
            {
                retVal = new Regeneration()
                {
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 20
                    }
                };
            }

            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_WEAK_REGENERATION)
            {
                retVal = new AppliedEffect()
                {
                    EffectAppliedId = UniqueIdentifier.EFFECT_WEAK_REGENERATION,
                    AppliedDisplayName = "effect.applied.regen.name".Localize(),
                    AppliedDisplayDescription = "effect.applied.regen.description".Localize(),
                };
            }

            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_STRENGTH_OF_GIANTS)
            {
                retVal = new StrengthOfGiants()
                {
                    Attributes = new Dictionary<Attributes, int>()
                    {
                        {  Attributes.MAX_HEALTH, 100 },
                    }
                };
            }

            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE)
            {
                retVal = new LuckyCoinLifeSave();
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
            return retVal;
        }
    }
}

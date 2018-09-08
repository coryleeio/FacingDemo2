namespace Gamepackage
{
    public static class EffectFactory
    {
        public static Effect Build(UniqueIdentifier uniqueIdentifier)
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
                    AppliedDisplayName = "Applied weak poison",
                    AppliedDisplayDescription = "The business end of this is coated in poison..."
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_WEAK_POISON)
            {
                retVal = new WeakPoison()
                {
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 3
                    }
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_STRONG_POISON)
            {
                retVal = new StrongPoison()
                {
                    Ticker = new Ticker()
                    {
                        TurnsRemaining = 3
                    }
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_CURE_POISON)
            {
                retVal = new CurePoison();
            }

            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE)
            {
                retVal = new LuckyCoinLifeSave();
            }

            if(retVal.StackingStrategy == null)
            {
                retVal.StackingStrategy = new AlwaysNewEffect();
            }

            if(retVal == null)
            {
                throw new NotImplementedException();
            }
            return retVal;
        }
    }
}

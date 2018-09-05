namespace Gamepackage
{
    public static class EffectFactory
    {
        public static Effect Build(UniqueIdentifier uniqueIdentifier)
        {
            if (uniqueIdentifier == UniqueIdentifier.EFFECT_TRAVERSE_STAIRCASE)
            {
                return new TraverseStaircase();
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE)
            {
                return new LuckyCoinLifeSave();
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON)
            {
                return new AppliedWeakPoison();
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_WEAK_POISON)
            {
                return new WeakPoison()
                {
                    Ticker = new Duration()
                    {
                        TurnsRemaining = 3
                    }
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_STRONG_POISON)
            {
                return new StrongPoison()
                {
                    Ticker = new Duration()
                    {
                        TurnsRemaining = 3
                    }
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE)
            {
                return new LuckyCoinLifeSave();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}

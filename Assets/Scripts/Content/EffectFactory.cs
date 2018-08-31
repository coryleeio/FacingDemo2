namespace Gamepackage
{
    public static class EffectFactory
    {
        public static Effect Build(UniqueIdentifier uniqueIdentifier)
        {
            if(uniqueIdentifier == UniqueIdentifier.EFFECT_TRAVERSE_STAIRCASE)
            {
                return new TraverseStaircase();
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE)
            {
                return new LuckyCoinLifeSave();
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_WEAK_POISON)
            {
                return new WeakPoison()
                {
                    TurnsRemaining = 3 
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.EFFECT_STRONG_POISON)
            {
                return new StrongPoison()
                {
                    TurnsRemaining = 3
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

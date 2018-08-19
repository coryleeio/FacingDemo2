namespace Gamepackage
{
    public static class EffectFactory
    {
        public static Effect Build(UniqueIdentifier uniqueIdentifier)
        {
            if(uniqueIdentifier == UniqueIdentifier.ABILITY_TRAVERSE_STAIRCASE)
            {
                return new TraverseStaircase();
            }
            else if (uniqueIdentifier == UniqueIdentifier.ABILITY_LUCKY_COIN_LIFE_SAVE)
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

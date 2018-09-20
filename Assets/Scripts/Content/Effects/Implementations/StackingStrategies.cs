namespace Gamepackage
{
    public static class StackingStrategies
    {
        public static void AddDuration(Entity entity, Effect effect)
        {
            var matchingEffects = entity.Body.Effects.FindAll((x) => { return x.Identifier == effect.Identifier; });
            if (matchingEffects.Count > 1)
            {
                throw new NotImplementedException("If these effects add to the duration of a matching effect, how the hell did you get two of them?");
            }
            if (!effect.CanTick)
            {
                throw new NotImplementedException("Cant add to the duration of an effect that does not have a duration.");
            }

            if (matchingEffects.Count == 0)
            {
                entity.Body.Effects.Add(entity, effect);
                return;
            }
            if (matchingEffects.Count == 1)
            {
                var existingEffect = matchingEffects[0];
                existingEffect.Ticker.SetLimitedDuration(existingEffect.Ticker.TurnsRemaining + effect.Ticker.TurnsRemaining);
            }
        }

        public static void AddDuplicate(Entity entity, Effect effect)
        {
            entity.Body.Effects.Add(entity, effect);
        }
    }
}

namespace Gamepackage
{
    // Add to the duration of an existing effect if ids match
    public class AddDuration : IStackingStrategy
    {
        public void Stack(Entity entity, Effect newEffect)
        {
            var matchingEffects = entity.Body.Effects.FindAll((x) => { return x.GetType() == newEffect.GetType(); });
            if(matchingEffects.Count > 1)
            {
                throw new NotImplementedException("If these effects add to the duration of a matching effect, how the hell did you get two of them?");
            }
            if(!newEffect.IsTickingEffect)
            {
                throw new NotImplementedException("Cant add to the duration of an effect that does not have a duration.");
            }

            if(matchingEffects.Count ==  0)
            {
                entity.Body.Effects.Add(entity, newEffect);
                return;
            }
            if(matchingEffects.Count == 1)
            {
                var existingEffect = matchingEffects[0];
                existingEffect.Ticker.SetLimitedDuration(existingEffect.Ticker.TurnsRemaining + newEffect.Ticker.TurnsRemaining);
            }
        }
    }
}

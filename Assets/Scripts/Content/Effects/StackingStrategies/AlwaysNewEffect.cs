namespace Gamepackage
{
    // Simplest way to stack effects, a new instance of the effect is just added for each application
    public class AlwaysNewEffect : IStackingStrategy
    {
        public void StackEffectOnEntity(Entity entity, Effect effect)
        {
            entity.Body.Effects.Add(entity, effect);
        }
    }
}

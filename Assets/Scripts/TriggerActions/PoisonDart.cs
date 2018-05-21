namespace Gamepackage
{
    public class PoisonDart : TriggerAction
    {
        public UniqueIdentifier EffectToApply;
        public TurnSystem _turnSystem;

        public PoisonDart(TurnSystem turnSystem)
        {
            _turnSystem = turnSystem;
        }
    }
}

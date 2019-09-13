namespace Gamepackage
{
    public class DialogOptionNode : DialogBaseNode
    {
        public ConditionNotMetBehaviour ConditionNotMetBehaviour;
        public override bool SelfValid => (Conditions.Count == 0 || ConditionNotMetBehaviour != ConditionNotMetBehaviour.NotSet);
    }
}

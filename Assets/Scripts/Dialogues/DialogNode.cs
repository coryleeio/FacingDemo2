namespace Gamepackage
{
    public class DialogNode : DialogBaseNode
    {
        public override bool SelfValid => Conditions.Count == 0;
    }
}

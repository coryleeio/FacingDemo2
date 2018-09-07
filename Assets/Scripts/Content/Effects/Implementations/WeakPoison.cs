namespace Gamepackage
{
    public class WeakPoison : Poison
    {
        public override string Description
        {
            get
            {
                return "You are poisoned, atleast it isn't very strong...";
            }
        }

        public override int PoisonAmount
        {
            get
            {
                return 1;
            }
        }
    }
}

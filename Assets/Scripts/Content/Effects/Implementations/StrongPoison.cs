namespace Gamepackage
{
    public class StrongPoison : Poison
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
                return 3;
            }
        }
    }
}

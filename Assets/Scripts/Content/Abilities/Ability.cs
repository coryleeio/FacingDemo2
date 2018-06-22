namespace Gamepackage
{
    public abstract class Ability : Action
    {
        public abstract bool CanPerform
        {
            get;
        }
    }
}

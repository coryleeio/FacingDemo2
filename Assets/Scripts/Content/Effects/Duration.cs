namespace Gamepackage
{
    public class Duration
    {
        // this cant be set in constructor, if it is loading your game will reset the time
        // due to the constructor begin called during deserialization.
        public int TurnsRemaining;
        public const int UnlimitedDurationValue = -99999;

        public void SetUnlimitedDuration()
        {
            TurnsRemaining = UnlimitedDurationValue;
        }
        public void SetLimitedDuration(int NumberOfTurnsRemaining)
        {
            TurnsRemaining = NumberOfTurnsRemaining;
        }

        public bool HasUnlimitedDuration
        {
            get
            {
                return TurnsRemaining == UnlimitedDurationValue;
            }
        }

        public bool ShouldExpire
        {
            get
            {
                return TurnsRemaining == 0;
            }
        }

        public void Tick(Entity source)
        {
            // Dont call base bc it will throw not implemented
            if (!HasUnlimitedDuration)
            {
                TurnsRemaining--;
            }
        }
    }
}

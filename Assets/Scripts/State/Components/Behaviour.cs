using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Behaviour : Component
    {
        public bool IsDoneThisTurn = false;
        public int TimeAccrued = 0;
        public Team Team;
        public bool IsThinking = false;

        public enum AIType
        {
            None,
            DumbMelee,
            Archer,
        }
        public AIType AI;

        [JsonIgnore]
        public Action NextAction = null;

        public abstract bool IsPlayer
        {
            get;
        }

        public abstract void FigureOutNextAction();
    }
}

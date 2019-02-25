using Newtonsoft.Json;
using UnityEngine;

namespace Gamepackage
{
    public class Behaviour
    {
        public Team OriginalTeam;
        public Team ActingTeam;
        public AIType AI;
        public bool IsDoneThisTurn = false;
        public int TimeAccrued = 0;
        public bool IsThinking = false;
        public bool IsPlayer;

        public Point LastKnownTargetPosition = null;

        [JsonIgnore]
        public Action NextAction = null;

        [JsonIgnore]
        public int PathsExpected = 0;

        [JsonIgnore]
        public int PathsReturned = 0;
    }
}

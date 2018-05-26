using Newtonsoft.Json;

namespace Gamepackage
{
    public class Game : IHasApplicationContext
    {
        public int FurthestLevelReached { get; set; }
        public int CurrentLevelIndex { get; set; }
        public int MonstersKilled { get; set; }
        public int CurrentTurn { get; set; }
        public bool IsPlayerTurn { get; set; }

        [JsonProperty]
        public int _nextId = 0;

        [JsonIgnore]
        public int NextId
        {
            get
            {
                _nextId = _nextId + 1;
                return _nextId;
            }
        }

        public Game()
        {

        }

        public Dungeon Dungeon;

        [JsonIgnore]
        public Level CurrentLevel
        {
            get
            {
                return Dungeon.Levels[CurrentLevelIndex];
            }
        }

        private ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
            Dungeon.InjectContext(Context);
        }
    }
}
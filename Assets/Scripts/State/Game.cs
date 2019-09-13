using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class Game
    {
        public int FurthestLevelReached { get; set; }
        public int CurrentLevelIndex { get; set; }
        public int MonstersKilled { get; set; }
        public int CurrentTurn { get; set; }
        public bool IsPlayerTurn { get; set; }
        public Dictionary<int, Dialog> DialogsById;
        public Dialog CurrentlyOpenDialog;

        public string CampaignTemplateIdentifier;
        [JsonIgnore]
        public CampaignTemplate CampaignTemplate
        {
            get
            {
                return Context.ResourceManager.Load<CampaignTemplate>(CampaignTemplateIdentifier);
            }
        }

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

        public Dungeon Dungeon;

        [JsonIgnore]
        public Level CurrentLevel
        {
            get
            {
                return Dungeon.Levels[CurrentLevelIndex];
            }
        }
    }
}
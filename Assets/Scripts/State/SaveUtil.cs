using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class SaveUtil
    {
        public static void NewGame(string CampaignTemplateIdentifier = null)
        {
            var gameSceneRoot = GameObject.FindObjectOfType<GameSceneController>();
            if (gameSceneRoot != null)
            {
                gameSceneRoot.Stopped = true;
            }
            Clear();
            Context.Game = new Game
            {
                FurthestLevelReached = 0,
                CurrentLevelIndex = 0,
                MonstersKilled = 0,
                CurrentTurn = 0,
                IsPlayerTurn = true,
            };

            Context.Game.CurrentlyOpenDialog = null;
            Context.Game.DialogsById = new System.Collections.Generic.Dictionary<int, Dialog>();
            Context.Game.Dungeon = new Dungeon();
            Context.Game.CampaignTemplateIdentifier = CampaignTemplateIdentifier != null ? CampaignTemplateIdentifier : "CAMPAIGN_CORE";
            Debug.Log("Starting new campaign: " + Context.Game.CampaignTemplateIdentifier);
            ResolveRulesEngine();
        }

        private static void ResolveRulesEngine()
        {
            Assert.IsNotNull(Context.Game.CampaignTemplate, "Could not resolve campaign template");
            Context.RulesEngine = (IRulesEngine)Context.ResourceManager.CreateInstanceFromAbstractOrInterfaceTypeAndName(typeof(IRulesEngine), Context.Game.CampaignTemplate.RulesEngineClassName);
            Debug.Log("Loaded rules engine: " + Context.Game.CampaignTemplate.RulesEngineClassName);
        }

        public static void Clear()
        {
            var gameSceneRoot = GameObject.FindObjectOfType<GameSceneController>();
            if (gameSceneRoot != null)
            {
                gameSceneRoot.Stopped = true;
            }
            Context.EntitySystem.Clear();
            Context.Game = null;
        }

        public static void SaveGame(Game game, string saveLocation)
        {
            // parameters are required to deserialize abstract types
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            File.WriteAllText(saveLocation, JsonConvert.SerializeObject(Context.Game, Formatting.Indented, parameters));
        }

        public static bool HasGameToLoad(string saveLocation)
        {
            return File.Exists(saveLocation);
        }

        public static void LoadGame()
        {
            NewGame();
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            Context.Game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(GetDefaultSaveLocation()), parameters);
            ResolveRulesEngine();
        }

        public static string GetDefaultSaveLocation()
        {
            return UnityEngine.Application.persistentDataPath + "/dev.sav";
        }
    }
}

using Mono.Data.Sqlite;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class ResourceManager
    {
        private Dictionary<string, object> _prototypesByUniqueIdentifier = new Dictionary<string, object>();

        public bool Contains(string input)
        {
            return _prototypesByUniqueIdentifier.ContainsKey(input);
        }

        public bool Contains<T>(string input)
        {

            if (_prototypesByUniqueIdentifier.ContainsKey(input))
            {
                return (_prototypesByUniqueIdentifier[input]) is T;
            }
            return false;
        }

        public void Clear()
        {

        }

        public TPrototype LoadUnityResource<TPrototype>(string identifier) where TPrototype : UnityEngine.Object
        {
            // This allows us to preserve the hot reload functionality in the editor
            // so we can see edits in realtime as we make modifications instead of needing to reload
            // the asset bundle.
            if (Application.isEditor)
            {
                if (typeof(TPrototype) == typeof(Sprite))
                {
                    var sprite = Resources.Load<TPrototype>("Sprites/" + identifier);
                    return sprite;
                }
            }
            return Load<TPrototype>(identifier);
        }

        public TPrototype Load<TPrototype>(string identifier)
        {
            try
            {
                if (!_prototypesByUniqueIdentifier.ContainsKey(identifier))
                {
                    Debug.LogWarning(string.Format("Failed to lookup prototype: {0}", identifier));
                    return default;
                }
                return (TPrototype)_prototypesByUniqueIdentifier[identifier];
            }
            catch (InvalidCastException ex)
            {
                throw new CouldNotResolvePrototypeException(string.Format("Found prototype for {0}, but it is not a {1}", identifier, typeof(TPrototype).Name));
            }
        }

        // Called by mod manager once all mods are loaded to resolve all the resources from all the mods
        public void ResolveAllResources(Mono.Data.Sqlite.SqliteConnection sqlConnection)
        {
            Debug.Log("Resolving all resources...");
            _prototypesByUniqueIdentifier.Clear();

            // LoadAll is called when we load the mod, but we specifically
            // cache references to these unity types in the asset bundle
            CacheResources(LoadUnityResource<Sprite>());
            CacheResources(LoadUnityResource<SkeletonDataAsset>());
            CacheResources(LoadUnityResource<GameObject>());

            CacheResources(LoadTilesets(sqlConnection));

            CacheResources(LoadMultitileViews(sqlConnection));
            CacheResources(LoadItemAppearances(sqlConnection));
            CacheResources(LoadViewTemplates(sqlConnection));
            CacheResources(LoadEffectImpls());
            CacheResources(LoadTriggerableActionImpls());
            CacheResources(LoadAI());
            CacheResources(LoadRulesEngines());
            CacheResources(LoadTriggerTemplates(sqlConnection));
            CacheResources(LoadEffectTemplates(sqlConnection));
            CacheResources(ProjectileAppearances.LoadAll());
            CacheResources(LoadEnchantmentTemplates(sqlConnection));

            CacheResources(LoadProbabilityTables(sqlConnection, "Items_EnchantmentTables", "Items_EnchantmentTables_Parcels", "Items_EnchantmentTables_ParcelEntries"));
            CacheResources(LoadProbabilityTables(sqlConnection, "ItemDropTable", "ItemDropTable_Parcels", "ItemDropTable_ParcelEntries"));
            CacheResources(LoadProbabilityTables(sqlConnection, "EncounterTable", "EncounterTable_Parcels", "EncounterTable_ParcelEntries"));
            CacheResources(LoadProbabilityTables(sqlConnection, "NameTable", "NameTable_Parcels", "NameTable_ParcelEntries"));
            CacheResources(LoadProbabilityTables(sqlConnection, "ViewTables", "ViewTables_Parcels", "ViewTables_ParcelEntries"));

            CacheResources(LoadItemTemplates(sqlConnection));
            CacheResources(LoadEntityTemplates(sqlConnection));
            CacheResources(LoadCampaignTemplates(sqlConnection));
        }

        private Dictionary<string, EntityTemplate> LoadEntityTemplates(SqliteConnection sqlConnection)
        {
            var sql = "select * from Entities";
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();

            var aggregate = new Dictionary<string, EntityTemplate>();
            while (reader.Read())
            {
                var entityTemplate = new EntityTemplate();
                entityTemplate.Identifier = reader[0].ToString();

                var nameListStr = reader[1].ToString();

                if (Context.ResourceManager.Contains<ProbabilityTable>(nameListStr))
                {
                    entityTemplate.NameList = Context.ResourceManager.Load<ProbabilityTable >(nameListStr);
                }
                else
                {
                    entityTemplate.NameList = ProbabilityTable.ForSingleValue(nameListStr);
                }

                entityTemplate.IsCombatant = ParseBoolFromIntString(reader[2].ToString());
                entityTemplate.DefaultWeaponIdentifier = reader[3].ToString();
                entityTemplate.BlocksPathing = ParseBoolFromIntString(reader[4].ToString());
                entityTemplate.ViewTemplateIdentifier = reader[5].ToString();
                entityTemplate.AIClassName = reader[6].ToString();
                entityTemplate.IsAlwaysVisible = ParseBoolFromIntString(reader[7].ToString());
                entityTemplate.Trigger = reader[8].ToString();

                entityTemplate.isFloating = (FloatingState) Enum.Parse(typeof(FloatingState), reader[9].ToString(), true);
                entityTemplate.CastsShadow = (ShadowCastState) Enum.Parse(typeof(ShadowCastState), reader[10].ToString(), true);

                entityTemplate.TemplateAttributes = ParseAttributesDictFromTable(sqlConnection, entityTemplate.Identifier, "Entities_Attributes");
                entityTemplate.EquipmentTables = ParseListOfStrings(sqlConnection, entityTemplate.Identifier, "Entities_EquipmentTables");
                entityTemplate.InventoryTables = ParseListOfStrings(sqlConnection, entityTemplate.Identifier, "Entities_InventoryTables");

                aggregate.Add(entityTemplate.Identifier, entityTemplate);
            }
            return aggregate;
        }

        private Dictionary<string, Dictionary<string, Type>> LoadAI()
        {
            return BuildTypeDictFromInterface(typeof(IAI));
        }

        private Dictionary<string, MultitileViewTemplate> LoadMultitileViews(SqliteConnection sqlConnection)
        {
            var sql = "select * from MultitileViews";
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();

            var aggregate = new Dictionary<string, MultitileViewTemplate>();
            while (reader.Read())
            {
                var viewTemplate = new MultitileViewTemplate();
                viewTemplate.Identifier = reader[0].ToString();

                viewTemplate.MultitileViewTemplateComponent.AddRange(LoadMultitileViewComponents(sqlConnection, viewTemplate.Identifier));
                aggregate.Add(viewTemplate.Identifier, viewTemplate);
            }
            return aggregate;
        }

        private IEnumerable<MultitileViewTemplateComponent> LoadMultitileViewComponents(SqliteConnection sqlConnection, string templateIdentifier)
        {
            var retVal = new List<MultitileViewTemplateComponent>();
            var sqlForData = string.Format("select * from [MultitileViews_Component] WHERE [Identifier] = @Identifier");
            var commandForData = new SqliteCommand(sqlForData, sqlConnection);
            commandForData.Parameters.AddWithValue("@Identifier", templateIdentifier);
            var readerForData = commandForData.ExecuteReader();
            while (readerForData.Read())
            {
                var identifier = readerForData[0].ToString();
                var entry = new MultitileViewTemplateComponent();
                entry.Identifier = readerForData[0].ToString();
                entry.Sprite = Context.ResourceManager.Load<Sprite>(readerForData[1].ToString());

                var engineOffsetXStr = readerForData[2].ToString();
                float.TryParse(engineOffsetXStr, out float engineOffsetX);
                entry.EngineOffsetX = engineOffsetX;

                var engineOffsetYStr = readerForData[3].ToString();
                float.TryParse(engineOffsetYStr, out float engineOffsetY);
                entry.EngineOffsetY = engineOffsetY;

                entry.SortingLayer = (SortingLayer)Enum.Parse(typeof(SortingLayer), readerForData[4].ToString(), true);

                var gridOffsetXStr = readerForData[5].ToString();
                int.TryParse(gridOffsetXStr, out int gridOffsetX);
                entry.GridOffsetX = gridOffsetX;

                var gridOffsetYStr = readerForData[6].ToString();
                int.TryParse(gridOffsetYStr, out int gridOffsetY);
                entry.GridOffsetY = gridOffsetY;

                var weightStr = readerForData[7].ToString();
                int.TryParse(weightStr, out int weight);
                entry.Weight = weight;

                var heightStr = readerForData[8].ToString();
                int.TryParse(heightStr, out int height);
                entry.Height = height;

                retVal.Add(entry);
            }
            return retVal;
        }

        private Dictionary<string, ViewTemplate> LoadViewTemplates(SqliteConnection sqlConnection)
        {
            var sql = "select * from Views";
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();

            var aggregate = new Dictionary<string, ViewTemplate>();
            while (reader.Read())
            {
                var viewTemplate = new ViewTemplate();
                viewTemplate.Identifier = reader[0].ToString();
                viewTemplate.ResourceIdentifier = reader[1].ToString();
                viewTemplate.SpineSkinName = reader[2].ToString();

                var scaleStr = reader[3].ToString();
                float.TryParse(scaleStr, out float scale);
                viewTemplate.Scale = scale;

                var shadowScaleStr = reader[4].ToString();
                float.TryParse(shadowScaleStr, out float shadowScale);
                viewTemplate.ShadowScale = shadowScale;

                var sortableWeightStr = reader[5].ToString();
                int.TryParse(sortableWeightStr, out int sortableWeight);
                viewTemplate.SortableWeight = sortableWeight;

                aggregate.Add(viewTemplate.Identifier, viewTemplate);
            }
            return aggregate;
        }

        private Dictionary<string, ProbabilityTable> LoadProbabilityTables(SqliteConnection sqlConnection, string tableTableName, string parcelTableName, string parcelEntriesTableName)
        {
            var sql = string.Format("select * from [" + tableTableName + "]");
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();

            var aggregate = new Dictionary<string, ProbabilityTable>();
            while (reader.Read())
            {
                var probabilityTable = new ProbabilityTable();
                var identifier = reader[0].ToString();
                probabilityTable.AddRange(LoadProbabilityTableParcels(sqlConnection, identifier, parcelTableName, parcelEntriesTableName));
                aggregate.Add(identifier, probabilityTable);
            }
            return aggregate;
        }

        private List<ProbabilityTableParcel> LoadProbabilityTableParcels(SqliteConnection sqlConnection, string tableIdentifier, string parcelTableName, string parcelEntriesTableName)
        {
            var retVal = new List<ProbabilityTableParcel>();
            var sqlForData = string.Format("select * from [" + parcelTableName + "] WHERE [Identifier] = @Identifier");
            var commandForData = new SqliteCommand(sqlForData, sqlConnection);
            commandForData.Parameters.AddWithValue("@Identifier", tableIdentifier);
            var readerForData = commandForData.ExecuteReader();
            while (readerForData.Read())
            {
                var identifier = readerForData[0].ToString();
                var parcelIdStr = readerForData[1].ToString();
                var weightStr = readerForData[2].ToString();

                int.TryParse(parcelIdStr, out int parcelId);
                int.TryParse(weightStr, out int weight);

                var entry = new ProbabilityTableParcel()
                {
                    Weight = weight
                };
                entry.Values.AddRange(LoadProbabilityTableParcelEntries(sqlConnection, parcelEntriesTableName, tableIdentifier, parcelId));
                retVal.Add(entry);

            }
            return retVal;
        }

        private List<string> LoadProbabilityTableParcelEntries(SqliteConnection sqlConnection, string parcelEntriesTableName, string tableIdentifier, int parcelId)
        {
            var retVal = new List<string>();
            var sqlForData = string.Format("select * from [" + parcelEntriesTableName + "] WHERE [Identifier] = @Identifier AND [ParcelId] = @ParcelId");
            var commandForData = new SqliteCommand(sqlForData, sqlConnection);
            commandForData.Parameters.AddWithValue("@Identifier", tableIdentifier);
            commandForData.Parameters.AddWithValue("@ParcelId", parcelId);
            var readerForData = commandForData.ExecuteReader();
            while (readerForData.Read())
            {
                var identifier = readerForData[0].ToString();
                var parcelIdStr = readerForData[1].ToString();
                var value = readerForData[2].ToString();
                retVal.Add(value);

            }
            return retVal;
        }

        private Dictionary<string, EnchantmentTemplate> LoadEnchantmentTemplates(SqliteConnection sqlConnection)
        {
            var sql = "select * from Enchantments";
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();

            var aggregate = new Dictionary<string, EnchantmentTemplate>();
            while (reader.Read())
            {
                var enchantmentTemplate = new EnchantmentTemplate();
                enchantmentTemplate.Identifier = reader[0].ToString();
                enchantmentTemplate.NameModifier = reader[1].ToString();

                var minChargesStr = reader[2].ToString();
                int.TryParse(minChargesStr, out int minCharges);
                enchantmentTemplate.MinCharges = minCharges;

                var maxChargesStr = reader[3].ToString();
                int.TryParse(maxChargesStr, out int maxCharges);
                enchantmentTemplate.MaxCharges = maxCharges;

                Assert.IsNotNull(enchantmentTemplate.NameModifier);
                enchantmentTemplate.CombatActionDescriptor = BuildCombatActionDescriptor(sqlConnection, enchantmentTemplate.Identifier, "Enchantments_CombatActionParameters", "Enchantments_ExplosionParameters", "Enchantments_ActionCosts");

                enchantmentTemplate.WornEffects = ParseListOfStrings(sqlConnection, enchantmentTemplate.Identifier, "Enchantments_WornEffects");
                enchantmentTemplate.AppliedEffects = ParseAppliedEffects(sqlConnection, enchantmentTemplate.Identifier, "Enchantments_AppliedEffects");
                aggregate.Add(enchantmentTemplate.Identifier, enchantmentTemplate);
            }
            return aggregate;
        }

        private Dictionary<CombatActionType, string> ParseAppliedEffects(SqliteConnection sqlConnection, string templateIdentifier, string table)
        {
            var retVal = new Dictionary<CombatActionType, string>();
            var sqlForData = string.Format("select * from [" + table + "] WHERE [Identifier] = @Identifier");
            var commandForData = new SqliteCommand(sqlForData, sqlConnection);
            commandForData.Parameters.AddWithValue("@Identifier", templateIdentifier);
            var readerForData = commandForData.ExecuteReader();
            while (readerForData.Read())
            {
                var identifier = readerForData[0].ToString();
                var interactionType = (CombatActionType)Enum.Parse(typeof(CombatActionType), readerForData[1].ToString(), true);
                var appliedEffect = readerForData[2].ToString();
                if (retVal.ContainsKey(interactionType))
                {
                    Debug.LogWarning("Overwriting applied effects on" + templateIdentifier + " for action type: " + interactionType + "likely a mod has overwritten it.");
                }
                retVal[interactionType] = appliedEffect;
            }
            return retVal;
        }

        private Dictionary<string, CampaignTemplate> LoadCampaignTemplates(SqliteConnection sqlConnection)
        {
            var sql = "select * from Campaigns";
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();

            var aggregate = new Dictionary<string, CampaignTemplate>();
            while (reader.Read())
            {
                var campaignTemplate = new CampaignTemplate();
                campaignTemplate.Identifier = reader[0].ToString();
                campaignTemplate.RulesEngineClassName = reader[1].ToString();
                aggregate.Add(campaignTemplate.Identifier, campaignTemplate);
            }
            return aggregate;
        }

        public bool ParseBoolFromIntString(string s)
        {
            int.TryParse(s, out int parsed);
            return parsed == 1;
        }

        private Dictionary<string, ItemTemplate> LoadItemTemplates(SqliteConnection sqlConnection)
        {
            var sql = "select * from Items";
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();

            var aggregate = new Dictionary<string, ItemTemplate>();
            while (reader.Read())
            {
                var itemTemplate = new ItemTemplate();
                itemTemplate.Identifier = reader[0].ToString();
                itemTemplate.LocalizationPrefix = reader[1].ToString();

                int.TryParse(reader[2].ToString(), out int minStackSize);
                itemTemplate.MinStackSize = minStackSize;

                int.TryParse(reader[3].ToString(), out int maxStackSize);
                itemTemplate.MaxStackSize = maxStackSize;

                itemTemplate.DestroyOnUse = ParseBoolFromIntString(reader[4].ToString());

                int.TryParse(reader[5].ToString(), out int chanceToSurviveLaunch);
                itemTemplate.ChanceToSurviveLaunch = chanceToSurviveLaunch;

                var ammoType = (AmmoType)Enum.Parse(typeof(AmmoType), reader[6].ToString(), true);
                itemTemplate.AmmoType = ammoType;


                var probabilityTableResource = reader[7].ToString();
                if (probabilityTableResource != null && probabilityTableResource != "")
                {
                    if (Contains<EnchantmentTemplate>(probabilityTableResource))
                    {
                        itemTemplate.PossibleEnchantments = ProbabilityTable.ForSingleValue(probabilityTableResource);
                    }
                    else if (Contains<ProbabilityTable>(probabilityTableResource))
                    {
                        itemTemplate.PossibleEnchantments = Load<ProbabilityTable>(probabilityTableResource);
                    }
                    else
                    {
                        Debug.LogError("Item template: " + itemTemplate.Identifier + " references Enchantment or probability table: " + probabilityTableResource + " neither of which exists.");
                    }
                }

                itemTemplate.ItemAppearanceIdentifier = reader[8].ToString();
                if (this.Load<ItemAppearance>(itemTemplate.ItemAppearanceIdentifier) == null)
                {
                    Debug.LogError("Item template: " + itemTemplate.Identifier + " references ItemAppearance: " + itemTemplate.ItemAppearanceIdentifier + " which does not exist.");
                }
                itemTemplate.CombatActionDescriptor = BuildCombatActionDescriptor(sqlConnection, itemTemplate.Identifier, "Items_CombatActionParameters", "Items_ExplosionParameters", "Items_ActionCosts");

                var slotsAsStrings = ParseListOfStrings(sqlConnection, itemTemplate.Identifier, "Items_SlotsOccupiedByWearing");
                itemTemplate.SlotsOccupiedByWearing = ConvertListOfStringsToEnumOfType<ItemSlot>(slotsAsStrings);

                var wearableSlotsAsStrings = ParseListOfStrings(sqlConnection, itemTemplate.Identifier, "Items_SlotsWearable");
                itemTemplate.SlotsWearable = ConvertListOfStringsToEnumOfType<ItemSlot>(wearableSlotsAsStrings);

                itemTemplate.TagsAppliedToEntity = ParseListOfStrings(sqlConnection, itemTemplate.Identifier, "Items_TagsAppliedToEntity");
                itemTemplate.TagsThatDescribeThisItem = ParseListOfStrings(sqlConnection, itemTemplate.Identifier, "Items_TagsThatDescribeThisItem");

                aggregate.Add(itemTemplate.Identifier, itemTemplate);
            }
            return aggregate;
        }

        private Dictionary<CombatActionType, CombatActionDescriptor> BuildCombatActionDescriptor(SqliteConnection con, string identifier, string combatActionTable, string explosionTable, string costTable)
        {
            var ret = new Dictionary<CombatActionType, CombatActionDescriptor>();
            var combatParams = ParseCombatActionParameters(con, identifier, combatActionTable);
            var explosionParams = ParseCombatActionParameters(con, identifier, explosionTable);
            var costParams = ParseCombatActionCost(con, identifier, costTable);

            foreach (var pair in combatParams)
            {
                if (!ret.ContainsKey(pair.Key))
                {
                    ret[pair.Key] = new CombatActionDescriptor();
                }
                ret[pair.Key].CombatActionParameters = pair.Value;
            }

            foreach (var pair in explosionParams)
            {
                if (!ret.ContainsKey(pair.Key))
                {
                    ret[pair.Key] = new CombatActionDescriptor();
                }
                ret[pair.Key].ExplosionParameters = pair.Value;
            }

            foreach (var pair in costParams)
            {
                if (!ret.ContainsKey(pair.Key))
                {
                    ret[pair.Key] = new CombatActionDescriptor();
                }
                ret[pair.Key].Cost = pair.Value;
            }
            return ret;
        }

        private Dictionary<CombatActionType, ActionCost> ParseCombatActionCost(SqliteConnection con, string templateIdentifier, string costTable)
        {
            var sql = string.Format("select * from [" + costTable + "] WHERE [Identifier] = @Identifier");
            var sqlCommand = new SqliteCommand(sql, con);
            sqlCommand.Parameters.AddWithValue("@Identifier", templateIdentifier);
            var reader = sqlCommand.ExecuteReader();

            var aggregate = new Dictionary<CombatActionType, ActionCost>();
            while (reader.Read())
            {
                var cost = new ActionCost();
                var identifier = reader[0].ToString();
                var interactionType = (CombatActionType)Enum.Parse(typeof(CombatActionType), reader[1].ToString(), true);

                var healthStr = reader[2].ToString();
                int.TryParse(healthStr, out var health);
                cost.Health = health;

                Assert.AreNotEqual(CombatActionType.NotSet, interactionType);
                aggregate.Add(interactionType, cost);
            }
            return aggregate;
        }

        private List<T> ConvertListOfStringsToEnumOfType<T>(List<string> input) where T : Enum
        {
            var ret = new List<T>();
            foreach (var val in input)
            {
                ret.Add((T)Enum.Parse(typeof(T), val, true));
            }
            return ret;
        }

        private Dictionary<CombatActionType, CombatActionParameters> ParseCombatActionParameters(SqliteConnection sqlConnection, string templateIdentifier, string table)
        {
            var retVal = new Dictionary<CombatActionType, CombatActionParameters>();
            var sqlForData = string.Format("select * from [" + table + "] WHERE [Identifier] = @Identifier");
            var commandForData = new SqliteCommand(sqlForData, sqlConnection);
            commandForData.Parameters.AddWithValue("@Identifier", templateIdentifier);
            var readerForData = commandForData.ExecuteReader();
            while (readerForData.Read())
            {
                var interactionParameters = new CombatActionParameters();
                var identifier = readerForData[0].ToString();
                var interactionType = (CombatActionType)Enum.Parse(typeof(CombatActionType), readerForData[1].ToString(), true);
                Assert.AreNotEqual(CombatActionType.NotSet, interactionType);
                int.TryParse(readerForData[2].ToString(), out int DyeSize);
                interactionParameters.DyeSize = DyeSize;

                int.TryParse(readerForData[3].ToString(), out int DyeNumber);
                interactionParameters.DyeNumber = DyeNumber;

                var damageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), readerForData[4].ToString(), true);
                Assert.AreNotEqual(DamageTypes.NOT_SET, damageType);
                interactionParameters.DamageType = damageType;
                interactionParameters.AttackMessagePrefix = readerForData[5].ToString();

                int.TryParse(readerForData[6].ToString(), out int Range);
                interactionParameters.Range = Range;

                int.TryParse(readerForData[7].ToString(), out int NumberOfTargetsToPierce);
                interactionParameters.NumberOfTargetsToPierce = NumberOfTargetsToPierce;

                var TargetingType = (CombatActionTargetingType)Enum.Parse(typeof(CombatActionTargetingType), readerForData[8].ToString(), true);
                Assert.AreNotEqual(CombatActionTargetingType.NotSet, TargetingType);

                interactionParameters.TargetingType = TargetingType;

                interactionParameters.ProjectileAppearanceIdentifier = readerForData[9].ToString();

                interactionParameters.InteractionProperties = ParseCommaSeparatedListIntoEnum<InteractionProperties>(readerForData[10].ToString());
                Assert.IsNotNull(interactionParameters.InteractionProperties);


                if (interactionParameters.ProjectileAppearanceIdentifier == "")
                {
                    interactionParameters.ProjectileAppearanceIdentifier = null;
                }
                if (interactionParameters.ProjectileAppearanceIdentifier != null && this.Load<ProjectileAppearanceTemplate>(interactionParameters.ProjectileAppearanceIdentifier) == null)
                {
                    Debug.LogError("Item template: " + identifier + " references ProjectileAppearanceIdentifier: " + interactionParameters.ProjectileAppearanceIdentifier + " which does not exist.");
                }
                if (!retVal.ContainsKey(interactionType))
                {
                    retVal.Add(interactionType, interactionParameters);
                }
                else
                {
                    Debug.LogError("Item template: " + identifier + " has multiple combatActionParams for type: " + interactionType);
                }
            }
            return retVal;
        }

        private static List<string> ParseCommaSeparatedList(string appliedEffectsTemplatesStr)
        {
            var appliedEffectsTemplatesarr = appliedEffectsTemplatesStr.Split(',');
            var listOfAppliedEffectsTrimmedStrs = new List<string>();
            foreach (var st in appliedEffectsTemplatesarr)
            {
                var trimmed = st.Trim();
                if (trimmed != "")
                {
                    listOfAppliedEffectsTrimmedStrs.Add(trimmed);
                }
            }

            return listOfAppliedEffectsTrimmedStrs;
        }

        private static List<T> ParseCommaSeparatedListIntoEnum<T>(string appliedEffectsTemplatesStr) where T : Enum
        {
            var ret = new List<T>();
            if (appliedEffectsTemplatesStr == null || appliedEffectsTemplatesStr == "")
            {
                return ret;
            }
            var strList = ParseCommaSeparatedList(appliedEffectsTemplatesStr);
            foreach (var val in strList)
            {
                ret.Add((T)Enum.Parse(typeof(T), val, true));
            }

            return ret;
        }

        private Dictionary<string, EffectTemplate> LoadEffectTemplates(SqliteConnection sqlConnection)
        {
            var sql = "select * from Effects";
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();

            var aggregate = new Dictionary<string, EffectTemplate>();
            while (reader.Read())
            {
                var effectTemplate = new EffectTemplate();
                effectTemplate.Identifier = reader[0].ToString();
                effectTemplate.LocalizationPrefix = reader[1].ToString();
                effectTemplate.EffectImplClassName = reader[2].ToString();
                effectTemplate.HasUnlimitedDuration = ParseBoolFromIntString(reader[3].ToString());

                int.TryParse(reader[4].ToString(), out int duration);
                effectTemplate.Duration = duration;

                var stackingStrategyString = reader[5].ToString();
                effectTemplate.StackingStrategy = (StackingStrategy)Enum.Parse(typeof(StackingStrategy), stackingStrategyString, true);

                effectTemplate.TemplateData = ParseStringToStringDict(sqlConnection, effectTemplate.Identifier, "Effects_Data");
                effectTemplate.TagsAppliedToEntity = ParseListOfStrings(sqlConnection, effectTemplate.Identifier, "Effects_TagsAppliedToEntity");
                effectTemplate.TagsThatBlockThisEffect = ParseListOfStrings(sqlConnection, effectTemplate.Identifier, "Effects_TagsThatBlockThisEffect");
                effectTemplate.TemplateAttributes = ParseAttributesDictFromTable(sqlConnection, effectTemplate.Identifier, "Effects_Attributes");
                aggregate.Add(effectTemplate.Identifier, effectTemplate);
            }
            return aggregate;
        }


        private Dictionary<string, TriggerTemplate> LoadTriggerTemplates(SqliteConnection sqlConnection)
        {
            var sql = "select * from Triggers";
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();

            var aggregate = new Dictionary<string, TriggerTemplate>();
            while (reader.Read())
            {
                var triggerTemplate = new TriggerTemplate();
                triggerTemplate.Identifier = reader[0].ToString();
                triggerTemplate.TriggerShape = (TriggerShape)Enum.Parse(typeof(TriggerShape), reader[1].ToString(), true);
                triggerTemplate.TriggerMode = (TriggerMode)Enum.Parse(typeof(TriggerMode), reader[2].ToString(), true);
                triggerTemplate.PressInputHint = reader[3].ToString();
                triggerTemplate.TriggerableActionClassName = reader[4].ToString();
                triggerTemplate.CombatActionParameters = ParseSingularCombatParam(sqlConnection, triggerTemplate, "Triggers_CombatActionParameters");
                Assert.AreNotEqual(triggerTemplate.TriggerMode, TriggerMode.NotSet);
                Assert.AreNotEqual(triggerTemplate.TriggerShape, TriggerShape.NotSet);
                aggregate.Add(triggerTemplate.Identifier, triggerTemplate);
            }
            return aggregate;
        }

        private CombatActionParameters ParseSingularCombatParam(SqliteConnection sqlConnection, TriggerTemplate triggerTemplate, string table)
        {
            var combatParams = ParseCombatActionParameters(sqlConnection, triggerTemplate.Identifier, table);
            Assert.IsTrue(combatParams.Count < 2);
            return combatParams.Count > 0 ? combatParams[0] : null;
        }

        private Dictionary<Attributes, int> ParseAttributesDictFromTable(SqliteConnection sqlConnection, string templateIdentifier, string table)
        {
            var retVal = new Dictionary<Attributes, int>();
            var sqlForData = string.Format("select * from [" + table + "] WHERE [Identifier] = @Identifier");
            var commandForData = new SqliteCommand(sqlForData, sqlConnection);
            commandForData.Parameters.AddWithValue("@Identifier", templateIdentifier);
            var readerForData = commandForData.ExecuteReader();
            while (readerForData.Read())
            {
                var identifier = readerForData[0].ToString();
                var attribute = (Attributes)Enum.Parse(typeof(Attributes), readerForData[1].ToString(), true);
                int.TryParse(readerForData[2].ToString(), out int value);
                retVal.Add(attribute, value);
            }
            return retVal;
        }

        private List<string> ParseListOfStrings(SqliteConnection sqlConnection, string templateIdentifier, string table)
        {
            var retVal = new List<string>();
            var sqlForData = string.Format("select * from [" + table + "] WHERE [Identifier] = @Identifier");
            var commandForData = new SqliteCommand(sqlForData, sqlConnection);
            commandForData.Parameters.AddWithValue("@Identifier", templateIdentifier);
            var readerForData = commandForData.ExecuteReader();
            while (readerForData.Read())
            {
                var identifier = readerForData[0];
                var tag = readerForData[1].ToString();
                retVal.Add(tag);
            }
            return retVal;
        }

        private Dictionary<string, string> ParseStringToStringDict(SqliteConnection sqlConnection, string templateIdentifier, string table)
        {
            var retVal = new Dictionary<string, string>();
            var sqlForData = string.Format("select * from [" + table + "] WHERE [Identifier] = @Identifier");
            var commandForData = new SqliteCommand(sqlForData, sqlConnection);
            commandForData.Parameters.AddWithValue("@Identifier", templateIdentifier);
            var readerForData = commandForData.ExecuteReader();
            while (readerForData.Read())
            {
                var identifier = readerForData[0];
                var key = readerForData[1].ToString();
                var value = readerForData[2].ToString();
                retVal.Add(key, value);
            }
            return retVal;
        }

        public object CreateInstanceFromAbstractOrInterfaceTypeAndName(Type abs, string name)
        {
            var typeDict = Load<Dictionary<string, Type>>(abs.FullName);
            if (!typeDict.ContainsKey(name))
            {
                Debug.Log("Could not instantiate " + abs + " :" + name);
            }
            var instance = Activator.CreateInstance(typeDict[name]);
            return instance;
        }

        private Dictionary<string, Dictionary<string, Type>> LoadEffectImpls()
        {
            return BuildTypeDictFromAbstract(typeof(EffectImpl));
        }

        private Dictionary<string, Dictionary<string, Type>> LoadRulesEngines()
        {
            return BuildTypeDictFromInterface(typeof(IRulesEngine));
        }

        private Dictionary<string, Dictionary<string, Type>> LoadTriggerableActionImpls()
        {
            return BuildTypeDictFromInterface(typeof(ITriggerableActionImpl));
        }

        private Dictionary<string, Dictionary<string, Type>> BuildTypeDictFromAbstract(Type abstractLoadableType)
        {
            var effectDefTypes = abstractLoadableType.ConcreteFromAbstract();
            return BuildDictForTypeLookup(abstractLoadableType, effectDefTypes);
        }

        private static Dictionary<string, Dictionary<string, Type>> BuildDictForTypeLookup(Type abstractLoadableType, Type[] effectDefTypes)
        {
            var effectDefTypesByName = new Dictionary<string, Type>();
            foreach (var typ in effectDefTypes)
            {
                effectDefTypesByName.Add(typ.FullName, typ);
            }

            var typeNameWrapper = new Dictionary<string, Dictionary<string, Type>>()
            {
                {abstractLoadableType.FullName,  effectDefTypesByName}
            };

            return typeNameWrapper;
        }

        private Dictionary<string, Dictionary<string, Type>> BuildTypeDictFromInterface(Type interfaceType)
        {
            var effectDefTypes = interfaceType.ConcreteFromInterface();
            return BuildDictForTypeLookup(interfaceType, effectDefTypes);
        }

        private Dictionary<string, ItemAppearance> LoadItemAppearances(SqliteConnection sqlConnection)
        {
            var sql = "select * from ItemAppearances";
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();
            var aggregate = new Dictionary<string, ItemAppearance>();
            while (reader.Read())
            {
                var itemAppearance = new ItemAppearance();
                itemAppearance.Identifier = reader[0].ToString();
                itemAppearance.InventorySprite = LoadUnityResource<Sprite>(reader[1].ToString());
                itemAppearance.WornItemSpritePerSlot = new Dictionary<SpriteAttachment, Sprite>();
                aggregate.Add(itemAppearance.Identifier, itemAppearance);
            }

            sql = "select * from ItemAppearances_SpriteAttachments";
            sqlCommand = new SqliteCommand(sql, sqlConnection);
            reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var identifier = reader[0].ToString();
                var spriteAttachmentString = reader[1].ToString();
                var spriteAttachment = (SpriteAttachment)Enum.Parse(typeof(SpriteAttachment), spriteAttachmentString, true);
                var spriteStr = reader[2].ToString();
                var sprite = LoadUnityResource<Sprite>(spriteStr);
                if (sprite == null)
                {
                    Debug.LogWarning("Failed to resolve sprite for " + spriteStr + " for itemAppearance: " + identifier);
                }
                if (!aggregate.ContainsKey(identifier))
                {
                    Debug.LogWarning("Trying to attach sprites to ProjectileAppearance" + identifier + " but it doesn't exist");
                }
                else
                {
                    var appearance = aggregate[identifier];
                    appearance.WornItemSpritePerSlot.Add(spriteAttachment, sprite);
                }
            }
            return aggregate;
        }

        private Dictionary<string, TilesetTemplate> LoadTilesets(SqliteConnection sqlConnection)
        {
            var sql = "select * from Tilesets";
            var sqlCommand = new SqliteCommand(sql, sqlConnection);
            var reader = sqlCommand.ExecuteReader();
            var aggregate = new Dictionary<string, TilesetTemplate>();
            while (reader.Read())
            {
                var tileset = new TilesetTemplate();
                tileset.Identifier = reader[0].ToString();
                tileset.FloorSprite = LoadUnityResource<Sprite>(reader[1].ToString());
                tileset.TeeSprite = LoadUnityResource<Sprite>(reader[2].ToString());
                tileset.NorthCornerSprite = LoadUnityResource<Sprite>(reader[3].ToString());
                tileset.EastCornerSprite = LoadUnityResource<Sprite>(reader[4].ToString());
                tileset.SouthCornerSprite = LoadUnityResource<Sprite>(reader[5].ToString());
                tileset.WestCornerSprite = LoadUnityResource<Sprite>(reader[6].ToString());
                tileset.NorthEastWallSprite = LoadUnityResource<Sprite>(reader[7].ToString());
                tileset.SouthEastWallSprite = LoadUnityResource<Sprite>(reader[8].ToString());
                tileset.SouthWestWallSprite = LoadUnityResource<Sprite>(reader[9].ToString());
                tileset.NorthWestWallSprite = LoadUnityResource<Sprite>(reader[10].ToString());
                tileset.NorthEastTeeSprite = LoadUnityResource<Sprite>(reader[11].ToString());
                tileset.SouthEastTeeSprite = LoadUnityResource<Sprite>(reader[12].ToString());
                tileset.SouthWestTeeSprite = LoadUnityResource<Sprite>(reader[13].ToString());
                tileset.NorthWestTeeSprite = LoadUnityResource<Sprite>(reader[14].ToString());
                aggregate.Add(tileset.Identifier, tileset);
            }
            return aggregate;
        }

        private Dictionary<string, TLoaded> LoadUnityResource<TLoaded>() where TLoaded : UnityEngine.Object
        {
            var aggregate = new Dictionary<string, TLoaded>();
            foreach (var mod in Context.ModManager.Mods)
            {
                foreach (var assetBundle in mod.AssetBundles)
                {
                    var assets = assetBundle.LoadAllAssets<TLoaded>();
                    foreach (var asset in assets)
                    {
                        aggregate.Add(asset.name, asset);
                    }
                }
            }
            return aggregate;
        }

        private void CacheResources<TResource>(Dictionary<string, TResource> pairs)
        {
            foreach (var pair in pairs)
            {
                CacheResource(pair.Key, pair.Value);
            }
        }

        private void CacheResource<TResource>(string key, TResource value)
        {
            Debug.Log("Found " + value.GetType().Name.ToString() + ": " + key);
            if (_prototypesByUniqueIdentifier.ContainsKey(key))
            {
                Debug.Log(string.Format("Replaced existing definition for: {0}.", key));
            }
            _prototypesByUniqueIdentifier[key] = value;
        }
    }
}

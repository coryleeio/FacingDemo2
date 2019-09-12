using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using YamlDotNet.Serialization;
using static Gamepackage.Localizer;

namespace Gamepackage
{
    public class ModManager
    {
        public Dictionary<string, Mod> ModsByName = new Dictionary<string, Mod>();
        public List<Mod> Mods = new List<Mod>();
        public Dictionary<string, Assembly> AssembliesByName = new Dictionary<string, Assembly>();
        public Dictionary<string, AssetBundle> AssetBundlesByName = new Dictionary<string, AssetBundle>();
        public bool NeedsToReloadResources = true;

        public void LoadModsAndResources()
        {
            if (NeedsToReloadResources)
            {
                Mods.Clear();
                ModsByName.Clear();
                AssetBundle.UnloadAllAssetBundles(true);
                string cs = "Data Source=:memory:";
                var sqlConnection = new SqliteConnection(cs);

                try
                {
                    sqlConnection.Open();
                    var modsDirectory = System.IO.Path.Combine(Application.streamingAssetsPath, "Mods");
                    if (Directory.Exists(modsDirectory))
                    {
                        var potentialMods = Directory.GetDirectories(modsDirectory, "*", SearchOption.TopDirectoryOnly);
                        foreach (var potentialMod in potentialMods)
                        {
                            Debug.Log("Discovered mod at " + potentialMod);
                            var mod = new Mod
                            {
                                ModFolderPath = potentialMod
                            };
                            if (mod.CanBeLoaded)
                            {
                                try
                                {
                                    LoadMod(mod, sqlConnection);
                                    Debug.Log("Successfully loaded: " + mod.Name + " | v" + mod.Version);
                                    ModsByName.Add(mod.Name, mod);
                                    Mods.Add(mod);
                                }
                                catch (Exception ex)
                                {
                                    Debug.LogWarning("Failed to load mod because an exception was thrown: " + ex.GetType());
                                    Debug.LogWarning(ex.Message);
                                }
                            }
                            else
                            {
                                Debug.Log("Couldn't load it though... It needs an About directory with About.yml inside.");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No mods loaded because " + modsDirectory + " did not exist");
                    }
                    Context.ResourceManager.ResolveAllResources(sqlConnection);
                }
                finally
                {
                    sqlConnection.Close();
                    NeedsToReloadResources = false;
                }
            }
        }

        private void LoadMod(Mod mod, SqliteConnection sqlConnection)
        {
            var deserializer = new Deserializer();
            var modDict = deserializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(mod.AboutYamlPath));
            mod.Name = modDict["Name"];
            mod.Description = modDict["Description"];
            mod.Author = modDict["Author"];
            mod.Name = modDict["Name"];
            mod.Version = modDict["Version"];
            mod.Assemblies = new List<Assembly>();
            mod.AssetBundles = new List<AssetBundle>();
            Debug.Log("Attempting to load: " + mod.Name + " | v" + mod.Version);
            LoadAssemblies(mod);
            LoadAssetBundles(mod);
            LoadLocalizations(mod);
            LoadTemplates(mod, sqlConnection);
        }

        private void LoadAssetBundles(Mod mod)
        {
            if (mod.AssetBundlesDirectoryExists)
            {
                var assetBundleFileNames = Directory.GetFiles(mod.AssetBundlesDirectoryPath, "*", SearchOption.TopDirectoryOnly);
                foreach (var assetBundleFileName in assetBundleFileNames)
                {
                    if (!assetBundleFileName.EndsWith(".meta"))
                    {
                        Debug.Log("Attempting to load asset bundle at: " + assetBundleFileName);
                        var assetBundle = AssetBundle.LoadFromFile(assetBundleFileName);
                        assetBundle.LoadAllAssets();
                        mod.AssetBundles.Add(assetBundle);
                        AssetBundlesByName.Add(assetBundle.name, assetBundle);
                        Debug.Log("Finished loading asset bundle at: " + assetBundleFileName);
                    }
                }
            }
        }

        private void LoadLocalizations(Mod mod)
        {
            if (mod.LocalizationsDirectoryExists)
            {
                var localizationFileNames = Directory.GetFiles(mod.LocalizationsDirectoryPath, "*", SearchOption.TopDirectoryOnly);
                var validLanguages = Enum.GetValues(typeof(LocalizerLanguages));
                List<string> validFileNames = GetValidLocalizationFileNames(mod.LocalizationsDirectoryPath);
                foreach (var localizationFileName in localizationFileNames)
                {
                    var languages = Enum.GetValues(typeof(LocalizerLanguages));
                    var found = false;

                    for(int i = 0; i < languages.Length; i++)
                    {
                        var language = (LocalizerLanguages) languages.GetValue(i);
                        if(localizationFileName.EndsWith(language.ToString() + ".yaml"))
                        {
                            found = true;
                            Debug.Log("Attempting to load localization file at: " + localizationFileName);
                            var localizationFileContents = File.ReadAllText(localizationFileName);
                            var deserializer = new Deserializer();
                            var deserializedLocalizationFileContents = deserializer.Deserialize<Dictionary<string, string>>(localizationFileContents);
                            Context.Localizer.LoadDictForLanguage(language, deserializedLocalizationFileContents);
                            Debug.Log("Finished running: " + localizationFileName);
                            break;
                        }
                    }

                    if(!found)
                    {
                        Debug.Log("Skipping " + localizationFileName + "Invalid localization file name");
                        foreach (var val in validFileNames)
                        {
                            Debug.Log(val);
                        }
                    }
                }
            }
        }

        private static LocalizerLanguages GetLocalizationFileLanguageByName(string fileName)
        {
            var language = (LocalizerLanguages)Enum.Parse(typeof(LocalizerLanguages), fileName, true);

            if (language == LocalizerLanguages.NotSet)
            {
                throw new NotImplementedException();
            }
            return language;
        }

        private static List<string> GetValidLocalizationFileNames(string prefix)
        {
            var validLanguages = Enum.GetValues(typeof(LocalizerLanguages));
            var validFileNames = new List<string>();

            foreach (var validLanguageUncast in validLanguages)
            {
                var validLanguage = (LocalizerLanguages)validLanguageUncast;
                validFileNames.Add(prefix + "/" + validLanguage.ToString() + ".yml");
                validFileNames.Add(prefix + "/" + validLanguage.ToString() + ".yaml");
            }

            return validFileNames;
        }

        private void LoadTemplates(Mod mod, SqliteConnection sqlConnection)
        {
            if (mod.TemplatesDirectoryExists)
            {
                var sqlFilenames = Directory.GetFiles(mod.TemplatesDirectoryPath, "*", SearchOption.TopDirectoryOnly);
                foreach (var sqlFilename in sqlFilenames)
                {
                    if (!sqlFilename.EndsWith(".meta"))
                    {
                        Debug.Log("Attempting to run sql found at: " + sqlFilename);
                        var sql = File.ReadAllText(sqlFilename);
                        var command = new SqliteCommand(sql, sqlConnection);
                        command.ExecuteNonQuery();
                        Debug.Log("Finished running: " + sqlFilename);
                    }
                }
            }
        }

        private void LoadAssemblies(Mod mod)
        {
            if (mod.AssembliesDirectoryExists)
            {
                var assemblies = Directory.GetFiles(mod.AssembliesDirectoryPath, "*.dll", SearchOption.TopDirectoryOnly);
                foreach (var assemblyPath in assemblies)
                {
                    if (!assemblyPath.EndsWith(".meta"))
                    {
                        if (!AssembliesByName.ContainsKey(assemblyPath))
                        {
                            Debug.Log("Attempting to load assembly at: " + assemblyPath);
                            var assembly = Assembly.LoadFile(assemblyPath);
                            foreach (var thing in assembly.ExportedTypes)
                            {
                                Debug.Log("Class found: " + thing);
                            }
                            AssembliesByName.Add(assemblyPath, assembly);
                            mod.Assemblies.Add(assembly);
                            Debug.Log("Loaded assembly at: " + assemblyPath);
                        }
                        else
                        {
                            Debug.Log("Did not reload this assembly: " + assemblyPath + " because it is already loaded. If it has been updated you will need to restart the game.");
                            mod.Assemblies.Add(AssembliesByName[assemblyPath]);
                        }
                    }
                }
            }
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Gamepackage
{
    public class ModSystem : IModSystem
    {
        private List<Mod> ModList = new List<Mod>(0);
        private static string ModsDirectory = Application.streamingAssetsPath + "/Mods";
        private Dictionary<string, AssetBundle> LoadedBundles = new Dictionary<string, AssetBundle>();
        private Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();
        private ILogSystem _logSystem;
        private bool Populated = false;

        public ModSystem(ILogSystem logSystem)
        {
            _logSystem = logSystem;
        }

        public void PopulateModList()
        {
            if (!Populated)
            {
                _logSystem.Log("Locating all mods in the mods directory: " + ModsDirectory);
                var modDirectories = Directory.GetDirectories(ModsDirectory, "*", SearchOption.TopDirectoryOnly);
                foreach (var modDirectory in modDirectories)
                {
                    var modFolderName = modDirectory.Substring(ModsDirectory.Length + 1);
                    Mod mod = CreateFromFolder(modFolderName);
                    if (mod != null)
                    {
                        _logSystem.Log("Found mod in folder: " + modFolderName);
                        ModList.Add(mod);
                    }
                }
                ModList.Sort(delegate (Mod c1, Mod c2) { return c1.LoadOrder.CompareTo(c2.LoadOrder); });
                Populated = true;
            }
        }

        public void LoadSqlFiles(IDbConnection dbConnection)
        {
            foreach (var mod in ModList)
            {
                _logSystem.Log("Loading SQL for " + mod.FolderShortName);
                foreach (var sqlFile in mod.SqlFiles)
                {
                    var fullPath = GetLongFilepath(ModsDirectory, mod.FolderShortName, sqlFile);
                    _logSystem.Log("Executing SQL: " + fullPath);
                    LoadSQL(fullPath, dbConnection);
                }
            }
        }

        public void LoadAssemblies()
        {
            foreach (var mod in ModList)
            {
                if (!mod.HasLoadedAssemblies)
                {
                    _logSystem.Log("Loading Assemblies for " + mod.FolderShortName);
                    foreach (var assembly in mod.Assemblies)
                    {
                        var fullPath = GetLongFilepath(ModsDirectory, mod.FolderShortName, assembly);
                        _logSystem.Log("Loading Assemblies: " + fullPath);
                        LoadAssembly(fullPath);
                    }
                    mod.HasLoadedAssemblies = true;
                }
            }
        }

        public void LoadAssetBundles()
        {
            foreach (var mod in ModList)
            {
                if (!mod.HasLoadedAssemblies)
                {
                    _logSystem.Log("Loading AssetBundles for " + mod.FolderShortName);
                    foreach (var bundle in mod.AssetBundles)
                    {
                        var fullPath = GetLongFilepath(ModsDirectory, mod.FolderShortName, bundle);
                        _logSystem.Log("Loading Asset Bundle: " + fullPath);
                        LoadAssetBundle(fullPath);
                    }
                    mod.HasLoadedAssetBundles = true;
                }
            }
        }

        private Mod CreateFromFolder(string modFolderName)
        {
            var metadataFullPath = GetLongFilepath(ModsDirectory, modFolderName, "metadata.json");
            if (!File.Exists(metadataFullPath))
            {
                _logSystem.Warn("Was not able to load modFolderName");
                return null;
            }

            var mod = JsonConvert.DeserializeObject<Mod>(File.ReadAllText(metadataFullPath));
            mod.FolderShortName = modFolderName;
            return mod;
        }

        private static string GetLongFilepath(string modsDirectory, string modFolderName, string filename)
        {
            return modsDirectory + "/" + modFolderName + "/" + filename;
        }

        private void LoadAssetBundle(string fullPath)
        {
            if (LoadedBundles.ContainsKey(fullPath))
            {
                throw new DuplicateBundleNameException("You bundles cannot have the same filename as another mod.");
            }
            else
            {
                var bundle = AssetBundle.LoadFromFile(fullPath);
                LoadedBundles[fullPath] = bundle;
            }
        }

        private void LoadAssembly(string fullPath)
        {
            if (LoadedAssemblies.ContainsKey(fullPath))
            {
                throw new DuplicateAssembliesException("Make sure all your assemblies have different names");
            }
            else
            {
                var assembly = Assembly.LoadFrom(fullPath);
                LoadedAssemblies[fullPath] = assembly;
            }
        }

        private void LoadSQL(string fullPath, IDbConnection dbConnection)
        {
            var dbcmd = dbConnection.CreateCommand();
            dbcmd.CommandText = File.ReadAllText(fullPath);
            dbcmd.ExecuteNonQuery();
            dbcmd.Dispose();
        }
    }
}

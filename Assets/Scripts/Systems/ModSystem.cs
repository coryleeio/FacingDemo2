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
        private List<Mod> _modList = new List<Mod>(0);
        private static string _modsDirectory = Application.streamingAssetsPath + "/Mods";
        private Dictionary<string, AssetBundle> _loadedBundles = new Dictionary<string, AssetBundle>();
        private Dictionary<string, Assembly> _loadedAssemblies = new Dictionary<string, Assembly>();

        private bool Populated = false;
        public ILogSystem LogSystem { get; set; }

        public ModSystem()
        {
        }

        public void PopulateModList()
        {
            if (!Populated)
            {
                LogSystem.Log("Locating all mods in the mods directory: " + _modsDirectory);
                var modDirectories = Directory.GetDirectories(_modsDirectory, "*", SearchOption.TopDirectoryOnly);
                foreach (var modDirectory in modDirectories)
                {
                    var modFolderName = modDirectory.Substring(_modsDirectory.Length + 1);
                    Mod mod = CreateFromFolder(modFolderName);
                    if (mod != null)
                    {
                        LogSystem.Log("Found mod in folder: " + modFolderName);
                        _modList.Add(mod);
                    }
                }
                _modList.Sort(delegate (Mod c1, Mod c2) { return c1.LoadOrder.CompareTo(c2.LoadOrder); });
                Populated = true;
            }
        }

        public void LoadSqlFiles(IDbConnection dbConnection)
        {
            foreach (var mod in _modList)
            {
                LogSystem.Log("Loading SQL for " + mod.FolderShortName);
                foreach (var sqlFile in mod.SqlFiles)
                {
                    var fullPath = GetLongFilepath(_modsDirectory, mod.FolderShortName, sqlFile);
                    LogSystem.Log("Executing SQL: " + fullPath);
                    LoadSQL(fullPath, dbConnection);
                }
            }
        }

        public void LoadAssemblies()
        {
            foreach (var mod in _modList)
            {
                if (!mod.HasLoadedAssemblies)
                {
                    LogSystem.Log("Loading Assemblies for " + mod.FolderShortName);
                    foreach (var assembly in mod.Assemblies)
                    {
                        var fullPath = GetLongFilepath(_modsDirectory, mod.FolderShortName, assembly);
                        LogSystem.Log("Loading Assemblies: " + fullPath);
                        LoadAssembly(fullPath);
                    }
                    mod.HasLoadedAssemblies = true;
                }
            }
        }

        public void LoadAssetBundles()
        {
            foreach (var mod in _modList)
            {
                if (!mod.HasLoadedAssemblies)
                {
                    LogSystem.Log("Loading AssetBundles for " + mod.FolderShortName);
                    foreach (var bundle in mod.AssetBundles)
                    {
                        var fullPath = GetLongFilepath(_modsDirectory, mod.FolderShortName, bundle);
                        LogSystem.Log("Loading Asset Bundle: " + fullPath);
                        LoadAssetBundle(fullPath);
                    }
                    mod.HasLoadedAssetBundles = true;
                }
            }
        }

        private Mod CreateFromFolder(string modFolderName)
        {
            var metadataFullPath = GetLongFilepath(_modsDirectory, modFolderName, "metadata.json");
            if (!File.Exists(metadataFullPath))
            {
                LogSystem.Warn("Was not able to load modFolderName");
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
            if (_loadedBundles.ContainsKey(fullPath))
            {
                throw new DuplicateBundleNameException("You bundles cannot have the same filename as another mod.");
            }
            else
            {
                var bundle = AssetBundle.LoadFromFile(fullPath);
                _loadedBundles[fullPath] = bundle;
            }
        }

        private void LoadAssembly(string fullPath)
        {
            if (_loadedAssemblies.ContainsKey(fullPath))
            {
                throw new DuplicateAssembliesException("Make sure all your assemblies have different names");
            }
            else
            {
                var assembly = Assembly.LoadFrom(fullPath);
                _loadedAssemblies[fullPath] = assembly;
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

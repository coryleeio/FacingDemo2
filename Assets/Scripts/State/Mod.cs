using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Gamepackage
{

    public class Mod
    {
        public string Name;
        public string Creator;
        public string ContactInfo;
        public int LoadOrder;
        public bool Active;
        public List<string> AssetBundles = new List<string>(0);
        public List<string> Assemblies = new List<string>(0);
        public List<string> SqlFiles = new List<string>(0);

        [JsonIgnore]
        public bool HasLoadedAssetBundles = false;
        [JsonIgnore]
        public bool HasLoadedAssemblies = false;

        [JsonIgnore]
        public string FolderShortName;

        [JsonIgnore]
        private AssetBundle _bundle;

        [JsonIgnore]
        private Assembly _assembly;
    }
}
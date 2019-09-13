using YamlDotNet.Serialization;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace Gamepackage
{
    public class Mod
    {
        public string Name;
        public string Description;
        public string Author;
        public string Version;
        public List<Assembly> Assemblies;
        public List<AssetBundle> AssetBundles;

        public bool CanBeLoaded
        {
            get
            {
                return ModFolderDirectoryExists && AboutYamlExists;
            }
        }

        public string ModFolderPath
        {
            get; set;
        }

        public bool ModFolderDirectoryExists
        {
            get
            {
                return Directory.Exists(ModFolderPath);
            }
        }

        public string AboutYamlPath
        {
            get
            {
                return System.IO.Path.Combine(AboutDirectoryPath, "About.yml");
            }
        }

        public bool AboutYamlExists
        {
            get
            {
                return AboutDirectoryExists && File.Exists(AboutYamlPath);
            }
        }

        public string AboutDirectoryPath
        {
            get
            {
                return System.IO.Path.Combine(ModFolderPath, "About");
            }
        }

        public bool AboutDirectoryExists
        {
            get
            {
                return Directory.Exists(AboutDirectoryPath);
            }
        }

        public string DialogDirectoryPath
        {
            get
            {
                return System.IO.Path.Combine(ModFolderPath, "Dialogs");
            }
        }

        public bool DialogDirectoryExists
        {
            get
            {
                return Directory.Exists(DialogDirectoryPath);
            }
        }

        public string AssembliesDirectoryPath
        {
            get
            {
                return System.IO.Path.Combine(ModFolderPath, "Assemblies");
            }
        }

        public bool AssembliesDirectoryExists
        {
            get
            {
                return Directory.Exists(AssembliesDirectoryPath);
            }
        }

        public string LocalizationsDirectoryPath
        {
            get
            {
                return System.IO.Path.Combine(ModFolderPath, "Localizations");
            }
        }

        public bool LocalizationsDirectoryExists
        {
            get
            {
                return Directory.Exists(LocalizationsDirectoryPath);
            }
        }

        public string AssetBundlesDirectoryPath
        {
            get
            {
                return System.IO.Path.Combine(ModFolderPath, "Asset Bundles");
            }
        }

        public bool AssetBundlesDirectoryExists
        {
            get
            {
                return Directory.Exists(AssetBundlesDirectoryPath);
            }
        }

        public string TemplatesDirectoryPath
        {
            get
            {
                return System.IO.Path.Combine(ModFolderPath, "Templates");
            }
        }

        public bool TemplatesDirectoryExists
        {
            get
            {
                return Directory.Exists(TemplatesDirectoryPath);
            }
        }

        public string SourceDirectoryPath
        {
            get
            {
                return System.IO.Path.Combine(ModFolderPath, "Source");
            }
        }

        public bool SourceDirectoryExists
        {
            get
            {
                return Directory.Exists(SourceDirectoryPath);
            }
        }
    }
}

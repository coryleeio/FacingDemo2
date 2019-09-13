namespace Gamepackage
{
    using UnityEditor;
    using System.IO;

    public class ExportCoreAssetBundle
    {
        [MenuItem("Tools/Export Core Asset Bundle")]
        static void BuildAssetBundles()
        {
            string assetBundleDirectory = "Assets/StreamingAssets/Mods/Core/Asset Bundles";
            var coreFile = System.IO.Path.Combine(assetBundleDirectory, "Core");
            if (File.Exists(coreFile))
            {
                File.Delete(coreFile);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

            var assetBundleFile = System.IO.Path.Combine(assetBundleDirectory, "Asset Bundles");
            var assetBundleFileManifest = System.IO.Path.Combine(assetBundleDirectory, "Asset Bundles.manifest");
            if (File.Exists(assetBundleFile))
            {
                File.Delete(assetBundleFile);
            }
            if (File.Exists(assetBundleFileManifest))
            {
                File.Delete(assetBundleFileManifest);
            }

            var coreMetaFile = System.IO.Path.Combine(assetBundleDirectory, "core.manifest");
            if (File.Exists(coreMetaFile))
            {
                File.Delete(coreMetaFile);
            }

            var uncapitalizedCore = System.IO.Path.Combine(assetBundleDirectory, "core");
            if(File.Exists(coreFile))
            {
                File.Move(uncapitalizedCore, coreFile);
            }
        }
    }
}

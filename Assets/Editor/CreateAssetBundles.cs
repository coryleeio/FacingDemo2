using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/Mods/Core/", BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}
using System.Diagnostics;
using System.IO;
using UnityEditor;
class Builder
{
    [MenuItem("Tools/Build All Platforms")]
    public static void BuildGame()
    {
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/root.unity" };

        var win32DirectoryPath = EnsureDirectoryExists(path + "/Win32");
        var win64DirectoryPath = EnsureDirectoryExists(path + "/Win64");
        var linux32DirectoryPath = EnsureDirectoryExists(path + "/Linux32");
        var linux64DirectoryPath = EnsureDirectoryExists(path + "/Linux64");
        var osxDirectoryPath = EnsureDirectoryExists(path + "/OSX");

        BuildPipeline.BuildPlayer(levels, win32DirectoryPath + "/Game.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
        BuildPipeline.BuildPlayer(levels, win64DirectoryPath + "/Game.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
        BuildPipeline.BuildPlayer(levels, linux32DirectoryPath + "/Game.exe", BuildTarget.StandaloneLinux, BuildOptions.None);
        BuildPipeline.BuildPlayer(levels, linux64DirectoryPath + "/Game.exe", BuildTarget.StandaloneLinux, BuildOptions.None);
        BuildPipeline.BuildPlayer(levels, osxDirectoryPath + "/Game.exe", BuildTarget.StandaloneOSX, BuildOptions.None);
    }

    private static string EnsureDirectoryExists(string path)
    {
        if(!Directory.Exists(path))
        {
            return Directory.CreateDirectory(path).FullName;
        }
        return path;
    }
}
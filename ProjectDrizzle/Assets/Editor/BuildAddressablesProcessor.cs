using System;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

/// <summary>
/// The script gives you choice to whether to build addressable bundles when clicking the build button.
/// For custom build script, call PreExport method yourself.
/// For cloud build, put BuildAddressablesProcessor.PreExport as PreExport command.
/// Discussion: https://forum.unity.com/threads/how-to-trigger-build-player-content-when-build-unity-project.689602/
/// </summary>
class BuildAddressablesProcessor
{
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
    }

    private static void BuildPlayer()
    {
        PreExport();
        string[] scenes = { "Assets/Scenes/MainScene.unity" };
        var commandLineArgs = System.Environment.GetCommandLineArgs();
        Console.WriteLine(commandLineArgs.Aggregate("", (s, s1) => s + ", " + s1));
        Debug.Log(commandLineArgs.Aggregate("", (s, s1) => s + ", " + s1));
        //BuildPipeline.BuildPlayer(scenes, );
    }

    private static void BuildPlayerHandler(BuildPlayerOptions options)
    {
        PreExport();
        BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
    }
    
    /// <summary>
    /// Run a clean build before export.
    /// </summary>
    public static void PreExport()
    {
        AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
        AddressableAssetSettings.BuildPlayerContent();
    }
}
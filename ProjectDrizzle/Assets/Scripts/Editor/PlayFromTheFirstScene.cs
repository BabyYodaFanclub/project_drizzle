using System;
using System.Linq;
using Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

/// <summary>
/// Source: http://answers.unity.com/answers/1248399/view.html
/// </summary>
public static class PlayFromTheFirstScene
{
    private const string PlayFromFirstMenuStr = "Edit/Always Start from the Main Scene &p";
    private const string TeleportPlayerMenuStr = "Edit/Teleport Player to started Chunk &t";

    private static bool PlayFromFirstScene
    {
        get => EditorPrefs.HasKey(PlayFromFirstMenuStr) && EditorPrefs.GetBool(PlayFromFirstMenuStr);
        set => EditorPrefs.SetBool(PlayFromFirstMenuStr, value);
    }

    private static bool TeleportPlayerToCorrectChunk
    {
        get => EditorPrefs.HasKey(TeleportPlayerMenuStr) && EditorPrefs.GetBool(TeleportPlayerMenuStr);
        set => EditorPrefs.SetBool(TeleportPlayerMenuStr, value);
    }

    [MenuItem(PlayFromFirstMenuStr, false, 150)]
    private static void PlayFromFirstSceneCheckMenu()
    {
        PlayFromFirstScene = !PlayFromFirstScene;
        Menu.SetChecked(PlayFromFirstMenuStr, PlayFromFirstScene);

        EditorLogger.NotifyAndLog(PlayFromFirstScene ? "Play from scene 0" : "Play from current scene");
    }

    // The menu won't be gray out, we use this validate method for update check state
    [MenuItem(PlayFromFirstMenuStr, true)]
    private static bool PlayFromFirstSceneCheckMenuValidate()
    {
        Menu.SetChecked(PlayFromFirstMenuStr, PlayFromFirstScene);
        return true;
    }

    [MenuItem(TeleportPlayerMenuStr, false, 151)]
    private static void TeleportPlayerCheckMenu()
    {
        TeleportPlayerToCorrectChunk = !TeleportPlayerToCorrectChunk;
        Menu.SetChecked(TeleportPlayerMenuStr, TeleportPlayerToCorrectChunk);

        EditorLogger.NotifyAndLog(TeleportPlayerToCorrectChunk ? "Teleport player to the started chunk" : "Do not teleport player");
    }

    // The menu won't be gray out, we use this validate method for update check state
    [MenuItem(TeleportPlayerMenuStr, true)]
    private static bool TeleportPlayerCheckMenuValidate()
    {
        Menu.SetChecked(TeleportPlayerMenuStr, TeleportPlayerToCorrectChunk);
        return true;
    }

    // This method is called before any Awake. It's the perfect callback for this feature
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadFirstSceneAtGameBegins()
    {
        if (!PlayFromFirstScene)
            return;

        if (EditorBuildSettings.scenes.Length == 0)
        {
            Debug.LogWarning("The scene build list is empty. Can't play from first scene.");
            return;
        }

        var startedSceneName = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadSceneAsync(0).completed += obj => OnSceneLoaded(obj, startedSceneName);
        
    }

    private static void OnSceneLoaded(AsyncOperation obj, string startedSceneName)
    {
        if (!TeleportPlayerToCorrectChunk || SceneManager.GetActiveScene().name.Equals(startedSceneName, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }
        
        var chunks = Object.FindObjectsOfType<Chunk>().ToList();
        var targetChunk = chunks
            .FirstOrDefault(s => s.ChunkName.Equals(startedSceneName, StringComparison.InvariantCultureIgnoreCase));

        if (targetChunk == null)
        {
            Debug.LogWarning($"The started Scene {startedSceneName} does not exist as a chunk in the main scene. Cannot place player in the correct chunk.");
            return;
        }
        
        var player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = targetChunk.transform.position;
    }
}
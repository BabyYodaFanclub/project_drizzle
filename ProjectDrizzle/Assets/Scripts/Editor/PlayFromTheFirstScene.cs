using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        EditorLogger.NotifyAndLog(TeleportPlayerToCorrectChunk
            ? "Teleport player to the started chunk"
            : "Do not teleport player");
    }

    // The menu won't be gray out, we use this validate method for update check state
    [MenuItem(TeleportPlayerMenuStr, true)]
    private static bool TeleportPlayerCheckMenuValidate()
    {
        Menu.SetChecked(TeleportPlayerMenuStr, TeleportPlayerToCorrectChunk);
        return true;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadMainSceneAtPlay()
    {
        if (!PlayFromFirstScene)
            return;

        if (EditorBuildSettings.scenes.Length == 0)
        {
            EditorLogger.NotifyAndLogWarning("The scene build list is empty. Can't play from first scene.");
            return;
        }

        var openScenes = MyEditorSceneManager.GetOpenScenes();

        if (openScenes.Any(scene => scene.buildIndex == MainSceneIndexEditor.MainSceneIndex))
        {
            Debug.Log("The main scene was started, cannot infer chunk to teleport the player to");
            return;
        }

        var openSceneNames = openScenes
            .Where(scene => scene.buildIndex != MainSceneIndexEditor.MainSceneIndex)
            .OrderByDescending(scene => SceneManager.GetActiveScene().name == scene.name)
            .Select(scene => scene.name)
            .ToList();
        
        SceneManager.LoadSceneAsync(MainSceneIndexEditor.MainSceneIndex, LoadSceneMode.Additive).completed +=
            _ => OnSceneLoaded(openSceneNames);
    }

    private static void OnSceneLoaded(IList<string> startedScenes)
    {
        if (!TeleportPlayerToCorrectChunk)
        {
            return;
        }

        var targetChunks = Object.FindObjectsOfType<Chunk>()
            .Where(chunk =>
                startedScenes
                    .Any(scene =>
                        scene.ToUpper().Contains(chunk.ChunkName.ToUpper()))
            )
            .ToList();

        if (targetChunks.Count == 0)
        {
            Debug.LogWarning(
                $"The started Scenes do not exist as a chunk in the main scene. Cannot place player in the correct chunk.");
            return;
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = targetChunks.First().transform.position;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AssignChunksAtPlay()
    {
        WaitForScenesLoadedThenAssignChunks();
    }

    private static async void WaitForScenesLoadedThenAssignChunks()
    {
        var openScenes = MyEditorSceneManager.GetOpenScenes();

        while (MyEditorSceneManager.GetOpenScenes().Any(scene => !scene.isLoaded))
        {
            await Task.Delay(100);
        }
        openScenes = MyEditorSceneManager.GetOpenScenes();
        var chunks = Object.FindObjectsOfType<Chunk>();

        var openChunkScenes = openScenes
            .Where(scene =>
                chunks.Any(chunk =>
                    scene.name.ToUpper().Contains(chunk.ChunkName.ToUpper())))
            .ToList();

        var unloadedChunks = chunks
            .Where(chunk => !chunk.Loaded && !chunk.Loading).ToList();

        foreach (var chunk in unloadedChunks)
        {
            var scenesForChunk = openChunkScenes
                .Where(s => s.name.ToUpper().Contains(chunk.ChunkName.ToUpper()))
                .ToList();
            openChunkScenes = openChunkScenes.Except(scenesForChunk).ToList();

            var sceneForChunk = scenesForChunk.FirstOrDefault();
            if (sceneForChunk == default)
                continue;

            chunk.AssignOpenScene(sceneForChunk);
            
            // Remove the unnecessary instances of the same chunk
            foreach (var scene in scenesForChunk.Skip(1))
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // Remove the unassigned scenes
        foreach (var scene in openChunkScenes)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}
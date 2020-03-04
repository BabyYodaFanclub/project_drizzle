using System.Linq;
using Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class ChunksMenu
{
    private const string OpenChunksInMainSceneMenuStr = "Chunks/Open Chunks in Main Scene &c";
    private const string CloseChunksInMainSceneMenuStr = "Chunks/Close Chunk Scenes in Main Scene %#C";

    [MenuItem (OpenChunksInMainSceneMenuStr, false, 10)]
    private static void DoOpenChunksInMainScene()
    {
        EditorLogger.NotifyAndLog("Opening all Chunks in the Main Scene");
        
        var chunks = Object.FindObjectsOfType<Chunk>();
        
        foreach (var chunk in chunks)
        {
            OpenChunk(chunk);
        }
    }

    private static void OpenChunk(Chunk chunk)
    {
        var chunkAssetGuid = AssetDatabase.FindAssets($"{chunk.ChunkName} t:scene").FirstOrDefault();
        var chunkPath = AssetDatabase.GUIDToAssetPath(chunkAssetGuid);
        var chunkScene = EditorSceneManager.OpenScene(chunkPath, OpenSceneMode.Additive);
        chunk.RelocateChunkObjectsToChunk(chunkScene.GetRootGameObjects());
    }

    [MenuItem (OpenChunksInMainSceneMenuStr, true)]
    private static bool ValidateOpenChunksInMainScene()
    {
        return SceneManager.GetActiveScene().buildIndex == MainSceneIndexEditor.MainSceneIndex;
    }

    [MenuItem (CloseChunksInMainSceneMenuStr, false, 20)]
    private static void DoCloseChunksInMainScene()
    {
        EditorLogger.NotifyAndLog("Closing all Chunks in the Main Scene");
        
        var scenes = Enumerable.Range(0, SceneManager.sceneCount)
            .Select(SceneManager.GetSceneAt)
            .Where(s => s.buildIndex != MainSceneIndexEditor.MainSceneIndex)
            .ToList();
        
        foreach (var scene in scenes)
        {
            EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new[] {scene});
            EditorSceneManager.CloseScene(scene, true);
        }
    }
    
    [MenuItem (CloseChunksInMainSceneMenuStr, true)]
    private static bool ValidateCloseChunksInMainScene()
    {
        return SceneManager.GetActiveScene().buildIndex == MainSceneIndexEditor.MainSceneIndex;
    }
}

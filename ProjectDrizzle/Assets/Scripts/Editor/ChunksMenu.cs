using System.Linq;
using Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class ChunksMenu
{
    private const string OpenChunksInMainSceneMenuStr = "Chunks/Open Chunks in Main Scene &c";
    private const string OpenChunkInMainSceneMenuStr = "Chunks/Open Chunk in Main Scene &#c";
    private const string CloseChunksInMainSceneMenuStr = "Chunks/Close Chunk Scenes in Main Scene %&c";

    private static bool IsMainSceneOpen
    {
        get
        {
            return MyEditorSceneManager.GetOpenScenes()
                .Any(scene => scene.buildIndex == MainSceneIndexEditor.MainSceneIndex);
        }
    }

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
        if (chunkAssetGuid == null)
        {
            EditorLogger.NotifyAndLogWarning($"No scene file found for the chunk name {chunk.ChunkName}");
            return;
        }
        
        var chunkPath = AssetDatabase.GUIDToAssetPath(chunkAssetGuid);
        var chunkScene = EditorSceneManager.OpenScene(chunkPath, OpenSceneMode.Additive);
        chunk.RelocateChunkObjectsToChunk(chunkScene.GetRootGameObjects());
    }

    [MenuItem (OpenChunksInMainSceneMenuStr, true)]
    private static bool ValidateOpenChunksInMainScene()
    {
        return IsMainSceneOpen && !Application.isPlaying;
    }

    [MenuItem (OpenChunkInMainSceneMenuStr, false, 10)]
    private static void DoOpenChunkInMainScene()
    {
        EditorLogger.NotifyAndLog("Opening Chunk in the Main Scene");

        var chunkScene = SceneManager.GetActiveScene();

        var scene = EditorSceneManager.OpenScene(
            MainSceneIndexEditor.MainScene.path,
            OpenSceneMode.Additive
            );
        SceneManager.SetActiveScene(scene);
        
        var chunk = Object.FindObjectsOfType<Chunk>()
            .FirstOrDefault(c => chunkScene.name.ToUpper().Contains(c.ChunkName.ToUpper()));

        if (chunk == null)
        {
            EditorLogger.NotifyAndLogWarning("No chunk object in the Main Scene references the current scene");
            EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new[] {scene});
            EditorSceneManager.CloseScene(scene, true);
        }
        else
        {
            chunk.RelocateChunkObjectsToChunk(chunkScene.GetRootGameObjects());
        }
    }

    [MenuItem (OpenChunkInMainSceneMenuStr, true)]
    private static bool ValidateOpenChunkInMainScene()
    {
        return SceneManager.sceneCount == 1 && !IsMainSceneOpen && !Application.isPlaying;
    }

    [MenuItem (CloseChunksInMainSceneMenuStr, false, 20)]
    private static void DoCloseChunksInMainScene()
    {
        EditorLogger.NotifyAndLog("Closing all Chunks in the Main Scene");
        
        foreach (var scene in MyEditorSceneManager.GetOpenScenes().Where(s => s.buildIndex != MainSceneIndexEditor.MainSceneIndex))
        {
            EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new[] {scene});
            EditorSceneManager.CloseScene(scene, true);
        }
    }
    
    [MenuItem (CloseChunksInMainSceneMenuStr, true)]
    private static bool ValidateCloseChunksInMainScene()
    {
        return IsMainSceneOpen && !Application.isPlaying;
    }
}

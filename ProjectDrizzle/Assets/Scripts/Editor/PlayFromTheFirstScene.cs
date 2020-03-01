using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class PlayFromTheFirstScene
{
    private const string PlayFromFirstMenuStr = "Edit/Always Start From Scene 0 &p";

    private static bool PlayFromFirstScene
    {
        get { return EditorPrefs.HasKey(PlayFromFirstMenuStr) && EditorPrefs.GetBool(PlayFromFirstMenuStr); }
        set { EditorPrefs.SetBool(PlayFromFirstMenuStr, value); }
    }

    [MenuItem(PlayFromFirstMenuStr, false, 150)]
    private static void PlayFromFirstSceneCheckMenu()
    {
        PlayFromFirstScene = !PlayFromFirstScene;
        Menu.SetChecked(PlayFromFirstMenuStr, PlayFromFirstScene);

        ShowNotifyOrLog(PlayFromFirstScene ? "Play from scene 0" : "Play from current scene");
    }

    // The menu won't be gray out, we use this validate method for update check state
    [MenuItem(PlayFromFirstMenuStr, true)]
    private static bool PlayFromFirstSceneCheckMenuValidate()
    {
        Menu.SetChecked(PlayFromFirstMenuStr, PlayFromFirstScene);
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

        var openSceneName = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadSceneAsync(0).completed += obj => OnSceneLoaded(obj, openSceneName);
        
    }

    private static void OnSceneLoaded(AsyncOperation obj, string openSceneName)
    {
        var chunks = Object.FindObjectsOfType<Chunk>().ToList();
        var targetChunk = chunks
            .FirstOrDefault(s => s.ChunkName.Equals(openSceneName, StringComparison.InvariantCultureIgnoreCase));

        if (targetChunk == null)
        {
            Debug.LogWarning($"The started Scene {openSceneName} does not exist as a chunk in the main scene. Cannot place player in the correct chunk.");
            return;
        }
        
        var player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = targetChunk.transform.position;
    }

    private static void ShowNotifyOrLog(string msg)
    {
        if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
            EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
        else
            Debug.Log(msg); // When there's no scene view opened, we just print a log
    }
}
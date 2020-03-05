using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public static class MainSceneIndexEditor
    {
        private const string SetMainSceneMenuStr = "Scenes/Set current scene as Main Scene";
        private const string MainSceneIndexKey = "MainSceneIndex";
        
        public static int MainSceneIndex
        {
            get => EditorPrefs.HasKey(MainSceneIndexKey) ? EditorPrefs.GetInt(MainSceneIndexKey) : 0;
            set => EditorPrefs.SetInt(MainSceneIndexKey, value);
        }
        
        public static EditorBuildSettingsScene MainScene => EditorBuildSettings.scenes[MainSceneIndex];

        [MenuItem(SetMainSceneMenuStr, false, 10)]
        private static void SetMainSceneMenu()
        {
            var scene = SceneManager.GetActiveScene();
            var index = scene.buildIndex;

            if (index < 0)
            {
                EditorLogger.NotifyAndLogWarning($"The Scene has to be registered in the Build Settings first!");
                return;
            }
            
            MainSceneIndex = index;
            
            EditorLogger.NotifyAndLog($"Main Scene set to {scene.name}");
        }

        // The menu won't be gray out, we use this validate method for update check state
        [MenuItem(SetMainSceneMenuStr, true)]
        private static bool SetMainSceneMenuValidate()
        {
            return SceneManager.GetActiveScene().buildIndex != MainSceneIndex && !Application.isPlaying;
        }
    }
}
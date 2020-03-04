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

        [MenuItem(SetMainSceneMenuStr, false, 10)]
        private static void SetMainSceneMenu()
        {
            var index = SceneManager.GetActiveScene().buildIndex;

            if (index < 0)
            {
                EditorLogger.NotifyAndLog($"The Scene has to be registered in the Build Settings first!");
                return;
            }
            
            MainSceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            EditorLogger.NotifyAndLog($"Main Scene set to {SceneManager.GetSceneByBuildIndex(index).name}");
        }

        // The menu won't be gray out, we use this validate method for update check state
        [MenuItem(SetMainSceneMenuStr, true)]
        private static bool SetMainSceneMenuValidate()
        {
            return SceneManager.GetActiveScene().buildIndex != MainSceneIndex;
        }
    }
}
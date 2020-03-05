using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Editor
{
    public static class MyEditorSceneManager
    {
        public static IList<Scene> GetOpenScenes()
        {
            return Enumerable.Range(0, SceneManager.sceneCount)
                .Select(SceneManager.GetSceneAt)
                .ToList();
        }
    }
}
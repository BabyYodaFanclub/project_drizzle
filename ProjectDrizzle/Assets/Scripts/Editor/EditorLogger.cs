using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class EditorLogger
    {
        public static void NotifyAndLog(string msg)
        {
            EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
            Debug.Log(msg);
        }
        
        public static void NotifyAndLogWarning(string msg)
        {
            EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
            Debug.LogWarning(msg);
        }
    }
}
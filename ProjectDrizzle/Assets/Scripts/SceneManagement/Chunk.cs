using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Debug = System.Diagnostics.Debug;

public class Chunk : MonoBehaviour
{
    public string ChunkName;
    private SceneInstance? _scene;

    public void LoadScene()
    {
        Debug.Assert( !string.IsNullOrWhiteSpace(ChunkName), nameof(ChunkName) + " is null or whitespace");
        Addressables.LoadSceneAsync(ChunkName, LoadSceneMode.Additive).Completed += OnSceneLoaded;
    }

    public void UnloadScene()
    {
        Debug.Assert(_scene != null, nameof(_scene) + " is null");
        
        // Kill all children
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        
        Addressables.UnloadSceneAsync(_scene.Value);
        _scene = null;
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOperationHandle)
    {
        _scene = asyncOperationHandle.Result;
        var sceneObjects = _scene.Value.Scene.GetRootGameObjects();
        foreach (var sceneObject in sceneObjects)
        {
            sceneObject.transform.parent = transform;
            sceneObject.transform.localPosition = Vector3.zero;
        }
    }
}
 
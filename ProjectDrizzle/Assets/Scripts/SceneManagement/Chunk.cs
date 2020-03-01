using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Chunk : MonoBehaviour
{
    private const int ChunkSize = 50;
    private const string DebugObjectsPrefix = "Debug";
    
    public string ChunkName;
    private SceneInstance? _scene;
    private bool _loaded;

    private void Start()
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(ChunkName), nameof(ChunkName) + " is null or whitespace");
        
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerDistance = transform.position - player.transform.position;
        if (Math.Abs(playerDistance.x) < ChunkSize && Math.Abs(playerDistance.z) < ChunkSize)
            LoadScene();
    }

    public void LoadScene()
    {
        if (_loaded || _scene.HasValue)
        {
            _loaded = true;
            return;
        }
        
        Addressables.LoadSceneAsync(ChunkName, LoadSceneMode.Additive).Completed += OnSceneLoaded;
        _loaded = true;
    }

    public void UnloadScene()
    {
        if (!_loaded || !_scene.HasValue)
        {
            _loaded = false;
            return;
        }

        Addressables.UnloadSceneAsync(_scene.Value);
        _scene = null;
        _loaded = false;
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOperationHandle)
    {
        _scene = asyncOperationHandle.Result;
        var sceneObjects = _scene.Value.Scene
            .GetRootGameObjects();
        
        foreach (var sceneObject in sceneObjects.Where(o => !o.name.StartsWith(DebugObjectsPrefix)))
        {
            sceneObject.transform.position = transform.position;
        }

        foreach (var obj in sceneObjects.Where(o => o.name.StartsWith(DebugObjectsPrefix, StringComparison.InvariantCultureIgnoreCase)))
        {
            Destroy(obj);
        }
    }
}
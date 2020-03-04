using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    private const int ChunkEdgeLength = 50;
    private const string DebugObjectsPrefix = "Debug";
    
    public string ChunkName;
    private SceneInstance? _scene;
    private bool _loaded;

    private void Start()
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(ChunkName), nameof(ChunkName) + " is null or whitespace");
        
        if (IsPlayerInChunk())
            Load();
    }

    private bool IsPlayerInChunk()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerDistance = transform.position - player.transform.position;
        return Math.Abs(playerDistance.x) < ChunkEdgeLength && Math.Abs(playerDistance.z) < ChunkEdgeLength;
    }

    public void Load()
    {
        if (_loaded || _scene.HasValue)
        {
            _loaded = true;
            return;
        }
        
        Addressables.LoadSceneAsync(ChunkName, LoadSceneMode.Additive).Completed += OnSceneLoaded;
        _loaded = true;
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOperationHandle)
    {
        _scene = asyncOperationHandle.Result;
        RelocateChunkObjectsToChunk(_scene.Value.Scene.GetRootGameObjects());
    }

    public void RelocateChunkObjectsToChunk(IEnumerable<GameObject> chunkObjects)
    {
        var chunkObjectsList = chunkObjects.ToList();
        
        foreach (var sceneObject in chunkObjectsList.Where(o => !o.name.StartsWith(DebugObjectsPrefix)))
        {
            sceneObject.transform.position = transform.position;
        }
        
        if (Application.isPlaying)
        {
            foreach (var obj in chunkObjectsList.Where(o =>
                o.name.StartsWith(DebugObjectsPrefix, StringComparison.InvariantCultureIgnoreCase)))
            {
                Destroy(obj);
            }
        }
    }

    public void Unload()
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

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Random.InitState(ChunkName.Sum(c => c));
        Gizmos.color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
        Gizmos.DrawCube(transform.position, new Vector3(ChunkEdgeLength * 2, 0, ChunkEdgeLength * 2));
    }

    private void OnDrawGizmos()
    {
        Random.InitState(ChunkName.Sum(c => c));
        Gizmos.color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
        Gizmos.DrawWireCube(transform.position, new Vector3(ChunkEdgeLength * 2, 0, ChunkEdgeLength * 2));
    }

    #endregion
}
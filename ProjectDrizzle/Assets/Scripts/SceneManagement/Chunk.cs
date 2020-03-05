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
    public string ChunkName;
    
    private const int ChunkEdgeLength = 50;
    private const string DebugObjectsPrefix = "Debug";

    private SceneInstance? _sceneInstance;
    private Scene? _scene;
    
    public bool Loaded => _scene.HasValue || _sceneInstance.HasValue;

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
        if (Loaded)
        {
            return;
        }

        Addressables.LoadSceneAsync(ChunkName, LoadSceneMode.Additive).Completed += OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOperationHandle)
    {
        _sceneInstance = asyncOperationHandle.Result;
        RelocateChunkObjectsToChunk(_sceneInstance.Value.Scene.GetRootGameObjects());
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
        if (!Loaded)
        {
            return;
        }

        if (_sceneInstance.HasValue)
        {
            Addressables.UnloadSceneAsync(_sceneInstance.Value);
            _sceneInstance = null;
        }
        else if (_scene.HasValue)
        {
            SceneManager.UnloadSceneAsync(_scene.Value, UnloadSceneOptions.None);
            _scene = null;
        }
    }

    public void AssignOpenScene(Scene sceneForChunk)
    {
        _scene = sceneForChunk;
        RelocateChunkObjectsToChunk(_scene.Value.GetRootGameObjects());
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
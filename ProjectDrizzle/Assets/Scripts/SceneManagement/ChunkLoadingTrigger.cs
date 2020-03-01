﻿using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions;

public class ChunkLoadingTrigger : MonoBehaviour
{
    public Collider[] EnterColliders;
    public Collider[] ExitColliders;
    public string ChunkName;
    private Chunk _chunk;
    
    private void Awake()
    {
        foreach (var childRenderer in GetComponentsInChildren<Renderer>())
        {
            // TODO: enable this
            //Destroy(childRenderer);
        }
    }

    private void Start()
    {
        Assert.IsFalse(string.IsNullOrWhiteSpace(ChunkName));
        
        // TODO Write a chunk manager in the main scene
        _chunk = FindObjectsOfType<Chunk>()
                    .First(c => c.ChunkName.Equals(ChunkName, StringComparison.InvariantCultureIgnoreCase));

        foreach (var col in EnterColliders)
            GetEventEmitterForCollider(col).OnTriggerEnterEvent += OnLoadTrigger;

        foreach (var col in ExitColliders)
            GetEventEmitterForCollider(col).OnTriggerEnterEvent += OnUnloadTrigger;
    }

    private void OnLoadTrigger(Collider other)
    {
        _chunk.Load();
    }

    private void OnUnloadTrigger(Collider other)
    {
        _chunk.Unload();
    }

    private static ColliderEventEmitter GetEventEmitterForCollider(Collider c)
    {
        var emitter = c.GetComponent<ColliderEventEmitter>();
        if (!emitter)
        {
            emitter = c.gameObject.AddComponent<ColliderEventEmitter>();
        }

        return emitter;
    }

    private void OnDestroy()
    {
        foreach (var col in EnterColliders)
            GetEventEmitterForCollider(col).OnTriggerEnterEvent -= OnLoadTrigger;

        foreach (var col in ExitColliders)
            GetEventEmitterForCollider(col).OnTriggerEnterEvent -= OnUnloadTrigger;
    }
}
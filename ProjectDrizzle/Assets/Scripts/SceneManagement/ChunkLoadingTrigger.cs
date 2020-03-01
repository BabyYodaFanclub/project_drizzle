using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions;

public class ChunkLoadingTrigger : MonoBehaviour
{
    public Chunk Chunk;
    public Collider EnterCollider;
    public Collider ExitCollider;

    private void Awake()
    {
        foreach (var childRenderer in GetComponentsInChildren<Renderer>())
        {
            //Destroy(childRenderer);
        }
    }

    private void Start()
    {
        Assert.IsNotNull(EnterCollider);
        Assert.IsNotNull(ExitCollider);
        
        GetEventForCollider(EnterCollider).OnTriggerEnterEvent += OnLoadTrigger;
        GetEventForCollider(ExitCollider).OnTriggerEnterEvent += OnUnloadTrigger;
    }

    private void OnLoadTrigger(Collider other)
    {
        Chunk.LoadScene();
    }

    private void OnUnloadTrigger(Collider other)
    {
        Chunk.UnloadScene();
    }

    private static ColliderEventEmitter GetEventForCollider(Collider c)
    {
        var emitter = c.GetComponent<ColliderEventEmitter>();
        if (!emitter)
        {
            emitter = c.gameObject.AddComponent<ColliderEventEmitter>();
        }

        return emitter;
    }
}
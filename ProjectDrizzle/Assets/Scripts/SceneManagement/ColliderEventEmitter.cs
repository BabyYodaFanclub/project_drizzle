using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ColliderEventEmitter : MonoBehaviour
{
    public event Action<Collider> OnTriggerEnterEvent;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }
}
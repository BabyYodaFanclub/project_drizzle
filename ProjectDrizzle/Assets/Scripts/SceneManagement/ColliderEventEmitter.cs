using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ColliderEventEmitter : MonoBehaviour
{
    public event Action<Collider> OnTriggerEnterEvent;
    public event Action<Collider> OnTriggerExitEvent;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other);
    }
}
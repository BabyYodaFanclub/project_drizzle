using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ColliderEventEmitter : MonoBehaviour
{
    public UnityAction<Collider> OnTriggerEnterEvent;
    
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent.Invoke(other);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using XNode;

[CreateAssetMenu(menuName = "Story/DialogueTree", fileName = "NewDialogue")]
public class DialogueGraph : NodeGraph
{
    [SerializeField] public SerializableDictionaryBase<string, object> Variables { get; }
    
    public DialogueGraph()
    {
        if (Variables == null)
        {
            Variables = new SerializableDictionaryBase<string, object>();
        }
    }
}
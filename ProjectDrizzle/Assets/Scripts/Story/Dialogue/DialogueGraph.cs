using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using XNode;

[CreateAssetMenu(menuName = "Story/DialogueTree", fileName = "NewDialogue")]
[Serializable]
public class DialogueGraph : NodeGraph
{
    [SerializeField]
    public DialogueVariablesDictionary Variables;
    
    public DialogueGraph()
    {
        if (Variables == null)
        {
            Variables = new DialogueVariablesDictionary();
        }
    }
}

[Serializable]
public class DialogueVariablesDictionary : SerializableDictionaryBase<string, string> { }
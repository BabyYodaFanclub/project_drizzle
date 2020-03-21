using System;
using System.Linq;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using XNode;

[CreateAssetMenu(menuName = "Story/DialogueTree", fileName = "NewDialogue")]
[Serializable]
public class DialogueGraph : NodeGraph
{
    [SerializeField] public DialogueVariablesDictionary Variables;

    private DialogueBaseNode _current;

    public DialogueGraph()
    {
        if (Variables == null)
        {
            Variables = new DialogueVariablesDictionary();
        }
    }

    public DialogueBaseNode Current
    {
        get
        {
            if (_current == null)
                _current = (DialogueBaseNode) nodes.First(n => n is DialogueStartNode);
            return _current;
        }
        set => _current = value;
    }

    public override Node AddNode(Type type)
    {
        if (type == typeof(DialogueStartNode) && nodes.Count(n => n is DialogueStartNode) >= 1)
        {
            Debug.LogWarning("There can only be one Start Node in the graph!");
        }
        return base.AddNode(type);
    }

    public string EnrichStatementWithInsertedVariables(string statement)
    {
        return Variables
            .Aggregate(statement,
                (current, graphVariable) => 
                    current.Replace("{{" + graphVariable.Key + "}}", graphVariable.Value));
    }
}

[Serializable]
public class DialogueVariablesDictionary : SerializableDictionaryBase<string, string>
{
}
using System;
using Story;
using UnityEngine;
using XNode;

[CreateNodeMenu("2 Answer Selection")]
[NodeTint(150, 150, 250), NodeWidth(300)]
public class DialogueTwoAnswerSelectionNode : DialogueBaseNode
{
    [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)]
    public DialogueBaseNode PredecessorNode;

    public string AnswerOne;

    [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedInverse)]
    public DialogueBaseNode SuccessorNodeOne;

    public string AnswerTwo;

    [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedInverse)]
    public DialogueBaseNode SuccessorNodeTwo;
    
    [HideInInspector] [NonSerialized]
    public int SelectedNode = -1;

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {
        if (port.fieldName == nameof(SuccessorNodeOne) && port.Connection != null)
        {
            return port.Connection?.node;
        }

        if (port.fieldName == nameof(SuccessorNodeTwo) && port.Connection != null)
        {
            return port.Connection?.node;
        }

        return null; // Replace this
    }

    public override void OnUpdateNode()
    {
    }

    public override DialogueBaseNode StepForwardInGraph()
    {
        if (SelectedNode < 0 || SelectedNode > 1)
            throw new InvalidOperationException("No (valid) answer has been selected!");

        DialogueBaseNode next = null;
        switch (SelectedNode)
        {
            case 0:
                next = (DialogueBaseNode) GetPort(nameof(SuccessorNodeOne)).Connection?.node;
                break;
            case 1:
                next = (DialogueBaseNode) GetPort(nameof(SuccessorNodeTwo)).Connection?.node;
                break;
        }

        DialogueGraph.Current = next;
        return next;
    }

    public override void Validate()
    {
        if (GetPort(nameof(SuccessorNodeOne)).Connection == null)
            Debug.LogWarning("The Dialogue Tree has an illegal State, not all ports connect to a valid node");
        if (GetPort(nameof(SuccessorNodeTwo)).Connection == null)
            Debug.LogWarning("The Dialogue Tree has an illegal State, not all ports connect to a valid node");
    }

    private void Reset()
    {
        name = "Selection";
    }
}
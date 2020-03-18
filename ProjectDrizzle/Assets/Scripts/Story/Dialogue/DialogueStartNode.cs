using UnityEngine;
using XNode;

[CreateNodeMenu("Start")]
[NodeTint(50, 150, 50), NodeWidth(80)]
public class DialogueStartNode : DialogueBaseNode
{
    [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedInverse)] public DialogueBaseNode Start;
	
    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
        if (port.fieldName == nameof(Start) && port.Connection != null)
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
        var next = (DialogueBaseNode) GetPort(nameof(Start)).Connection?.node;
        DialogueGraph.Current = next;
        return next;
    }

    public override void Validate()
    {
        if (GetPort(nameof(Start)).Connection == null)
            Debug.LogError("The Dialogue Tree has an illegal State, not all ports connect to a valid node");
    }

    private void Reset()
    {
        name = "Start";
    }
}
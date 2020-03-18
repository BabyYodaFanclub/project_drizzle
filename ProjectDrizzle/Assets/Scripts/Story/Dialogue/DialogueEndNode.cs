using UnityEngine;
using XNode;

[CreateNodeMenu("End")]
[NodeTint(100, 100, 100), NodeWidth(80)]
public class DialogueEndNode : DialogueBaseNode
{
    [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public DialogueBaseNode End;
	
    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
        return null; // Replace this
    }

    public override void OnUpdateNode()
    {
        
    }

    public override DialogueBaseNode StepForwardInGraph()
    {
        Debug.LogError("The end node does not have a next node");
        throw new System.NotImplementedException();
    }

    public override void Validate()
    {
    }

    private void Reset()
    {
        name = "End";
    }
}
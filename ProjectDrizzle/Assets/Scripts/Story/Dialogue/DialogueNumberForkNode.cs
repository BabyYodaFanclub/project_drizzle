using System;
using System.Linq;
using UnityEngine;
using XNode;

[CreateNodeMenu("NumberFork")]
[NodeTint(150, 50, 50), NodeWidth(200)]
public class DialogueNumberForkNode : DialogueBaseNode
{
    [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public DialogueBaseNode InputNode;
    [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedInverse)] public DialogueBaseNode Bigger;
    [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedInverse)] public DialogueBaseNode Equal;
    [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedInverse)] public DialogueBaseNode Smaller;

    public string VariableName;
    public float ComparisonValue;

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
        return null; // Replace this
    }

    public override void OnUpdateNode()
    {
    }

    private void Reset()
    {
        name = "NumberFork";
    }
}
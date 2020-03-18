using System;
using UnityEngine;
using XNode;

[CreateNodeMenu("NumberFork")]
[NodeTint(150, 150, 250), NodeWidth(200)]
public class DialogueNumberForkNode : DialogueBaseNode
{
    [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)]
    public DialogueBaseNode InputNode;

    [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)]
    public float Variable;


    [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedInverse)]
    public DialogueBaseNode Bigger;

    [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedInverse)]
    public DialogueBaseNode Equal;

    [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedInverse)]
    public DialogueBaseNode Smaller;

    public float ComparisonValue;

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {
        if (port.fieldName == nameof(Smaller))
        {
            return (DialogueBaseNode) GetPort(nameof(Smaller)).Connection?.node;
        }

        if (port.fieldName == nameof(Equal))
        {
            return (DialogueBaseNode) GetPort(nameof(Equal)).Connection?.node;
        }

        if (port.fieldName == nameof(Bigger))
        {
            return (DialogueBaseNode) GetPort(nameof(Bigger)).Connection?.node;
        }

        return null; // Replace this
    }

    public override DialogueBaseNode StepForwardInGraph()
    {
        DialogueBaseNode next;
        if (Math.Abs((float) (GetPort(nameof(Variable)).GetInputValue()) - ComparisonValue) < 0.00001)
        {
            next = (DialogueBaseNode) GetPort(nameof(Equal)).Connection?.node;
        }
        else if ((float) (GetPort(nameof(Variable)).GetInputValue()) < ComparisonValue)
        {
            next = (DialogueBaseNode) GetPort(nameof(Smaller)).Connection?.node;
        }
        else
        {
            next = (DialogueBaseNode) GetPort(nameof(Bigger)).Connection?.node;
        }

        DialogueGraph.Current = next;

        return next;
    }

    public override void Validate()
    {
        if (!float.TryParse(GetPort(nameof(Variable)).GetInputValue().ToString(), out var _))
        {
            Debug.LogError("The connected Variable is not a valid number");
        }

        if (GetPort(nameof(Smaller)).Connection == null)
            Debug.LogError("The Dialogue Tree has an illegal State, not all ports connect to a valid node");
        if (GetPort(nameof(Equal)).Connection == null)
            Debug.LogError("The Dialogue Tree has an illegal State, not all ports connect to a valid node");
        if (GetPort(nameof(Bigger)).Connection == null)
            Debug.LogError("The Dialogue Tree has an illegal State, not all ports connect to a valid node");
    }

    public override void OnUpdateNode()
    {
    }

    private void Reset()
    {
        name = "NumberFork";
    }
}
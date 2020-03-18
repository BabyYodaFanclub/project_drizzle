using UnityEngine;
using XNode;

namespace Story.Dialogue
{
    [CreateNodeMenu("VariableChange")]
    [NodeTint(150, 150, 150), NodeWidth(300)]
    public class DialogueVariableChangeNode : DialogueBaseNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public DialogueBaseNode PredecessorNode;
        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.InheritedInverse)] public DialogueBaseNode SuccessorNode;
        
        public string VariableName;
        public string Value;
	
        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(SuccessorNode) && port.Connection != null)
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
            ((DialogueGraph) graph).Variables[VariableName] = Value;
            
            var next = (DialogueBaseNode) GetPort(nameof(SuccessorNode)).Connection?.node;
            DialogueGraph.Current = next;
            return next;
        }

        public override void Validate()
        {
            if (GetPort(nameof(SuccessorNode)).Connection == null)
                Debug.LogWarning("The Dialogue Tree has an illegal State, not all ports connect to a valid node");
        }

        private void Reset()
        {
            name = "VariableChange";
        }
    }
}
using XNode;

namespace Story.Dialogue
{
    [CreateNodeMenu("Variable")]
    [NodeTint(150, 150, 150), NodeWidth(300)]
    public class DialogueVariableNode : DialogueBaseNode
    {
        [Output(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.InheritedInverse)] public float Value;
        
        public string VariableName;
	
        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(Value) && graph as DialogueGraph)
            {
                var dialogueGraph = (DialogueGraph) graph;
                return dialogueGraph.Variables.ContainsKey(VariableName) ? dialogueGraph.Variables[VariableName] : null;
            }
            
            return null; // Replace this
        }

        public override void OnUpdateNode()
        {
        
        }

        public override DialogueBaseNode StepForwardInGraph()
        {
            throw new System.NotImplementedException();
        }

        public override void Validate()
        {
            
        }

        private void Reset()
        {
            name = "Variable";
        }
    }
}
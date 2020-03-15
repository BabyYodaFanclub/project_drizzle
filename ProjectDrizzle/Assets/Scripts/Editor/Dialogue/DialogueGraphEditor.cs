using System;
using XNode;
using XNodeEditor;

namespace Editor.Dialogue
{
    [CustomNodeGraphEditor(typeof(DialogueGraph))]
    public class DialogueGraphEditor : NodeGraphEditor {
        
        public override string GetNodeMenuName(Type node)
        {
            if (node.IsSubclassOf(typeof(DialogueBaseNode))) {
                return base.GetNodeMenuName(node);
            }
            
            return null;
        }
    }
}
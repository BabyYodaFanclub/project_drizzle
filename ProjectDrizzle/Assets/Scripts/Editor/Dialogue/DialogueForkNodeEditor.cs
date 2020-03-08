using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Editor.Dialogue
{
    [CustomNodeEditor(typeof(DialogueNumberForkNode))]
    public class DialogueForkNodeEditor : DialogueNodeEditorBase
    {
        private DialogueNumberForkNode _numberForkNode;
        private bool _selected;

        public override void OnBodyGUI()
        {
            // Only change the selected bool when the event is Layout, if it is changed on another event,
            // the gui throws an exception due to mismatched state between calls
            if (Event.current.type == EventType.Layout)
            {
                _selected = Selection.activeObject == target;
            }

            if (_numberForkNode == null) _numberForkNode = (DialogueNumberForkNode) target;
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_numberForkNode.InputNode)));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_numberForkNode.VariableName)));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_numberForkNode.ComparisonValue)));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_numberForkNode.Bigger)));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_numberForkNode.Equal)));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_numberForkNode.Smaller)));
        }
    }
}
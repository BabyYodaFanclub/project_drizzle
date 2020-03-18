using System.Linq;
using Story.Dialogue;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Editor.Dialogue
{
    [CustomNodeEditor(typeof(DialogueVariableChangeNode))]
    public class DialogueVariableChangeNodeEditor : DialogueNodeEditorBase
    {
        private DialogueVariableChangeNode _variableChangeNode;
        private bool _selected;

        public override void OnBodyGUI()
        {
            // Only change the selected bool when the event is Layout, if it is changed on another event,
            // the gui throws an exception due to mismatched state between calls
            if (Event.current.type == EventType.Layout)
            {
                _selected = Selection.activeObject == target;
            }

            if (_variableChangeNode == null) _variableChangeNode = (DialogueVariableChangeNode) target;

            serializedObject.Update();

            var dialogueGraph = (DialogueGraph) _variableChangeNode.graph;
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_variableChangeNode.PredecessorNode)));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_variableChangeNode.SuccessorNode)));
            
            var options = dialogueGraph.Variables.Keys.ToArray();
            var selectedValue =  EditorGUILayout.Popup("Variable:", options.ToList().IndexOf(_variableChangeNode.VariableName), options);
            _variableChangeNode.VariableName = selectedValue >= 0 ? options[selectedValue] : "";
            
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_variableChangeNode.Value)));
            
            // Apply property modifications
            serializedObject.ApplyModifiedProperties();
        }
    }
}
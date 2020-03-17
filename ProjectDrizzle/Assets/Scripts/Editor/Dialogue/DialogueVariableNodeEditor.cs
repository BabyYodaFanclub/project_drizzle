using System;
using System.Linq;
using System.Runtime.InteropServices;
using Story.Dialogue;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Editor.Dialogue
{
    [CustomNodeEditor(typeof(DialogueVariableNode))]
    public class DialogueVariableNodeEditor : DialogueNodeEditorBase
    {
        private DialogueVariableNode _variableNode;
        private bool _selected;

        public override void OnBodyGUI()
        {
            // Only change the selected bool when the event is Layout, if it is changed on another event,
            // the gui throws an exception due to mismatched state between calls
            if (Event.current.type == EventType.Layout)
            {
                _selected = Selection.activeObject == target;
            }

            if (_variableNode == null) _variableNode = (DialogueVariableNode) target;

            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_variableNode.Value)));

            var dialogueGraph = (DialogueGraph) _variableNode.graph;
            var options = dialogueGraph.Variables.Keys.ToArray();
            var selectedValue =  EditorGUILayout.Popup("Variable:", options.ToList().IndexOf(_variableNode.VariableName), options);
            _variableNode.VariableName = selectedValue >= 0 ? options[selectedValue] : "";

            /*if (_selected)
            {
            }
            else
            {
                base.OnBodyGUI();
            }*/


            // Apply property modifications
            serializedObject.ApplyModifiedProperties();
        }
    }
}
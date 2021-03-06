using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Editor.Dialogue
{
    [CustomNodeEditor(typeof(DialogueStatementNode))]
    public class DialogueStatementNodeEditor : DialogueNodeEditorBase
    {
        private DialogueStatementNode _statementNode;
        private bool _selected;

        public override void OnBodyGUI()
        {
            // Only change the selected bool when the event is Layout, if it is changed on another event,
            // the gui throws an exception due to mismatched state between calls
            if (Event.current.type == EventType.Layout)
            {
                _selected = Selection.activeObject == target;
            }

            if (_statementNode == null) _statementNode = (DialogueStatementNode) target;

            serializedObject.Update();

            if (_selected)
            {
                base.OnBodyGUI();
            }
            else
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_statementNode.PredecessorNode)));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_statementNode.SuccessorNode)));
                
                var speaker = _statementNode.Speaker;
                if (speaker && speaker.Thumbnail)
                {
                    GUILayout.BeginHorizontal(speaker.Thumbnail, new GUIStyle(GUI.skin.box)
                    {
                        fixedHeight = 150
                    });
                    GUILayout.Label(speaker.Name);
                    GUILayout.EndHorizontal();
                }
                else if (!speaker)
                {
                    GUILayout.BeginHorizontal(new GUIStyle(GUI.skin.box)
                    {
                        fixedHeight = 150
                    });
                    GUILayout.Box("Put player image here");
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.BeginHorizontal(new GUIStyle(GUI.skin.box)
                    {
                        fixedHeight = 150
                    });
                    GUILayout.Box("No image set");
                    GUILayout.EndHorizontal();
                }

                var dialogueText = _statementNode.DialogueText;

                if (string.IsNullOrWhiteSpace(dialogueText))
                    GUILayout.Label("No Text set!", GetErrorStyleForSkin(GUI.skin.label));
                else
                    GUILayout.Label(dialogueText, new GUIStyle(GUI.skin.label)
                    {
                        normal = {background = Texture2D.grayTexture},
                        fontStyle = FontStyle.BoldAndItalic,
                        fontSize = 18
                    });
            }


            // Apply property modifications
            serializedObject.ApplyModifiedProperties();
        }
    }
}
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using XNodeEditor;

namespace Editor.Dialogue
{
    [CustomNodeGraphEditor(typeof(DialogueGraph))]
    public class DialogueGraphEditor : NodeGraphEditor
    {
        private bool _newValueFade;
        private string _newKey;
        private string _newValue;

        public override string GetNodeMenuName(Type node)
        {
            if (node.IsSubclassOf(typeof(DialogueBaseNode)))
            {
                return base.GetNodeMenuName(node);
            }

            return null;
        }
        
        public override void OnCreate()
        {
            base.OnCreate();
            
            if (!((DialogueGraph)target).nodes.Any(n => n is DialogueStartNode))
                CreateNode(typeof(DialogueStartNode), Vector2.left * 400);
            
            if (!((DialogueGraph)target).nodes.Any(n => n is DialogueEndNode))
                CreateNode(typeof(DialogueEndNode), Vector2.right * 400);
        }

        public override void OnGUI()
        {
            var graph = (DialogueGraph) target;
            EditorGUILayout.BeginVertical(new GUIStyle
            {
                normal = {background = Texture2D.whiteTexture},
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(20, 20, 20, 20),
                fixedWidth = 400
            });

            _newValueFade = EditorGUILayout.ToggleLeft("Graph Variables", _newValueFade,
                new GUIStyle {fontSize = 18, fontStyle = FontStyle.Bold});
            EditorGUILayout.LabelField("To use variables in Dialogue Texts, simply insert {{VARIABLE_NAME}} into the text");
            
            if (EditorGUILayout.BeginFadeGroup(_newValueFade ? 1 : 0))
            {
                foreach (var graphVariable in graph.Variables.ToList())
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.BeginHorizontal();
                    var editedKey = EditorGUILayout.TextField("Key", graphVariable.Key);
                    var editedValue = EditorGUILayout.TextField("Value", graphVariable.Value?.ToString());
                    if (EditorGUI.EndChangeCheck() && !string.IsNullOrWhiteSpace(editedKey))
                    {
                        graph.Variables[editedKey] = editedValue;
                        if (editedKey != graphVariable.Key)
                            graph.Variables.Remove(graphVariable.Key);

                        EditorUtility.SetDirty(graph);
                    }

                    if (GUILayout.Button("Remove"))
                    {
                        graph.Variables.Remove(graphVariable.Key);

                        EditorUtility.SetDirty(graph);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (graph.Variables.Count > 0)
                    EditorGUILayout.Separator();

                EditorGUILayout.LabelField("New Variable", new GUIStyle {fontSize = 14, fontStyle = FontStyle.Bold});
                EditorGUILayout.BeginHorizontal();
                _newKey = EditorGUILayout.TextField("Key", _newKey);
                _newValue = EditorGUILayout.TextField("Value", _newValue);
                EditorGUILayout.EndHorizontal();

                if (!string.IsNullOrWhiteSpace(_newKey))
                {
                    if (graph.Variables.Keys.Contains(_newKey))
                        EditorGUI.BeginDisabledGroup(true);
                    if (GUILayout.Button("Add"))
                    {
                        graph.Variables.Add(_newKey, _newValue);
                        _newKey = "";
                        _newValue = "";
                        EditorGUI.FocusTextInControl(null);

                        EditorUtility.SetDirty(graph);
                    }

                    EditorGUI.EndDisabledGroup();
                }
            }

            EditorGUILayout.EndFadeGroup();


            EditorGUILayout.EndVertical();
        }
    }
}
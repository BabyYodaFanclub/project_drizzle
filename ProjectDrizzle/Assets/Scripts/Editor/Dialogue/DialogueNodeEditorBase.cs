using UnityEngine;
using XNode;
using XNodeEditor;

namespace Editor.Dialogue
{
    public abstract class DialogueNodeEditorBase : NodeEditor
    {
        static DialogueNodeEditorBase()
        {
            onUpdateNode += OnUpdateNode;
        }

        private static void OnUpdateNode(Node node)
        {
            if (node is DialogueBaseNode dialogueNode)
            {
                dialogueNode.Validate();
                dialogueNode.OnUpdateNode();
            }
        }

        protected GUIStyle GetErrorStyleForSkin(GUIStyle skin)
        {
            return new GUIStyle(skin) {normal =
                {
                    textColor = Color.Lerp(Color.red, Color.black, 0.2f),
                    background = Texture2D.whiteTexture
                }
            };
        }
    }
}
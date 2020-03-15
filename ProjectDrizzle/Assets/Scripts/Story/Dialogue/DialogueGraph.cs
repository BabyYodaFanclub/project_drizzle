using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu(menuName = "Story/DialogueTree", fileName = "NewDialogue")]
public class DialogueGraph : NodeGraph {
	public Dictionary<string, object> Variables { get; } = new Dictionary<string, object>();
}
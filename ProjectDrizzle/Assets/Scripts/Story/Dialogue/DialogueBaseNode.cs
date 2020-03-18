using System;
using XNode;

public abstract class DialogueBaseNode : Node
{
	protected DialogueGraph DialogueGraph => (DialogueGraph) graph;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	private void OnValidate()
	{
		Validate();
	}

	public override void OnRemoveConnection(NodePort port)
	{
		base.OnRemoveConnection(port);
	}

	public override void OnCreateConnection(NodePort @from, NodePort to)
	{
		base.OnCreateConnection(@from, to);
	}

	public abstract void OnUpdateNode();
	
	/// <summary>
	/// Updates the graph with the new current node
	/// </summary>
	/// <returns></returns>
	public abstract DialogueBaseNode StepForwardInGraph();
	public abstract void Validate();
}
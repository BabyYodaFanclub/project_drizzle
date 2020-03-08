using System;
using System.Collections;
using System.Collections.Generic;
using XNode;

public abstract class DialogueBaseNode : Node
{
	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	public abstract void OnUpdateNode();
}
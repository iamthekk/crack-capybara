using System;

namespace XNode.GameEvent
{
	[Node.NodeTintAttribute("#846244")]
	public class GameEventIFNode : Node
	{
		protected override void Init()
		{
			base.Init();
		}

		public override object GetValue(NodePort port)
		{
			return null;
		}

		[Node.InputAttribute(1, 0, 0, false)]
		public Empty enter;

		[Node.OutputAttribute(0, 0, 0, false)]
		public bool ifTrue;

		[Node.OutputAttribute(0, 0, 0, false)]
		public bool ifFalse;

		public IFEnum ifType;

		public OPEnum opType;

		public float num;
	}
}

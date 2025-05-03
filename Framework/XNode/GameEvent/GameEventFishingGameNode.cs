using System;

namespace XNode.GameEvent
{
	[Node.NodeTintAttribute(78, 161, 89)]
	public class GameEventFishingGameNode : Node
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
		public Empty exit;
	}
}

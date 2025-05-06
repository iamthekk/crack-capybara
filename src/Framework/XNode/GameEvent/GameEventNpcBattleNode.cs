using System;

namespace XNode.GameEvent
{
	[Node.NodeTintAttribute(161, 85, 78)]
	public class GameEventNpcBattleNode : Node
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

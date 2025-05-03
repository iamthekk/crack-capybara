using System;

namespace XNode.GameEvent
{
	[Node.NodeTintAttribute(128, 161, 89)]
	public class GameEventTalentSkillNode : Node
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

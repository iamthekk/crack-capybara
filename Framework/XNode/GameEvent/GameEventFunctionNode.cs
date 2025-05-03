using System;
using UnityEngine.Serialization;

namespace XNode.GameEvent
{
	[Node.NodeTintAttribute(178, 80, 60)]
	public class GameEventFunctionNode : Node
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
		public FuncEmpty enter;

		[FormerlySerializedAs("npcOption")]
		public EventFunction eventFunction;

		[FormerlySerializedAs("npcParam")]
		[FormerlySerializedAs("npcId")]
		public string funcParam;
	}
}

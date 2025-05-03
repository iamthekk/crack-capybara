using System;

namespace XNode.GameEvent
{
	[Node.NodeTintAttribute(78, 161, 89)]
	public class GameEventItemNode : Node
	{
		protected override void Init()
		{
			base.Init();
		}

		public override object GetValue(NodePort port)
		{
			return null;
		}

		public void SetElements(GameEventItemNode.ItemStruct[] items)
		{
			this.elements = items;
		}

		[Node.InputAttribute(1, 0, 0, false)]
		public Empty enter;

		[Node.OutputAttribute(0, 0, 0, false)]
		public Empty exit;

		[Node.OutputAttribute(0, 0, 0, false, dynamicPortList = true)]
		public GameEventItemNode.ItemStruct[] elements;

		[Serializable]
		public struct ItemStruct
		{
			public int type;

			public int id;

			public int num;
		}
	}
}

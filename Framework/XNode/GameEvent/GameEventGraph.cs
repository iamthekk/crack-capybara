using System;
using UnityEngine;

namespace XNode.GameEvent
{
	[CreateAssetMenu(fileName = "New Event Graph", menuName = "战斗事件/创建新事件")]
	public class GameEventGraph : NodeGraph
	{
		public string assetName { get; private set; }

		public Node GetRootNode()
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				Node node = this.nodes[i];
				if (node is GameEventNormalNode && node.GetInputPort("enter").GetConnections().Count == 0)
				{
					return node;
				}
			}
			return this.nodes[0];
		}

		public void SetAssetName(string aName)
		{
			this.assetName = aName;
		}
	}
}

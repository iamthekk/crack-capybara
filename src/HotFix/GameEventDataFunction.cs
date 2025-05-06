using System;
using XNode.GameEvent;

namespace HotFix
{
	public class GameEventDataFunction : GameEventData
	{
		public GameEventDataFunction(GameEventPoolData poolData, EventFunction eventFunctionType, string functionParam)
		{
			this.poolData = poolData;
			this.eventFunctionType = eventFunctionType;
			this.functionParam = functionParam;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Function;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Normal;
		}

		public override string GetInfo()
		{
			return "";
		}

		public override GameEventData GetNext(int index)
		{
			if (this.children.Count > 0 && index < this.children.Count)
			{
				GameEventData gameEventData = this.children[index];
				while (gameEventData != null && gameEventData.GetNodeOptionType() == GameEventNodeOptionType.Option)
				{
					gameEventData = gameEventData.GetNext(0);
				}
				return gameEventData;
			}
			return null;
		}

		public EventFunction eventFunctionType;

		public string functionParam;
	}
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotFix
{
	public class GameEventFunctionGetItemServer : GameEventFunctionBase
	{
		public GameEventFunctionGetItemServer(GameEventDataFunction data)
			: base(data)
		{
		}

		public override GameEventFunctionBase.FunctionDoType GetDoType()
		{
			return GameEventFunctionBase.FunctionDoType.ChildTrigger;
		}

		public override int GetDoOrder()
		{
			return 100;
		}

		public override void Create()
		{
			this.itemParamList = this.functionData.GetServerDrop();
		}

		public override async Task DoFunction()
		{
			if (this.itemParamList != null && this.itemParamList.Count > 0)
			{
				Singleton<GameEventController>.Instance.AddDrops(this.itemParamList);
			}
			this.MarkDone();
			await Task.CompletedTask;
		}

		public override List<NodeItemParam> GetShowItems()
		{
			return this.itemParamList;
		}

		private List<NodeItemParam> itemParamList;
	}
}

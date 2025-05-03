using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotFix
{
	public class GameEventFunctionLostAllFood : GameEventFunctionBase
	{
		public GameEventFunctionLostAllFood(GameEventDataFunction data)
			: base(data)
		{
		}

		public override GameEventFunctionBase.FunctionDoType GetDoType()
		{
			return GameEventFunctionBase.FunctionDoType.Immediately;
		}

		public override int GetDoOrder()
		{
			return 100;
		}

		public override void Create()
		{
		}

		public override async Task DoFunction()
		{
			NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.Food, (double)(-(double)Singleton<GameEventController>.Instance.PlayerData.Food), ChapterDropSource.Event, 1);
			Singleton<GameEventController>.Instance.MergerAttribute(new List<NodeAttParam> { nodeAttParam });
			this.MarkDone();
			await Task.CompletedTask;
		}

		public override List<string> GetShowInfos()
		{
			return new List<string> { "UIGameEvent_LostAllFood" };
		}
	}
}

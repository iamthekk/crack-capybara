using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotFix
{
	public class GameEventFunctionLevelUp : GameEventFunctionBase
	{
		public GameEventFunctionLevelUp(GameEventDataFunction data)
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
		}

		public override async Task DoFunction()
		{
			List<NodeAttParam> list = new List<NodeAttParam>();
			NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.Exp, (double)Singleton<GameEventController>.Instance.PlayerData.NextExp, ChapterDropSource.Event, 1);
			list.Add(nodeAttParam);
			Singleton<GameEventController>.Instance.MergerAttribute(list);
			this.MarkDone();
			await Task.CompletedTask;
		}

		public override List<string> GetShowInfos()
		{
			return new List<string> { "UIGameEvent_LevelUp" };
		}
	}
}

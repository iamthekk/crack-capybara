using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class GameEventFunctionGetItem : GameEventFunctionBase
	{
		public GameEventFunctionGetItem(GameEventDataFunction data)
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
			this.itemParamList = base.ToItemParamList(this.functionData.functionParam);
		}

		public override async Task DoFunction()
		{
			if (this.itemParamList != null && this.itemParamList.Count > 0)
			{
				GameTGATools.Ins.AddStageClickTempItem(this.itemParamList, true);
				for (int i = 0; i < this.itemParamList.Count; i++)
				{
					if (this.itemParamList[i].type == NodeItemType.ChapterEvent)
					{
						Singleton<GameEventController>.Instance.AddEventItem(this.itemParamList[i].itemId, (int)this.itemParamList[i].FinalCount, this.functionData.poolData.stage);
					}
				}
			}
			this.MarkDone();
			await Task.CompletedTask;
		}

		public override List<NodeItemParam> GetShowItems()
		{
			List<NodeItemParam> list = new List<NodeItemParam>();
			for (int i = 0; i < this.itemParamList.Count; i++)
			{
				NodeItemParam nodeItemParam = this.itemParamList[i];
				if (nodeItemParam.type == NodeItemType.ChapterEvent)
				{
					Chapter_eventItem elementById = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(nodeItemParam.itemId);
					if (elementById != null && elementById.showUI > 0)
					{
						list.Add(nodeItemParam);
					}
				}
				else
				{
					list.Add(nodeItemParam);
				}
			}
			return list;
		}

		private List<NodeItemParam> itemParamList;
	}
}

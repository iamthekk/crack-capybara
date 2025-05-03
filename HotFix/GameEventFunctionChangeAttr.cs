using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotFix
{
	public class GameEventFunctionChangeAttr : GameEventFunctionBase
	{
		public GameEventFunctionChangeAttr(GameEventDataFunction data)
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
			this.attParamList = base.ToAttributeParamList(this.functionData.functionParam);
		}

		public override async Task DoFunction()
		{
			if (this.attParamList != null && this.attParamList.Count > 0)
			{
				Singleton<GameEventController>.Instance.MergerAttribute(this.attParamList);
				GameTGATools.Ins.AddStageClickTempAtt(this.attParamList, true);
			}
			this.MarkDone();
			await Task.CompletedTask;
		}

		public override List<NodeAttParam> GetShowAttributes()
		{
			return this.attParamList;
		}

		private List<NodeAttParam> attParamList;
	}
}

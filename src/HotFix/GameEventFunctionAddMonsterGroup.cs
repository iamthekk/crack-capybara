using System;
using System.Threading.Tasks;
using HotFix.Client;

namespace HotFix
{
	public class GameEventFunctionAddMonsterGroup : GameEventFunctionBase
	{
		public GameEventFunctionAddMonsterGroup(GameEventDataFunction data)
			: base(data)
		{
		}

		public override GameEventFunctionBase.FunctionDoType GetDoType()
		{
			return GameEventFunctionBase.FunctionDoType.Immediately;
		}

		public override int GetDoOrder()
		{
			return 1;
		}

		public override void Create()
		{
		}

		public override async Task DoFunction()
		{
			int num;
			if (int.TryParse(this.functionData.functionParam, out num) && num > 0)
			{
				await EventMemberController.Instance.EventAddMonsterGroup(num, this.functionData.poolData.atkUpgrade, this.functionData.poolData.hpUpgrade);
			}
			this.MarkDone();
		}
	}
}

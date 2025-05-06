using System;
using System.Threading.Tasks;
using HotFix.Client;

namespace HotFix
{
	public class GameEventFunctionPassingNpc : GameEventFunctionBase
	{
		public GameEventFunctionPassingNpc(GameEventDataFunction data)
			: base(data)
		{
		}

		public override GameEventFunctionBase.FunctionDoType GetDoType()
		{
			return GameEventFunctionBase.FunctionDoType.Immediately;
		}

		public override int GetDoOrder()
		{
			return 10;
		}

		public override void Create()
		{
		}

		public override async Task DoFunction()
		{
			int num;
			if (int.TryParse(this.functionData.functionParam, out num) && num > 0)
			{
				await EventMemberController.Instance.EventAddNpc(num, NpcFunction.PassingNpc, 0);
			}
			this.MarkDone();
		}
	}
}

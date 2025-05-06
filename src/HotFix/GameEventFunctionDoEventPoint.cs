using System;
using System.Threading.Tasks;
using HotFix.Client;

namespace HotFix
{
	public class GameEventFunctionDoEventPoint : GameEventFunctionBase
	{
		public GameEventFunctionDoEventPoint(GameEventDataFunction data)
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
			if (int.TryParse(this.functionData.functionParam, out num))
			{
				await EventMemberController.Instance.DoEventPoint(num);
			}
			this.MarkDone();
		}
	}
}

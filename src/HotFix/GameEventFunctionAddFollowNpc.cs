using System;
using System.Threading.Tasks;
using HotFix.Client;

namespace HotFix
{
	public class GameEventFunctionAddFollowNpc : GameEventFunctionBase
	{
		public GameEventFunctionAddFollowNpc(GameEventDataFunction data)
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
			string[] array = this.functionData.functionParam.Split('|', StringSplitOptions.None);
			int num;
			if (array.Length != 0 && int.TryParse(array[0], out num) && num > 0)
			{
				int num2 = 0;
				int num3;
				if (array.Length > 1 && !string.IsNullOrEmpty(array[1]) && int.TryParse(array[1], out num3))
				{
					num2 = num3;
				}
				await EventMemberController.Instance.EventAddNpc(num, NpcFunction.FollowPlayer, num2);
			}
			this.MarkDone();
		}
	}
}

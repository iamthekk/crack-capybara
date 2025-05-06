using System;
using System.Threading.Tasks;
using HotFix.Client;

namespace HotFix
{
	public class GameEventFunctionMonsterGroupLeave : GameEventFunctionBase
	{
		public GameEventFunctionMonsterGroupLeave(GameEventDataFunction data)
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
			EventMemberController.Instance.MonsterGroupLeave();
			this.MarkDone();
			await Task.CompletedTask;
		}
	}
}

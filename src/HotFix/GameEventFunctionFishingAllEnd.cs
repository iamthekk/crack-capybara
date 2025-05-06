using System;
using System.Threading.Tasks;
using Framework;

namespace HotFix
{
	public class GameEventFunctionFishingAllEnd : GameEventFunctionBase
	{
		public GameEventFunctionFishingAllEnd(GameEventDataFunction data)
			: base(data)
		{
		}

		public override GameEventFunctionBase.FunctionDoType GetDoType()
		{
			return GameEventFunctionBase.FunctionDoType.Immediately;
		}

		public override int GetDoOrder()
		{
			return 100000;
		}

		public override void Create()
		{
		}

		public override async Task DoFunction()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_Fishing_AllFinish, null);
			this.MarkDone();
			await Task.CompletedTask;
		}
	}
}

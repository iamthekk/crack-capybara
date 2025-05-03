using System;
using System.Threading.Tasks;
using Framework;

namespace HotFix
{
	public class GameEventFunctionEmoticons : GameEventFunctionBase
	{
		public GameEventFunctionEmoticons(GameEventDataFunction data)
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
			EventArgsString eventArgsString = new EventArgsString();
			eventArgsString.SetData(this.functionData.functionParam);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowEmoticons, eventArgsString);
			this.MarkDone();
			await Task.CompletedTask;
		}
	}
}

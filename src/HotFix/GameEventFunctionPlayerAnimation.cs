using System;
using System.Threading.Tasks;
using Framework;

namespace HotFix
{
	public class GameEventFunctionPlayerAnimation : GameEventFunctionBase
	{
		public GameEventFunctionPlayerAnimation(GameEventDataFunction data)
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
			EventArgsEventPlayMemebrAni eventArgsEventPlayMemebrAni = new EventArgsEventPlayMemebrAni();
			eventArgsEventPlayMemebrAni.SetData(this.functionData.functionParam);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_PlayMainMemberAni, eventArgsEventPlayMemebrAni);
			this.MarkDone();
			await Task.CompletedTask;
		}
	}
}

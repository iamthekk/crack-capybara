using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HotFix
{
	public class GameEventFunctionRandomBox : GameEventFunctionBase
	{
		public GameEventFunctionRandomBox(GameEventDataFunction data)
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

		public override Task DoFunction()
		{
			GameEventFunctionRandomBox.<DoFunction>d__4 <DoFunction>d__;
			<DoFunction>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DoFunction>d__.<>4__this = this;
			<DoFunction>d__.<>1__state = -1;
			<DoFunction>d__.<>t__builder.Start<GameEventFunctionRandomBox.<DoFunction>d__4>(ref <DoFunction>d__);
			return <DoFunction>d__.<>t__builder.Task;
		}
	}
}

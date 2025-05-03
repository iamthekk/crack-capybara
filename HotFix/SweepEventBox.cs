using System;
using System.Threading.Tasks;

namespace HotFix
{
	public class SweepEventBox : SweepEventBase
	{
		public override void OnInit()
		{
			base.GoNext(0f);
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
		}

		protected override void ShowUI()
		{
		}
	}
}

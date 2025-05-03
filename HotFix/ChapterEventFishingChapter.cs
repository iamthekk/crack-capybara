using System;
using System.Threading.Tasks;
using Framework;

namespace HotFix
{
	public class ChapterEventFishingChapter : ChapterEventBase
	{
		public override void OnInit()
		{
			FishingViewModule.OpenData openData = new FishingViewModule.OpenData();
			openData.seed = this.currentData.poolData.randomSeed;
			GameApp.View.OpenView(ViewName.FishingViewModule, openData, 1, null, null);
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			if (pushType == GameEventPushType.CloseFishingGame)
			{
				base.MarkDone();
				base.OnClickScroll();
			}
		}

		protected override void ShowUI()
		{
		}
	}
}

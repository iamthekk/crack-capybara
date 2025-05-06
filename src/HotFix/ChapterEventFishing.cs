using System;
using System.Threading.Tasks;
using Framework;

namespace HotFix
{
	public class ChapterEventFishing : ChapterEventBase
	{
		public override void OnInit()
		{
			GameEventDataFishing gameEventDataFishing = this.currentData as GameEventDataFishing;
			if (gameEventDataFishing != null)
			{
				EventArgEventPause eventArgEventPause = new EventArgEventPause();
				eventArgEventPause.SetData(true);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, eventArgEventPause);
				EventArgFishing eventArgFishing = new EventArgFishing();
				eventArgFishing.SetData(gameEventDataFishing.npcId, gameEventDataFishing.fishType, this.currentData.poolData.atkUpgrade, this.currentData.poolData.hpUpgrade);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_Fishing_Enter, eventArgFishing);
				return;
			}
			HLog.LogError("[GameEventFishing] not found [GameEventDataFishing] data");
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			if (pushType == GameEventPushType.CloseFishingResult)
			{
				EventArgEventPause eventArgEventPause = new EventArgEventPause();
				eventArgEventPause.SetData(false);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, eventArgEventPause);
				base.MarkDone();
				base.OnClickScroll();
			}
		}

		protected override void ShowUI()
		{
		}
	}
}

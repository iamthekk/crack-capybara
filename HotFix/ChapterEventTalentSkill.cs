using System;
using System.Threading.Tasks;
using Framework;

namespace HotFix
{
	public class ChapterEventTalentSkill : ChapterEventBase
	{
		public override void OnInit()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_SelectBeginSkill, null);
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			if (pushType == GameEventPushType.CloseSelectSkill)
			{
				this.ShowUI();
				this.group.SetCurrentData(null);
				base.MarkDone();
			}
		}

		protected override void ShowUI()
		{
		}
	}
}

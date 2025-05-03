using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;

namespace HotFix
{
	public class ChapterEventRoundSkill : ChapterEventBase
	{
		public override void OnInit()
		{
			GameEventDataRoundSkill gameEventDataRoundSkill = this.currentData as GameEventDataRoundSkill;
			if (gameEventDataRoundSkill == null)
			{
				return;
			}
			RoundSelectSkillViewModule.OpenData openData = new RoundSelectSkillViewModule.OpenData();
			openData.Seed = gameEventDataRoundSkill.poolData.randomSeed;
			openData.SourceType = (SkillBuildSourceType)gameEventDataRoundSkill.sourceId;
			openData.TotalRound = gameEventDataRoundSkill.round;
			openData.RandomSkillNum = gameEventDataRoundSkill.randomNum;
			openData.SelectSkillNum = gameEventDataRoundSkill.selectNum;
			openData.OnSaveSkillAction = new Action<Dictionary<int, int[]>>(this.OnSaveSkillAction);
			GameApp.View.OpenView(ViewName.RoundSelectSkillViewModule, openData, 1, null, null);
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
				if (this.currentData.poolData.stage == 1)
				{
					Singleton<EventRecordController>.Instance.SavePlayerData();
				}
				base.MarkDone();
				await this.OnClickButton(0);
			}
		}

		protected override void ShowUI()
		{
		}

		private void OnSaveSkillAction(Dictionary<int, int[]> obj)
		{
		}
	}
}

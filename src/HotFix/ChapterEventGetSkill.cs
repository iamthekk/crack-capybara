using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;

namespace HotFix
{
	public class ChapterEventGetSkill : ChapterEventBase
	{
		public override void OnInit()
		{
			GameEventDataSkill gameEventDataSkill = this.currentData as GameEventDataSkill;
			if (gameEventDataSkill == null)
			{
				return;
			}
			if (gameEventDataSkill.skillBuildId > 0)
			{
				GetSkillViewModule.OpenData openData = new GetSkillViewModule.OpenData();
				openData.skillBuildId = gameEventDataSkill.skillBuildId;
				GameApp.View.OpenView(ViewName.GetSkillViewModule, openData, 1, null, null);
				return;
			}
			if (gameEventDataSkill.randomNum == 1)
			{
				if (gameEventDataSkill.sourceId == 0)
				{
					gameEventDataSkill.sourceId = 1;
					HLog.LogError(string.Format("事件指定的技能随机池为0, id={0}", gameEventDataSkill.poolData.tableId));
				}
				GetSkillViewModule.OpenData openData2 = new GetSkillViewModule.OpenData();
				openData2.sourceType = (SkillBuildSourceType)gameEventDataSkill.sourceId;
				openData2.seed = gameEventDataSkill.poolData.randomSeed;
				GameApp.View.OpenView(ViewName.GetSkillViewModule, openData2, 1, null, null);
				return;
			}
			if (gameEventDataSkill.sourceId == 0)
			{
				gameEventDataSkill.sourceId = 1;
				HLog.LogError(string.Format("事件指定的技能随机池为0, id={0}", gameEventDataSkill.poolData.tableId));
			}
			SelectSkillViewModule.OpenData openData3 = new SelectSkillViewModule.OpenData();
			openData3.type = SelectSkillViewModule.SelectSkillType.GetSkill;
			openData3.sourceType = (SkillBuildSourceType)gameEventDataSkill.sourceId;
			openData3.randomNum = gameEventDataSkill.randomNum;
			openData3.selectNum = gameEventDataSkill.selectNum;
			openData3.randomSeed = gameEventDataSkill.poolData.randomSeed;
			if (openData3.randomNum == 0)
			{
				HLog.LogError(string.Format("配置的随机技能数量为0，请立即检查，id={0}", gameEventDataSkill.poolData.tableId));
				openData3.randomNum = 1;
			}
			if (openData3.selectNum > openData3.randomNum)
			{
				HLog.LogError(string.Format("技能事件配置的随机技能数量大于选择技能数量，请立即检查，id={0}", gameEventDataSkill.poolData.tableId));
				openData3.selectNum = 1;
			}
			GameApp.View.OpenView(ViewName.SelectSkillViewModule, openData3, 1, null, null);
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			if (pushType == GameEventPushType.CloseSelectSkill)
			{
				List<GameEventSkillBuildData> list = param as List<GameEventSkillBuildData>;
				if (list != null)
				{
					this.skills = list;
				}
				this.ShowUI();
				this.group.SetCurrentData(null);
				if (this.currentData.poolData.stage == 1)
				{
					Singleton<EventRecordController>.Instance.SavePlayerData();
				}
				base.MarkDone();
			}
		}

		protected override void ShowUI()
		{
			if (this.skills == null || this.skills.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.skills.Count; i++)
			{
				GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, "GameEventData_35", new object[] { this.skills[i].skillName }, null, null, null, null, null);
				EventArgAddEvent eventArgAddEvent = new EventArgAddEvent();
				eventArgAddEvent.uiData = gameEventUIData;
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent);
			}
		}

		private List<GameEventSkillBuildData> skills;
	}
}

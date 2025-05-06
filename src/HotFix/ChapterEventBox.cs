using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using HotFix.Client;
using LocalModels.Bean;

namespace HotFix
{
	public class ChapterEventBox : ChapterEventBase
	{
		public override void OnInit()
		{
			EventArgRandomBox instance = Singleton<EventArgRandomBox>.Instance;
			GameEventBoxViewModule.RandomBoxData randomBoxData = new GameEventBoxViewModule.RandomBoxData();
			int currentBoxId = Singleton<GameEventController>.Instance.GetCurrentBoxId();
			Chapter_boxBuild chapter_boxBuild = GameApp.Table.GetManager().GetChapter_boxBuild(currentBoxId);
			if (chapter_boxBuild != null)
			{
				randomBoxData.memberId = chapter_boxBuild.memberId;
				switch (currentBoxId)
				{
				case 1:
					randomBoxData.sourceType = SkillBuildSourceType.BoxBronze;
					break;
				case 2:
					randomBoxData.sourceType = SkillBuildSourceType.BoxSilver;
					break;
				case 3:
					randomBoxData.sourceType = SkillBuildSourceType.BoxGold;
					break;
				}
			}
			randomBoxData.seed = this.currentData.poolData.randomSeed;
			instance.SetData(randomBoxData);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowBoxUI, instance);
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			if (pushType == GameEventPushType.CloseBox)
			{
				List<GameEventSkillBuildData> list = param as List<GameEventSkillBuildData>;
				if (list != null)
				{
					this.skills = list;
				}
				this.ShowUI();
				this.group.SetCurrentData(null);
				int currentBoxId = Singleton<GameEventController>.Instance.GetCurrentBoxId();
				if (GameApp.Table.GetManager().GetChapter_boxBuildModelInstance().GetElementById(currentBoxId) == null)
				{
					HLog.LogError(string.Format("Not found Chapter_boxBuild table, id ={0}", currentBoxId));
				}
				else
				{
					await EventMemberController.Instance.RemoveBox();
					base.MarkDone();
				}
			}
		}

		protected override void ShowUI()
		{
			GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, "UIGameEventBox_72", null, null, null, null, null, null);
			EventArgAddEvent eventArgAddEvent = new EventArgAddEvent();
			eventArgAddEvent.uiData = gameEventUIData;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent);
			if (this.skills == null || this.skills.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.skills.Count; i++)
			{
				GameEventUIData gameEventUIData2 = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, "GameEventData_35", new object[] { this.skills[i].skillName }, null, null, null, null, null);
				EventArgAddEvent eventArgAddEvent2 = new EventArgAddEvent();
				eventArgAddEvent2.uiData = gameEventUIData2;
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent2);
			}
		}

		private List<GameEventSkillBuildData> skills;
	}
}

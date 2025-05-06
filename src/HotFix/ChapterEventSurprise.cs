using System;
using System.Threading.Tasks;
using Framework;

namespace HotFix
{
	public class ChapterEventSurprise : ChapterEventBase
	{
		public override void OnInit()
		{
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			this.eventParam = param;
			if (pushType - GameEventPushType.CloseAngel > 4)
			{
				if (pushType == GameEventPushType.SurpriseNpcActionFinish)
				{
					EventArgSurprise instance = Singleton<EventArgSurprise>.Instance;
					instance.SetData(Singleton<GameEventController>.Instance.GetCurrentSurpriseId(), this.currentData.poolData.randomSeed);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowSurpriseUI, instance);
				}
			}
			else
			{
				this.eventPushType = pushType;
				this.ShowUI();
				this.group.SetCurrentData(null);
				base.MarkDone();
			}
		}

		protected override void ShowUI()
		{
			string text = "";
			object[] array = null;
			switch (this.eventPushType)
			{
			case GameEventPushType.CloseAngel:
			{
				GameEventSkillBuildData gameEventSkillBuildData = this.eventParam as GameEventSkillBuildData;
				if (gameEventSkillBuildData != null)
				{
					text = "GameEventSurprise_103";
					array = new object[] { gameEventSkillBuildData.skillName };
				}
				else
				{
					object obj = this.eventParam;
					if (obj is long)
					{
						long num = (long)obj;
						text = "GameEventSurprise_102";
						array = new object[] { num };
					}
				}
				break;
			}
			case GameEventPushType.CloseDemon:
			{
				GameEventSkillBuildData gameEventSkillBuildData2 = this.eventParam as GameEventSkillBuildData;
				if (gameEventSkillBuildData2 != null)
				{
					text = "GameEventSurprise_104";
					array = new object[] { gameEventSkillBuildData2.skillName };
				}
				else
				{
					text = "GameEventSurprise_105";
				}
				break;
			}
			case GameEventPushType.CloseAdventurer:
			{
				NodeAttParam nodeAttParam = this.eventParam as NodeAttParam;
				if (nodeAttParam != null)
				{
					string attName = NodeAttParam.GetAttName(nodeAttParam.attType, (float)((int)nodeAttParam.baseCount));
					text = "GameEventSurprise_106";
					string text2 = "";
					if (nodeAttParam.IsRate())
					{
						text2 = "%";
					}
					array = new object[]
					{
						attName,
						nodeAttParam.baseCount.ToString() + text2
					};
				}
				break;
			}
			case GameEventPushType.CloseSlot:
			{
				string text3 = "GameEventSurprise_109";
				object obj = this.eventParam;
				if (obj is bool)
				{
					bool flag = (bool)obj;
					if (flag)
					{
						text3 = "GameEventSurprise_108";
					}
				}
				text = text3;
				break;
			}
			case GameEventPushType.CloseSlotTrain:
				text = "GameEventSurprise_109";
				break;
			}
			GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, text, array, null, null, null, null, null);
			EventArgAddEvent eventArgAddEvent = new EventArgAddEvent();
			eventArgAddEvent.uiData = gameEventUIData;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent);
		}

		private GameEventPushType eventPushType;

		private object eventParam;
	}
}

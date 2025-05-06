using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;

namespace HotFix
{
	public class GameEventFunctionRandomSkill : GameEventFunctionBase
	{
		public int sourceId { get; private set; }

		public int randomNum { get; private set; }

		public int selectNum { get; private set; }

		public GameEventFunctionRandomSkill(GameEventDataFunction data)
			: base(data)
		{
		}

		public override GameEventFunctionBase.FunctionDoType GetDoType()
		{
			return GameEventFunctionBase.FunctionDoType.ChildTrigger;
		}

		public override int GetDoOrder()
		{
			return 10000;
		}

		public override void Create()
		{
			string[] array = this.functionData.functionParam.Split('|', StringSplitOptions.None);
			int num;
			int num2;
			int num3;
			if (array.Length >= 3 && int.TryParse(array[0], out num) && int.TryParse(array[1], out num2) && int.TryParse(array[2], out num3))
			{
				this.sourceId = num;
				this.randomNum = num2;
				this.selectNum = num3;
			}
			if (this.randomNum == 0 || this.selectNum > this.randomNum)
			{
				HLog.LogError(string.Format("技能参数错误，eventId={0}", this.functionData.poolData.tableId));
				this.randomNum = 3;
				this.selectNum = 1;
			}
			if (this.sourceId == 0)
			{
				HLog.LogError(string.Format("配置的技能池为0，eventId={0}", this.functionData.poolData.tableId));
				this.sourceId = 1;
			}
		}

		public override async Task DoFunction()
		{
			if (this.randomNum > this.selectNum)
			{
				GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CloseSelectSkill, new HandlerEvent(this.OnEventCloseSelectSkill));
				SelectSkillViewModule.OpenData openData = new SelectSkillViewModule.OpenData();
				openData.type = SelectSkillViewModule.SelectSkillType.GetSkill;
				openData.sourceType = (SkillBuildSourceType)this.sourceId;
				openData.randomNum = this.randomNum;
				openData.selectNum = this.selectNum;
				openData.randomSeed = this.functionData.poolData.randomSeed;
				GameApp.View.OpenView(ViewName.SelectSkillViewModule, openData, 1, null, null);
			}
			this.MarkDone();
			await Task.CompletedTask;
		}

		private void OnEventCloseSelectSkill(object sender, int type, BaseEventArgs eventArgs)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_CloseSelectSkill, new HandlerEvent(this.OnEventCloseSelectSkill));
			EventArgSelectSkillEnd eventArgSelectSkillEnd = eventArgs as EventArgSelectSkillEnd;
			if (eventArgSelectSkillEnd != null)
			{
				List<GameEventSkillBuildData> skills = eventArgSelectSkillEnd.skills;
				for (int i = 0; i < skills.Count; i++)
				{
					GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(0, this.functionData.poolData.stage, false, "GameEventData_35", new object[] { skills[i].skillName }, null, null, null, null, null);
					EventArgAddEvent eventArgAddEvent = new EventArgAddEvent();
					eventArgAddEvent.uiData = gameEventUIData;
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent);
				}
			}
		}
	}
}

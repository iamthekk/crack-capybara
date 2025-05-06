using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;

namespace HotFix
{
	public class GameEventFunctionGetSkill : GameEventFunctionBase
	{
		public int skillBuildId { get; private set; }

		public GameEventFunctionGetSkill(GameEventDataFunction data)
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
			int num;
			if (int.TryParse(this.functionData.functionParam, out num))
			{
				this.skillBuildId = num;
			}
		}

		public override async Task DoFunction()
		{
			if (this.skillBuildId > 0)
			{
				GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CloseSelectSkill, new HandlerEvent(this.OnEventCloseSelectSkill));
				GetSkillViewModule.OpenData openData = new GetSkillViewModule.OpenData();
				openData.skillBuildId = this.skillBuildId;
				GameApp.View.OpenView(ViewName.GetSkillViewModule, openData, 1, null, null);
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

		public override List<NodeSkillParam> GetShowSkills()
		{
			List<NodeSkillParam> list = new List<NodeSkillParam>();
			if (this.skillBuildId > 0)
			{
				list.Add(new NodeSkillParam
				{
					skillBuildId = this.skillBuildId,
					isLost = false
				});
			}
			return list;
		}
	}
}

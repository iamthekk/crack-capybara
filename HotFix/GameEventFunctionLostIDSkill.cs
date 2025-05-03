using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotFix
{
	public class GameEventFunctionLostIDSkill : GameEventFunctionBase
	{
		public GameEventSkillBuildData lostSkill { get; private set; }

		public GameEventFunctionLostIDSkill(GameEventDataFunction data)
			: base(data)
		{
		}

		public override GameEventFunctionBase.FunctionDoType GetDoType()
		{
			return GameEventFunctionBase.FunctionDoType.ChildTrigger;
		}

		public override int GetDoOrder()
		{
			return 1000;
		}

		public override void Create()
		{
			int num;
			if (int.TryParse(this.functionData.functionParam, out num))
			{
				this.lostSkill = Singleton<GameEventController>.Instance.GetSkillByID(num);
			}
		}

		public override async Task DoFunction()
		{
			if (this.lostSkill != null && Singleton<GameEventController>.Instance.IsHaveSkillBuild(this.lostSkill.id))
			{
				Singleton<GameEventController>.Instance.LostSkill(this.lostSkill);
			}
			this.MarkDone();
			await Task.CompletedTask;
		}

		public override List<NodeSkillParam> GetShowSkills()
		{
			List<NodeSkillParam> list = new List<NodeSkillParam>();
			if (this.lostSkill != null)
			{
				list.Add(new NodeSkillParam
				{
					skillBuildId = this.lostSkill.id,
					isLost = true
				});
			}
			return list;
		}

		public override bool IsUndo()
		{
			return this.lostSkill == null || !Singleton<GameEventController>.Instance.IsHaveSkillBuild(this.lostSkill.id);
		}

		public override string GetUndoTip()
		{
			if (this.lostSkill != null)
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_NoLostSkill", new object[] { this.lostSkill.skillName });
			}
			return "";
		}
	}
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotFix
{
	public class GameEventFunctionLostTagSkills : GameEventFunctionBase
	{
		public GameEventFunctionLostTagSkills(GameEventDataFunction data)
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
				this.lostSkills = Singleton<GameEventController>.Instance.GetLostSkills(num);
			}
		}

		public override async Task DoFunction()
		{
			if (this.lostSkills != null)
			{
				for (int i = 0; i < this.lostSkills.Count; i++)
				{
					Singleton<GameEventController>.Instance.LostSkill(this.lostSkills[i]);
				}
			}
			this.MarkDone();
			await Task.CompletedTask;
		}

		public override List<NodeSkillParam> GetShowSkills()
		{
			List<NodeSkillParam> list = new List<NodeSkillParam>();
			if (this.lostSkills != null)
			{
				for (int i = 0; i < this.lostSkills.Count; i++)
				{
					list.Add(new NodeSkillParam
					{
						skillBuildId = this.lostSkills[i].id,
						isLost = true
					});
				}
			}
			return list;
		}

		private List<GameEventSkillBuildData> lostSkills;
	}
}

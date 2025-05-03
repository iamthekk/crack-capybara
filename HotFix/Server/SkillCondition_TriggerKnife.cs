using System;

namespace Server
{
	public class SkillCondition_TriggerKnife : ConditionBase
	{
		public override void OnInit()
		{
		}

		public override void OnDeInit()
		{
		}

		public override void SetParameter(string parameter)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_TriggerKnife.Data>(parameter) : new SkillCondition_TriggerKnife.Data());
		}

		public override bool IsMatchCondition()
		{
			bool flag = false;
			if (this.m_data.triggerType.Equals(1))
			{
				flag = this.m_curSkill.Owner.memberData.attribute.IsBattleStartKnife > 0;
			}
			else if (this.m_data.triggerType.Equals(2))
			{
				flag = this.m_curSkill.Owner.memberData.attribute.IsRoundStartKnife > 0;
			}
			return flag;
		}

		public SkillCondition_TriggerKnife.Data m_data;

		[Serializable]
		public class Data
		{
			public int triggerType = -1;
		}
	}
}

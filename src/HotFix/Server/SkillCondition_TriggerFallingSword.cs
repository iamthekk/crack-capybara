using System;

namespace Server
{
	public class SkillCondition_TriggerFallingSword : ConditionBase
	{
		public override void OnInit()
		{
		}

		public override void OnDeInit()
		{
		}

		public override void SetParameter(string parameter)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_TriggerFallingSword.Data>(parameter) : new SkillCondition_TriggerFallingSword.Data());
		}

		public override bool IsMatchCondition()
		{
			bool flag = false;
			if (this.m_data.triggerType.Equals(1))
			{
				flag = this.m_curSkill.Owner.memberData.attribute.IsBattleStartFallingSword > 0;
			}
			else if (this.m_data.triggerType.Equals(2))
			{
				flag = this.m_curSkill.Owner.memberData.attribute.IsRoundStartFallingSword > 0;
			}
			else if (this.m_data.triggerType.Equals(3))
			{
				flag = this.m_curSkill.Owner.memberData.attribute.IsBigSkillAfterFallingSword > 0;
			}
			return flag;
		}

		public SkillCondition_TriggerFallingSword.Data m_data;

		[Serializable]
		public class Data
		{
			public int triggerType = -1;
		}
	}
}

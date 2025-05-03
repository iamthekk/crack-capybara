using System;

namespace Server
{
	public class SkillCondition_ComboCount : ConditionBase
	{
		public override void OnInit()
		{
		}

		public override void OnDeInit()
		{
			this.m_data = null;
		}

		public override void SetParameter(string parameter)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_ComboCount.Data>(parameter) : new SkillCondition_ComboCount.Data());
		}

		public override bool IsMatchCondition()
		{
			return this.m_curSkill.Owner.memberData.CurComboCount >= this.m_data.comboCount;
		}

		public SkillCondition_ComboCount.Data m_data;

		[Serializable]
		public class Data
		{
			public int comboCount;
		}
	}
}

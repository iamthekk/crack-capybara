using System;

namespace Server
{
	public class SkillCondition_Probability : ConditionBase
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
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_Probability.Data>(parameter) : new SkillCondition_Probability.Data());
		}

		public override bool IsMatchCondition()
		{
			return this.m_curSkill.Owner.m_controller.IsMatchProbability(this.m_data.probability);
		}

		public SkillCondition_Probability.Data m_data;

		[Serializable]
		public class Data
		{
			public float probability;
		}
	}
}

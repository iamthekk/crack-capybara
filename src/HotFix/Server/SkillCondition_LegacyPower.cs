using System;

namespace Server
{
	public class SkillCondition_LegacyPower : ConditionBase
	{
		public override void OnInit()
		{
		}

		public override void OnDeInit()
		{
		}

		public override void SetParameter(string parameter)
		{
		}

		public override bool IsMatchCondition()
		{
			int id = this.m_curSkill.skillData.m_id;
			FP curLegacyPower = this.m_curSkill.Owner.memberData.GetCurLegacyPower(id);
			FP maxLegacyPower = this.m_curSkill.Owner.memberData.GetMaxLegacyPower(id);
			return maxLegacyPower > 0 && curLegacyPower >= maxLegacyPower;
		}
	}
}

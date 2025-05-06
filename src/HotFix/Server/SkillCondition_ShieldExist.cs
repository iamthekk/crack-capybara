using System;

namespace Server
{
	public class SkillCondition_ShieldExist : ConditionBase
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
			return this.m_curSkill.Owner.memberData.CurShield > FP._0;
		}

		public override void OnRefresh()
		{
		}
	}
}

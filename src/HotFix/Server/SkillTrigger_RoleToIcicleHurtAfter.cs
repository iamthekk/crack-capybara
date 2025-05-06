using System;

namespace Server
{
	public class SkillTrigger_RoleToIcicleHurtAfter : BaseSkillTrigger
	{
		public override int GetName()
		{
			return 31;
		}

		public override bool IsCanPlay(SkillTriggerData triggerData)
		{
			return this.m_skill != null && this.m_skill.CurCD <= 0 && base.IsMatchComtitions() && this.m_skill.Owner != null && !this.m_skill.Owner.IsDeath;
		}
	}
}

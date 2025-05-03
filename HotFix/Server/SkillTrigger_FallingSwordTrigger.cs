using System;

namespace Server
{
	public class SkillTrigger_FallingSwordTrigger : BaseSkillTrigger
	{
		public override int GetName()
		{
			return 113;
		}

		public override bool IsCanPlay(SkillTriggerData triggerData)
		{
			return this.m_skill != null && this.m_skill.CurCD <= 0 && base.IsMatchComtitions() && this.m_skill.Owner != null && !this.m_skill.Owner.IsDeath;
		}
	}
}

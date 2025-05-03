using System;

namespace Server
{
	public class SkillTrigger_HitByHurtedBefore : BaseSkillTrigger
	{
		public override int GetName()
		{
			return 60;
		}

		public override bool IsCanPlay(SkillTriggerData triggerData)
		{
			return this.m_skill != null && this.m_skill.CurCD <= 0 && this.m_skill.Owner != null && !this.m_skill.Owner.IsDeath && base.IsMatchComtitions();
		}
	}
}

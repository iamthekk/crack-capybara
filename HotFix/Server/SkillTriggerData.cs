using System;
using System.Collections.Generic;

namespace Server
{
	public class SkillTriggerData
	{
		public SkillTriggerType m_triggerType { get; set; }

		public SSkillBase m_iHitTargetSkill { get; private set; }

		public SBuffBase m_iHitTargetBuff { get; private set; }

		public SMemberBase m_attacker { get; private set; }

		public void SetTarget(SMemberBase target)
		{
			this.m_iHitTargetList = new List<SMemberBase> { target };
		}

		public void SetTriggerSkill(SSkillBase skill)
		{
			if (skill != null)
			{
				this.m_iHitTargetSkill = skill;
				this.m_attacker = skill.Owner;
			}
		}

		public void SetAttacker(SMemberBase attacker)
		{
			this.m_attacker = attacker;
		}

		public void SetTriggerBuff(SBuffBase buff)
		{
			if (buff != null)
			{
				this.m_iHitTargetBuff = buff;
				this.m_attacker = buff.m_owner;
			}
		}

		public List<SMemberBase> m_iHitTargetList = new List<SMemberBase>();

		public object m_parameter;
	}
}

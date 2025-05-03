using System;
using System.Collections.Generic;

namespace Server
{
	public class SkillSelectAddHitMeMemberForTriggerData : ISkillSelect
	{
		public void MathSelectTarget(SkillTriggerData triggerData, Dictionary<int, SMemberBase> allMembers, SMemberBase owner, SSkillBase skill, ref List<SMemberBase> targetMembers)
		{
			if (allMembers == null)
			{
				return;
			}
			if (owner == null)
			{
				return;
			}
			if (skill == null)
			{
				return;
			}
			targetMembers.Clear();
			if (triggerData == null)
			{
				return;
			}
			if (triggerData.m_attacker == null)
			{
				return;
			}
			if (triggerData.m_attacker.memberData.cardData.IsPet)
			{
				return;
			}
			targetMembers.Add(triggerData.m_attacker);
		}
	}
}

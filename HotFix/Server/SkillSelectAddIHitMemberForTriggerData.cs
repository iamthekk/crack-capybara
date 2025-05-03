using System;
using System.Collections.Generic;

namespace Server
{
	public class SkillSelectAddIHitMemberForTriggerData : ISkillSelect
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
			if (triggerData == null)
			{
				return;
			}
			if (triggerData.m_iHitTargetList == null)
			{
				return;
			}
			targetMembers.Clear();
			for (int i = 0; i < triggerData.m_iHitTargetList.Count; i++)
			{
				SMemberBase smemberBase = triggerData.m_iHitTargetList[i];
				if (smemberBase != null && !smemberBase.IsDeath && smemberBase.memberData.Camp != owner.memberData.Camp)
				{
					targetMembers.Add(smemberBase);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;

namespace Server
{
	public class SkillSelectAddFriendly : ISkillSelect
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
			foreach (KeyValuePair<int, SMemberBase> keyValuePair in allMembers)
			{
				SMemberBase value = keyValuePair.Value;
				if (!value.memberData.IsDeath && owner.memberData.Camp == value.memberData.Camp && !value.memberData.cardData.IsPet && value.m_instanceId != owner.m_instanceId)
				{
					targetMembers.Add(value);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;

namespace Server
{
	public class SkillSelectAddLocal : ISkillSelect
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
			targetMembers.Add(owner);
		}
	}
}

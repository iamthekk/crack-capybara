using System;
using System.Collections.Generic;

namespace Server
{
	public interface ISkillSelect
	{
		void MathSelectTarget(SkillTriggerData triggerData, Dictionary<int, SMemberBase> allMembers, SMemberBase owner, SSkillBase skill, ref List<SMemberBase> targetMembers);
	}
}

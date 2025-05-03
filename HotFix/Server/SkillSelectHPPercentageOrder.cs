using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
	public class SkillSelectHPPercentageOrder : ISkillSelect
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
			targetMembers = targetMembers.OrderBy((SMemberBase c) => this.MathPercentage(c)).ToList<SMemberBase>();
		}

		public FP MathPercentage(SMemberBase member)
		{
			return member.memberData.HPPercent;
		}
	}
}

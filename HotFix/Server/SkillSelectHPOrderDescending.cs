﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
	public class SkillSelectHPOrderDescending : ISkillSelect
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
			targetMembers = targetMembers.OrderByDescending((SMemberBase c) => c.memberData.CurHP).ToList<SMemberBase>();
		}
	}
}

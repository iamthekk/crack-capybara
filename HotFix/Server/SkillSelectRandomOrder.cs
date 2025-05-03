using System;
using System.Collections.Generic;

namespace Server
{
	public class SkillSelectRandomOrder : ISkillSelect
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
			targetMembers = this.RandomSort(targetMembers);
		}

		private List<SMemberBase> RandomSort(List<SMemberBase> list)
		{
			List<SMemberBase> list2 = new List<SMemberBase>();
			for (int i = 0; i < list.Count; i++)
			{
				SMemberBase smemberBase = list[i];
				if (!smemberBase.memberData.cardData.IsPet)
				{
					list2.Add(smemberBase);
				}
			}
			List<SMemberBase> list3 = new List<SMemberBase>(list2);
			int j = 0;
			int count = list3.Count;
			while (j < count)
			{
				SMemberBase smemberBase2 = list3[j];
				list3.RemoveAt(j);
				int num = this.random.Next(0, list3.Count);
				list3.Insert(num, smemberBase2);
				j++;
			}
			return list3;
		}

		private Random random = new Random();
	}
}

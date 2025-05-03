using System;
using System.Collections.Generic;

namespace Server
{
	[Serializable]
	public class EquipAction
	{
		public List<MergeAttributeData> GetMergeAttributeData()
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			if (this.attr == null || this.attr.Length == 0)
			{
				return list;
			}
			return this.attr.GetMergeAttributeData();
		}

		public string[] attr = new string[0];

		public int rageSkill;

		public int[] skillIds = new int[0];
	}
}

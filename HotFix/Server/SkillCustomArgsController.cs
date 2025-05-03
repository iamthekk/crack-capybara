using System;
using System.Collections.Generic;

namespace Server
{
	public class SkillCustomArgsController
	{
		public int GetCount(int skillId)
		{
			int num;
			if (this.m_dic.TryGetValue(skillId, out num))
			{
				return num;
			}
			return 0;
		}

		public int AddCount(int skillId)
		{
			int num;
			if (this.m_dic.TryGetValue(skillId, out num))
			{
				num++;
				this.m_dic[skillId] = num;
				return num;
			}
			this.m_dic.Add(skillId, 1);
			return 1;
		}

		public void RefreshByServer(Dictionary<int, int> list)
		{
			this.m_dic = list;
		}

		private Dictionary<int, int> m_dic = new Dictionary<int, int>();
	}
}

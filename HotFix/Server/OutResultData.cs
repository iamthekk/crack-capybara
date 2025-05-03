using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
	public class OutResultData
	{
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("isWin = {0}  m_MainRecount = {1}  RevivedCount = {2} \n", this.m_isWin, this.m_MainTotalDamage, this.m_revivedCount));
			for (int i = 0; i < this.m_members.Count; i++)
			{
				OutResultMemberData outResultMemberData = this.m_members[i];
				stringBuilder.Append(string.Format("i = {0}:{1} \n", i, outResultMemberData));
			}
			return stringBuilder.ToString();
		}

		public bool m_isWin;

		public int m_revivedCount;

		public List<OutResultMemberData> m_members = new List<OutResultMemberData>();

		public long m_MainTotalDamage;

		public string m_sbMainTotalDamage = string.Empty;
	}
}

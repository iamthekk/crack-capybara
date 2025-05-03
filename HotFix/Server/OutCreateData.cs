using System;
using System.Collections.Generic;

namespace Server
{
	public class OutCreateData
	{
		public void AddMember(OutMemberData member)
		{
			this.m_members.Add(member);
		}

		public void AddMembers(List<OutMemberData> members)
		{
			this.m_members.AddRange(members);
		}

		public List<OutMemberData> GetAllMembers()
		{
			return this.m_members;
		}

		public List<OutMemberData> GetMembers(int waveId)
		{
			if (waveId <= 0)
			{
				waveId = 1;
			}
			List<OutMemberData> list = new List<OutMemberData>();
			for (int i = 0; i < this.m_members.Count; i++)
			{
				if (this.m_members[i].m_waveID == waveId)
				{
					list.Add(this.m_members[i]);
				}
			}
			return list;
		}

		public List<OutMemberData> m_members = new List<OutMemberData>();
	}
}

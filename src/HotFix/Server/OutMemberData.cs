using System;
using System.Collections.Generic;

namespace Server
{
	public class OutMemberData
	{
		public List<int> GetSkillIds()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.m_skills.Count; i++)
			{
				OutSkillData outSkillData = this.m_skills[i];
				if (outSkillData != null && outSkillData.m_skillID != 0)
				{
					list.Add(outSkillData.m_skillID);
				}
			}
			return list;
		}

		public int m_waveID;

		public int m_rowID;

		public int m_memberID;

		public int m_memberInstanceID;

		public bool m_isMainMember;

		public MemberPos m_posIndex;

		public MemberRace m_memberRace;

		public MemberCamp m_camp;

		public FP m_curHp;

		public FP m_maxHp;

		public bool isEnemyPlayer;

		public List<OutSkillData> m_skills = new List<OutSkillData>();
	}
}

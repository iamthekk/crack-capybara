using System;

namespace Server
{
	public class OutResultMemberData
	{
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Format("InstanceID = {0}  ", this.m_memberInstanceID),
				string.Format("Camp = {0}  ", this.m_camp),
				string.Format("isMainMember = {0}  ", this.m_isMainMember),
				string.Format("curHp = {0}  ", this.m_curHp),
				string.Format("maxHp = {0}  ", this.m_maxHp),
				string.Format("attack = {0}  ", this.m_attack),
				string.Format("defense = {0}  ", this.m_defense)
			});
		}

		public int m_rowID;

		public int m_memberInstanceID;

		public MemberCamp m_camp;

		public bool m_isMainMember;

		public FP m_curHp;

		public FP m_maxHp;

		public FP m_attack;

		public FP m_defense;
	}
}

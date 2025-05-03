using System;

namespace Server
{
	public class BattleReportData_HurtOneData
	{
		public int m_hitInstanceID { get; set; }

		public int m_hitMemberId { get; set; }

		public int m_skillId { get; set; }

		public int m_bulletId { get; set; }

		public int m_buffId { get; set; }

		public int m_fireBulletID { get; set; }

		public int m_attackerInstanceID { get; set; }

		public bool IsShowCombo { get; set; }

		public int CurComboCount { get; set; }

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Format("InstanceID：{0}\n", this.m_hitInstanceID),
				string.Format("MemberId：{0}\n", this.m_hitMemberId),
				string.Format("SkillId：{0}\n", this.m_skillId),
				string.Format("FireBulletID：{0}\n", this.m_fireBulletID),
				string.Format("BulletId：{0}\n", this.m_bulletId),
				string.Format("ChangeHPData：{0}\n", this.m_changeHPData),
				string.Format("CurHp：{0}\n", this.m_curHp),
				string.Format("MaxHp：{0}\n", this.m_maxHp),
				string.Format("Attack：{0}\n", this.m_attack),
				string.Format("Defense：{0}\n", this.m_defense),
				string.Format("TargetInstanceID：{0}\n", this.m_attackerInstanceID),
				"\n"
			});
		}

		public ChangeHPData m_changeHPData;

		public FP m_curHp;

		public FP m_maxHp;

		public FP m_attack;

		public FP m_defense;
	}
}

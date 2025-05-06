using System;

namespace Server
{
	public class BattleReportData_CreateBullet : BaseBattleReportData
	{
		public int BulletID { get; set; }

		public int SkillID { get; set; }

		public int TargetInstanceID { get; set; }

		public int AttackerInstanceID { get; set; }

		public int FireBulletID { get; set; }

		public bool IsMainTarget { get; set; }

		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.CreateBullet;
			}
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Format("BulletID：{0}, ", this.BulletID),
				string.Format("SkillID：{0}, ", this.SkillID),
				string.Format("TargetInstaceID：{0}, ", this.TargetInstanceID),
				string.Format("AttackerInstanceID：{0}", this.AttackerInstanceID),
				"\n"
			});
		}
	}
}

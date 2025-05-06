using System;

namespace Server
{
	public class BattleReportData_BuffUpdate : BaseBattleReportData
	{
		public int TargetInstanceID { get; private set; }

		public int AttackerInstanceID { get; private set; }

		public int BuffId { get; private set; }

		public string Guid { get; private set; }

		public FP Attack { get; private set; }

		public FP MaxHp { get; private set; }

		public FP Defense { get; private set; }

		public int BuffLayer { get; private set; }

		public int BuffRound { get; private set; }

		public void SetData(int attackerInstanceId, int targetInstanceId, int buffId, string guid, FP attack, FP maxHp, FP defense, int buffLayer, int buffRound)
		{
			this.AttackerInstanceID = attackerInstanceId;
			this.TargetInstanceID = targetInstanceId;
			this.BuffId = buffId;
			this.Guid = guid;
			this.Attack = attack;
			this.MaxHp = maxHp;
			this.Defense = defense;
			this.BuffLayer = buffLayer;
			this.BuffRound = buffRound;
		}

		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.BuffUpdate;
			}
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Format("AttackerInstanceID：{0}, ", this.AttackerInstanceID),
				string.Format("TargetInstanceID：{0}, ", this.TargetInstanceID),
				string.Format("Add BuffId：{0}, ", this.BuffId),
				"Guid：",
				this.Guid,
				", ",
				string.Format("Attack：{0}, ", this.Attack),
				string.Format("MaxHp：{0}, ", this.MaxHp),
				string.Format("Defense：{0}, ", this.Defense),
				string.Format("BuffLayer：{0}, ", this.BuffLayer),
				string.Format("BuffRound：{0}", this.BuffRound)
			});
		}
	}
}

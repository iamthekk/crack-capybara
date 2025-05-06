using System;

namespace HotFix
{
	public class TowerEnemyData
	{
		public int MemberId { get; private set; }

		public int Level { get; private set; }

		public long Power { get; private set; }

		public TowerEnemyData(int id, int lv, long power)
		{
			this.MemberId = id;
			this.Level = lv;
			this.Power = power;
		}
	}
}

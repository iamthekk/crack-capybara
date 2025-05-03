using System;
using Framework.EventSystem;
using Server;

namespace HotFix
{
	public class EventArgCrossArenaHpInfo : BaseEventArgs
	{
		public void SetData(FP curHpAllFriendly, FP cmaxHpAllFriendly, FP curHpAllEnemy, FP maxHpAllEnemy)
		{
			this.m_curHpAllFriendly = curHpAllFriendly;
			this.m_cmaxHpAllFriendly = cmaxHpAllFriendly;
			this.m_curHpAllEnemy = curHpAllEnemy;
			this.m_maxHpAllEnemy = maxHpAllEnemy;
		}

		public override void Clear()
		{
		}

		public FP m_curHpAllFriendly = FP._0;

		public FP m_cmaxHpAllFriendly = FP._0;

		public FP m_curHpAllEnemy = FP._0;

		public FP m_maxHpAllEnemy = FP._0;
	}
}

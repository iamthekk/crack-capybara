using System;

namespace Server
{
	public class OutBattleData
	{
		public int m_endWave;

		public OutCreateData m_createData = new OutCreateData();

		public BattleReport m_battleReport = new BattleReport();

		public OutResultData m_resultData = new OutResultData();
	}
}

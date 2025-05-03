using System;
using System.Collections.Generic;

namespace Server
{
	public class InBattleData
	{
		public List<CardData> GetWaveData(int waveId)
		{
			int num = waveId - 2;
			if (num < 0 || num + 1 > this.m_otherWareDatas.Count)
			{
				return null;
			}
			return this.m_otherWareDatas[num];
		}

		public int m_seed;

		public int m_durationRound;

		public int m_waveMax = 1;

		public bool m_isNeedReport = true;

		public int m_revivedCount;

		public BattleMode m_battleMode;

		public List<CardData> m_cardDatas = new List<CardData>();

		public List<List<CardData>> m_otherWareDatas = new List<List<CardData>>();
	}
}

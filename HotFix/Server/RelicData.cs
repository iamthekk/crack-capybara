using System;

namespace Server
{
	public class RelicData
	{
		public RelicData()
		{
		}

		public RelicData(int id, int level, int quality)
		{
			this.m_id = id;
			this.m_level = level;
			this.m_quality = quality;
		}

		public int m_id;

		public int m_level;

		public int m_quality;

		public bool m_active;
	}
}

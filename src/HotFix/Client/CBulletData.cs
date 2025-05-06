using System;
using LocalModels.Bean;

namespace HotFix.Client
{
	public class CBulletData
	{
		public void SetTableData(GameBullet_bullet data)
		{
			this.m_bulletId = data.id;
			this.m_bulletType = data.bulletType;
			this.m_parameters = data.parameters;
		}

		public int m_bulletId;

		public int m_bulletType;

		public string m_parameters = string.Empty;

		public int m_startSoundID;

		public int m_hitSoundID;
	}
}

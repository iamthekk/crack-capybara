using System;
using LocalModels.Bean;

namespace Server
{
	public class SkillFireBulletData
	{
		public void SetTableData(int index, GameSkill_fireBullet fireBullet)
		{
			this.m_index = index;
			this.m_fireBulletID = fireBullet.id;
			this.m_bulletID = fireBullet.bulletID;
			this.m_bulletStartPrefabID = fireBullet.bulletStartPrefabID;
		}

		public int m_index;

		public int m_fireBulletID;

		public int m_bulletID;

		public int m_bulletStartPrefabID;
	}
}

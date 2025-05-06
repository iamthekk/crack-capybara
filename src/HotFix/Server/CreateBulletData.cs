using System;
using System.Collections.Generic;

namespace Server
{
	public class CreateBulletData
	{
		public int m_bulletID;

		public int m_fireBulletID;

		public bool m_isLastBullet;

		public SSkillBase m_skill;

		public List<SkillSelectTargetData> m_selectTargets;
	}
}

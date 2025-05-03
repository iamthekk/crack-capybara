using System;

namespace HotFix
{
	public class EquipSkillInfo
	{
		public EquipSkillInfo(int quality, int skillID, int skillLevel)
		{
			this.m_quality = quality;
			this.m_skillID = skillID;
			this.m_skillLevel = skillLevel;
		}

		public int m_quality;

		public int m_skillID;

		public int m_skillLevel;
	}
}

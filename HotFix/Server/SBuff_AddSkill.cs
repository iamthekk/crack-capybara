using System;
using System.Collections.Generic;

namespace Server
{
	public class SBuff_AddSkill : SBuffBase
	{
		protected override void OnInit()
		{
			List<int> list = new List<int>(this.m_data.SkillIDs);
			this.m_owner.skillFactory.AddSkills(list);
		}

		protected override void OnDeInit()
		{
			List<int> list = new List<int>(this.m_data.SkillIDs);
			SMemberBase owner = this.m_owner;
			if (owner != null)
			{
				SSkillFactory skillFactory = owner.skillFactory;
				if (skillFactory != null)
				{
					skillFactory.RemoveSkills(list);
				}
			}
			this.m_data = null;
		}

		protected override void ReadParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SBuff_AddSkill.Data>(parameters) : new SBuff_AddSkill.Data());
		}

		public SBuff_AddSkill.Data m_data;

		public class Data
		{
			public int[] SkillIDs;
		}
	}
}

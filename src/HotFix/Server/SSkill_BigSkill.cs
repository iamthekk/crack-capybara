using System;

namespace Server
{
	public class SSkill_BigSkill : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_BigSkill.Data>(parameters) : new SSkill_BigSkill.Data());
		}

		public override void OnInit(SSkillFactory skillFactory, SSkillData skillData)
		{
			base.OnInit(skillFactory, skillData);
			base.OnFullBloodAddDamage(this.m_data != null && this.m_data.IsFullBloodAddDamage > 0);
			base.OnLessThanBloodAddDamage(this.m_data != null && this.m_data.IsLessThanBloodAddDamage > 0);
		}

		public SSkill_BigSkill.Data m_data;

		public class Data
		{
			public int IsFullBloodAddDamage;

			public int IsLessThanBloodAddDamage;
		}
	}
}

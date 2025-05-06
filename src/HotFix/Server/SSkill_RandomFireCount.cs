using System;

namespace Server
{
	public class SSkill_RandomFireCount : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_RandomFireCount.Data>(parameters) : new SSkill_RandomFireCount.Data());
		}

		public override void DoEventNodes()
		{
			int num = new Random().Next(this.m_data.Min, this.m_data.Max);
			for (int i = 0; i < num; i++)
			{
				this.FireBullet(0, true);
				base.AddSkillLegacyPower();
			}
			base.AddSkillRecharge();
			this.FinishSkill();
		}

		public SSkill_RandomFireCount.Data m_data;

		public class Data
		{
			public int Min = 1;

			public int Max = 1;
		}
	}
}

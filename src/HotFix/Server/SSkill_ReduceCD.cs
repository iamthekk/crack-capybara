using System;
using System.Collections.Generic;

namespace Server
{
	public class SSkill_ReduceCD : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_ReduceCD.Data>(parameters) : new SSkill_ReduceCD.Data());
		}

		protected override void OnFunction()
		{
			this.ChuangeCD();
		}

		private void ChuangeCD()
		{
			if (this.m_data.Tag.Equals(0))
			{
				return;
			}
			List<SSkillBase> allSkill = base.Owner.skillFactory.allSkill;
			for (int i = 0; i < allSkill.Count; i++)
			{
				SSkillBase sskillBase = allSkill[i];
				if (sskillBase.skillData.skillTag == this.m_data.Tag)
				{
					sskillBase.skillData.SetMaxInitCD(this.m_data.ReduceInitCD, this.m_data.MinInitCD);
					sskillBase.skillData.SetMaxCD(this.m_data.ReduceCD, this.m_data.MinCD);
					int num = sskillBase.CurCD - this.m_data.ReduceCD;
					sskillBase.CurCD = MathTools.Clamp(num, 0, num);
				}
			}
		}

		public SSkill_ReduceCD.Data m_data;

		public class Data
		{
			public int Tag;

			public int ReduceInitCD;

			public int MinInitCD;

			public int ReduceCD;

			public int MinCD = 1;
		}
	}
}

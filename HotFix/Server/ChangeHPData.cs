using System;

namespace Server
{
	public class ChangeHPData
	{
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Format("[m_type:{0}, ", this.m_type),
				string.Format("m_hpUpdate:{0}, ", this.m_hpUpdate.AsLong()),
				string.Format("m_shieldUpdate:{0}, ", this.m_shieldUpdate.AsLong()),
				string.Format("m_shieldCurrentValue:{0}, ", this.m_shieldCurrentValue.AsLong()),
				string.Format("m_invincibleCount:{0}]", this.m_invincibleCount)
			});
		}

		public ChangeHPType m_type;

		public FP m_hpUpdate = FP._0;

		public FP m_shieldUpdate = FP._0;

		public FP m_shieldCurrentValue = FP._0;

		public FP m_invincibleCount = FP._0;
	}
}

using System;

namespace Server
{
	public class HurtData
	{
		public HurtData(HurtType hurtType, FP attack)
		{
			this.m_hurtType = hurtType;
			this.m_attack = attack;
		}

		public void Merge(HurtData data)
		{
			if (data == null)
			{
				return;
			}
			if (data.m_hurtType != this.m_hurtType)
			{
				return;
			}
			this.m_attack += data.m_attack;
		}

		public HurtType m_hurtType;

		public FP m_attack;
	}
}

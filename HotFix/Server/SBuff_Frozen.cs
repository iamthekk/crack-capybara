using System;

namespace Server
{
	public class SBuff_Frozen : SBuffBase
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.m_data = null;
			this.m_frozenData = null;
		}

		protected override void ReadParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SBuff_Frozen.Data>(parameters) : new SBuff_Frozen.Data());
			this.m_frozenData = new FrozenData
			{
				RoundTime = this.m_data.RoundTime,
				m_timeOverlayType = (TimeOverlayType)this.m_data.TimeOverlayType
			};
		}

		protected override void OnTrigger()
		{
			if (this.m_owner == null)
			{
				return;
			}
			if (this.m_owner.m_frozen == null)
			{
				return;
			}
			base.OnTrigger();
			this.m_owner.m_frozen.Play(this.m_frozenData);
			this.m_owner.memberFactory.OnEnemyFreezeAfter(this);
		}

		public SBuff_Frozen.Data m_data;

		public FrozenData m_frozenData;

		public class Data
		{
			public int RoundTime = 1;

			public int TimeOverlayType = 2;
		}
	}
}

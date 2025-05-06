using System;

namespace Server
{
	public class SBuff_Stun : SBuffBase
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.m_data = null;
			this.m_stunData = null;
		}

		protected override void ReadParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SBuff_Stun.Data>(parameters) : new SBuff_Stun.Data());
			this.m_stunData = new StunData
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
			if (this.m_owner.m_stun == null)
			{
				return;
			}
			base.OnTrigger();
			this.m_owner.m_stun.Play(this.m_stunData);
		}

		public SBuff_Stun.Data m_data;

		public StunData m_stunData;

		public class Data
		{
			public int RoundTime = 1;

			public int TimeOverlayType = 2;
		}
	}
}

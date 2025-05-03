using System;

namespace Server
{
	public class SBuff_Silence : SBuffBase
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.m_data = null;
			this.m_silenceData = null;
		}

		protected override void ReadParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SBuff_Silence.Data>(parameters) : new SBuff_Silence.Data());
			this.m_silenceData = new SilenceData
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
			if (this.m_owner.m_Silence == null)
			{
				return;
			}
			this.m_owner.m_Silence.Play(this.m_silenceData);
			base.OnTrigger();
		}

		public SBuff_Silence.Data m_data;

		public SilenceData m_silenceData;

		public class Data
		{
			public int RoundTime = 1;

			public int TimeOverlayType = 2;
		}
	}
}

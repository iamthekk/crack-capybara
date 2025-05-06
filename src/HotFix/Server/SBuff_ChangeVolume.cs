using System;

namespace Server
{
	public class SBuff_ChangeVolume : SBuffBase
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.m_data = null;
		}

		protected override void ReadParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.LitJson_ToObjectFp<SBuff_ChangeVolume.Data>(parameters) : new SBuff_ChangeVolume.Data());
		}

		public SBuff_ChangeVolume.Data m_data;

		public class Data
		{
			public FP AddScale = FP._020;

			public FP Speed = 2;
		}
	}
}

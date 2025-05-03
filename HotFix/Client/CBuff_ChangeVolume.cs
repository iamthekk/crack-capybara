using System;
using Server;

namespace HotFix.Client
{
	public class CBuff_ChangeVolume : CBuffBase
	{
		public override void OnInit()
		{
			if (this.m_owner == null)
			{
				return;
			}
			this.OnTrigger(1);
		}

		public override void OnDeInit()
		{
			if (this.m_owner == null)
			{
				return;
			}
			if (this.curLayer > 0)
			{
				this.m_owner.AddVolume(-(float)this.m_data.AddScale * (float)this.curLayer, (float)this.m_data.Speed);
			}
			this.curLayer = 0;
		}

		public override void ReadParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<CBuff_ChangeVolume.Data>(parameters) : new CBuff_ChangeVolume.Data());
		}

		public override void OnTrigger(int layer)
		{
			int num = layer - this.curLayer;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					this.m_owner.AddVolume((float)this.m_data.AddScale, (float)this.m_data.Speed);
				}
				this.curLayer = layer;
			}
		}

		public CBuff_ChangeVolume.Data m_data;

		private int curLayer;

		public class Data
		{
			public double AddScale = 0.20000000298023224;

			public double Speed = 2.0;
		}
	}
}

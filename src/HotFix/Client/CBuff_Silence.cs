using System;

namespace HotFix.Client
{
	public class CBuff_Silence : CBuffBase
	{
		public override void OnInit()
		{
			if (this.m_owner == null)
			{
				return;
			}
			this.m_owner.m_silence.Play(this.m_buffData.m_id, this.m_guid);
		}

		public override void OnDeInit()
		{
			if (this.m_owner == null)
			{
				return;
			}
			this.m_owner.m_silence.Stop(this.m_buffData.m_id, this.m_guid);
		}

		public override void ReadParameters(string parameters)
		{
		}
	}
}

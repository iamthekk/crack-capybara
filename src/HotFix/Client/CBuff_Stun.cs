using System;

namespace HotFix.Client
{
	public class CBuff_Stun : CBuffBase
	{
		public override void OnInit()
		{
			if (this.m_owner == null)
			{
				return;
			}
			this.m_owner.m_stun.Play(this.m_buffData.m_id, this.m_guid);
		}

		public override void OnDeInit()
		{
			if (this.m_owner == null)
			{
				return;
			}
			this.m_owner.m_stun.Stop(this.m_buffData.m_id, this.m_guid);
		}

		public override void ReadParameters(string parameters)
		{
		}
	}
}

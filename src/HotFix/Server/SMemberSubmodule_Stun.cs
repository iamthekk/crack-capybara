using System;

namespace Server
{
	public class SMemberSubmodule_Stun : SMemberBaseSubmodule
	{
		public SMemberSubmodule_Stun(SMemberBase member)
			: base(member)
		{
		}

		public bool IsControlled
		{
			get
			{
				return this.m_data != null;
			}
		}

		protected override void OnInit()
		{
			this.OnReset();
		}

		protected override void OnDeInit()
		{
			this.OnReset();
		}

		protected override void OnReset()
		{
			this.m_isPlaying = false;
			this.m_data = null;
			this.m_curRoundTime = 0;
		}

		public void Play(StunData data)
		{
			if (base.m_owner.IsDeath)
			{
				return;
			}
			if (this.m_data == null)
			{
				this.m_data = data;
				this.m_curRoundTime = data.RoundTime;
			}
			else
			{
				this.m_curRoundTime = data.m_timeOverlayType.Math(this.m_curRoundTime, data.RoundTime);
			}
			this.m_isPlaying = true;
		}

		public override void OnRoleRoundEnd(int roundCount = 1)
		{
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_curRoundTime -= roundCount;
			if (this.m_curRoundTime <= 0)
			{
				this.OnReset();
			}
		}

		public StunData m_data;

		public int m_curRoundTime;

		private bool m_isPlaying;
	}
}

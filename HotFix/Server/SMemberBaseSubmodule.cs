using System;

namespace Server
{
	public abstract class SMemberBaseSubmodule
	{
		private protected SMemberBase m_owner { protected get; private set; }

		public SMemberBaseSubmodule(SMemberBase member)
		{
			this.m_owner = member;
		}

		public void Init()
		{
			this.OnInit();
		}

		public void DeInit()
		{
			this.OnDeInit();
		}

		protected abstract void OnInit();

		protected abstract void OnDeInit();

		protected abstract void OnReset();

		public virtual void OnRoleRoundEnd(int roundCount = 1)
		{
		}
	}
}

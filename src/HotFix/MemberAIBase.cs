using System;
using HotFix.Client;
using Server;

namespace HotFix
{
	public abstract class MemberAIBase
	{
		public CMemberBase Owner { get; private set; }

		public void SetOwner(CMemberBase owner)
		{
			this.Owner = owner;
		}

		public virtual void OnInit()
		{
			this.m_stateManager = new MemberStateManager();
			this.m_stateManager.OnInit();
		}

		public virtual void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			MemberStateManager stateManager = this.m_stateManager;
			if (stateManager == null)
			{
				return;
			}
			stateManager.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public virtual void OnDeInit()
		{
			this.m_stateManager.OnDeInit();
			this.m_stateManager = null;
			this.Owner = null;
		}

		public void SwitchState(MemberState state)
		{
			if (this.m_stateManager == null)
			{
				return;
			}
			this.m_stateManager.ActiveState((int)state);
			if (this.m_stateManager.CurState != null)
			{
				this.Owner.SetMemberState(state);
			}
		}

		protected MemberStateManager m_stateManager;
	}
}

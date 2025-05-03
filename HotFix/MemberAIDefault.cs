using System;

namespace HotFix
{
	public class MemberAIDefault : MemberAIBase
	{
		public override void OnInit()
		{
			base.OnInit();
			this.m_stateManager.RegisterState(new MemberAIStateIdle(base.Owner, this));
			this.m_stateManager.RegisterState(new MemberAIStateDeath(base.Owner, this));
			this.m_stateManager.RegisterState(new MemberAIStateWin(base.Owner, this));
			this.m_stateManager.RegisterState(new MemberAIStateFailure(base.Owner, this));
			this.m_stateManager.RegisterState(new MemberAIStateControlled(base.Owner, this));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnDeInit()
		{
			this.m_stateManager.UnRegisterStateByName(0);
			this.m_stateManager.UnRegisterStateByName(4);
			this.m_stateManager.UnRegisterStateByName(5);
			this.m_stateManager.UnRegisterStateByName(6);
			this.m_stateManager.UnRegisterStateByName(8);
			base.OnDeInit();
		}
	}
}

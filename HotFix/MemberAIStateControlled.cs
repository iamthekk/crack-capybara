using System;
using Framework.EventSystem;
using HotFix.Client;

namespace HotFix
{
	public class MemberAIStateControlled : AIMemberState
	{
		public MemberAIStateControlled(CMemberBase owner, MemberAIBase ai)
			: base(owner, ai)
		{
		}

		public override int GetName()
		{
			return 8;
		}

		public override void OnEnter()
		{
			base.m_owner.PauseAnimation(true);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnExit()
		{
			base.m_owner.PauseAnimation(false);
			if (!base.m_owner.IsDeath)
			{
				base.m_owner.PlayAnimationIdle();
			}
		}

		public override void RegisterEvents(EventSystemManager eventManager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager eventManager)
		{
		}
	}
}

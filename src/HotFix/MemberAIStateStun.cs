using System;
using Framework.EventSystem;
using HotFix.Client;

namespace HotFix
{
	public class MemberAIStateStun : AIMemberState
	{
		public MemberAIStateStun(CMemberBase owner, MemberAIBase ai)
			: base(owner, ai)
		{
		}

		public override int GetName()
		{
			return 7;
		}

		public override void OnEnter()
		{
			if (!base.m_owner.IsDeath)
			{
				base.m_owner.PlayAnimationIdle();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnExit()
		{
		}

		public override void RegisterEvents(EventSystemManager eventManager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager eventManager)
		{
		}
	}
}

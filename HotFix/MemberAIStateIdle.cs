using System;
using Framework.EventSystem;
using HotFix.Client;

namespace HotFix
{
	public class MemberAIStateIdle : AIMemberState
	{
		public MemberAIStateIdle(CMemberBase owner, MemberAIBase ai)
			: base(owner, ai)
		{
		}

		public override int GetName()
		{
			return 0;
		}

		public override void OnEnter()
		{
			if (!base.m_owner.IsDeath)
			{
				base.m_owner.PlayAnimationIdle();
			}
		}

		public override void OnExit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}
	}
}

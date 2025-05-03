using System;
using Framework.EventSystem;
using HotFix.Client;

namespace HotFix
{
	public class MemberAIStateAttack : AIMemberState
	{
		public MemberAIStateAttack(CMemberBase owner, MemberAIBase ai)
			: base(owner, ai)
		{
		}

		public override int GetName()
		{
			return 3;
		}

		public override void OnEnter()
		{
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

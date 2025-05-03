using System;
using Framework.EventSystem;
using HotFix.Client;

namespace HotFix
{
	public abstract class AIMemberState : IMemberState
	{
		private protected CMemberBase m_owner { protected get; private set; }

		public AIMemberState(CMemberBase owner, MemberAIBase ai)
		{
			this.m_owner = owner;
			this.m_ai = ai;
		}

		public abstract int GetName();

		public abstract void OnEnter();

		public abstract void OnUpdate(float deltaTime, float unscaledDeltaTime);

		public abstract void OnExit();

		public abstract void RegisterEvents(EventSystemManager eventManager);

		public abstract void UnRegisterEvents(EventSystemManager eventManager);

		protected MemberAIBase m_ai;
	}
}

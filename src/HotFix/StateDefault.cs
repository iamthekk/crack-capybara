using System;

namespace HotFix
{
	public class StateDefault : StateMachine.State
	{
		public StateDefault(int id, Action<StateDefault> onEnterAction = null, Action<StateDefault> onUpdateAction = null, Action<StateDefault> onExitAction = null)
			: base(id)
		{
			this.m_onEnterAction = onEnterAction;
			this.m_onUpdateAction = onUpdateAction;
			this.m_onExitAction = onExitAction;
		}

		public Action<StateDefault> onEnterAction
		{
			get
			{
				return this.m_onEnterAction;
			}
		}

		public Action<StateDefault> onUpdateAction
		{
			get
			{
				return this.m_onUpdateAction;
			}
		}

		public Action<StateDefault> onExitAction
		{
			get
			{
				return this.m_onExitAction;
			}
		}

		public override void OnEnter()
		{
			if (this.onEnterAction != null)
			{
				this.onEnterAction(this);
			}
		}

		public override void OnUpdate(float deltaTime)
		{
			if (this.onUpdateAction != null)
			{
				this.onUpdateAction(this);
			}
		}

		public override void OnExit()
		{
			if (this.onExitAction != null)
			{
				this.onExitAction(this);
			}
		}

		private Action<StateDefault> m_onEnterAction;

		private Action<StateDefault> m_onUpdateAction;

		private Action<StateDefault> m_onExitAction;
	}
}

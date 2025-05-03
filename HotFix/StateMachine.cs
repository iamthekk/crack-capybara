using System;
using System.Collections.Generic;

namespace HotFix
{
	public class StateMachine
	{
		public void ActiveState(int stateName)
		{
			StateMachine.State state = this.GetState<StateMachine.State>(stateName);
			if (this.m_currentState != null)
			{
				this.m_currentState.OnExit();
			}
			state.OnEnter();
			this.m_currentState = state;
		}

		public int GetCurrentStateName()
		{
			if (this.m_currentState == null)
			{
				return -1;
			}
			return this.m_currentState.ID;
		}

		public T GetState<T>(int stateName) where T : StateMachine.State
		{
			T t = default(T);
			StateMachine.State state = null;
			if (this.m_states.TryGetValue(stateName, out state))
			{
				t = state as T;
			}
			return t;
		}

		public void RegisterState(StateMachine.State state)
		{
			if (state == null)
			{
				return;
			}
			this.m_states[state.ID] = state;
		}

		public void UnRegisterState(StateMachine.State state)
		{
			if (state == null)
			{
				return;
			}
			this.m_states.Remove(state.ID);
		}

		public void UnAllRegisterState()
		{
			this.m_states.Clear();
		}

		public void UnRegisterStateByName(int stateName)
		{
			this.m_states.Remove(stateName);
		}

		public void OnUpdate(float deltaTime)
		{
			if (this.m_currentState != null)
			{
				this.m_currentState.OnUpdate(deltaTime);
			}
		}

		private Dictionary<int, StateMachine.State> m_states = new Dictionary<int, StateMachine.State>();

		private StateMachine.State m_currentState;

		public abstract class State
		{
			public int ID { get; set; }

			public State(int id)
			{
				this.ID = id;
			}

			public abstract void OnEnter();

			public abstract void OnUpdate(float deltaTime);

			public abstract void OnExit();
		}
	}
}

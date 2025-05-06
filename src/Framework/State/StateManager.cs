using System;
using System.Collections.Generic;
using Framework.EventSystem;
using UnityEngine;

namespace Framework.State
{
	public class StateManager : MonoBehaviour, IModuleManager
	{
		public void Init()
		{
			if (this.isInited)
			{
				return;
			}
			this.isInited = true;
		}

		public void ActiveState(int stateName)
		{
			State state = this.GetState<State>(stateName);
			if (this.m_currentState != null)
			{
				this.m_currentState.UnRegisterEvents(this.m_eventSystemManager);
				this.m_currentState.OnExit();
			}
			state.OnEnter();
			state.RegisterEvents(this.m_eventSystemManager);
			this.m_currentState = state;
			Action<int> action = this.onStateChange;
			if (action == null)
			{
				return;
			}
			action(stateName);
		}

		public int GetCurrentStateName()
		{
			if (this.m_currentState == null)
			{
				return -1;
			}
			return this.m_currentState.GetName();
		}

		public State GetCurrentState()
		{
			return this.m_currentState;
		}

		public T GetState<T>(int stateName) where T : State
		{
			State state;
			if (this.m_states.TryGetValue(stateName, out state))
			{
				return state as T;
			}
			return default(T);
		}

		public void RegisterState(State state)
		{
			if (state != null)
			{
				this.m_states[state.GetName()] = state;
			}
		}

		public void UnRegisterState(State state)
		{
			if (state != null)
			{
				this.m_states.Remove(state.GetName());
			}
		}

		public void UnRegisterStateByName(int stateName)
		{
			this.m_states.Remove(stateName);
		}

		public void UnRegisterAllState(params int[] ignoreIDs)
		{
			if (this.m_currentState != null)
			{
				this.m_currentState.UnRegisterEvents(this.m_eventSystemManager);
			}
			HashSet<int> hashSet = ((ignoreIDs != null && ignoreIDs.Length != 0) ? new HashSet<int>(ignoreIDs) : null);
			List<State> list = new List<State>();
			foreach (KeyValuePair<int, State> keyValuePair in this.m_states)
			{
				if (keyValuePair.Value != null && (hashSet == null || !hashSet.Contains(keyValuePair.Key)))
				{
					list.Add(keyValuePair.Value);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				State state = list[i];
				if (state != null)
				{
					this.UnRegisterState(state);
				}
			}
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_currentState != null)
			{
				this.m_currentState.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public void OnLateUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_currentState != null)
			{
				this.m_currentState.OnLateUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public void RegisterChangeState(Action<int> onChange)
		{
			this.onStateChange = (Action<int>)Delegate.Combine(this.onStateChange, onChange);
		}

		public void UnRegisterChangeState(Action<int> onChange)
		{
			this.onStateChange = (Action<int>)Delegate.Remove(this.onStateChange, onChange);
		}

		private State m_currentState;

		private Dictionary<int, State> m_states = new Dictionary<int, State>();

		[SerializeField]
		private EventSystemManager m_eventSystemManager;

		private Action<int> onStateChange;

		private bool isInited;
	}
}

using System;
using System.Collections.Generic;
using Framework;

namespace HotFix
{
	public class MemberStateManager
	{
		public IMemberState CurState
		{
			get
			{
				return this.m_curState;
			}
		}

		public void ActiveState(int stateName)
		{
			IMemberState state = this.GetState<IMemberState>(stateName);
			if (state == null)
			{
				return;
			}
			if (this.m_curState != null)
			{
				this.m_curState.UnRegisterEvents(GameApp.Event);
				this.m_curState.OnExit();
			}
			state.RegisterEvents(GameApp.Event);
			state.OnEnter();
			this.m_curState = state;
		}

		public void OnInit()
		{
		}

		public void OnDeInit()
		{
			this.m_states.Clear();
			this.m_curState = null;
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			IMemberState curState = this.m_curState;
			if (curState == null)
			{
				return;
			}
			curState.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void RegisterState(IMemberState state)
		{
			if (state != null)
			{
				this.m_states[state.GetName()] = state;
			}
		}

		public void UnRegisterStateByName(int stateName)
		{
			this.m_states.Remove(stateName);
		}

		public T GetState<T>(int stateName) where T : IMemberState
		{
			T t = default(T);
			IMemberState memberState;
			if (this.m_states.TryGetValue(stateName, out memberState))
			{
				t = (T)((object)memberState);
			}
			return t;
		}

		private IMemberState m_curState;

		private Dictionary<int, IMemberState> m_states = new Dictionary<int, IMemberState>();
	}
}

using System;

namespace HotFix
{
	public class StateTimer : StateMachine.State
	{
		public float Time
		{
			get
			{
				return this.m_time;
			}
		}

		public float Duration
		{
			get
			{
				return this.m_duration;
			}
		}

		public StateTimer(int id, float duration, Action<StateTimer> onTimerStartAction = null, Action<StateTimer> onTimerUpdateAction = null, Action<StateTimer> onTimerFinished = null)
			: base(id)
		{
			this.m_duration = duration;
			this.m_onTimerStartAction = onTimerStartAction;
			this.m_onTimerUpdateAction = onTimerUpdateAction;
			this.m_onTimerFinished = onTimerFinished;
		}

		public override void OnEnter()
		{
			this.m_time = 0f;
			this.m_isPlaying = true;
			if (this.m_onTimerStartAction != null)
			{
				this.m_onTimerStartAction(this);
			}
		}

		public override void OnUpdate(float deltaTime)
		{
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_time += deltaTime;
			if (this.m_onTimerUpdateAction != null)
			{
				this.m_onTimerUpdateAction(this);
			}
			if (this.m_time >= this.m_duration)
			{
				this.m_time = this.m_duration;
				this.m_isPlaying = false;
				if (this.m_onTimerFinished != null)
				{
					this.m_onTimerFinished(this);
				}
			}
		}

		public override void OnExit()
		{
			this.m_isPlaying = false;
		}

		private float m_time;

		private float m_duration;

		private Action<StateTimer> m_onTimerStartAction;

		private Action<StateTimer> m_onTimerUpdateAction;

		private Action<StateTimer> m_onTimerFinished;

		private bool m_isPlaying;
	}
}

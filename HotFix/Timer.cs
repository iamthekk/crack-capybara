using System;
using UnityEngine;

namespace HotFix
{
	public class Timer
	{
		public Timer()
		{
		}

		public Timer(int id)
		{
			this.m_id = id;
		}

		public bool IsPlaying
		{
			get
			{
				return this.m_isPlaying;
			}
		}

		public bool IsPause
		{
			get
			{
				return this.m_isPause;
			}
		}

		public int ID
		{
			get
			{
				return this.m_id;
			}
		}

		public void OnInit()
		{
			this.m_isPlaying = false;
			this.m_isPause = false;
			this.m_time = 0f;
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_isPause)
			{
				return;
			}
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_time += deltaTime;
			if (this.m_time >= this.m_duration)
			{
				this.m_isPlaying = false;
				this.m_time = this.m_duration;
				if (this.m_onFinished != null)
				{
					this.m_onFinished(this);
				}
			}
		}

		public void OnDeInit()
		{
			this.m_onFinished = null;
			this.m_isPause = false;
			this.m_isPlaying = false;
		}

		public void Play(float duration)
		{
			this.m_time = 0f;
			this.m_duration = duration;
			this.m_isPlaying = true;
		}

		public void Stop()
		{
			this.m_isPlaying = false;
		}

		public void OnPause(bool pause)
		{
			this.m_isPause = pause;
		}

		public void OnReset()
		{
			this.m_time = 0f;
			this.m_isPlaying = false;
			this.m_isPause = false;
		}

		[SerializeField]
		private float m_duration = 1f;

		[SerializeField]
		private bool m_isPlaying;

		[SerializeField]
		private bool m_isPause;

		[SerializeField]
		private float m_time;

		[SerializeField]
		private int m_id;

		public Action<Timer> m_onFinished;
	}
}

using System;
using Framework;
using Proto.User;

namespace HotFix
{
	public class HeartbeatCtrl : IUpdater
	{
		public void OnInit()
		{
			this.m_duration = (float)Singleton<GameConfig>.Instance.HeartBeatInterval;
			this.m_isSending = false;
			this.m_isPlaying = true;
			this.m_currentTime = 0f;
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (GameApp.State.GetCurrentStateName() != 103)
			{
				return;
			}
			if (!this.m_isPlaying)
			{
				return;
			}
			if (this.m_isSending)
			{
				return;
			}
			this.m_currentTime += deltaTime;
			if (this.m_currentTime >= this.m_duration)
			{
				this.m_isPlaying = false;
				this.m_currentTime = 0f;
				this.DoUserHeartbeatSyncRequest();
			}
		}

		public void OnDeInit()
		{
			this.m_isPlaying = false;
			this.m_isSending = false;
		}

		public void OnDispose()
		{
		}

		private void DoUserHeartbeatSyncRequest()
		{
			this.m_isSending = true;
			NetworkUtils.User.DoUserHeartbeatSyncRequest(delegate(bool isSuccess, UserHeartbeatSyncResponse response)
			{
				if (!isSuccess)
				{
					return;
				}
				this.m_currentTime = 0f;
				this.m_isPlaying = true;
				this.m_isSending = false;
			});
		}

		public float m_duration = 5f;

		public float m_currentTime;

		public bool m_isPlaying;

		public bool m_isSending;
	}
}

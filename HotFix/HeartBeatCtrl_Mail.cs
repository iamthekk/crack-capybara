using System;
using Framework;

namespace HotFix
{
	public class HeartBeatCtrl_Mail
	{
		public void OnInit()
		{
			this.m_isSending = false;
			this.SendRequest();
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
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
				this.SendRequest();
			}
		}

		public void OnDeInit()
		{
			this.m_isPlaying = false;
			this.m_isSending = false;
		}

		private void SendRequest()
		{
			if (!GameApp.Mail.IsEnable)
			{
				return;
			}
			this.m_isSending = true;
			GameApp.Mail.GetManager().OnRefreshMailListData(delegate
			{
				this.m_currentTime = 0f;
				this.m_isPlaying = true;
				this.m_isSending = false;
			});
		}

		public float m_duration = 300f;

		public float m_currentTime;

		public bool m_isPlaying;

		public bool m_isSending;
	}
}

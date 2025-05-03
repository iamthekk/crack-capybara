using System;
using System.Runtime.CompilerServices;
using Framework;
using Framework.Logic.GameTestTools;
using Framework.ResourcesModule;

namespace HotFix
{
	public class HeartBeatCtrl_Update
	{
		public void OnInit()
		{
			bool @bool = GameApp.Config.GetBool("IsLoadHost");
			bool bool2 = GameApp.Config.GetBool("IsReleaseServer");
			string @string = GameApp.Config.GetString("ChannelName");
			this.currentVersion = GameApp.Config.GetString("Version");
			this.versionUrl = Singleton<PathManager>.Instance.GetResourcesSeverVersion(bool2, @string);
			this.m_isCheckAppUpdate = !@bool && !string.IsNullOrEmpty(this.versionUrl);
			bool bool3 = GameApp.Config.GetBool("IsBuildIn");
			this.m_isCheckResourcesUpdate = this.m_isCheckAppUpdate && !bool3;
			if (!this.m_isCheckAppUpdate && !this.m_isCheckResourcesUpdate)
			{
				return;
			}
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
			this.m_isSending = true;
			if (this.m_isCheckAppUpdate)
			{
				Singleton<APP_Update>.Instance.CheckUpdate(this.versionUrl, this.currentVersion, new Action<string>(this.OnFinishedForCheckUpdate), null);
			}
		}

		private void OnFinishedForCheckUpdate(string version)
		{
			Version version2 = new Version(this.currentVersion);
			Version version3 = new Version(version);
			if (!(version2 >= version3))
			{
				GameApp.View.OpenView(ViewName.UpdateAppViewModule, null, 3, null, null);
				return;
			}
			if (this.m_isCheckResourcesUpdate)
			{
				this.SendResourcesRequest();
				return;
			}
			this.ToPlay();
		}

		private void ToPlay()
		{
			this.m_currentTime = 0f;
			this.m_isPlaying = true;
			this.m_isSending = false;
		}

		private void SendResourcesRequest()
		{
			HeartBeatCtrl_Update.<SendResourcesRequest>d__14 <SendResourcesRequest>d__;
			<SendResourcesRequest>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<SendResourcesRequest>d__.<>4__this = this;
			<SendResourcesRequest>d__.<>1__state = -1;
			<SendResourcesRequest>d__.<>t__builder.Start<HeartBeatCtrl_Update.<SendResourcesRequest>d__14>(ref <SendResourcesRequest>d__);
		}

		[GameTestMethod("Update", "OpenUpdateAppViewModule", "", 7000)]
		private static void OpenUpdateAppViewModule()
		{
			GameApp.View.OpenView(ViewName.UpdateAppViewModule, null, 3, null, null);
		}

		[GameTestMethod("", "OpenUpdateResourcesViewModule", "", 7001)]
		private static void OpenUpdateResourcesViewModule()
		{
			GameApp.View.OpenView(ViewName.UpdateResourcesViewModule, null, 3, null, null);
		}

		public float m_duration = 180f;

		public float m_currentTime;

		public bool m_isPlaying;

		public bool m_isSending;

		private string versionUrl;

		private string currentVersion;

		private bool m_isCheckAppUpdate;

		private bool m_isCheckResourcesUpdate;
	}
}

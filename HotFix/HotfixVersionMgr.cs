using System;
using System.Threading.Tasks;
using Framework;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class HotfixVersionMgr : Singleton<HotfixVersionMgr>
	{
		public int ResVersion { get; private set; }

		public async Task LoadVersion()
		{
			try
			{
				AsyncOperationHandle<HotfixConfigData> handle = GameApp.Resources.LoadAssetAsync<HotfixConfigData>("Assets/_Resources/Scriptable/HotfixConfig.asset");
				await handle.Task;
				if (handle.Result != null)
				{
					this.mHotfixConfigData = handle.Result;
					this.ResVersion = this.GetResVersion();
				}
				handle = default(AsyncOperationHandle<HotfixConfigData>);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public string GetInfo(string name)
		{
			return this.mHotfixConfigData.GetConfigInfo(name);
		}

		private int GetResVersion()
		{
			int num;
			if (this.mHotfixConfigData != null && int.TryParse(this.mHotfixConfigData.GetConfigInfo("ResVersion"), out num))
			{
				return num;
			}
			return 0;
		}

		public string GetAppVersion()
		{
			if (this.mHotfixConfigData != null)
			{
				return this.mHotfixConfigData.GetConfigInfo("AppVersion");
			}
			return string.Empty;
		}

		public const string VERSION_PATH = "Assets/_Resources/Scriptable/HotfixConfig.asset";

		private HotfixConfigData mHotfixConfigData;
	}
}

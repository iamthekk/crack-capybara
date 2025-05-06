using System;
using System.Threading.Tasks;
using Framework;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class BattleServerVersionMgr : Singleton<BattleServerVersionMgr>
	{
		public int BattleServerVersion { get; private set; }

		public async Task LoadVersion()
		{
			this.BattleServerVersion = 0;
			try
			{
				AsyncOperationHandle<TextAsset> handle = GameApp.Resources.LoadAssetAsync<TextAsset>("Assets/_Resources/BSVersion/bsv.txt");
				await handle.Task;
				int num;
				if (handle.Result != null && int.TryParse(handle.Result.text, out num))
				{
					this.BattleServerVersion = num;
				}
				handle = default(AsyncOperationHandle<TextAsset>);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public string GetVersion()
		{
			return this.BattleServerVersion.ToString();
		}

		private const string VERSION_PATH = "Assets/_Resources/BSVersion/bsv.txt";
	}
}

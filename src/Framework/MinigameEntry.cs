using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
	public class MinigameEntry : MonoBehaviour
	{
		private void Start()
		{
			this.LoadLaunchScene();
		}

		public void LoadLaunchScene()
		{
			Debug.Log("[WX] 开始加载登录场景...");
			base.StartCoroutine(this.LoadLaunchSceneLocal());
		}

		public void LoadLaunchSceneRemote()
		{
			MinigameEntry.<LoadLaunchSceneRemote>d__3 <LoadLaunchSceneRemote>d__;
			<LoadLaunchSceneRemote>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<LoadLaunchSceneRemote>d__.<>1__state = -1;
			<LoadLaunchSceneRemote>d__.<>t__builder.Start<MinigameEntry.<LoadLaunchSceneRemote>d__3>(ref <LoadLaunchSceneRemote>d__);
		}

		public IEnumerator LoadLaunchSceneLocal()
		{
			Debug.Log("[WX] LoadLaunchSceneLocal...");
			Stopwatch loadWatcher = new Stopwatch();
			loadWatcher.Start();
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Assets/Scene/GameLauncherMini.unity");
			if (asyncOperation != null)
			{
				asyncOperation.allowSceneActivation = false;
				float percent = 0f;
				while (!asyncOperation.isDone)
				{
					if (asyncOperation.progress - percent > 0.05f)
					{
						percent = asyncOperation.progress;
						Debug.Log(string.Format("[WX] 登录场景加载进度:{0},webGameFinish:{1}", asyncOperation.progress, this._webGameFinish));
					}
					if (asyncOperation.progress >= 0.9f && !asyncOperation.allowSceneActivation)
					{
						asyncOperation.allowSceneActivation = true;
						loadWatcher.Stop();
						Debug.Log("[WX]登录场景加载完成...");
					}
					yield return null;
				}
			}
			else
			{
				loadWatcher.Stop();
				Debug.LogError("[WX] 登录场景加载失败...");
			}
			yield break;
		}

		private bool _webGameFinish;
	}
}

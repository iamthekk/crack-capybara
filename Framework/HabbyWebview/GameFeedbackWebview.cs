using System;
using Framework.HabbyTimerManager;
using Framework.Logic.Platform;
using Newtonsoft.Json;
using ThinkingAnalytics;
using UnityEngine;

namespace Framework.HabbyWebview
{
	public class GameFeedbackWebview : FullScreenWebView
	{
		public GameFeedbackWebview(string url)
		{
			this.tryloadUrl = url;
			this.mDisposed = false;
		}

		public void Show()
		{
			this.mIsMaskMode = true;
			this.mDisposed = false;
			this.webviewFrameTopAndBottomSpace = 100;
			base.InitWebView();
			base.StartOutTimer(8f);
			this.StartBackKeyCheck();
		}

		private void StartBackKeyCheck()
		{
			this.RemoveBackCheckTimer();
			if (this.mDisposed)
			{
				return;
			}
			this._keyTimer = HabbyTimer.Instance.Execute(delegate(int time)
			{
				if (!this.mDisposed && Input.GetKeyDown(27))
				{
					this.HideWebView();
					this.Clean();
					Action<bool> onClose = this.OnClose;
					if (onClose == null)
					{
						return;
					}
					onClose(true);
				}
			}, 0.0166666675f, int.MaxValue, false, 1f);
		}

		private void RemoveBackCheckTimer()
		{
			if (this._keyTimer > 0)
			{
				HabbyTimer.Cancel(this._keyTimer);
				this._keyTimer = 0;
			}
		}

		public static string GetPlayerInfo()
		{
			return JsonConvert.SerializeObject(new GameFeedbackWebview.PlayerInfo
			{
				serveruserid = GameApp.SDK.WebView.ServerUserid.ToString(),
				accountId = GameApp.SDK.WebView.AccountID,
				serveruseridsub = "",
				uuid = PlatformHelper.GetUUID(),
				platform = Singleton<PlatformHelper>.Instance.GetPlatformID(),
				sdklogintype = "",
				sdkloginid = GameApp.SDK.Login.channel_login_get_userid(),
				devicemodel = Singleton<PlatformHelper>.Instance.GetDeviceModel(),
				memorysize = Singleton<PlatformHelper>.Instance.GetSystemMemorySize(),
				appversion = Singleton<PlatformHelper>.Instance.GetAppVersion(),
				operationsystem = Singleton<PlatformHelper>.Instance.GetOperationSystem(),
				graphicsDeviceName = Singleton<PlatformHelper>.Instance.GetGraphicsDeviceName(),
				nettype = "",
				tga_deviceid = ThinkingAnalyticsAPI.GetDeviceId(),
				tga_distinctid = ThinkingAnalyticsAPI.GetDistinctId(""),
				packageName = Application.identifier
			}) ?? "";
		}

		public override void OnPageFinished(UniWebView view, int statusCode, string url)
		{
			if (this.mDisposed)
			{
				return;
			}
			base.OnPageFinished(view, statusCode, url);
			if (this.webView)
			{
				this.webView.EvaluateJavaScript("get_player_info('" + GameFeedbackWebview.GetPlayerInfo() + "');", delegate(UniWebViewNativeResultPayload response)
				{
					if (response == null)
					{
						HLog.LogError("FeedBack == send_player_info response is null!");
					}
				});
			}
			this.ShowWebView();
		}

		public override void OnPageErrorReceived(UniWebView webView, int errorCode, string errorMessage)
		{
			if (this.mDisposed)
			{
				return;
			}
			base.OnPageErrorReceived(webView, errorCode, errorMessage);
			this.HideWebView();
			this.Clean();
		}

		public override void OnMessageReceived(UniWebView view, UniWebViewMessage message)
		{
			if (this.mDisposed)
			{
				return;
			}
			base.OnMessageReceived(view, message);
			if (message.Path == "close")
			{
				this.HideWebView();
				this.Clean();
				Action<bool> onClose = this.OnClose;
				if (onClose == null)
				{
					return;
				}
				onClose(true);
			}
		}

		public override void Clean()
		{
			base.Clean();
			this.RemoveBackCheckTimer();
			this.mDisposed = true;
		}

		public Action<bool> OnClose;

		private bool mDisposed;

		protected int _keyTimer;

		[Serializable]
		public class PlayerInfo
		{
			public string serveruserid;

			public string serveruseridsub;

			public string uuid;

			public string platform;

			public string sdklogintype;

			public string sdkloginid;

			public string devicemodel;

			public int memorysize;

			public string appversion;

			public string operationsystem;

			public string graphicsDeviceName;

			public string nettype;

			public string tga_deviceid;

			public string tga_distinctid;

			public string ip;

			public string packageName;

			public string accountId;
		}
	}
}

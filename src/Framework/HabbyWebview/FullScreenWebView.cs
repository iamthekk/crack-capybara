using System;
using Framework.HabbyTimerManager;
using UnityEngine;

namespace Framework.HabbyWebview
{
	public class FullScreenWebView
	{
		protected GameObject FullScreenWebMode()
		{
			this._webviewWindowUI.customWebviewObj.SetActive(false);
			this._webviewWindowUI.customWebviewObjFullScreen.SetActive(true);
			return this._webviewWindowUI.customWebviewObjFullScreen;
		}

		protected GameObject FullScreenMaskWebMode()
		{
			this._webviewWindowUI.customWebviewObj.SetActive(true);
			this._webviewWindowUI.customWebviewObjFullScreen.SetActive(false);
			return this._webviewWindowUI.customWebviewObj;
		}

		protected void HideWebviewObj()
		{
			this._webviewWindowUI.customWebviewObj.SetActive(false);
			this._webviewWindowUI.customWebviewObjFullScreen.SetActive(false);
		}

		protected void StartOutTimer(float time)
		{
			this._timer = HabbyTimer.Instance.Execute(delegate(int time)
			{
				if (!this._isLoaded)
				{
					this.Clean();
				}
			}, time, 1, false, 1f);
		}

		public static string GetValueFromMessageByKey(UniWebViewMessage message, string key)
		{
			string text = string.Empty;
			if (message.Args == null)
			{
				return text;
			}
			if (message.Args.ContainsKey(key))
			{
				text = message.Args[key];
			}
			if ("undefined" == text)
			{
				text = "";
			}
			return text;
		}

		public static int GetIntFromMessageByKey(UniWebViewMessage message, string key)
		{
			string text = "0";
			if (message.Args == null)
			{
				return 0;
			}
			if (message.Args.ContainsKey(key))
			{
				text = message.Args[key];
			}
			int num;
			try
			{
				num = int.Parse(text);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				num = 0;
			}
			return num;
		}

		public static string GetNewValueIfNotEmpty(string str, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return str;
			}
			return value;
		}

		public void InitWebView()
		{
			if (string.IsNullOrEmpty(this.tryloadUrl))
			{
				return;
			}
			if (this._webviewWindowUI == null)
			{
				GameObject gameObject = Resources.Load<GameObject>(this.webviewPrefabPath);
				this._webviewWindowUI = Object.Instantiate<GameObject>(gameObject).GetComponent<HabbyWebviewWindowUI>();
			}
			GameObject gameObject2;
			if (!this.mIsMaskMode)
			{
				gameObject2 = this.FullScreenWebMode();
			}
			else
			{
				gameObject2 = this.FullScreenMaskWebMode();
			}
			if (!gameObject2)
			{
				throw new Exception("--- can not find webview gameobj:WebViewObj");
			}
			gameObject2.SetActive(true);
			RectTransform component = gameObject2.GetComponent<RectTransform>();
			component.offsetMin = new Vector2(component.offsetMin.x, (float)this.webviewFrameTopAndBottomSpace);
			component.offsetMax = new Vector2(component.offsetMax.x, (float)(-(float)this.webviewFrameTopAndBottomSpace));
			this.webView = gameObject2.GetComponent<UniWebView>();
			if (this.webView == null)
			{
				this.webView = gameObject2.AddComponent<UniWebView>();
				if (this.webviewFrameTopAndBottomSpace <= 0)
				{
					this.webviewFrameTopAndBottomSpace = this._webviewWindowUI.defaultTopAndButtom;
				}
				this.webView.SetUseWideViewPort(true);
				this.webView.Frame = new Rect(0f, 0f, component.rect.width, component.rect.height);
				this.webView.AddUrlScheme("uniwebview");
				this.webView.AddUrlScheme("unityevent");
				this.webView.OnPageFinished += new UniWebView.PageFinishedDelegate(this.OnPageFinished);
				this.webView.OnShouldClose += new UniWebView.ShouldCloseDelegate(this.OnShouldClose);
				this.webView.OnMessageReceived += new UniWebView.MessageReceivedDelegate(this.OnMessageReceived);
				this.webView.OnPageErrorReceived += new UniWebView.PageErrorReceivedDelegate(this.OnPageErrorReceived);
				this.webView.SetShowToolbar(false, false, true, false);
				this.webView.SetBackButtonEnabled(false);
				this.webView.SetZoomEnabled(true);
				this.webView.SetUseWideViewPort(true);
				this.webView.SetLoadWithOverviewMode(true);
				this.webView.SetBouncesEnabled(false);
				this.webView.BackgroundColor = new Color(0f, 0f, 0f, 0.74f);
				this.webView.ReferenceRectTransform = gameObject2.transform as RectTransform;
			}
			this.webView.CleanCache();
			this.webView.Load(this.tryloadUrl, false, null);
		}

		public virtual void ShowWebView()
		{
			if (string.IsNullOrEmpty(this.tryloadUrl))
			{
				return;
			}
			if (this.webView)
			{
				this.webView.Show(false, 0, 0.4f, null);
				return;
			}
			this.InitWebView();
		}

		public virtual void HideWebView()
		{
			if (this.webView)
			{
				this.webView.Hide(false, 0, 0.4f, null);
				this.HideWebviewObj();
			}
		}

		public void EvaluateJavaScript(string jsCode, Action<string> onSuccess)
		{
			if (this.webView)
			{
				this.webView.EvaluateJavaScript(jsCode, delegate(UniWebViewNativeResultPayload payload)
				{
					if ("0".Equals((payload != null) ? payload.resultCode : null))
					{
						Action<string> onSuccess2 = onSuccess;
						if (onSuccess2 == null)
						{
							return;
						}
						onSuccess2((payload != null) ? payload.data : null);
					}
				});
			}
		}

		public virtual void OnPageFinished(UniWebView view, int statusCode, string url)
		{
			this._isLoaded = true;
			this.loadedWebUrl = url;
		}

		public virtual bool OnShouldClose(UniWebView view)
		{
			return true;
		}

		public virtual void OnMessageReceived(UniWebView view, UniWebViewMessage message)
		{
		}

		public virtual void OnPageErrorReceived(UniWebView webView, int errorCode, string errorMessage)
		{
			webView.Hide(false, 0, 0.4f, null);
		}

		public void Restart()
		{
			if (this.webView != null)
			{
				this.webView.Reload();
			}
		}

		public void CleanCache()
		{
			if (this.webView)
			{
				this.webView.CleanCache();
			}
		}

		public virtual void Clean()
		{
			if (this._timer > 0)
			{
				HabbyTimer.Cancel(this._timer);
				this._timer = 0;
			}
			this.CleanCache();
			if (this.webView != null)
			{
				this.HideWebView();
				this.webView.OnPageFinished -= new UniWebView.PageFinishedDelegate(this.OnPageFinished);
				this.webView.OnShouldClose -= new UniWebView.ShouldCloseDelegate(this.OnShouldClose);
				this.webView.OnMessageReceived -= new UniWebView.MessageReceivedDelegate(this.OnMessageReceived);
				this.webView.OnPageErrorReceived -= new UniWebView.PageErrorReceivedDelegate(this.OnPageErrorReceived);
				Object.Destroy(this.webView);
				this.webView = null;
			}
			if (this._webviewWindowUI != null)
			{
				Object.Destroy(this._webviewWindowUI.gameObject);
				this._webviewWindowUI = null;
			}
			this.webViewObjName = null;
		}

		protected HabbyWebviewWindowUI _webviewWindowUI;

		private readonly string webviewPrefabPath = "HabbySDK/HabbyWebviewObj";

		protected UniWebView webView;

		protected string loadedWebUrl;

		protected string tryloadUrl;

		protected string webViewObjName = "WebViewObj";

		protected bool mIsMaskMode;

		protected int webviewFrameWidth;

		protected int webviewFrameTopAndBottomSpace;

		protected bool _isLoaded;

		protected int _timer;
	}
}

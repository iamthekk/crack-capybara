using System;
using Framework.EventSystem;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.UI;
using Framework.ResourcesModule;
using Framework.RunTimeManager;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework.Logic.Modules
{
	public class CheckAssetsViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.m_languageDataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			this.m_tipOkBt.onClick.AddListener(new UnityAction(this.OnClickTipOkBtn));
			this.SetTipOkBtnTxt(4);
			this.ShowTips(false);
			this.SwitchState(CheckAssetsViewModule.State.CheckVersion);
			GameApp.SDK.Analyze.TrackLogin("检查更新", null);
			GameApp.SDK.InitCloudConfig(delegate(bool res)
			{
				this.OnCheckAsset();
			});
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			GameApp.Resources.OnUpdate(deltaTime, unscaledDeltaTime);
			if (!this.m_isPlaying)
			{
				return;
			}
			if (this.m_state == CheckAssetsViewModule.State.Download)
			{
				return;
			}
			this.m_time += Time.deltaTime;
			if (this.m_time >= this.m_duration)
			{
				this.m_time = this.m_duration;
				this.m_isPlaying = false;
			}
			this.SetPercent(this.m_time / this.m_duration);
		}

		public override void OnClose()
		{
			this.m_tipOkBt.onClick.RemoveAllListeners();
			this.m_onClickTipOkBtn = null;
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickTipOkBtn()
		{
			Action onClickTipOkBtn = this.m_onClickTipOkBtn;
			if (onClickTipOkBtn == null)
			{
				return;
			}
			onClickTipOkBtn();
		}

		private void OnCheckAsset()
		{
			if (GameApp.Config.GetBool("IsLoadHost"))
			{
				this.OnCheckResourcesUpdate();
				return;
			}
			bool @bool = GameApp.Config.GetBool("IsReleaseServer");
			string @string = GameApp.Config.GetString("ChannelName");
			string currentVersion = GameApp.Config.GetString("Version");
			string resourcesSeverVersion = Singleton<PathManager>.Instance.GetResourcesSeverVersion(@bool, @string);
			if (string.IsNullOrEmpty(resourcesSeverVersion))
			{
				this.OnCheckResourcesUpdate();
				return;
			}
			int mainStepIndex = LoadingTime.MainStepIndex;
			Singleton<APP_Update>.Instance.CheckUpdate(resourcesSeverVersion, currentVersion, delegate(string version)
			{
				Version version2 = new Version(currentVersion);
				Version version3 = new Version(version);
				if (version2 >= version3)
				{
					this.OnCheckResourcesUpdate();
					return;
				}
				this.ShowUpdateVersion();
				HLog.LogError("需要升级App，才可进入!!!");
			}, delegate(string sendCode, string severCode)
			{
				this.ShowUpdateVerionHttpError(severCode);
				HLog.LogError("App Update 检查失败失败原因 : " + severCode);
			});
		}

		private void OnCheckResourcesUpdate()
		{
			GameApp.Resources.SwitchPreHotFix();
			GameApp.Resources.CheckUpdate(new Action<bool, long>(this.OnCheckUpdate), new Action<long>(this.OnDownPrompt), new Action<float, long, long>(this.OnCheckUpdateProgress), new Action<bool, string>(this.OnCheckUpdateFinished));
		}

		private void OnCheckUpdate(bool isUpdate, long fileSize)
		{
			this.m_isUpdate = isUpdate;
			if (isUpdate)
			{
				GameApp.SDK.Analyze.TrackHotUpdate("start", fileSize, "", "");
				this.SwitchState(CheckAssetsViewModule.State.Download);
				return;
			}
			this.SwitchState(CheckAssetsViewModule.State.LoadResource);
		}

		public async void WaitStopForDownLoad()
		{
			await GameApp.RunTime.StopPreLoad();
			this.SwitchState(CheckAssetsViewModule.State.Download);
		}

		public async void WaitStopForLoadResource()
		{
			await GameApp.RunTime.StopPreLoad();
			this.SwitchState(CheckAssetsViewModule.State.LoadResource);
		}

		private void OnDownPrompt(long downPrompt)
		{
			this.ShowUpdateResources((float)downPrompt / 1024f / 1024f);
		}

		private void OnCheckUpdateProgress(float progress, long current, long all)
		{
			this.SetPercent(progress, current, all);
		}

		private void OnCheckUpdateFinished(bool isOk, string error)
		{
			if (!isOk)
			{
				GameApp.SDK.Analyze.TrackHotUpdate("fail", 0L, error, "");
			}
			if (!isOk)
			{
				this.ShowUpdateResourcesHttpError(error);
				return;
			}
			RunTimeManager runTime = GameApp.RunTime;
			runTime.m_onFinished = (Action)Delegate.Combine(runTime.m_onFinished, new Action(this.OnRunTimeFinished));
			GameApp.RunTime.Load();
			GameApp.SDK.Analyze.TrackLogin("加载资源", null);
		}

		private void OnRunTimeFinished()
		{
		}

		public void SwitchState(CheckAssetsViewModule.State state)
		{
			switch (state)
			{
			case CheckAssetsViewModule.State.CheckVersion:
				this.m_percent = 0f;
				this.SetPercent(this.m_percent);
				this.SetProgressTxt(1, Array.Empty<object>());
				this.PlayProgress();
				break;
			case CheckAssetsViewModule.State.Download:
				this.m_percent = 0f;
				this.SetPercent(this.m_percent);
				this.SetProgressTxt(2, Array.Empty<object>());
				this.StopProgress();
				break;
			case CheckAssetsViewModule.State.LoadResource:
				this.m_percent = 0f;
				this.SetPercent(this.m_percent);
				this.SetProgressTxt(3, Array.Empty<object>());
				this.PlayProgress();
				break;
			default:
				throw new ArgumentOutOfRangeException("state", state, null);
			}
			this.m_state = state;
		}

		private void PlayProgress()
		{
			this.m_time = 0f;
			this.m_isPlaying = true;
		}

		private void StopProgress()
		{
			this.m_isPlaying = false;
		}

		public void SetPercent(float percent)
		{
			this.m_progress.value = percent;
		}

		public void SetPercent(float percent, long currentSize, long maxSize)
		{
			this.m_progress.value = percent;
			float num = (float)currentSize / 1024f / 1024f;
			float num2 = (float)maxSize / 1024f / 1024f;
			string text = this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, 2);
			text = string.Format("{0} {1:0.00}MB / {2:0.00}MB", text, num, num2);
			if (this.m_progressTxt != null)
			{
				this.m_progressTxt.text = text;
			}
		}

		public void SetProgressTxt(int id, params object[] args)
		{
			if (this.m_progressTxt == null)
			{
				return;
			}
			this.m_progressTxt.text = string.Format(this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, id), args);
		}

		private void ShowTips(bool value)
		{
			if (this.m_tipGroup == null)
			{
				return;
			}
			this.m_tipGroup.gameObject.SetActive(value);
		}

		public void SetTipOkBtnTxt(int id)
		{
			if (this.m_tipOkBtTxt == null)
			{
				return;
			}
			this.m_tipOkBtTxt.text = this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, id);
		}

		private void ShowUpdateVersion()
		{
			this.ShowTips(true);
			this.m_tipState = CheckAssetsViewModule.TipState.UpdateVersion;
			if (this.m_tipTitleTxt != null)
			{
				this.m_tipTitleTxt.text = this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, 10);
			}
			if (this.m_tipConentTxt != null)
			{
				this.m_tipConentTxt.text = this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, 9);
			}
			this.m_onClickTipOkBtn = delegate
			{
				string @string = GameApp.Config.GetString("ChannelName");
				Application.OpenURL(Singleton<PathManager>.Instance.GetAppUrl(@string));
			};
		}

		private void ShowUpdateResources(float mb)
		{
			this.ShowTips(true);
			if (this.m_tipTitleTxt != null)
			{
				this.m_tipTitleTxt.text = this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, 10);
			}
			if (this.m_tipConentTxt != null)
			{
				this.m_tipConentTxt.text = string.Format(this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, 7), string.Format("{0:0.00}", mb));
			}
			this.m_onClickTipOkBtn = async delegate
			{
				this.ShowTips(false);
				await GameApp.Resources.OnDownAssets();
			};
			this.m_tipState = CheckAssetsViewModule.TipState.UpdateResources;
		}

		private void ShowUpdateVerionHttpError(string code)
		{
			this.ShowTips(true);
			if (this.m_tipTitleTxt != null)
			{
				this.m_tipTitleTxt.text = this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, 10);
			}
			if (this.m_tipConentTxt != null)
			{
				this.m_tipConentTxt.text = string.Format(this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, 6), code);
			}
			this.m_onClickTipOkBtn = delegate
			{
				this.ShowTips(false);
				this.OnCheckAsset();
			};
			this.m_tipState = CheckAssetsViewModule.TipState.UpdateVerionHttpError;
		}

		private void ShowUpdateResourcesHttpError(string code)
		{
			this.ShowTips(true);
			if (this.m_tipTitleTxt != null)
			{
				this.m_tipTitleTxt.text = this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, 10);
			}
			if (this.m_tipConentTxt != null)
			{
				this.m_tipConentTxt.text = string.Format(this.m_checkAssetsStaticData.GetLanguageListString(this.m_languageDataModule.GetCurrentLanguageType, 6), code);
			}
			this.m_onClickTipOkBtn = delegate
			{
				this.ShowTips(false);
				this.OnCheckResourcesUpdate();
			};
			this.m_tipState = CheckAssetsViewModule.TipState.UpdateResourcesHttpError;
		}

		[Label]
		public CheckAssetsViewModule.State m_state;

		[Label]
		public CheckAssetsViewModule.TipState m_tipState;

		public Slider m_progress;

		public CustomText m_progressTxt;

		public RectTransform m_tipGroup;

		public CustomText m_tipTitleTxt;

		public CustomText m_tipConentTxt;

		public CustomButton m_tipOkBt;

		public CustomText m_tipOkBtTxt;

		public CheckAssetsStaticData m_checkAssetsStaticData;

		private LanguageDataModule m_languageDataModule;

		private float m_percent;

		private float m_time;

		private bool m_isPlaying;

		public float m_duration = 0.5f;

		private Action m_onClickTipOkBtn;

		private bool m_isUpdate;

		public enum State
		{
			CheckVersion,
			Download,
			LoadResource
		}

		public enum TipState
		{
			UpdateVersion,
			UpdateResources,
			UpdateVerionHttpError,
			UpdateResourcesHttpError
		}
	}
}

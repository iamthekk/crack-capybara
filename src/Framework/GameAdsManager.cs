using System;
using Habby.Ads;

namespace Framework
{
	public class GameAdsManager : AdsCallback
	{
		private GameAdsManager()
		{
		}

		public void InitAds()
		{
			AdsManager.initAds(GameAdsManager._gameAdsConfigConfig, delegate
			{
				AdsManager.rewarded_addcallback(this, 0);
			});
		}

		public void InitUmp(bool isShow)
		{
			AdsManager.InitUmp(GameAdsManager._gameAdsConfigConfig, isShow);
		}

		public void setGDPRConsentGranted()
		{
			AdsManager.setGDPRConsentGranted();
		}

		public bool IfAdsLoaded(int adapterId = 0)
		{
			return AdsManager.rewarded_ads_isLoaded(adapterId);
		}

		private void RefreshShowData(Action onShow, Action onClick, Action onReward, Action onClose, Action<string> onFail)
		{
			this.currentShow = null;
			this.currentClose = null;
			this.currentClick = null;
			this.currentReward = null;
			this.currentPlayFail = null;
			this.currentShow = onShow;
			this.currentClose = onClose;
			this.currentClick = onClick;
			this.currentReward = onReward;
			this.currentPlayFail = onFail;
		}

		public void PlayRewardVideo(Action<bool> callback, Action showCallback, Action<string> failCallback)
		{
			bool isAdRewardGet = false;
			this.TryShow(1, delegate
			{
				Action showCallback2 = showCallback;
				if (showCallback2 == null)
				{
					return;
				}
				showCallback2();
			}, delegate
			{
			}, delegate
			{
				isAdRewardGet = true;
			}, delegate
			{
				Action<bool> callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2(isAdRewardGet);
			}, delegate(string error)
			{
				Action<string> failCallback2 = failCallback;
				if (failCallback2 == null)
				{
					return;
				}
				failCallback2(error);
			});
		}

		public bool TryShow(int source, Action onShow, Action onClick, Action onReward, Action onClose, Action<string> onPlayFail)
		{
			if (!this.IfAdsLoaded(0))
			{
				if (onPlayFail != null)
				{
					onPlayFail("AD_NOT_READY");
				}
				return false;
			}
			this.RefreshShowData(onShow, onClick, onReward, onClose, onPlayFail);
			AdsManager.rewarded_ads_show(0, this, source);
			return true;
		}

		public void onRequest(AdsDriver sender, string networkName)
		{
		}

		public void onLoad(AdsDriver sender, string networkName)
		{
		}

		public void onFail(AdsDriver sender, string msg)
		{
		}

		public void onOpen(AdsDriver sender, string networkName)
		{
			Action action = this.currentShow;
			if (action == null)
			{
				return;
			}
			action();
		}

		public void onPlayFail(AdsDriver sender, string networkName)
		{
			Action<string> action = this.currentPlayFail;
			if (action == null)
			{
				return;
			}
			action("");
		}

		public void onClose(AdsDriver sender, string networkName)
		{
			Action action = this.currentClose;
			if (action == null)
			{
				return;
			}
			action();
		}

		public void onClick(AdsDriver sender, string networkName)
		{
			Action action = this.currentClick;
			if (action == null)
			{
				return;
			}
			action();
		}

		public void onReward(AdsDriver sender, string networkName)
		{
			Action action = this.currentReward;
			if (action == null)
			{
				return;
			}
			action();
		}

		private static GameAdsConfigInterface _gameAdsConfigConfig = new GameAdsConfigImpl();

		public static GameAdsManager Instance = new GameAdsManager();

		private Action currentShow;

		private Action currentClose;

		private Action currentReward;

		private Action currentClick;

		private Action<string> currentPlayFail;
	}
}

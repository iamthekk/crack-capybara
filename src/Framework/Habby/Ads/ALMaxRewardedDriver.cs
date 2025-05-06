using System;

namespace Habby.Ads
{
	internal class ALMaxRewardedDriver : BaseDriver
	{
		public override string getName()
		{
			return "ALMaxRewardedDriver" + this.adUnitId;
		}

		public ALMaxRewardedDriver(string adUnitId)
		{
			this.adUnitId = adUnitId;
		}

		public override void Init(AdsCallback callback)
		{
			base.LogFunc("Init()");
			MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += delegate(string adUnit, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[] { "MaxSdkCallbacks.Rewarded.OnAdLoadedEvent(", adUnit, ",", adInfo.NetworkName, ")" }));
				this.loaded = true;
				callback.onLoad(this, adInfo.NetworkName);
			};
			MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += delegate(string adUnit, MaxSdkBase.ErrorInfo errInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[]
				{
					"MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent(",
					adUnit,
					", ",
					errInfo.Code.ToString(),
					": ",
					errInfo.Message,
					")"
				}));
				this.loaded = false;
				callback.onFail(this, errInfo.Message);
			};
			MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += delegate(string adUnit, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[] { "MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent(", adUnit, ",", adInfo.NetworkName, ")" }));
				this.playing = true;
				callback.onOpen(this, adInfo.NetworkName);
			};
			MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += delegate(string adUnit, MaxSdkBase.ErrorInfo errInfo, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[]
				{
					"MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent(",
					adUnit,
					",",
					adInfo.NetworkName,
					", ",
					errInfo.Code.ToString(),
					": ",
					errInfo.Message,
					")"
				}));
				this.playing = false;
				this.busy = false;
				this.loaded = false;
				callback.onClose(this, adInfo.NetworkName);
				callback.onPlayFail(this, errInfo.Message);
			};
			MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += delegate(string adUnit, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[] { "MaxSdkCallbacks.Rewarded.OnAdHiddenEvent(", adUnit, ",", adInfo.NetworkName, ")" }));
				this.playing = false;
				this.busy = false;
				this.loaded = false;
				callback.onClose(this, adInfo.NetworkName);
			};
			MaxSdkCallbacks.Rewarded.OnAdClickedEvent += delegate(string adUnit, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[] { "MaxSdkCallbacks.Rewarded.OnAdClickedEvent(", adUnit, ",", adInfo.NetworkName, ")" }));
				callback.onClick(this, adInfo.NetworkName);
			};
			MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += delegate(string adUnit, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[] { "MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent(", adUnit, ",", adInfo.NetworkName, ")" }));
				callback.onReward(this, adInfo.NetworkName);
			};
			this.callback = callback;
		}

		public override void doRequest()
		{
			base.LogFunc("doRequest()");
			if (this.isLoaded())
			{
				base.LogFunc("AdsRequestHelper.doRequest: " + string.Format("isLoaded is true [{0}]", this.adUnitId));
				return;
			}
			base.LogFunc("MaxSdk.LoadRewardedAd(" + this.adUnitId + ")");
			MaxSdkAndroid.LoadRewardedAd(this.adUnitId);
			this.callback.onRequest(this, "ALMax");
		}

		public override bool isLoaded()
		{
			if (this.loaded != MaxSdkAndroid.IsRewardedAdReady(this.adUnitId))
			{
				base.LogFunc(string.Concat(new string[]
				{
					"loaded = ",
					this.loaded.ToString(),
					", MaxSdk.IsRewardedAdReady(",
					this.adUnitId,
					") = ",
					(!this.loaded).ToString()
				}));
			}
			return MaxSdkAndroid.IsRewardedAdReady(this.adUnitId);
		}

		public override bool Show()
		{
			base.LogFunc("Show()");
			if (this.isLoaded())
			{
				this.busy = true;
				base.LogFunc("MaxSdk.ShowRewardedAd(" + this.adUnitId + ")");
				MaxSdkAndroid.ShowRewardedAd(this.adUnitId, null, null);
				return true;
			}
			return false;
		}

		public override bool isBusy()
		{
			return this.busy;
		}

		public override bool isPlaying()
		{
			return this.playing;
		}

		private bool playing;

		private bool busy;

		private bool loaded;
	}
}

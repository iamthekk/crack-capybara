using System;

namespace Habby.Ads
{
	internal class ALMaxInterstitialDriver : BaseDriver
	{
		public override string getName()
		{
			return "ALMaxInterstitialDriver" + this.adUnitId;
		}

		public ALMaxInterstitialDriver(string adUnitId)
		{
			this.adUnitId = adUnitId;
		}

		public override void Init(AdsCallback callback)
		{
			base.LogFunc("Init()");
			MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += delegate(string adUnit, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[] { "MaxSdkCallbacks.Interstitial.OnAdLoadedEvent(", adUnit, ",", adInfo.NetworkName, ")" }));
				this.loaded = true;
				callback.onLoad(this, adInfo.NetworkName);
			};
			MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += delegate(string adUnit, MaxSdkBase.ErrorInfo errInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[]
				{
					"MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent(",
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
			MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += delegate(string adUnit, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[] { "MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent(", adUnit, ",", adInfo.NetworkName, ")" }));
				this.playing = true;
				callback.onOpen(this, adInfo.NetworkName);
			};
			MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += delegate(string adUnit, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[] { "MaxSdkCallbacks.Interstitial.OnAdHiddenEvent(", adUnit, ",", adInfo.NetworkName, ")" }));
				this.playing = false;
				this.busy = false;
				this.loaded = false;
				callback.onClose(this, adInfo.NetworkName);
			};
			MaxSdkCallbacks.Interstitial.OnAdClickedEvent += delegate(string adUnit, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[] { "MaxSdkCallbacks.Interstitial.OnAdClickedEvent(", adUnit, ",", adInfo.NetworkName, ")" }));
				callback.onClick(this, adInfo.NetworkName);
			};
			MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += delegate(string adUnit, MaxSdkBase.ErrorInfo errInfo, MaxSdkBase.AdInfo adInfo)
			{
				if (adUnit != this.adUnitId)
				{
					return;
				}
				this.LogFunc(string.Concat(new string[]
				{
					"MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent(",
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
			this.callback = callback;
		}

		public override void doRequest()
		{
			base.LogFunc("doRequest()");
			base.LogFunc("MaxSdk.LoadInterstitial(" + this.adUnitId + ")");
			MaxSdkAndroid.LoadInterstitial(this.adUnitId);
			this.callback.onRequest(this, "ALMax");
		}

		public override bool isLoaded()
		{
			if (this.loaded != MaxSdkAndroid.IsInterstitialReady(this.adUnitId))
			{
				base.LogFunc(string.Concat(new string[]
				{
					"loaded = ",
					this.loaded.ToString(),
					", MaxSdk.IsInterstitialReady(",
					this.adUnitId,
					") = ",
					(!this.loaded).ToString()
				}));
			}
			return MaxSdkAndroid.IsInterstitialReady(this.adUnitId);
		}

		public override bool isBusy()
		{
			return this.busy;
		}

		public override bool isPlaying()
		{
			return this.playing;
		}

		public override bool Show()
		{
			base.LogFunc("Show()");
			if (this.isLoaded())
			{
				this.busy = true;
				base.LogFunc("MaxSdk.ShowInterstitial(" + this.adUnitId + ")");
				MaxSdkAndroid.ShowInterstitial(this.adUnitId, null, null);
				return true;
			}
			return false;
		}

		private bool playing;

		private bool busy;

		private bool loaded;
	}
}

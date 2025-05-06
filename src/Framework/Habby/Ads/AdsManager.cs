using System;
using System.Collections.Generic;

namespace Habby.Ads
{
	public class AdsManager
	{
		public static void initAds(GameAdsConfigInterface gameAdsConfigInterface, Action onComplete = null)
		{
			if (!AdsManager.Initialize)
			{
				AdsTgaUtil.init(new Action<string, Dictionary<string, object>>(gameAdsConfigInterface.track));
				AdsRequestHelper.Init(gameAdsConfigInterface, delegate
				{
					AdsManager.Initialize = true;
					Action onComplete3 = onComplete;
					if (onComplete3 == null)
					{
						return;
					}
					onComplete3();
				});
				return;
			}
			Action onComplete2 = onComplete;
			if (onComplete2 == null)
			{
				return;
			}
			onComplete2();
		}

		public static void setGDPRConsentGranted()
		{
			AdsRequestHelper.setGDPRConsentGranted();
		}

		public static void InitUmp(GameAdsConfigInterface gameAdsConfigInterface, bool isShow)
		{
			AdsRequestHelper.InitUmp(gameAdsConfigInterface, isShow);
		}

		public static void rewarded_addcallback(AdsCallback callback, int adapterId = 0)
		{
			AdsRequestHelper.rewarded_addcallback(callback, adapterId);
		}

		public static bool rewarded_ads_isLoaded(int adapterId)
		{
			return AdsRequestHelper.rewarded_ads_isLoaded(adapterId);
		}

		public static void rewarded_ads_show(int adapterId, AdsCallback callback, int source = 0)
		{
			AdsRequestHelper.rewarded_ads_show(adapterId, callback, source);
		}

		private static bool Initialize;
	}
}

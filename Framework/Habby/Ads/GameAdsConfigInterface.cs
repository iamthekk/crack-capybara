using System;
using System.Collections.Generic;

namespace Habby.Ads
{
	public interface GameAdsConfigInterface
	{
		void track(string eventName, Dictionary<string, object> properties);

		AdsRequestHelper.AdsConfiguration GetDebugConfig();

		AdsRequestHelper.AdsConfiguration GetProductionConfig();

		List<string> GetTestDeviceId();

		bool isTest();
	}
}

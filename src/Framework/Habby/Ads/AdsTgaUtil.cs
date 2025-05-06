using System;
using System.Collections.Generic;
using Framework;
using ThinkingData.Analytics;

namespace Habby.Ads
{
	internal static class AdsTgaUtil
	{
		public static void init(Action<string, Dictionary<string, object>> trackAction)
		{
			AdsTgaUtil.gametrack = trackAction;
		}

		public static void send_ad_value(Dictionary<string, object> adValue, Dictionary<string, object> adResponseInfo)
		{
			AdsTgaUtil.gametrack("ad_value", new Dictionary<string, object>
			{
				{ "ad_value", adValue },
				{ "ad_response_info", adResponseInfo }
			});
		}

		public static void send_ad_debug(DateTime logAdTime, string logAdUnit, string logEvent, bool isInterstitial, string errMsg, int tryCount, float duration, string requestId, string networkName, string source)
		{
			if (GameApp.Config.GetBool("IsReleaseServer"))
			{
				return;
			}
			TDAnalytics.Track("ad_debug", new Dictionary<string, object>
			{
				{ "ad_unit", logAdUnit },
				{ "ad_time", logAdTime },
				{ "step", logEvent },
				{
					"source",
					source ?? ""
				},
				{
					"error_code",
					errMsg ?? ""
				},
				{ "count", tryCount },
				{ "duration", duration },
				{
					"ad_request_id",
					requestId ?? ""
				},
				{
					"ad_network",
					networkName ?? ""
				}
			}, "");
		}

		private static Action<string, Dictionary<string, object>> gametrack;
	}
}

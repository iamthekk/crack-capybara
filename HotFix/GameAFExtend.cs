using System;
using System.Collections.Generic;
using Framework;
using Framework.SDKManager;
using UnityEngine;

namespace HotFix
{
	public static class GameAFExtend
	{
		public static void Track_AFPurchase(this SDKManager.SDKAppsFlyer sdk, decimal revenue, string currency, string contentid)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary[sdk.GetAFInAppEvents(49)] = revenue.ToString();
			dictionary[sdk.GetAFInAppEvents(30)] = currency;
			dictionary[sdk.GetAFInAppEvents(31)] = "1";
			dictionary[sdk.GetAFInAppEvents(28)] = contentid;
			sdk.sendEvent(sdk.GetAFInAppEvents(7), dictionary);
		}

		public static void Track_iaptimes_2()
		{
			GameApp.SDK.AppsFlyerSDK.Track("iaptimes_2", new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track("iaptimes_2", new Dictionary<string, string>());
		}

		public static void Track_account_login_finish()
		{
			GameApp.SDK.AppsFlyerSDK.Track("account_login_finish", new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track("account_login_finish", new Dictionary<string, string>());
		}

		public static void Track_user_login_finish()
		{
			GameApp.SDK.AppsFlyerSDK.Track("user_login_finish", new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track("user_login_finish", new Dictionary<string, string>());
		}

		public static void Track_start_game()
		{
			GameApp.SDK.AppsFlyerSDK.Track("start_game", new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track("start_game", new Dictionary<string, string>());
		}

		public static void Track_rv(int index)
		{
			string text = ((index == 1) ? "rv" : string.Format("rv_{0}", index));
			GameApp.SDK.AppsFlyerSDK.Track(text, new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track(text, new Dictionary<string, string>());
		}

		public static void Track_s_day(int day)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			GameApp.SDK.AppsFlyerSDK.Track(string.Format("s_day{0}", day), dictionary);
			GameApp.SDK.FirebaseSDK.Track(string.Format("s_day{0}", day), dictionary);
		}

		public static void Track_retention_day1()
		{
			GameApp.SDK.AppsFlyerSDK.Track("retention_day1", new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track("retention_day1", new Dictionary<string, string>());
		}

		public static void Track_p_stage(int chapter)
		{
			string text = string.Format("p_stage_{0}", chapter);
			GameApp.SDK.AppsFlyerSDK.Track(text, new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track(text, new Dictionary<string, string>());
		}

		public static void Track_p_day(int day)
		{
			string text = string.Format("p_day{0}", day);
			GameApp.SDK.AppsFlyerSDK.Track(text, new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track(text, new Dictionary<string, string>());
		}

		public static void Track_iap_24h()
		{
			GameApp.SDK.AppsFlyerSDK.Track("iap_24h", new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track("iap_24h", new Dictionary<string, string>());
		}

		public static void Track_purchase_package_stage(int index)
		{
			string text = string.Format("purchase_package_stage{0}", index);
			GameApp.SDK.AppsFlyerSDK.Track(text, new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track(text, new Dictionary<string, string>());
		}

		public static void Track_iap_times(int times)
		{
			string text = string.Format("iap_times_{0}", times);
			GameApp.SDK.AppsFlyerSDK.Track(text, new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track(text, new Dictionary<string, string>());
		}

		public static void Track_iap_battlepass()
		{
			GameApp.SDK.AppsFlyerSDK.Track("iap_battlepass", new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track("iap_battlepass", new Dictionary<string, string>());
		}

		public static void Track_purchase_r()
		{
			GameApp.SDK.AppsFlyerSDK.Track("purchase_r", new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track("purchase_r", new Dictionary<string, string>());
		}

		public static void Track_stage(int chapter, int stage)
		{
			string text = string.Format("stage_{0}_{1}", chapter, stage);
			GameApp.SDK.AppsFlyerSDK.Track(text, new Dictionary<string, string>());
			GameApp.SDK.FirebaseSDK.Track(text, new Dictionary<string, string>());
		}

		private static void Track(this SDKManager.SDKAppsFlyer sdk, string eventName, Dictionary<string, string> properties)
		{
			Debug.Log("[DebugAF]Track AF, " + eventName);
			sdk.sendEvent(eventName, properties);
		}

		private static void Track(this SDKManager.SDKFirebase sdk, string eventName, Dictionary<string, string> properties)
		{
			Debug.Log("[DebugAF]Track FireBase, " + eventName);
			sdk.SendEvent(eventName, properties);
		}

		public const bool EnableTrack = true;
	}
}

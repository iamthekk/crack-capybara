using System;
using Firebase.Crashlytics;

namespace Habby.Ads
{
	internal static class AdsFirebaseCrashUtil
	{
		public static void LogCrashlytics(this BaseDriver driver, string message)
		{
			Crashlytics.Log(message);
		}
	}
}

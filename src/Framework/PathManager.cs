using System;

namespace Framework
{
	public class PathManager : Singleton<PathManager>
	{
		public string GetResourcesUrl(bool isRelease)
		{
			if (!isRelease)
			{
				return "https://test-dataupdate.advrpg.com/";
			}
			return "https://dataupdate.advrpg.com/";
		}

		public string GetAppUrl(string channelName)
		{
			string text = string.Empty;
			if (!(channelName == "AppStore"))
			{
				if (channelName == "GoogleStore")
				{
					text = "https://play.google.com/store/apps/details?id=com.habby.capybara";
				}
			}
			else
			{
				text = "https://apps.apple.com/app/capybara-go/id6596787726";
			}
			return text;
		}

		public string GetResourcesSeverVersion(bool isRelease, string channelName)
		{
			string text = (isRelease ? "Release" : "Debug");
			return string.Concat(new string[]
			{
				this.GetResourcesUrl(isRelease),
				channelName,
				"_",
				text,
				"/Version.maggic"
			});
		}

		public const string AddressableCheckUpdateLabel = "CheckUpdate";
	}
}

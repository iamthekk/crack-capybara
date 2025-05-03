using System;

namespace Habby.Ads
{
	public interface AdsDriver
	{
		bool isLoaded();

		bool isBusy();

		bool isPlaying();

		bool Show();
	}
}

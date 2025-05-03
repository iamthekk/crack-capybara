using System;

namespace Habby.Ads
{
	public interface AdsCallback
	{
		void onRequest(AdsDriver sender, string networkName);

		void onLoad(AdsDriver sender, string networkName);

		void onFail(AdsDriver sender, string msg);

		void onOpen(AdsDriver sender, string networkName);

		void onPlayFail(AdsDriver sender, string networkName);

		void onClose(AdsDriver sender, string networkName);

		void onClick(AdsDriver sender, string networkName);

		void onReward(AdsDriver sender, string networkName);
	}
}

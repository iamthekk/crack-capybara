using System;

namespace Habby.Ads
{
	internal class WeChatMiniRewardDriver : BaseDriver
	{
		public override string getName()
		{
			return "WeChatMiniRewardDriver[" + this.adUnitId + "]";
		}

		public WeChatMiniRewardDriver(string adUnitId)
		{
			this.adUnitId = adUnitId;
		}

		public override void Init(AdsCallback callback)
		{
			throw new NotImplementedException();
		}

		public override bool isLoaded()
		{
			throw new NotImplementedException();
		}

		public override bool isBusy()
		{
			throw new NotImplementedException();
		}

		public override bool isPlaying()
		{
			throw new NotImplementedException();
		}

		public override bool Show()
		{
			throw new NotImplementedException();
		}

		public override void doRequest()
		{
			throw new NotImplementedException();
		}

		private bool playing;

		private bool busy;

		private bool loaded;
	}
}

using System;

namespace Habby.Ads
{
	internal abstract class BaseDriver : AdsDriver
	{
		public abstract void Init(AdsCallback callback);

		public abstract bool isLoaded();

		public abstract bool isBusy();

		public abstract bool isPlaying();

		public abstract bool Show();

		public abstract void doRequest();

		public abstract string getName();

		public virtual void updateConfig(string config)
		{
		}

		public virtual AdsCallback getCallback()
		{
			return this.callback;
		}

		public virtual void setCallback(AdsCallback callback)
		{
			this.callback = callback;
		}

		public void LogFunc(string log)
		{
		}

		protected string adUnitId;

		protected AdsCallback callback;
	}
}

using System;
using System.Collections.Generic;

namespace Habby.Ads
{
	internal class CallbackRouter : CallbackManager, AdsCallback
	{
		public void AddCallback(AdsCallback callback)
		{
			this.callbacks.Add(callback);
		}

		public void RemoveCallback(AdsCallback callback)
		{
			this.callbacks.Remove(callback);
		}

		public void SetExclusiveCallback(AdsCallback callback)
		{
			this.exclusiveCallback = callback;
		}

		public void onRequest(AdsDriver sender, string networkName)
		{
			foreach (AdsCallback adsCallback in this.callbacks)
			{
				adsCallback.onRequest(sender, networkName);
			}
		}

		public void onLoad(AdsDriver sender, string networkName)
		{
			foreach (AdsCallback adsCallback in this.callbacks)
			{
				adsCallback.onLoad(sender, networkName);
			}
		}

		public void onFail(AdsDriver sender, string msg)
		{
			foreach (AdsCallback adsCallback in this.callbacks)
			{
				adsCallback.onFail(sender, msg);
			}
		}

		public void onOpen(AdsDriver sender, string networkName)
		{
			if (this.exclusiveCallback != null)
			{
				if (this.callbacks.Contains(this.exclusiveCallback))
				{
					this.exclusiveCallback.onOpen(sender, networkName);
					return;
				}
			}
			else
			{
				foreach (AdsCallback adsCallback in this.callbacks)
				{
					adsCallback.onOpen(sender, networkName);
				}
			}
		}

		public void onPlayFail(AdsDriver sender, string networkName)
		{
			if (this.exclusiveCallback != null)
			{
				if (this.callbacks.Contains(this.exclusiveCallback))
				{
					this.exclusiveCallback.onPlayFail(sender, networkName);
					return;
				}
			}
			else
			{
				foreach (AdsCallback adsCallback in this.callbacks)
				{
					adsCallback.onPlayFail(sender, networkName);
				}
			}
		}

		public void onClose(AdsDriver sender, string networkName)
		{
			if (this.exclusiveCallback != null)
			{
				if (this.callbacks.Contains(this.exclusiveCallback))
				{
					this.exclusiveCallback.onClose(sender, networkName);
					return;
				}
			}
			else
			{
				foreach (AdsCallback adsCallback in this.callbacks)
				{
					adsCallback.onClose(sender, networkName);
				}
			}
		}

		public void onClick(AdsDriver sender, string networkName)
		{
			if (this.exclusiveCallback != null)
			{
				if (this.callbacks.Contains(this.exclusiveCallback))
				{
					this.exclusiveCallback.onClick(sender, networkName);
					return;
				}
			}
			else
			{
				foreach (AdsCallback adsCallback in this.callbacks)
				{
					adsCallback.onClick(sender, networkName);
				}
			}
		}

		public void onReward(AdsDriver sender, string networkName)
		{
			if (this.exclusiveCallback != null)
			{
				if (this.callbacks.Contains(this.exclusiveCallback))
				{
					this.exclusiveCallback.onReward(sender, networkName);
					return;
				}
			}
			else
			{
				foreach (AdsCallback adsCallback in this.callbacks)
				{
					adsCallback.onReward(sender, networkName);
				}
			}
		}

		private List<AdsCallback> callbacks = new List<AdsCallback>();

		private AdsCallback exclusiveCallback;
	}
}

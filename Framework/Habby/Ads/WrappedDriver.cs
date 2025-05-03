using System;
using System.Collections;
using UnityEngine;

namespace Habby.Ads
{
	internal class WrappedDriver : BaseDriver, AdsCallback
	{
		public override string getName()
		{
			return this.driver.getName() + "+WrappedDriver";
		}

		public WrappedDriver(char loopId, BaseDriver driver)
		{
			this.driver = driver;
			this.loopId = loopId;
		}

		public override void Init(AdsCallback callback)
		{
			base.LogFunc("Init()");
			this.callback = callback;
			this.queue = AdsRequestHelper.inst.queue;
			this.loop = new AdsRequestHelper.RequestLoop(new AdsRequestHelper.RequestLoop.DoRequest(this.driver.doRequest), new AdsRequestHelper.RequestLoop.CheckLoaded(this.driver.isLoaded), new AdsRequestHelper.RequestLoop.ReportError(this.reportError));
			this.driver.Init(this);
			this.loop.Init();
			if (!AdsRequestHelper.inst.loops.ContainsKey(this.loopId))
			{
				base.LogFunc("AdsRequestHelper loops add " + this.loopId.ToString() + ", " + this.getName());
				AdsRequestHelper.inst.loops.Add(this.loopId, this.loop);
				return;
			}
			base.LogFunc(string.Concat(new string[]
			{
				"AdsRequestHelper.loops.Add(",
				this.loopId.ToString(),
				", ",
				this.getName(),
				") fail."
			}));
		}

		protected void reportError(string error)
		{
			this.onFail(this, error);
		}

		public override void doRequest()
		{
			base.LogFunc("doRequest()");
		}

		public override bool isLoaded()
		{
			return this.driver.isLoaded();
		}

		public override bool isBusy()
		{
			return this.driver.isBusy();
		}

		public override bool isPlaying()
		{
			return this.driver.isPlaying();
		}

		public override bool Show()
		{
			base.LogFunc("Show()");
			if (this.isLoaded())
			{
				this.loop.onOpen();
			}
			return this.driver.Show();
		}

		public void onRequest(AdsDriver sender, string networkName)
		{
			this.queue.Run(delegate
			{
				this.LogFunc("onRequest()");
				this.callback.onRequest(this, networkName);
			});
		}

		public void onLoad(AdsDriver sender, string networkName)
		{
			this.queue.Run(delegate
			{
				this.LogFunc("onLoad()");
				this.callback.onLoad(this, networkName);
				this.loop.onLoad();
			});
		}

		public void onFail(AdsDriver sender, string msg)
		{
			this.queue.Run(delegate
			{
				this.LogFunc("onFail()");
				this.callback.onFail(this, msg);
				this.loop.onFail();
			});
		}

		public void onOpen(AdsDriver sender, string networkName)
		{
			this.ResetCloseCheck();
			this.queue.Run(delegate
			{
				this.LogFunc("onOpen()");
				this.callback.onOpen(this, networkName);
			});
		}

		public void onPlayFail(AdsDriver sender, string networkName)
		{
			this.queue.Run(delegate
			{
				this.LogFunc("onPlayFail()");
				this.callback.onPlayFail(this, networkName);
			});
		}

		public void onClose(AdsDriver sender, string networkName)
		{
			this.queue.Run(delegate
			{
				this.LogFunc("onClose()");
				if (this.rewardGot || !this.cacheClose)
				{
					this.rewardGot = false;
					this.callback.onClose(this, networkName);
				}
				else
				{
					this.closeGot = true;
					this.closeCheck = AdsRequestHelper.inst.StartCoroutine(this.CloseCheck(networkName));
				}
				this.loop.onClose();
			});
		}

		private IEnumerator CloseCheck(string networkName)
		{
			yield return new WaitForSeconds(0.5f);
			this.closeGot = false;
			this.closeCheck = null;
			this.callback.onClose(this, networkName);
			yield break;
		}

		private void ResetCloseCheck()
		{
			this.rewardGot = false;
			this.closeGot = false;
			if (this.closeCheck != null)
			{
				AdsRequestHelper.inst.StopCoroutine(this.closeCheck);
				this.closeCheck = null;
			}
		}

		public void onClick(AdsDriver sender, string networkName)
		{
			this.queue.Run(delegate
			{
				this.LogFunc("onClick()");
				this.callback.onClick(this, networkName);
			});
		}

		public void onReward(AdsDriver sender, string networkName)
		{
			this.queue.Run(delegate
			{
				this.LogFunc("onReward()");
				this.callback.onReward(this, networkName);
				if (this.closeGot)
				{
					this.ResetCloseCheck();
					this.callback.onClose(this, networkName);
					return;
				}
				this.rewardGot = true;
			});
		}

		private BaseDriver driver;

		private AdsRequestHelper.RequestLoop loop;

		private AdsRequestHelper.MsgQueue queue;

		private char loopId;

		private bool cacheClose = true;

		private bool rewardGot;

		private bool closeGot;

		private Coroutine closeCheck;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Habby.Ads
{
	internal class CombinedDriver : BaseDriver, AdsCallback
	{
		public override string getName()
		{
			return "CombinedDriver";
		}

		public override void updateConfig(string config)
		{
			base.LogFunc("UpdateConfig(" + config + ")");
			if (config == null || this.driverList == null)
			{
				return;
			}
			string[] array = config.Split(',', StringSplitOptions.None);
			CombinedDriver.Strategy strategy = CombinedDriver.Strategy.PRIORITIZED;
			List<BaseDriver> list = new List<BaseDriver>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].Length >= 1)
				{
					char c = array[i][0];
					if (this.driverList.ContainsKey(c))
					{
						int num;
						if (array[i].Length > 1)
						{
							if (!int.TryParse(array[i].Substring(1), out num) || num <= 0)
							{
								goto IL_00B2;
							}
							strategy = CombinedDriver.Strategy.RANDOM;
						}
						else
						{
							num = 0;
						}
						list.Add(this.driverList[c]);
						list2.Add(num);
					}
				}
				IL_00B2:;
			}
			this.rates = list2.ToArray();
			this.drivers = list.ToArray();
			this.strategy = strategy;
			base.LogFunc(string.Concat(new string[]
			{
				"UpdateConfig(",
				config,
				") SUCCESS, strategy = ",
				this.strategy.ToString(),
				", drivers = ",
				this.drivers.Length.ToString()
			}));
			for (int j = 0; j < this.drivers.Length; j++)
			{
				base.LogFunc(string.Concat(new string[]
				{
					"drivers[",
					j.ToString(),
					"] = ",
					this.drivers[j].getName(),
					", rates[",
					j.ToString(),
					"] = ",
					this.rates[j].ToString()
				}));
			}
		}

		public CombinedDriver(Dictionary<char, BaseDriver> driverList, string defaultConfig)
		{
			this.driverList = driverList;
			this.updateConfig(defaultConfig);
		}

		public override void Init(AdsCallback callback)
		{
			base.LogFunc("Init()");
			this.callback = callback;
			if (this.drivers != null)
			{
				foreach (BaseDriver baseDriver in this.drivers)
				{
					AdsCallback callback2 = baseDriver.getCallback();
					CallbackRouter callbackRouter;
					if (callback2 == null)
					{
						callbackRouter = new CallbackRouter();
						baseDriver.Init(callbackRouter);
					}
					else if (callback2 is CallbackRouter)
					{
						callbackRouter = (CallbackRouter)callback2;
					}
					else
					{
						callbackRouter = new CallbackRouter();
						callbackRouter.AddCallback(callback2);
						baseDriver.setCallback(callbackRouter);
					}
					callbackRouter.AddCallback(this);
				}
			}
			this.loaded = false;
		}

		public override void doRequest()
		{
			if (this.driverList != null)
			{
				base.LogFunc("doRequest() This shouldn't be called.");
				foreach (BaseDriver baseDriver in this.driverList.Values)
				{
					baseDriver.doRequest();
				}
			}
			this.loaded = false;
		}

		public override bool isLoaded()
		{
			if (this.drivers == null)
			{
				return false;
			}
			for (int i = 0; i < this.drivers.Length; i++)
			{
				if ((this.strategy != CombinedDriver.Strategy.RANDOM || this.rates[i] != 0) && this.drivers[i].isLoaded())
				{
					return true;
				}
			}
			return false;
		}

		public override bool isBusy()
		{
			if (this.drivers == null)
			{
				return false;
			}
			for (int i = 0; i < this.drivers.Length; i++)
			{
				if (this.drivers[i].isBusy())
				{
					return true;
				}
			}
			return false;
		}

		public override bool isPlaying()
		{
			if (this.drivers == null)
			{
				return false;
			}
			for (int i = 0; i < this.drivers.Length; i++)
			{
				if (this.drivers[i].isPlaying())
				{
					return true;
				}
			}
			return false;
		}

		public override bool Show()
		{
			if (this.drivers == null)
			{
				return false;
			}
			base.LogFunc("Show()");
			int num = 0;
			List<int> list = new List<int>();
			for (int i = 0; i < this.drivers.Length; i++)
			{
				string[] array = new string[6];
				array[0] = "drivers[";
				array[1] = i.ToString();
				array[2] = "] = ";
				array[3] = this.drivers[i].getName();
				array[4] = ", isLoaded = ";
				array[5] = this.drivers[i].isLoaded().ToString();
				base.LogFunc(string.Concat(array));
				if (this.drivers[i].isLoaded())
				{
					if (this.strategy == CombinedDriver.Strategy.PRIORITIZED)
					{
						((CallbackRouter)this.drivers[i].getCallback()).SetExclusiveCallback(this);
						this.drivers[i].Show();
						return true;
					}
					num += this.rates[i];
					list.Add(i);
				}
			}
			if (this.strategy == CombinedDriver.Strategy.RANDOM)
			{
				if (num <= 0)
				{
					return false;
				}
				float num2 = Random.Range(0f, (float)num);
				foreach (int num3 in list)
				{
					num2 -= (float)this.rates[num3];
					if (num2 <= 0f)
					{
						((CallbackRouter)this.drivers[num3].getCallback()).SetExclusiveCallback(this);
						this.drivers[num3].Show();
						return true;
					}
				}
				return false;
			}
			return false;
		}

		public void onRequest(AdsDriver sender, string networkName)
		{
			base.LogFunc("onRequest()");
			for (int i = 0; i < this.drivers.Length; i++)
			{
				if (this.drivers[i] == sender)
				{
					this.callback.onRequest(this, networkName);
					return;
				}
			}
		}

		public void onLoad(AdsDriver sender, string networkName)
		{
			base.LogFunc("onLoad()");
			for (int i = 0; i < this.drivers.Length; i++)
			{
				if (this.drivers[i] == sender)
				{
					this.loaded = true;
					this.callback.onLoad(this, networkName);
					return;
				}
			}
		}

		public void onFail(AdsDriver sender, string msg)
		{
			base.LogFunc("onFail()");
			for (int i = 0; i < this.drivers.Length; i++)
			{
				if (this.drivers[i] == sender)
				{
					this.loaded = false;
					this.callback.onFail(this, msg);
					return;
				}
			}
		}

		public void onOpen(AdsDriver sender, string networkName)
		{
			base.LogFunc("onOpen()");
			this.callback.onOpen(this, networkName);
		}

		public void onPlayFail(AdsDriver sender, string networkName)
		{
			base.LogFunc("onPlayFail()");
			this.callback.onPlayFail(this, networkName);
		}

		public void onClose(AdsDriver sender, string networkName)
		{
			base.LogFunc("onClose()");
			this.callback.onClose(this, networkName);
		}

		public void onClick(AdsDriver sender, string networkName)
		{
			base.LogFunc("onClick()");
			this.callback.onClick(this, networkName);
		}

		public void onReward(AdsDriver sender, string networkName)
		{
			base.LogFunc("onReward()");
			this.callback.onReward(this, networkName);
		}

		private BaseDriver[] drivers;

		private int[] rates;

		private Dictionary<char, BaseDriver> driverList;

		private bool loaded;

		private CombinedDriver.Strategy strategy;

		private enum Strategy
		{
			PRIORITIZED,
			RANDOM
		}
	}
}

using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class AdDataModule : IDataModule
	{
		public Dictionary<int, AdData> adDataDict { get; private set; } = new Dictionary<int, AdData>();

		public long resetTime { get; private set; }

		public int AccountTotalADTimes { get; private set; }

		public int GetName()
		{
			return 155;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
			this.adDataDict.Clear();
			this.resetTime = 0L;
		}

		public AdData GetAdData(int adId)
		{
			bool flag = this.CheckCloudDataAdOpen();
			AdData adData;
			if (this.adDataDict.TryGetValue(adId, out adData))
			{
				return adData;
			}
			Shop_Ad elementById = GameApp.Table.GetManager().GetShop_AdModelInstance().GetElementById(adId);
			if (elementById == null)
			{
				HLog.LogError("AdDataModule.GetAdMaxTimes adTable is null, adId = " + adId.ToString());
				return null;
			}
			adData = new AdData();
			adData.watchCountMax = ((!flag) ? 0 : elementById.adTimes);
			adData.adCountDown = elementById.adCountDown;
			this.adDataDict.Add(adId, adData);
			return adData;
		}

		public int GetWatchTimes(int configId)
		{
			if (configId <= 0)
			{
				return 0;
			}
			AdData adData;
			if (this.adDataDict.TryGetValue(configId, out adData))
			{
				return adData.watchCount;
			}
			return 0;
		}

		public int GetMaxTimes(int adId)
		{
			if (adId <= 0)
			{
				return 0;
			}
			if (!this.CheckCloudDataAdOpen())
			{
				return 0;
			}
			AdData adData;
			if (this.adDataDict.TryGetValue(adId, out adData))
			{
				return adData.watchCountMax;
			}
			Shop_Ad elementById = GameApp.Table.GetManager().GetShop_AdModelInstance().GetElementById(adId);
			if (elementById == null)
			{
				HLog.LogError("AdDataModule.GetAdMaxTimes adTable is null, adId = " + adId.ToString());
				return 0;
			}
			return elementById.adTimes;
		}

		public void InitServerData(AdDataDto adDataDto)
		{
			this.resetTime = 0L;
			this.adDataDict.Clear();
			this.UpdateAdData(adDataDto);
		}

		public void UpdateAdData(AdDataDto adDataDto)
		{
			if (adDataDto == null)
			{
				return;
			}
			foreach (KeyValuePair<int, AdData> keyValuePair in this.adDataDict)
			{
				keyValuePair.Value.watchCount = 0;
			}
			this.resetTime = (long)adDataDto.ResetTime;
			long num = 0L;
			foreach (KeyValuePair<uint, uint> keyValuePair2 in adDataDto.AdDataMap)
			{
				AdData adData;
				if (!this.adDataDict.TryGetValue((int)keyValuePair2.Key, out adData))
				{
					int key = (int)keyValuePair2.Key;
					Shop_Ad elementById = GameApp.Table.GetManager().GetShop_AdModelInstance().GetElementById(key);
					if (elementById == null)
					{
						HLog.LogError("AdDataModule.UpdateAdData adTable is null, adId = " + key.ToString());
						continue;
					}
					adData = new AdData();
					adData.watchCountMax = elementById.adTimes;
					adData.adCountDown = elementById.adCountDown;
					this.adDataDict.Add((int)keyValuePair2.Key, adData);
				}
				if (adData != null)
				{
					adData.watchCount = (int)keyValuePair2.Value;
					ulong num2;
					if (adDataDto.LastAdvTime != null && adDataDto.LastAdvTime.TryGetValue(keyValuePair2.Key, ref num2))
					{
						adData.lastWatchTime = (long)num2;
					}
					else
					{
						adData.lastWatchTime = 0L;
					}
					long watchCD = this.GetWatchCD(adData.adId);
					if (watchCD > 0L)
					{
						if (num == 0L)
						{
							num = watchCD;
						}
						else if (watchCD < num)
						{
							num = watchCD;
						}
					}
				}
				if (num > 0L)
				{
					long num3 = DxxTools.Time.ServerTimestamp + num;
					DxxTools.UI.RemoveServerTimeClockCallback("ADTimeRefreshKey");
					DxxTools.UI.AddServerTimeCallback("ADTimeRefreshKey", delegate
					{
						RedPointController.Instance.ReCalc("Main.Shop", true);
					}, num3, 0);
				}
			}
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.AD);
		}

		public void UpdateTGAInfo(TGAInfoDto dto)
		{
			if (dto == null)
			{
				return;
			}
			this.AccountTotalADTimes = dto.TotalAdvTimes;
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.AD);
		}

		public void UpdateAdWatchTime(int adId)
		{
			AdData adData;
			if (this.adDataDict.TryGetValue(adId, out adData))
			{
				long serverTimestamp = DxxTools.Time.ServerTimestamp;
				adData.lastWatchTime = serverTimestamp;
			}
		}

		public long GetWatchCD(int adId)
		{
			AdData adData;
			if (this.adDataDict.TryGetValue(adId, out adData))
			{
				if (adData.watchCountMax == 1 || adData.watchCount == adData.watchCountMax)
				{
					return 0L;
				}
				long num = DxxTools.Time.ServerTimestamp - adData.lastWatchTime;
				if (adData.adCountDown > 0)
				{
					long num2 = (long)adData.adCountDown - num;
					if (num2 <= 0L)
					{
						return 0L;
					}
					return num2;
				}
			}
			return 0L;
		}

		public bool CheckCloudDataAdOpen()
		{
			return GameApp.SDK.GetCloudDataValue<bool>("IfShowAd", true);
		}

		private const string ADTimeRefreshKey = "ADTimeRefreshKey";
	}
}

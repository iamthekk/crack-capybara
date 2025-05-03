using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public class PushGiftDataModule : IDataModule
	{
		public int GetName()
		{
			return 159;
		}

		public bool HaveValidPushGiftData(PushGiftPosType posType)
		{
			bool flag = false;
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			List<PushGiftData> list;
			if (this.PushGiftDataDicByPosType.TryGetValue(posType, out list))
			{
				using (List<PushGiftData>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.EndTime > serverTimestamp)
						{
							flag = true;
							break;
						}
					}
				}
			}
			return flag;
		}

		public void UpdateData(PushIapDto pushIapDto)
		{
			if (pushIapDto == null || pushIapDto.PushIapItemDto.Count == 0)
			{
				this.PushGiftDataDic.Clear();
				this.PushGiftDataDicByPosType.Clear();
				GameApp.Event.DispatchNow(this, 247, null);
				return;
			}
			List<PushIapItemDto> list = pushIapDto.PushIapItemDto.OrderBy((PushIapItemDto x) => x.ETime).ToList<PushIapItemDto>();
			this.PushGiftDataDicByPosType.Clear();
			this._tempAddDatas = new List<PushGiftData>();
			foreach (PushIapItemDto pushIapItemDto in list)
			{
				PushGiftData pushGiftData = new PushGiftData();
				if (pushGiftData.Init(pushIapItemDto))
				{
					if (!this.PushGiftDataDic.ContainsKey(pushIapItemDto.ConfigId))
					{
						this.AddPushGift(pushGiftData);
					}
					this._tempAddDatas.Add(pushGiftData);
				}
			}
			this.PushGiftDataDic.Clear();
			foreach (PushGiftData pushGiftData2 in this._tempAddDatas)
			{
				this.PushGiftDataDic.Add(pushGiftData2.ConfigId, pushGiftData2);
				if (this.PushGiftDataDicByPosType.ContainsKey(pushGiftData2.PosType))
				{
					this.PushGiftDataDicByPosType[pushGiftData2.PosType].Add(pushGiftData2);
				}
				else
				{
					this.PushGiftDataDicByPosType.Add(pushGiftData2.PosType, new List<PushGiftData> { pushGiftData2 });
				}
			}
			GameApp.Event.DispatchNow(this, 247, null);
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CS_RefreshPushGiftData, new HandlerEvent(this.OnRefreshPushGiftData));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.OnLoginFinish));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_DayChangeRefreshPushGiftData, new HandlerEvent(this.OnRefreshPushGiftDataByDayChange));
			this.RegisterSupplyGiftEvent();
		}

		private void OnLoginFinish(object obj, int type, BaseEventArgs args)
		{
			this.OnRefreshPushGiftDataByDayChange(obj, type, args);
			this.InitSupplyData();
		}

		private void OnRefreshPushGiftData(object obj, int type, BaseEventArgs args)
		{
			if (Time.time - this._lastReqTime < 30f)
			{
				return;
			}
			this._lastReqTime = Time.time;
			NetworkUtils.PushGift.DoRefreshPushGiftData(delegate(bool isSuccess, GetIapPushDtoResponse response)
			{
				if (isSuccess)
				{
					this.UpdateData(response.PushIapDto);
				}
			});
		}

		private void OnRefreshPushGiftDataByDayChange(object obj, int type, BaseEventArgs args)
		{
			NetworkUtils.PushGift.DoRefreshPushGiftData(delegate(bool isSuccess, GetIapPushDtoResponse response)
			{
				if (isSuccess)
				{
					this.UpdateData(response.PushIapDto);
				}
			});
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CS_RefreshPushGiftData, new HandlerEvent(this.OnRefreshPushGiftData));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.OnLoginFinish));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_DayChangeRefreshPushGiftData, new HandlerEvent(this.OnRefreshPushGiftDataByDayChange));
			this.UnRegisterSupplyGiftEvent();
		}

		private void AddPushGift(PushGiftData data)
		{
			this._pushGiftDatas.Add(data);
		}

		private void OnTriggerNewPop(object obj, int type, BaseEventArgs eventArgs)
		{
			if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.PushGift))
			{
				return;
			}
			if (this._pushGiftDatas.Count > 0)
			{
				PushGiftData pushGiftData = this.GetPushGiftData();
				if (pushGiftData == null)
				{
					return;
				}
				this._pushGiftDatas.Remove(pushGiftData);
				if (DxxTools.Time.ServerTimestamp < pushGiftData.EndTime)
				{
					pushGiftData.IsPop = true;
					GameApp.View.OpenView(ViewName.PushGiftPopViewModule, pushGiftData, 2, null, null);
					return;
				}
				this.OnTriggerNewPop(null, 0, null);
			}
		}

		private void OnClosePopView(object obj, int type, BaseEventArgs eventArgs)
		{
			EventArgsPushGiftData eventArgsPushGiftData = eventArgs as EventArgsPushGiftData;
			if (eventArgsPushGiftData == null)
			{
				return;
			}
			if (eventArgsPushGiftData.PushGiftData.PushConfig.type == 0)
			{
				this.OnTriggerNewPop(obj, type, eventArgs);
			}
		}

		private PushGiftData GetPushGiftData()
		{
			if (this._pushGiftDatas.Count <= 0)
			{
				return null;
			}
			PushGiftData pushGiftData = this._pushGiftDatas.First<PushGiftData>();
			if (this.PushGiftDataDic.ContainsKey(pushGiftData.ConfigId))
			{
				return pushGiftData;
			}
			this._pushGiftDatas.Remove(pushGiftData);
			return this.GetPushGiftData();
		}

		public void Reset()
		{
			this.PushGiftDataDic.Clear();
		}

		private void RegisterSupplyGiftEvent()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnBuySupplyGiftSuccess));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_DAY_CHANGE, new HandlerEvent(this.ClearLocalData));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ItemNotEnough, new HandlerEvent(this.OnItemNotEnoughEvent));
		}

		private void UnRegisterSupplyGiftEvent()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnBuySupplyGiftSuccess));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_DAY_CHANGE, new HandlerEvent(this.ClearLocalData));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ItemNotEnough, new HandlerEvent(this.OnItemNotEnoughEvent));
			PlayerPrefsKeys.SetPlayerExitTime(DxxTools.Time.ServerTimestamp);
		}

		private void OnItemNotEnoughEvent(object obj, int type, BaseEventArgs args)
		{
			if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.PushGift))
			{
				return;
			}
			EventArgsInt eventArgsInt = args as EventArgsInt;
			if (eventArgsInt == null)
			{
				return;
			}
			SupplyData supplyData;
			if (this._supplyDataDic.TryGetValue(eventArgsInt.Value, out supplyData))
			{
				if (supplyData.Progress == -1)
				{
					return;
				}
				PushGiftData pushGiftData = supplyData.Configs[supplyData.Progress];
				pushGiftData.IsPop = true;
				GameApp.View.OpenView(ViewName.PushGiftPopViewModule, pushGiftData, 2, null, null);
				this._popingData = supplyData;
			}
		}

		public PushGiftData OnGetSupplyData(int conditionParams)
		{
			SupplyData supplyData;
			if (!this._supplyDataDic.TryGetValue(conditionParams, out supplyData))
			{
				return null;
			}
			if (supplyData.Progress == -1)
			{
				return null;
			}
			return supplyData.Configs[supplyData.Progress];
		}

		public SupplyData OnGetSupplyBaseData(int conditionParams)
		{
			SupplyData supplyData;
			if (!this._supplyDataDic.TryGetValue(conditionParams, out supplyData))
			{
				return null;
			}
			if (supplyData.Progress == -1)
			{
				return null;
			}
			return supplyData;
		}

		private void InitSupplyData()
		{
			this._supplyDataDic = new Dictionary<int, SupplyData>();
			Dictionary<int, List<IAP_pushIap>> dictionary = (from x in (from x in GameApp.Table.GetManager().GetIAP_pushIapElements()
					where x.type == 1
					select x).ToList<IAP_pushIap>()
				group x by x.conditionParams).ToDictionary((IGrouping<int, IAP_pushIap> x) => x.Key, (IGrouping<int, IAP_pushIap> x) => x.OrderBy((IAP_pushIap config) => config.id).ToList<IAP_pushIap>());
			foreach (int num in dictionary.Keys)
			{
				List<IAP_pushIap> list = dictionary[num];
				SupplyData supplyData = new SupplyData();
				supplyData.ItemId = num;
				supplyData.Progress = PlayerPrefsKeys.GetUserSupplyGift_Record(num);
				supplyData.MaxProgress = list.Count;
				supplyData.Configs = new List<PushGiftData>();
				foreach (IAP_pushIap iap_pushIap in list)
				{
					PushGiftData pushGiftData = new PushGiftData();
					pushGiftData.Init(iap_pushIap);
					supplyData.Configs.Add(pushGiftData);
				}
				this._supplyDataDic.Add(num, supplyData);
			}
			long playerLastExitTime = PlayerPrefsKeys.GetPlayerLastExitTime();
			bool flag = false;
			if (playerLastExitTime == 0L)
			{
				flag = true;
			}
			else
			{
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				DateTime dateTime2 = dateTime.AddSeconds((double)playerLastExitTime).AddHours((double)DxxTools.Time.Timezone);
				DateTime serverLocal = DxxTools.Time.ServerLocal;
				if (dateTime2.Year != serverLocal.Year || dateTime2.Day != serverLocal.Day)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.ClearLocalData(null, 0, null);
			}
		}

		private void OnBuySupplyGiftSuccess(object obj, int type, BaseEventArgs args)
		{
			EventArgsBuySupplyGiftSuccess eventArgsBuySupplyGiftSuccess = args as EventArgsBuySupplyGiftSuccess;
			if (eventArgsBuySupplyGiftSuccess == null)
			{
				return;
			}
			int configId = eventArgsBuySupplyGiftSuccess.ConfigId;
			this.SetNextProgress(configId);
		}

		private void SetNextProgress(int itemId)
		{
			SupplyData supplyData;
			if (this._supplyDataDic.TryGetValue(itemId, out supplyData))
			{
				if (this._popingData != null && this._popingData.ItemId == itemId)
				{
					supplyData.Progress++;
				}
				else
				{
					supplyData.Progress = 0;
				}
				int maxProgress = supplyData.MaxProgress;
				if (supplyData.Progress == maxProgress)
				{
					supplyData.Progress = -1;
				}
				PlayerPrefsKeys.SetPlayerSupplyGift_Record(itemId, supplyData.Progress);
			}
		}

		private void ClearLocalData(object obj, int type, BaseEventArgs args)
		{
			this._popingData = null;
			foreach (int num in this._supplyDataDic.Keys)
			{
				this._supplyDataDic[num].Progress = 0;
				PlayerPrefsKeys.SetPlayerSupplyGift_Record(num, 0);
			}
		}

		public readonly Dictionary<int, PushGiftData> PushGiftDataDic = new Dictionary<int, PushGiftData>();

		public readonly Dictionary<PushGiftPosType, List<PushGiftData>> PushGiftDataDicByPosType = new Dictionary<PushGiftPosType, List<PushGiftData>>();

		public HashSet<PushGiftData> _pushGiftDatas = new HashSet<PushGiftData>();

		private long _serverTime;

		private float _lastReqTime = -30f;

		private const float ReqInterval = 30f;

		private List<PushGiftData> _tempAddDatas = new List<PushGiftData>();

		public SupplyData _popingData;

		private Dictionary<int, SupplyData> _supplyDataDic = new Dictionary<int, SupplyData>();
	}
}

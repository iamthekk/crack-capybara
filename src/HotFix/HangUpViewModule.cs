using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.Chapter;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HangUpViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.hangUpDataModule = GameApp.Data.GetDataModule(DataName.HangUpDataModule);
			this.adDataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnCloseSelf));
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnCloseSelf));
			this.buttonAd.onClick.AddListener(new UnityAction(this.OnClickAd));
			this.buttonReward.onClick.AddListener(new UnityAction(this.OnClickReward));
			this.copyCurrencyItem.SetActiveSafe(false);
			this.copyItem.SetActiveSafe(false);
			this.buttonAd.gameObject.SetActiveSafe(false);
			this.CreateCurrencyItem(CurrencyType.Gold);
			this.CreateCurrencyItem(CurrencyType.Diamond);
		}

		public override void OnOpen(object data)
		{
			this.Refresh();
			this.CheckReward();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.hangUpDataModule.HungUpInfoDto != null)
			{
				long num = DxxTools.Time.ServerTimestamp - this.hangUpDataModule.HungUpInfoDto.LastSettlementTime + (long)this.hangUpDataModule.HungUpInfoDto.TotalSettleTime;
				if (num < 0L)
				{
					num = 0L;
				}
				int num2 = this.hangUpDataModule.GetHangUpMaxTime() * 60;
				if (num >= (long)num2)
				{
					this.isMaxReward = true;
					string text = DxxTools.SecondsToTime((long)num2);
					this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("uihangup_collection_time_max", new object[] { text });
				}
				else
				{
					this.isMaxReward = false;
					this.textTime.text = DxxTools.SecondsToTime(num);
				}
				if (!this.isMaxReward && !this.isCheckReward)
				{
					int num3 = GameConfig.HangUp_Drop_Time * 60;
					if (num % (long)num3 == 1L)
					{
						this.CheckReward();
					}
				}
			}
			this.RefreshAdTime();
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnCloseSelf));
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnCloseSelf));
			this.buttonAd.onClick.RemoveListener(new UnityAction(this.OnClickAd));
			this.buttonReward.onClick.RemoveListener(new UnityAction(this.OnClickReward));
			foreach (UIHangUpCurrencyItem uihangUpCurrencyItem in this.currencyItemDic.Values)
			{
				uihangUpCurrencyItem.DeInit();
			}
			this.currencyItemDic.Clear();
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
			}
			this.itemList.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void CreateCurrencyItem(CurrencyType type)
		{
			if (!this.currencyItemDic.ContainsKey(type))
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.copyCurrencyItem);
				gameObject.SetParentNormal(this.dropCurrencyParent, false);
				gameObject.SetActiveSafe(true);
				UIHangUpCurrencyItem component = gameObject.GetComponent<UIHangUpCurrencyItem>();
				component.Init();
				this.currencyItemDic.Add(type, component);
			}
		}

		private void Refresh()
		{
			float num = (float)GameConfig.HangUp_AD_Time / 60f;
			this.textAdTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uihangup_ad", new object[] { num });
			int maxTimes = this.adDataModule.GetMaxTimes(GameConfig.HangUp_AD_ID);
			int watchTimes = this.adDataModule.GetWatchTimes(GameConfig.HangUp_AD_ID);
			long watchCD = this.adDataModule.GetWatchCD(GameConfig.HangUp_AD_ID);
			if (watchTimes >= maxTimes)
			{
				this.adGray.SetUIGray();
				this.redNodeAd.gameObject.SetActiveSafe(false);
			}
			else
			{
				this.adGray.Recovery();
				this.redNodeAd.gameObject.SetActiveSafe(watchCD == 0L);
			}
			if (this.hangUpDataModule.IsHaveReward())
			{
				this.collectGray.Recovery();
			}
			else
			{
				this.collectGray.SetUIGray();
			}
			bool flag = this.hangUpDataModule.IsMaxReward();
			this.redNodeMaxGet.gameObject.SetActiveSafe(flag);
			foreach (CurrencyType currencyType in this.currencyItemDic.Keys)
			{
				long dropCurrency = this.hangUpDataModule.GetDropCurrency(currencyType);
				int timeDropCurrency = this.hangUpDataModule.GetTimeDropCurrency(currencyType);
				this.currencyItemDic[currencyType].SetData(currencyType, dropCurrency, timeDropCurrency);
			}
			List<PropData> dropRewards = this.hangUpDataModule.GetDropRewards();
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].SetActive(false);
			}
			for (int j = 0; j < dropRewards.Count; j++)
			{
				UIItem uiitem;
				if (j < this.itemList.Count)
				{
					uiitem = this.itemList[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(this.dropItemParent, false);
					uiitem = gameObject.GetComponent<UIItem>();
					uiitem.Init();
					this.itemList.Add(uiitem);
				}
				uiitem.gameObject.SetActiveSafe(true);
				uiitem.SetData(dropRewards[j]);
				uiitem.OnRefresh();
			}
		}

		private void RefreshAdTime()
		{
			bool flag = this.iapDataModule.MonthCard.IsActivePrivilege(CardPrivilegeKind.NoAd);
			int maxTimes = this.adDataModule.GetMaxTimes(GameConfig.HangUp_AD_ID);
			int watchTimes = this.adDataModule.GetWatchTimes(GameConfig.HangUp_AD_ID);
			long watchCD = this.adDataModule.GetWatchCD(GameConfig.HangUp_AD_ID);
			this.textAd.text = ((!flag && watchCD > 0L && watchTimes < maxTimes) ? Singleton<LanguageManager>.Instance.GetTime(watchCD) : string.Format("({0}/{1})", maxTimes - watchTimes, maxTimes));
			this.redNodeAd.gameObject.SetActiveSafe(watchTimes < maxTimes && watchCD == 0L);
		}

		private void OnCloseSelf()
		{
			GameApp.View.CloseView(ViewName.HangUpViewModule, null);
		}

		private void OnClickAd()
		{
			int maxTimes = this.adDataModule.GetMaxTimes(GameConfig.HangUp_AD_ID);
			int watchTimes = this.adDataModule.GetWatchTimes(GameConfig.HangUp_AD_ID);
			int adTime = GameConfig.HangUp_AD_Time * 60;
			if (watchTimes < maxTimes)
			{
				Action<bool, GetHangUpRewardResponse> <>9__1;
				AdBridge.PlayRewardVideo(GameConfig.HangUp_AD_ID, delegate(bool isSuccess)
				{
					if (isSuccess)
					{
						bool flag = true;
						Action<bool, GetHangUpRewardResponse> action;
						if ((action = <>9__1) == null)
						{
							action = (<>9__1 = delegate(bool result, GetHangUpRewardResponse resp)
							{
								if (result && resp != null && resp.CommonData != null && resp.CommonData.Reward != null)
								{
									if (resp.CommonData.Reward.Count > 0)
									{
										DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
									}
									GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(GameConfig.HangUp_AD_ID), "REWARD ", "", resp.CommonData.Reward, null);
									GameApp.SDK.Analyze.Track_IdleReward((long)adTime, true, resp.CommonData.Reward);
								}
								if (GameApp.View.IsOpened(ViewName.HangUpViewModule))
								{
									this.Refresh();
								}
							});
						}
						NetworkUtils.Chapter.DoGetHangUpRewardRequest(flag, action);
					}
				});
				return;
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uihangup_noad");
			GameApp.View.ShowStringTip(infoByID);
		}

		private void OnClickReward()
		{
			if (this.hangUpDataModule.IsHaveReward())
			{
				long second = DxxTools.Time.ServerTimestamp - this.hangUpDataModule.HungUpInfoDto.LastSettlementTime + (long)this.hangUpDataModule.HungUpInfoDto.TotalSettleTime;
				if (second < 0L)
				{
					second = 0L;
				}
				int maxSecond = this.hangUpDataModule.GetHangUpMaxTime() * 60;
				NetworkUtils.Chapter.DoGetHangUpRewardRequest(false, delegate(bool result, GetHangUpRewardResponse response)
				{
					if (result)
					{
						if (response.CommonData.Reward != null && response.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(response.CommonData.Reward, null, true);
						}
						if (GameApp.View.IsOpened(ViewName.HangUpViewModule))
						{
							this.Refresh();
						}
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Refresh_HangUp, null);
						if (second >= (long)maxSecond)
						{
							GameApp.SDK.Analyze.Track_IdleReward((long)maxSecond, false, response.CommonData.Reward);
							return;
						}
						GameApp.SDK.Analyze.Track_IdleReward(second, false, response.CommonData.Reward);
					}
				});
				return;
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uihangup_noreward");
			GameApp.View.ShowStringTip(infoByID);
		}

		private void CheckReward()
		{
			this.isCheckReward = true;
			NetworkUtils.Chapter.DoGetHangUpInfoRequest(delegate(bool result, GetHangUpInfoResponse response)
			{
				if (GameApp.View.IsOpened(ViewName.HangUpViewModule))
				{
					this.Refresh();
				}
				DelayCall.Instance.CallOnce(2000, delegate
				{
					this.isCheckReward = false;
				});
			});
		}

		public CustomButton buttonMask;

		public CustomButton buttonClose;

		public CustomButton buttonAd;

		public CustomButton buttonReward;

		public UIGrays adGray;

		public UIGrays collectGray;

		public CustomText textAdTitle;

		public CustomText textAd;

		public CustomText textTime;

		public GameObject dropCurrencyParent;

		public GameObject dropItemParent;

		public GameObject copyCurrencyItem;

		public GameObject copyItem;

		public GameObject redNodeAd;

		public GameObject redNodeMaxGet;

		private Dictionary<CurrencyType, UIHangUpCurrencyItem> currencyItemDic = new Dictionary<CurrencyType, UIHangUpCurrencyItem>();

		private List<UIItem> itemList = new List<UIItem>();

		private HangUpDataModule hangUpDataModule;

		private AdDataModule adDataModule;

		private IAPDataModule iapDataModule;

		private LoginDataModule loginDataModule;

		private bool isMaxReward;

		private bool isCheckReward;
	}
}

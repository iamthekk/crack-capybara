using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIRechargeGiftCardNodeCtrl : UIRechargeGiftCardNodeBase
	{
		private IAPDataModule iapDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.IAPDataModule);
			}
		}

		protected override void OnNodeInit()
		{
			this.buttonSelf.onClick.AddListener(new UnityAction(this.OnClickSelf));
			this.timeCtrl.OnRefreshText += this.OnRefreshNextTimeText;
			this.timeCtrl.OnChangeState += this.OnChangeStateNextTime;
			this.timeCtrl.Init();
			switch (this.cardType)
			{
			case IAPMonthCardData.CardType.NoAd:
				RedPointController.Instance.RegRecordChange("IAPPrivileggeCard.Card.NoAd", new Action<RedNodeListenData>(this.OnRedPoint));
				return;
			case IAPMonthCardData.CardType.Month:
				RedPointController.Instance.RegRecordChange("IAPPrivileggeCard.Card.Month", new Action<RedNodeListenData>(this.OnRedPoint));
				return;
			case IAPMonthCardData.CardType.Free:
				break;
			case IAPMonthCardData.CardType.Lifetime:
				RedPointController.Instance.RegRecordChange("IAPPrivileggeCard.Card.Lifetime", new Action<RedNodeListenData>(this.OnRedPoint));
				return;
			case IAPMonthCardData.CardType.AutoMining:
				RedPointController.Instance.RegRecordChange("IAPPrivileggeCard.Card.AutoMining", new Action<RedNodeListenData>(this.OnRedPoint));
				break;
			default:
				return;
			}
		}

		protected override void OnNodeDeInit()
		{
			this.buttonSelf.onClick.RemoveListener(new UnityAction(this.OnClickSelf));
			this.timeCtrl.DeInit();
			switch (this.cardType)
			{
			case IAPMonthCardData.CardType.NoAd:
				RedPointController.Instance.UnRegRecordChange("IAPPrivileggeCard.Card.NoAd", new Action<RedNodeListenData>(this.OnRedPoint));
				return;
			case IAPMonthCardData.CardType.Month:
				RedPointController.Instance.UnRegRecordChange("IAPPrivileggeCard.Card.Month", new Action<RedNodeListenData>(this.OnRedPoint));
				return;
			case IAPMonthCardData.CardType.Free:
				break;
			case IAPMonthCardData.CardType.Lifetime:
				RedPointController.Instance.UnRegRecordChange("IAPPrivileggeCard.Card.Lifetime", new Action<RedNodeListenData>(this.OnRedPoint));
				return;
			case IAPMonthCardData.CardType.AutoMining:
				RedPointController.Instance.UnRegRecordChange("IAPPrivileggeCard.Card.AutoMining", new Action<RedNodeListenData>(this.OnRedPoint));
				break;
			default:
				return;
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.timeRectTrans.gameObject.activeSelf)
			{
				this.timeCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void OnRefresh()
		{
			bool flag = false;
			int tableID = this.iapDataModule.MonthCard.GetTableID(this.cardType);
			IAP_MonthCard elementById = GameApp.Table.GetManager().GetIAP_MonthCardModelInstance().GetElementById(tableID);
			if (elementById != null)
			{
				List<ItemData> dayRewardItemData = elementById.GetDayRewardItemData();
				if (dayRewardItemData != null && dayRewardItemData.Count > 0)
				{
					flag = true;
				}
			}
			int lastDay = this.iapDataModule.MonthCard.GetLastDay(this.cardType);
			bool flag2 = this.iapDataModule.MonthCard.IsCanReward(this.cardType);
			bool flag3 = this.iapDataModule.MonthCard.IsActivation(this.cardType);
			bool flag4 = flag3 && !flag2;
			this.activeObj.SetActiveSafe(flag3);
			this.inActiveObj.SetActiveSafe(!flag3);
			if (flag)
			{
				if (flag4)
				{
					this.timeRectTrans.gameObject.SetActiveSafe(lastDay > 0);
					this.maskObj.SetActiveSafe(lastDay > 0);
					this.canCollectObj.SetActiveSafe(false);
					this.timeCtrl.SetState(IAPShopTimeCtrl.State.Show);
					this.timeCtrl.Play();
					this.timeCtrl.OnRefresh();
				}
				else
				{
					this.timeRectTrans.gameObject.SetActiveSafe(false);
					this.canCollectObj.SetActiveSafe(flag3);
					this.maskObj.SetActiveSafe(flag2);
					this.timeCtrl.Stop();
				}
			}
			else
			{
				this.timeRectTrans.gameObject.SetActive(false);
				this.canCollectObj.SetActiveSafe(false);
				this.maskObj.SetActiveSafe(false);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.timeRectTrans);
		}

		private void OnClickSelf()
		{
			GameApp.View.OpenView(ViewName.IAPPrivilegeCardViewModule, this.cardType, 1, null, null);
		}

		private IAPShopTimeCtrl.State OnChangeStateNextTime(IAPShopTimeCtrl arg)
		{
			if (this.iapDataModule.MonthCard.GetNextTime(this.cardType) <= 0L)
			{
				NetworkUtils.PlayerData.SendUserGetIapInfoRequest(delegate
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIRechargeGift_Refresh, null);
					this.OnRefresh();
				});
				return IAPShopTimeCtrl.State.Load;
			}
			return IAPShopTimeCtrl.State.Show;
		}

		private string OnRefreshNextTimeText(IAPShopTimeCtrl arg)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID("uirechargegift_card_time", new object[] { this.iapDataModule.MonthCard.GetNextTimeString(this.cardType) });
		}

		private void OnRedPoint(RedNodeListenData redData)
		{
			if (this.redNode == null)
			{
				return;
			}
			this.redNode.gameObject.SetActive(redData.m_count > 0);
		}

		public IAPMonthCardData.CardType cardType;

		public CustomButton buttonSelf;

		public GameObject activeObj;

		public GameObject inActiveObj;

		public RectTransform timeRectTrans;

		public IAPShopTimeCtrl timeCtrl;

		public GameObject canCollectObj;

		public RedNodeOneCtrl redNode;

		public GameObject maskObj;
	}
}

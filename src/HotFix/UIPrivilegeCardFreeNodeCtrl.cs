using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Pay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIPrivilegeCardFreeNodeCtrl : UIPrivilegeCardNodeCtrlBase
	{
		private IAPDataModule iapDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.IAPDataModule);
			}
		}

		protected override void OnInit()
		{
			this.buttonCollect.onClick.AddListener(new UnityAction(this.OnClickCollect));
			this.timeCtrl.OnRefreshText += this.OnRefreshNextTimeText;
			this.timeCtrl.OnChangeState += this.OnChangeStateNextTime;
			this.timeCtrl.Init();
			this.copyItem.SetActiveSafe(false);
			int tableID = this.iapDataModule.MonthCard.GetTableID(this.cardType);
			this.cardTable = GameApp.Table.GetManager().GetIAP_MonthCardModelInstance().GetElementById(tableID);
			if (this.cardTable == null)
			{
				return;
			}
			List<ItemData> dayRewardItemData = this.cardTable.GetDayRewardItemData();
			if (dayRewardItemData != null)
			{
				for (int i = 0; i < dayRewardItemData.Count; i++)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(this.layout, false);
					gameObject.SetActiveSafe(true);
					UIItem component = gameObject.GetComponent<UIItem>();
					component.Init();
					component.SetData(dayRewardItemData[i].ToPropData());
					component.OnRefresh();
					component.SetTextCountScale(Vector3.one * 1.2f);
					this.rewardItems.Add(component);
				}
			}
			RedPointController.Instance.RegRecordChange("IAPPrivileggeCard.Card.Free", new Action<RedNodeListenData>(this.OnRedPoint));
		}

		protected override void OnDeInit()
		{
			this.buttonCollect.onClick.RemoveListener(new UnityAction(this.OnClickCollect));
			this.timeCtrl.DeInit();
			for (int i = 0; i < this.rewardItems.Count; i++)
			{
				this.rewardItems[i].DeInit();
			}
			this.rewardItems.Clear();
			RedPointController.Instance.UnRegRecordChange("IAPPrivileggeCard.Card.Free", new Action<RedNodeListenData>(this.OnRedPoint));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.timeRectTrans.gameObject.activeSelf)
			{
				this.timeCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void Refresh()
		{
			bool flag = this.iapDataModule.MonthCard.IsCanReward(this.cardType);
			bool flag2 = this.iapDataModule.MonthCard.IsActivation(this.cardType) && !flag;
			this.timeCtrl.SetActive(flag2);
			if (flag2)
			{
				this.timeRectTrans.gameObject.SetActiveSafe(true);
				this.timeCtrl.SetState(IAPShopTimeCtrl.State.Show);
				this.timeCtrl.Play();
				this.timeCtrl.OnRefresh();
				this.textCollect.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegecard_collected");
				this.btnGray.SetUIGray();
			}
			else
			{
				this.timeRectTrans.gameObject.SetActiveSafe(false);
				this.timeCtrl.Stop();
				this.textCollect.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegecard_collect");
				this.btnGray.Recovery();
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.timeRectTrans);
		}

		private void OnClickCollect()
		{
			if (!this.iapDataModule.MonthCard.IsCanReward(this.cardType))
			{
				return;
			}
			if (this.cardTable == null)
			{
				return;
			}
			NetworkUtils.Purchase.SendMonthCardGetRewardRequest(this.cardTable.id, delegate(bool isOk, MonthCardGetRewardResponse resp)
			{
				if (isOk)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIRechargeGift_Refresh, null);
					DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
				}
			});
		}

		private IAPShopTimeCtrl.State OnChangeStateNextTime(IAPShopTimeCtrl arg)
		{
			if (this.iapDataModule.MonthCard.GetNextTime(this.cardType) <= 0L)
			{
				NetworkUtils.PlayerData.SendUserGetIapInfoRequest(delegate
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Refresh_PrivilegeCard, null);
					this.Refresh();
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

		public RectTransform timeRectTrans;

		public IAPShopTimeCtrl timeCtrl;

		public CustomButton buttonCollect;

		public CustomText textCollect;

		public UIGrays btnGray;

		public GameObject layout;

		public GameObject copyItem;

		public RedNodeOneCtrl redNode;

		private readonly IAPMonthCardData.CardType cardType = IAPMonthCardData.CardType.Free;

		private List<UIItem> rewardItems = new List<UIItem>();

		private IAP_MonthCard cardTable;
	}
}

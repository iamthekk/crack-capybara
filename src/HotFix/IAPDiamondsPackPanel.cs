using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class IAPDiamondsPackPanel : IAPDiamondsSubPanelBase
	{
		public override IAPDiamondsType PanelType
		{
			get
			{
				return IAPDiamondsType.DiamondsPack;
			}
		}

		protected override void OnPreInit()
		{
			this.isRefreshViewed = false;
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.packItemPrefab.SetActive(false);
			this.giftItemPool = LocalUnityObjctPool.Create(base.gameObject);
			this.giftItemPool.CreateCache<IAPDiamondsPackItem>(this.packItemPrefab.gameObject);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventIAPInfoData));
			this.infoButton.onClick.AddListener(new UnityAction(this.OnInfoButtonClick));
			this.vipUI.Init();
		}

		protected override void OnPreDeInit()
		{
			this.ResetAllGiftItem();
			this.giftItemPool.CollectAll();
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventIAPInfoData));
			this.infoButton.onClick.RemoveListener(new UnityAction(this.OnInfoButtonClick));
			IAPDiamondsPackVIP iapdiamondsPackVIP = this.vipUI;
			if (iapdiamondsPackVIP == null)
			{
				return;
			}
			iapdiamondsPackVIP.DeInit();
		}

		protected override void OnSelect(IAPShopJumpTabData jumpTabData)
		{
			base.OnSelect(jumpTabData);
			if (!this.isRefreshViewed)
			{
				this.RefreshView();
			}
			this.vipUI.OnOpen();
		}

		protected override void OnUnSelect()
		{
			base.OnUnSelect();
			this.vipUI.OnClose();
		}

		private void OnEventIAPInfoData(object sender, int type, BaseEventArgs eventargs)
		{
			this.RefreshView();
		}

		private void RefreshView()
		{
			this.isRefreshViewed = true;
			this.ResetAllGiftItem();
			foreach (PurchaseCommonData.PurchaseData purchaseData in this.iapDataModule.DiamondsPackData.GetShowPurchaseData())
			{
				this.ActiveGiftItem(purchaseData.m_id);
			}
		}

		private void ActiveGiftItem(int purchaseId)
		{
			IAPDiamondsPackItem iapdiamondsPackItem = this.giftItemPool.DeQueue<IAPDiamondsPackItem>();
			iapdiamondsPackItem.Init();
			iapdiamondsPackItem.RefreshData(purchaseId);
			Transform transform = iapdiamondsPackItem.transform;
			transform.SetParent(this.packItemParent);
			transform.localPosition = Vector3.zero;
			transform.localScale = Vector3.one;
			iapdiamondsPackItem.transform.SetAsLastSibling();
			this.curDiamondsGiftItemList.Add(iapdiamondsPackItem);
		}

		private void ResetAllGiftItem()
		{
			foreach (IAPDiamondsPackItem iapdiamondsPackItem in this.curDiamondsGiftItemList)
			{
				iapdiamondsPackItem.DeInit();
				this.giftItemPool.EnQueue<IAPDiamondsPackItem>(iapdiamondsPackItem.gameObject);
			}
			this.curDiamondsGiftItemList.Clear();
		}

		private void OnInfoButtonClick()
		{
			InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData
			{
				m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(2815),
				m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(2816)
			};
			GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
		}

		[SerializeField]
		private IAPDiamondsPackItem packItemPrefab;

		[SerializeField]
		private Transform packItemParent;

		[SerializeField]
		private CustomButton infoButton;

		[SerializeField]
		private IAPDiamondsPackVIP vipUI;

		private IAPDataModule iapDataModule;

		private LocalUnityObjctPool giftItemPool;

		private readonly List<IAPDiamondsPackItem> curDiamondsGiftItemList = new List<IAPDiamondsPackItem>();

		private bool isRefreshViewed;
	}
}

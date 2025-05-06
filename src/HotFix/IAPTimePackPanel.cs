using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public abstract class IAPTimePackPanel : IAPDiamondsSubPanelBase
	{
		private protected IAPDataModule IAPDataModule { protected get; private set; }

		protected override void OnPreInit()
		{
			this.isRefreshViewed = false;
			this.itemPrefab.SetActive(false);
			this.IAPDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventRefresh));
			this.infoButton.onClick.AddListener(new UnityAction(this.OnInfoButtonClick));
			this.timeCtrl.OnRefreshText += this.OnRefreshTimeText;
			this.timeCtrl.OnChangeState += this.OnChangeState;
			this.timeCtrl.Init();
		}

		protected override void OnPreDeInit()
		{
			this.infoButton.onClick.RemoveListener(new UnityAction(this.OnInfoButtonClick));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventRefresh));
			this.timeCtrl.DeInit();
			this.DestroyNodes();
			this.dataList.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (base.IsSelect)
			{
				this.timeCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void OnSelect(IAPShopJumpTabData jumpTabData)
		{
			base.OnSelect(jumpTabData);
			if (!this.isRefreshViewed)
			{
				this.RefreshView();
			}
		}

		private void OnEventRefresh(object sender, int type, BaseEventArgs eventargs)
		{
			this.RefreshView();
		}

		private void RefreshView()
		{
			this.isRefreshViewed = true;
			this.DestroyNodes();
			this.CreateNodes();
			this.timeCtrl.Play();
		}

		private void CreateNodes()
		{
			this.dataList = this.GetPurchaseData();
			foreach (PurchaseCommonData.PurchaseData purchaseData in this.dataList)
			{
				if (purchaseData != null)
				{
					IAPTimePackItem iaptimePackItem = Object.Instantiate<IAPTimePackItem>(this.itemPrefab);
					iaptimePackItem.transform.SetParentNormal(this.nodeParent, false);
					iaptimePackItem.SetActive(true);
					iaptimePackItem.SetData(purchaseData);
					iaptimePackItem.Init();
					this.nodeDic[iaptimePackItem.GetInstanceID()] = iaptimePackItem;
				}
			}
		}

		private void DestroyNodes()
		{
			foreach (KeyValuePair<int, IAPTimePackItem> keyValuePair in this.nodeDic)
			{
				keyValuePair.Value.DeInit();
				Object.Destroy(keyValuePair.Value.gameObject);
			}
			this.nodeDic.Clear();
		}

		private IAPShopTimeCtrl.State OnChangeState(IAPShopTimeCtrl arg)
		{
			if (this.GetRefreshTime() <= 0L)
			{
				NetworkUtils.PlayerData.TipSendUserGetInfoRequest("shop_data_refresh", null);
				return IAPShopTimeCtrl.State.Load;
			}
			return IAPShopTimeCtrl.State.Show;
		}

		private string OnRefreshTimeText(IAPShopTimeCtrl arg)
		{
			return this.GetRefreshTimeString();
		}

		private void OnInfoButtonClick()
		{
			int num;
			int num2;
			this.GetInfoLanguageKey(out num, out num2);
			InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData
			{
				m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(num),
				m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(num2)
			};
			GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
		}

		protected abstract List<PurchaseCommonData.PurchaseData> GetPurchaseData();

		protected abstract long GetRefreshTime();

		protected abstract string GetRefreshTimeString();

		protected abstract void GetInfoLanguageKey(out int titleKey, out int contextKey);

		[SerializeField]
		private IAPTimePackItem itemPrefab;

		[SerializeField]
		private RectTransform nodeParent;

		[SerializeField]
		private IAPShopTimeCtrl timeCtrl;

		[SerializeField]
		private CustomButton infoButton;

		private List<PurchaseCommonData.PurchaseData> dataList = new List<PurchaseCommonData.PurchaseData>();

		private readonly Dictionary<int, IAPTimePackItem> nodeDic = new Dictionary<int, IAPTimePackItem>();

		private bool isRefreshViewed;
	}
}

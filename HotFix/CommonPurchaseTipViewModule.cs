using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class CommonPurchaseTipViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmClick));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelClick));
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.itemPrefab.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			this.openData = new CommonPurchaseTipViewModule.OpenData?((CommonPurchaseTipViewModule.OpenData)data);
			if (this.openData != null)
			{
				this.iapPurchase = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(this.openData.Value.PurchaseId);
			}
			this.RefreshView();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.confirmButton.onClick.RemoveListener(new UnityAction(this.OnConfirmClick));
			this.cancelButton.onClick.RemoveListener(new UnityAction(this.OnCancelClick));
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.DeInitAllItemCtrl();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void RefreshView()
		{
			if (this.iapPurchase == null)
			{
				this.confirmButton.enabled = false;
				this.tipInfoText.text = string.Empty;
				return;
			}
			this.confirmButton.enabled = true;
			this.tipInfoText.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(2935, new object[] { GameApp.Purchase.Manager.GetPriceForPlatformID(this.iapPurchase.platformID) });
			this.ResetAllItemCtrl();
			foreach (ItemData itemData in this.iapPurchase.GetShowProductsData(false))
			{
				if (itemData != null)
				{
					UIItem itemCtrl = this.GetItemCtrl();
					itemCtrl.SetData(itemData.ToPropData());
					itemCtrl.OnRefresh();
				}
			}
		}

		private UIItem GetItemCtrl()
		{
			UIItem uiitem;
			if (this.uiItemCtrlPool.Count > 0)
			{
				uiitem = this.uiItemCtrlPool.Pop();
			}
			else
			{
				uiitem = Object.Instantiate<GameObject>(this.itemPrefab, this.itemParent).GetComponent<UIItem>();
				uiitem.onClick = new Action<UIItem, PropData, object>(this.OnItemClick);
				uiitem.SetCountShowType(UIItem.CountShowType.ShowAll);
				uiitem.Init();
			}
			uiitem.SetActive(true);
			this.uiItemCtrlList.Add(uiitem);
			return uiitem;
		}

		private void OnItemClick(UIItem item, PropData data, object arg)
		{
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = data,
				m_openDataType = ItemInfoOpenDataType.eShow,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, default(Vector3), 0f);
		}

		private void ResetAllItemCtrl()
		{
			foreach (UIItem uiitem in this.uiItemCtrlList)
			{
				if (!(uiitem == null))
				{
					uiitem.SetActive(false);
					this.uiItemCtrlPool.Push(uiitem);
				}
			}
			this.uiItemCtrlList.Clear();
		}

		private void DeInitAllItemCtrl()
		{
			foreach (UIItem uiitem in this.uiItemCtrlList)
			{
				if (!(uiitem == null))
				{
					uiitem.DeInit();
				}
			}
			this.uiItemCtrlList.Clear();
			while (this.uiItemCtrlPool.Count > 0)
			{
				UIItem uiitem2 = this.uiItemCtrlPool.Pop();
				if (uiitem2 != null)
				{
					uiitem2.DeInit();
				}
			}
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.CommonPurchaseTipViewModule, null);
		}

		private void OnConfirmClick()
		{
			if (this.openData == null || this.iapPurchase == null)
			{
				return;
			}
			GameApp.Purchase.Manager.Buy(this.openData.Value.PurchaseId, 0, this.openData.Value.ExtraInfo, delegate(bool isOk)
			{
				Action<bool> onBuyCallback = this.openData.Value.OnBuyCallback;
				if (onBuyCallback != null)
				{
					onBuyCallback(isOk);
				}
				if (isOk)
				{
					this.CloseSelf();
				}
			}, null);
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnCancelClick();
			}
		}

		private void OnCancelClick()
		{
			this.CloseSelf();
		}

		[SerializeField]
		private CustomText tipInfoText;

		[SerializeField]
		private UIPopCommon uiPopCommon;

		[SerializeField]
		private CustomButton confirmButton;

		[SerializeField]
		private CustomButton cancelButton;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private Transform itemParent;

		private CommonPurchaseTipViewModule.OpenData? openData;

		private IAP_Purchase iapPurchase;

		private readonly List<UIItem> uiItemCtrlList = new List<UIItem>();

		private readonly Stack<UIItem> uiItemCtrlPool = new Stack<UIItem>();

		public struct OpenData
		{
			public int PurchaseId { readonly get; private set; }

			public string ExtraInfo { readonly get; private set; }

			public Action<bool> OnBuyCallback { readonly get; private set; }

			public void SetData(int purchaseId, Action<bool> pnBuyCallback = null)
			{
				this.PurchaseId = purchaseId;
				this.ExtraInfo = string.Empty;
				this.OnBuyCallback = pnBuyCallback;
			}

			public void SetData(int purchaseId, string extraInfo, Action<bool> pnBuyCallback = null)
			{
				this.PurchaseId = purchaseId;
				this.ExtraInfo = extraInfo;
				this.OnBuyCallback = pnBuyCallback;
			}
		}
	}
}

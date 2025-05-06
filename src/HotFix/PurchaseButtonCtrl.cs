using System;
using Framework;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	[RequireComponent(typeof(CustomButton))]
	public class PurchaseButtonCtrl : MonoBehaviour
	{
		public int PurchaseId
		{
			get
			{
				return this.purchaseId;
			}
		}

		private void Awake()
		{
			this.customButton = base.GetComponent<CustomButton>();
			this.customButton.onClick.AddListener(new UnityAction(this.OnClick));
		}

		private void OnDestroy()
		{
			this.customButton.onClick.RemoveListener(new UnityAction(this.OnClick));
		}

		public void SetData(int purchaseIdVal, string priceTextId, Action<bool> onSuccessVal, Action onCloseReward)
		{
			try
			{
				this.onCloseRewardUI = onCloseReward;
				this.SetData(purchaseIdVal, null, null, null, onSuccessVal, priceTextId);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public void SetData(int purchaseIdVal, Predicate<int> canPurchaseMatchVal = null, Action<int> onOverrideClickVal = null, Action<int> onBeginBuyVal = null, Action<bool> onSuccessVal = null, string priceTextId = null)
		{
			try
			{
				this.onOverrideClick = onOverrideClickVal;
				this.canPurchaseMatch = canPurchaseMatchVal;
				this.onBeginBuy = onBeginBuyVal;
				this.onSuccess = onSuccessVal;
				this.purchaseId = purchaseIdVal;
				this.mPriceTextId = priceTextId;
				this.RefreshUI();
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public void SetBought(string text)
		{
			this.textPrice.text = text;
			this.isBought = true;
		}

		private void RefreshUI()
		{
			string text = "";
			string text2 = "";
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(this.purchaseId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("[IAP]PurchaseID not find !!! : {0}", this.purchaseId));
				if (this.textPrice == null)
				{
					HLog.LogError("textPrice is null");
					return;
				}
				this.textPrice.text = string.Empty;
				return;
			}
			else
			{
				text2 = elementById.price1.ToString();
				try
				{
					if (elementById.price1 > 0f)
					{
						if (string.IsNullOrEmpty(this.mPriceTextId))
						{
							text = GameApp.Purchase.Manager.GetPriceForPlatformID(elementById.platformID);
						}
						else
						{
							string priceForPlatformID = GameApp.Purchase.Manager.GetPriceForPlatformID(elementById.platformID);
							text = Singleton<LanguageManager>.Instance.GetInfoByID(this.mPriceTextId, new object[] { priceForPlatformID });
						}
						GameApp.SDK.Analyze.Track_IAPShow(GameTGAIAPParam.Create(this.purchaseId));
					}
					else
					{
						text = Singleton<LanguageManager>.Instance.GetInfoByID("101");
					}
				}
				catch (Exception ex)
				{
					text = "???";
					HLog.LogException(ex);
					throw;
				}
				if (this.textPrice == null)
				{
					HLog.LogError("textPrice is null");
					return;
				}
				if (text != null)
				{
					this.textPrice.text = text;
					return;
				}
				if (text2 != null)
				{
					this.textPrice.text = text2;
					return;
				}
				this.textPrice.text = "???";
				return;
			}
		}

		private void OnClick()
		{
			if (this.isBought)
			{
				return;
			}
			if (this.onOverrideClick != null)
			{
				this.onOverrideClick(this.purchaseId);
				return;
			}
			PurchaseButtonCtrl.DoDefaultBuy(this.purchaseId, this.canPurchaseMatch, this.onBeginBuy, this.onSuccess, this.onCloseRewardUI);
		}

		private static void DoDefaultBuy(int purchaseIdVal, Predicate<int> canPurchaseMatchVal, Action<int> onBeginBuyVal, Action<bool> onSuccess, Action onCloseRewardUI)
		{
			if (canPurchaseMatchVal != null && !canPurchaseMatchVal(purchaseIdVal))
			{
				return;
			}
			if (GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(purchaseIdVal) == null)
			{
				return;
			}
			if (onBeginBuyVal != null)
			{
				onBeginBuyVal(purchaseIdVal);
			}
			GameApp.Purchase.Manager.Buy(purchaseIdVal, 0, string.Empty, onSuccess, onCloseRewardUI);
		}

		[GameTestMethod("购买", "OpenPurchase", "", 0)]
		private static void OnTestBuy()
		{
			PurchaseButtonCtrl.DoDefaultBuy(101, null, null, null, null);
		}

		[SerializeField]
		public CustomText textPrice;

		private CustomButton customButton;

		private int purchaseId = -1;

		private string mPriceTextId;

		private Action<int> onOverrideClick;

		private Action<int> onBeginBuy;

		private Action<bool> onSuccess;

		private Predicate<int> canPurchaseMatch;

		private bool isBought;

		private Action onCloseRewardUI;
	}
}

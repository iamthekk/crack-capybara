using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class CommonFundCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.scrollCtrl.Init();
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonGoto.onClick.AddListener(new UnityAction(this.OnClickGoto));
			this.textTips.text = "";
		}

		protected override void OnDeInit()
		{
			this.scrollCtrl.DeInit();
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonGoto.onClick.RemoveListener(new UnityAction(this.OnClickGoto));
		}

		public void SetStyle(Sprite bg, Sprite title, string titleId)
		{
			if (bg)
			{
				this.imageMap.sprite = bg;
			}
			if (title)
			{
				this.imageTitle.sprite = title;
			}
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(titleId);
		}

		public void SetColorStyle(string lightHtmlColor, string starHtmlColor)
		{
			Color color;
			if (ColorUtility.TryParseHtmlString(lightHtmlColor, ref color))
			{
				this.imageLight1.color = color;
				this.imageLight2.color = color;
			}
			for (int i = 0; i < this.starParent.transform.childCount; i++)
			{
				Image component = this.starParent.transform.GetChild(i).GetComponent<Image>();
				Color color2;
				if (component != null && ColorUtility.TryParseHtmlString(starHtmlColor, ref color2))
				{
					component.color = color2;
				}
			}
		}

		public void SetTips(string tips)
		{
			this.textTips.text = tips;
		}

		public void SetButton(bool isShowClose, bool isShowGoto, Action onClose, Action onGoto, string gotoTextId)
		{
			this.buttonClose.gameObject.SetActiveSafe(isShowClose);
			this.buttonGoto.gameObject.SetActiveSafe(isShowGoto);
			this.OnClickCloseCall = onClose;
			this.OnClickGotoCall = onGoto;
			this.textGoto.text = Singleton<LanguageManager>.Instance.GetInfoByID(gotoTextId);
		}

		public void SetPurchaseButton(bool isShowPurchase, int purchaseId, Action<bool> onBuySuccess, Action onCloseReward, string priceTextId = null)
		{
			this.purchaseButtonCtrl.gameObject.SetActiveSafe(isShowPurchase);
			this.purchaseButtonCtrl.SetData(purchaseId, priceTextId, onBuySuccess, onCloseReward);
		}

		public void SetData(List<CommonFundUIData> dataList, int atlasId, string icon, string finalTipId, Action<int, int> onGetReward, Action onGetFinal)
		{
			this.scrollCtrl.SetData(dataList, atlasId, icon, finalTipId, onGetReward, onGetFinal);
		}

		public void SetStatus(int curScore, List<int> freeGet, List<int> payGet, int getFinalNum, bool hasBuy)
		{
			this.scrollCtrl.SetStatus(curScore, freeGet, payGet, getFinalNum, hasBuy);
			this.activeObj.SetActiveSafe(hasBuy);
			this.purchaseButtonCtrl.gameObject.SetActiveSafe(!hasBuy);
		}

		public void SetJumpItem(int index, float offset)
		{
			this.scrollCtrl.SetJumpItem(index, offset);
		}

		public void SetTime(string time)
		{
			this.textTime.text = time;
		}

		public void Refresh()
		{
			this.scrollCtrl.Refresh();
		}

		private void OnClickClose()
		{
			Action onClickCloseCall = this.OnClickCloseCall;
			if (onClickCloseCall == null)
			{
				return;
			}
			onClickCloseCall();
		}

		private void OnClickGoto()
		{
			Action onClickGotoCall = this.OnClickGotoCall;
			if (onClickGotoCall == null)
			{
				return;
			}
			onClickGotoCall();
		}

		public CustomImage imageMap;

		public CustomImage imageTitle;

		public CustomImage imageLight1;

		public CustomImage imageLight2;

		public GameObject starParent;

		public CustomText textTitle;

		public CustomText textTime;

		public CustomButton buttonClose;

		public CustomButton buttonGoto;

		public CustomText textGoto;

		public PurchaseButtonCtrl purchaseButtonCtrl;

		public CommonFundScrollCtrl scrollCtrl;

		public GameObject activeObj;

		public CustomText textTips;

		private Action OnClickCloseCall;

		private Action OnClickGotoCall;
	}
}

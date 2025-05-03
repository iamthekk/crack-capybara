using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIEnergyGiftNodeCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.copyItem.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
			}
		}

		public void SetData(IAP_GiftPacks energyPack)
		{
			if (energyPack == null)
			{
				return;
			}
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uienergygift_title");
			this.payButtonCtrl.SetData(energyPack.id, "", new Action<bool>(this.OnPaySuccess), null);
			List<ItemData> rewardItemData = energyPack.GetRewardItemData();
			for (int i = 0; i < rewardItemData.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
				gameObject.SetParentNormal(this.itemParent, false);
				gameObject.SetActiveSafe(true);
				UIEnergyGiftItem component = gameObject.GetComponent<UIEnergyGiftItem>();
				if (component)
				{
					component.Init();
					component.SetData(rewardItemData[i].ToPropData());
					this.itemList.Add(component);
				}
			}
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.IAPEnergyGiftViewModule, null);
		}

		private void OnPaySuccess(bool isOk)
		{
			if (isOk)
			{
				this.OnClickClose();
			}
		}

		public CustomText textTitle;

		public GameObject itemParent;

		public GameObject copyItem;

		public PurchaseButtonCtrl payButtonCtrl;

		public CustomButton buttonClose;

		private List<UIEnergyGiftItem> itemList = new List<UIEnergyGiftItem>();
	}
}

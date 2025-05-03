using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class IAPDiamondsPackItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
		}

		protected override void OnDeInit()
		{
		}

		public void RefreshData(int purchaseId)
		{
			this.iapPurchaseTable = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(purchaseId);
			this.titleText.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.iapPurchaseTable.titleID);
			this.nameText.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.iapPurchaseTable.nameID);
			this.shopIconImage.SetImage(GameApp.Table.GetAtlasPath(this.iapPurchaseTable.iconAtlasID), this.iapPurchaseTable.iconName);
			this.purchaseButtonCtrl.SetData(purchaseId, null, null, null, null, null);
			if (!string.IsNullOrEmpty(this.iapPurchaseTable.descID))
			{
				this.descParent.SetActive(true);
				this.descText.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.iapPurchaseTable.descID);
			}
			else
			{
				this.descParent.SetActive(false);
			}
			bool flag = this.iapDataModule.DiamondsPackData.IsShowRedPoint(purchaseId);
			this.redNode.gameObject.SetActive(flag);
			this.discountText.text = "";
		}

		[SerializeField]
		private CustomText titleText;

		[SerializeField]
		private CustomText nameText;

		[SerializeField]
		private CustomImage shopIconImage;

		[SerializeField]
		private PurchaseButtonCtrl purchaseButtonCtrl;

		[SerializeField]
		private RedNodeOneCtrl redNode;

		[SerializeField]
		private GameObject descParent;

		[SerializeField]
		private CustomText descText;

		[SerializeField]
		private GameObject discountParent;

		[SerializeField]
		private CustomText discountText;

		private IAPDataModule iapDataModule;

		private IAP_Purchase iapPurchaseTable;
	}
}

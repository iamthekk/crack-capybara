using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class MainShopDiamondPackItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int purchaseId)
		{
			this.iapPurchaseTable = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(purchaseId);
			this.purchaseButtonCtrl.SetData(purchaseId, null, null, null, null, null);
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.iapPurchaseTable.titleID);
			this.txtName.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.iapPurchaseTable.nameID);
			this.imgIcon.SetImage(GameApp.Table.GetAtlasPath(this.iapPurchaseTable.iconAtlasID), this.iapPurchaseTable.iconName);
			if (!string.IsNullOrEmpty(this.iapPurchaseTable.descID))
			{
				this.descParent.SetActive(true);
				this.txtDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.iapPurchaseTable.descID);
			}
			else
			{
				this.descParent.SetActive(false);
			}
			bool flag = this.iapDataModule.DiamondsPackData.IsShowRedPoint(purchaseId);
			this.redNode.gameObject.SetActive(flag);
		}

		public PurchaseButtonCtrl purchaseButtonCtrl;

		public CustomImage imgIcon;

		public CustomText txtTitle;

		public CustomText txtName;

		public GameObject descParent;

		public CustomText txtDesc;

		public RedNodeOneCtrl redNode;

		private IAPDataModule iapDataModule;

		private IAP_Purchase iapPurchaseTable;
	}
}

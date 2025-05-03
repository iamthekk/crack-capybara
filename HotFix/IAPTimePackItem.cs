using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class IAPTimePackItem : CustomBehaviour
	{
		public void SetData(PurchaseCommonData.PurchaseData dataVal)
		{
			this.data = dataVal;
		}

		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			if (this.data == null)
			{
				return;
			}
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(this.data.m_id);
			IAP_GiftPacks elementById2 = GameApp.Table.GetManager().GetIAP_GiftPacksModelInstance().GetElementById(this.data.m_id);
			int buyCount = this.iapDataModule.TimePackData.GetBuyCount(this.data.m_id);
			int num = Mathf.Max(elementById.limitCount - buyCount, 0);
			this.SetName(elementById.nameID, num);
			this.SetIcon(elementById.iconAtlasID, elementById.iconName);
			this.SetDiscount(0);
			this.rewardCtrl.SetData(elementById2.GetRewardItemData(), false);
			this.rewardCtrl.Init();
			this.rewardCtrl.SetActiveForReceive(false);
			bool flag = this.iapDataModule.TimePackData.IsMaxBuyCount(this.data.m_id);
			this.SetGray(flag);
			this.soldOut.SetActive(flag);
			this.purchaseButtonCtrl.SetData(this.data.m_id, delegate(int id)
			{
				bool flag2 = this.iapDataModule.TimePackData.IsMaxBuyCount(this.data.m_id);
				if (flag2)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(2954));
				}
				return !flag2;
			}, null, null, null, null);
			this.OnRefreshRedNode();
		}

		protected override void OnDeInit()
		{
			this.rewardCtrl.DeInit();
		}

		private void SetName(string nameId, int count)
		{
			bool flag = !string.IsNullOrEmpty(nameId);
			this.nameParent.gameObject.SetActive(flag);
			this.nameText.gameObject.SetActive(flag);
			if (flag)
			{
				this.nameText.text = Singleton<LanguageManager>.Instance.GetInfoByID(nameId, new object[] { count });
			}
		}

		private void SetIcon(int uitaleID, string sprite)
		{
			string atlasPath = GameApp.Table.GetAtlasPath(uitaleID);
			this.iconImage.SetImage(atlasPath, sprite);
		}

		private void SetDiscount(int id)
		{
			this.discountText.text = "";
		}

		private void SetGray(bool isGray)
		{
			if (isGray)
			{
				this.grays.SetUIGray();
			}
			else
			{
				this.grays.Recovery();
			}
			this.rewardCtrl.SetGrayState(isGray);
		}

		private void OnRefreshRedNode()
		{
			bool flag = this.iapDataModule.TimePackData.IsShowRedPoint(this.data.m_id);
			this.redNode.gameObject.SetActive(flag);
		}

		[SerializeField]
		private GameObject nameParent;

		[SerializeField]
		private CustomText nameText;

		[SerializeField]
		private CustomImage iconImage;

		[SerializeField]
		private GameObject discountParent;

		[SerializeField]
		private CustomText discountText;

		[SerializeField]
		private PurchaseButtonCtrl purchaseButtonCtrl;

		[SerializeField]
		private RedNodeOneCtrl redNode;

		[SerializeField]
		private GameObject soldOut;

		[SerializeField]
		private IAPShopRewardCtrl rewardCtrl;

		[SerializeField]
		private UIGrays grays;

		private PurchaseCommonData.PurchaseData data;

		private IAPDataModule iapDataModule;
	}
}

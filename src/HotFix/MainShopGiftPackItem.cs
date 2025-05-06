using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class MainShopGiftPackItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.redNode.Value = 0;
			this.prefabItem.gameObject.SetActive(false);
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void SetData(PurchaseCommonData.PurchaseData data)
		{
			this.data = data;
			if (data == null)
			{
				return;
			}
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(data.m_id);
			IAP_GiftPacks elementById2 = GameApp.Table.GetManager().GetIAP_GiftPacksModelInstance().GetElementById(data.m_id);
			int buyCount = this.iapDataModule.TimePackData.GetBuyCount(data.m_id);
			int num = Mathf.Max(elementById.limitCount - buyCount, 0);
			this.SetName(elementById.nameID, num);
			this.UpdateRewardItems(elementById2.GetRewardItemData());
			this.SetDiscount(elementById.valueBet);
			bool flag = this.iapDataModule.TimePackData.IsMaxBuyCount(data.m_id);
			if (flag)
			{
				this.btnBuy.GetComponent<UIGrays>().SetUIGray();
			}
			else
			{
				this.btnBuy.GetComponent<UIGrays>().Recovery();
			}
			this.redNode.Value = ((!flag && elementById.platformID == 0) ? 1 : 0);
			this.btnBuy.SetData(data.m_id, delegate(int id)
			{
				bool flag2 = this.iapDataModule.TimePackData.IsMaxBuyCount(data.m_id);
				if (flag2)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(2954));
				}
				return !flag2;
			}, null, null, null, null);
		}

		private void SetName(string nameID, int count)
		{
			if (this.data != null)
			{
				int maxBuyCount = this.iapDataModule.TimePackData.GetMaxBuyCount(this.data.m_id);
				int buyCount = this.iapDataModule.TimePackData.GetBuyCount(this.data.m_id);
				int num = Mathf.Max(maxBuyCount - buyCount, 0);
				this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(nameID) + string.Format(" ({0}/{1})", num, maxBuyCount);
				return;
			}
			this.txtTitle.text = "";
		}

		private void SetDiscount(int valueBet)
		{
			bool flag = valueBet > 0;
			this.goOff.gameObject.SetActive(flag);
			if (flag)
			{
				this.txtOffValue.text = string.Format("{0}X", valueBet);
			}
		}

		private void UpdateRewardItems(List<ItemData> rewards)
		{
			if (this.items.Count < rewards.Count)
			{
				for (int i = this.items.Count; i < rewards.Count; i++)
				{
					UIItem uiitem = Object.Instantiate<UIItem>(this.prefabItem, this.prefabItem.transform.parent, false);
					uiitem.gameObject.SetActive(true);
					uiitem.Init();
					this.items.Add(uiitem);
				}
			}
			else if (this.items.Count > rewards.Count)
			{
				for (int j = this.items.Count - 1; j >= rewards.Count; j--)
				{
					this.items[j].gameObject.SetActive(false);
				}
			}
			for (int k = 0; k < rewards.Count; k++)
			{
				this.items[k].SetData(rewards[k].ToPropData());
				this.items[k].OnRefresh();
			}
		}

		public RectTransform fg;

		public CustomText txtTitle;

		public GameObject goOff;

		public CustomText txtOffValue;

		public UIItem prefabItem;

		public PurchaseButtonCtrl btnBuy;

		public RedNodeOneCtrl redNode;

		private List<UIItem> items = new List<UIItem>();

		private IAPDataModule iapDataModule;

		private PurchaseCommonData.PurchaseData data;
	}
}

using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class MainShopSuperValuePackItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.redNode.Value = 0;
			this.prefabUIItem.gameObject.SetActive(false);
			this.goCheckMark.SetActive(false);
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(PurchaseCommonData.PurchaseData data)
		{
			if (data == null)
			{
				return;
			}
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(data.m_id);
			int buyCount = this.iapDataModule.SuperValuePack.GetBuyCount(data.m_id);
			int limitCount = elementById.limitCount;
			int num = Mathf.Max(limitCount - buyCount, 0);
			this.SetName(elementById.nameID, num, limitCount);
			this.UpdateRewardItems(DxxTools.Reward.ParseReward(elementById.showProducts));
			this.SetValueBet(elementById.valueBet);
			bool flag = this.iapDataModule.SuperValuePack.IsMaxBuyCount(data.m_id);
			this.goCheckMark.SetActive(flag);
			this.btnBuy.textPrice.gameObject.SetActive(!flag);
			this.btnBuy.SetData(data.m_id, delegate(int id)
			{
				bool flag2 = this.iapDataModule.SuperValuePack.IsMaxBuyCount(data.m_id);
				if (flag2)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(2954));
				}
				return !flag2;
			}, null, null, null, null);
		}

		private void SetValueBet(int valueBet)
		{
			this.goOff.SetActive(valueBet > 0);
			this.txtOffValue.text = string.Format("{0}X", valueBet);
		}

		private void SetName(string nameID, int remainCount, int totalCount)
		{
			if (totalCount > 0)
			{
				this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(nameID) + string.Format("({0}/{1})", remainCount, totalCount);
				return;
			}
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(nameID);
		}

		private void UpdateRewardItems(List<ItemData> rewards)
		{
			if (this.items.Count < rewards.Count)
			{
				for (int i = this.items.Count; i < rewards.Count; i++)
				{
					UIItem uiitem = Object.Instantiate<UIItem>(this.prefabUIItem, this.prefabUIItem.transform.parent, false);
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
				if (rewards.Count == 1)
				{
					this.items[k].transform.SetParent(this.points1[k]);
				}
				else if (rewards.Count == 2)
				{
					this.items[k].transform.SetParent(this.points2[k]);
				}
				else if (rewards.Count == 3)
				{
					this.items[k].transform.SetParent(this.points3[k]);
				}
				else
				{
					this.items[k].transform.SetParent(this.points1[0]);
				}
				this.items[k].transform.localPosition = Vector3.zero;
				this.items[k].transform.localScale = Vector3.one;
				this.items[k].SetData(rewards[k].ToPropData());
				this.items[k].OnRefresh();
			}
		}

		public PurchaseButtonCtrl btnBuy;

		public CustomText txtTitle;

		public UIItem prefabUIItem;

		public GameObject goOff;

		public CustomText txtOffValue;

		public GameObject goCheckMark;

		public RedNodeOneCtrl redNode;

		public List<Transform> points1;

		public List<Transform> points2;

		public List<Transform> points3;

		private IAPDataModule iapDataModule;

		private List<UIItem> items = new List<UIItem>();
	}
}

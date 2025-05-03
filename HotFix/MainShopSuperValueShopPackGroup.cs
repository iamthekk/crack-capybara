using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace HotFix
{
	public class MainShopSuperValueShopPackGroup : MainShopPackGroupBase
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.prefabMainShopSuperValuePackItem.gameObject.SetActive(false);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].DeInit();
			}
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 2;
			subPriority = 0;
		}

		public override void UpdateContent()
		{
			List<PurchaseCommonData.PurchaseData> showPurchaseData = this.iapDataModule.SuperValuePack.GetShowPurchaseData();
			if (this.items.Count < showPurchaseData.Count)
			{
				for (int i = this.items.Count; i < showPurchaseData.Count; i++)
				{
					MainShopSuperValuePackItem mainShopSuperValuePackItem = Object.Instantiate<MainShopSuperValuePackItem>(this.prefabMainShopSuperValuePackItem, this.prefabMainShopSuperValuePackItem.transform.parent, false);
					mainShopSuperValuePackItem.gameObject.SetActive(true);
					mainShopSuperValuePackItem.Init();
					this.items.Add(mainShopSuperValuePackItem);
				}
			}
			else if (this.items.Count > showPurchaseData.Count)
			{
				for (int j = this.items.Count - 1; j >= showPurchaseData.Count; j--)
				{
					this.items[j].gameObject.SetActive(false);
				}
			}
			for (int k = 0; k < showPurchaseData.Count; k++)
			{
				this.items[k].SetData(showPurchaseData[k]);
			}
		}

		public override int PlayAnimation(float startTime, int index)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].gameObject.AddComponent<EnterScaleAnimationCtrl>().PlayShowAnimation(startTime, index + i, 100025);
			}
			return 0;
		}

		public MainShopSuperValuePackItem prefabMainShopSuperValuePackItem;

		private IAPDataModule iapDataModule;

		private List<MainShopSuperValuePackItem> items = new List<MainShopSuperValuePackItem>();
	}
}

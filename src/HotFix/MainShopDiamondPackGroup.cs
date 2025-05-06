using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace HotFix
{
	public class MainShopDiamondPackGroup : MainShopPackGroupBase
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.prefabItem.gameObject.SetActive(false);
		}

		protected override void OnDeInit()
		{
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 5;
			subPriority = 0;
		}

		public override void UpdateContent()
		{
			List<PurchaseCommonData.PurchaseData> showPurchaseData = this.iapDataModule.DiamondsPackData.GetShowPurchaseData();
			int count = showPurchaseData.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.items.Count <= i)
				{
					MainShopDiamondPackItem mainShopDiamondPackItem = Object.Instantiate<MainShopDiamondPackItem>(this.prefabItem, this.prefabItem.transform.parent, false);
					mainShopDiamondPackItem.Init();
					this.items.Add(mainShopDiamondPackItem);
				}
				this.items[i].gameObject.SetActive(true);
				int id = showPurchaseData[i].m_id;
				this.items[i].SetData(id);
			}
			if (this.items.Count > count)
			{
				for (int j = count; j < this.items.Count; j++)
				{
					this.items[j].gameObject.SetActive(false);
				}
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

		public MainShopDiamondPackItem prefabItem;

		private List<MainShopDiamondPackItem> items = new List<MainShopDiamondPackItem>();

		private IAPDataModule iapDataModule;
	}
}

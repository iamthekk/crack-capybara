using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class MainShopCoinPackGroup : MainShopPackGroupBase
	{
		protected override void OnInit()
		{
			this.prefabItem.gameObject.SetActive(false);
		}

		protected override void OnDeInit()
		{
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 7;
			subPriority = 0;
		}

		public override void UpdateContent()
		{
			IList<Shop_ShopSell> allElements = GameApp.Table.GetManager().GetShop_ShopSellModelInstance().GetAllElements();
			List<Shop_ShopSell> list = new List<Shop_ShopSell>();
			for (int i = 0; i < allElements.Count; i++)
			{
				if (allElements[i].type == 1)
				{
					list.Add(allElements[i]);
				}
			}
			int count = list.Count;
			for (int j = 0; j < count; j++)
			{
				if (this.items.Count <= j)
				{
					MainShopCoinPackItem mainShopCoinPackItem = Object.Instantiate<MainShopCoinPackItem>(this.prefabItem, this.prefabItem.transform.parent, false);
					mainShopCoinPackItem.Init();
					this.items.Add(mainShopCoinPackItem);
				}
				this.items[j].gameObject.SetActive(true);
				this.items[j].SetData(list[j]);
			}
			if (this.items.Count > count)
			{
				for (int k = count; k < this.items.Count; k++)
				{
					this.items[k].gameObject.SetActive(false);
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

		public MainShopCoinPackItem prefabItem;

		private List<MainShopCoinPackItem> items = new List<MainShopCoinPackItem>();
	}
}

using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace HotFix
{
	public class MainShopEquipActivityPackGroup : MainShopPackGroupBase
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.originPrefabItem.gameObject.SetActive(false);
			this.shopEquipDrawItem.Init();
		}

		protected override void OnDeInit()
		{
			this.shopEquipDrawItem.DeInit();
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].DeInit();
			}
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 3;
			subPriority = 0;
		}

		public override void UpdateContent()
		{
			List<IAPShopActivityData> shopActivities = this.iapDataModule.GetShopActivities(1);
			if (shopActivities == null || shopActivities.Count == 0)
			{
				this.shopEquipDrawItem.SetActive(true);
				this.shopEquipDrawItem.SetData();
				return;
			}
			this.shopEquipDrawItem.SetActive(false);
			for (int i = 0; i < shopActivities.Count; i++)
			{
				if (this.items.Count <= i)
				{
					MainShopEquipActivityItem mainShopEquipActivityItem = Object.Instantiate<MainShopEquipActivityItem>(this.originPrefabItem, this.originPrefabItem.transform.parent, false);
					mainShopEquipActivityItem.gameObject.SetActive(true);
					mainShopEquipActivityItem.Init();
					this.items.Add(mainShopEquipActivityItem);
				}
			}
			if (this.items.Count > shopActivities.Count)
			{
				for (int j = shopActivities.Count; j < this.items.Count; j++)
				{
					this.items[j].gameObject.SetActive(false);
				}
			}
			for (int k = 0; k < shopActivities.Count; k++)
			{
				this.items[k].SetData(shopActivities[k]);
			}
		}

		public override int PlayAnimation(float startTime, int index)
		{
			int num = 0;
			this.titleFg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index, 10024);
			num++;
			if (this.shopEquipDrawItem.isActiveAndEnabled)
			{
				this.shopEquipDrawItem.fg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index + 1, 10024);
				num++;
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].isActiveAndEnabled)
				{
					this.items[i].fg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index + i + 1, 10024);
					num++;
				}
			}
			return index + num + 1;
		}

		public IAPDataModule iapDataModule;

		public MainShopEquipActivityItem originPrefabItem;

		public MainShopEquipChestDiamond shopEquipDrawItem;

		private List<MainShopEquipActivityItem> items = new List<MainShopEquipActivityItem>();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Pool;

namespace HotFix
{
	public class EquipShopProbabilityViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonClose.m_onClick = new Action(this.CloseSelf);
			this.buttonMask.m_onClick = new Action(this.CloseSelf);
			this.poolParentDefaultLocalPos = this.poolParent.localPosition;
			this.poolCtrlPool = new ObjectPool<EquipShopProbabilityPoolCtrl>(() => Object.Instantiate<EquipShopProbabilityPoolCtrl>(this.poolCtrl, this.poolParent), delegate(EquipShopProbabilityPoolCtrl item)
			{
				item.gameObject.SetActive(true);
			}, delegate(EquipShopProbabilityPoolCtrl item)
			{
				item.gameObject.SetActive(false);
			}, null, true, 10, 10000);
			this.uiItemPool = new ObjectPool<EquipShopProbabilityRewardItem>(delegate
			{
				EquipShopProbabilityRewardItem equipShopProbabilityRewardItem = Object.Instantiate<EquipShopProbabilityRewardItem>(this.RewardItem);
				equipShopProbabilityRewardItem.Init();
				return equipShopProbabilityRewardItem;
			}, delegate(EquipShopProbabilityRewardItem item)
			{
				item.gameObject.SetActive(true);
			}, delegate(EquipShopProbabilityRewardItem item)
			{
				item.gameObject.SetActive(false);
			}, null, true, 10, 10000);
			this.PoolQualityGroundAtlasPath = GameApp.Table.GetAtlas(105).path;
		}

		public override void OnDelete()
		{
			this.buttonClose.m_onClick = null;
			this.buttonMask.m_onClick = null;
			this.poolCtrlPool.Dispose();
			this.uiItemPool.Dispose();
		}

		public override void OnOpen(object data)
		{
			this.shopSummonTable = GameApp.Table.GetManager().GetShop_Summon((int)data);
			if (this.shopSummonTable.rateShow == null || this.shopSummonTable.rateShow.Length == 0)
			{
				this.CloseSelf();
				return;
			}
			this.poolParent.localPosition = this.poolParentDefaultLocalPos;
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("shop_equip_probability_title");
			this.poolInfos.Clear();
			for (int i = 0; i < this.shopSummonTable.rateShow.Length; i++)
			{
				EquipShopProbabilityViewModule.PoolInfo poolInfo = new EquipShopProbabilityViewModule.PoolInfo();
				string[] array = this.shopSummonTable.rateShow[i].Split(',', StringSplitOptions.None);
				poolInfo.Id = int.Parse(array[0]);
				poolInfo.Rate = int.Parse(array[1]);
				poolInfo.QualityNameId = array[2];
				if (array.Length > 3)
				{
					poolInfo.Quality = int.Parse(array[3]);
				}
				this.poolInfos.Add(poolInfo.Id, poolInfo);
			}
			IEnumerable<Shop_SummonPool> shop_SummonPoolElements = GameApp.Table.GetManager().GetShop_SummonPoolElements();
			bool flag = true;
			foreach (Shop_SummonPool shop_SummonPool in shop_SummonPoolElements)
			{
				EquipShopProbabilityViewModule.PoolInfo poolInfo2;
				if (this.poolInfos.TryGetValue(shop_SummonPool.poolID, out poolInfo2))
				{
					poolInfo2.PoolItems.Add(shop_SummonPool);
					if (flag && !poolInfo2.ShowQualityIcon)
					{
						Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(shop_SummonPool.itemID);
						if (item_Item.itemType == 1 && GameApp.Table.GetManager().GetEquip_equip(int.Parse(item_Item.itemTypeParam[0])).rank == 2)
						{
							poolInfo2.ShowQualityIcon = true;
							flag = false;
						}
					}
				}
			}
			List<EquipShopProbabilityViewModule.PoolInfo> list = this.poolInfos.Values.ToList<EquipShopProbabilityViewModule.PoolInfo>();
			list.Sort(new Comparison<EquipShopProbabilityViewModule.PoolInfo>(this.PoolInfoSort));
			for (int j = 0; j < list.Count; j++)
			{
				EquipShopProbabilityViewModule.PoolInfo poolInfo3 = list[j];
				EquipShopProbabilityPoolCtrl equipShopProbabilityPoolCtrl = this.poolCtrlPool.Get();
				equipShopProbabilityPoolCtrl.transform.SetAsLastSibling();
				poolInfo3.PoolCtrl = equipShopProbabilityPoolCtrl;
				equipShopProbabilityPoolCtrl.textPoolName.text = Singleton<LanguageManager>.Instance.GetInfoByID(poolInfo3.QualityNameId);
				equipShopProbabilityPoolCtrl.textProbability.text = Singleton<LanguageManager>.Instance.GetInfoByID("shop_equip_probability_pool", new object[] { (float)poolInfo3.Rate / 100f });
				equipShopProbabilityPoolCtrl.imageQualityGround.SetImage(this.PoolQualityGroundAtlasPath, "pet_quality_frame_" + poolInfo3.Quality.ToString());
				equipShopProbabilityPoolCtrl.imageQualityIcon.enabled = poolInfo3.ShowQualityIcon;
				int num = 0;
				for (int k = 0; k < poolInfo3.PoolItems.Count; k++)
				{
					num += poolInfo3.PoolItems[k].weight;
				}
				for (int l = 0; l < poolInfo3.PoolItems.Count; l++)
				{
					EquipShopProbabilityRewardItem equipShopProbabilityRewardItem = this.uiItemPool.Get();
					poolInfo3.UIItems.Add(equipShopProbabilityRewardItem);
					equipShopProbabilityRewardItem.transform.SetParent(equipShopProbabilityPoolCtrl.transform);
					equipShopProbabilityRewardItem.transform.SetAsLastSibling();
					equipShopProbabilityRewardItem.transform.localPosition = this.RewardItem.transform.localPosition;
					equipShopProbabilityRewardItem.transform.localScale = this.RewardItem.transform.localScale;
					equipShopProbabilityRewardItem.SetData(poolInfo3.PoolItems[l], poolInfo3.Rate, num);
				}
			}
		}

		private int PoolInfoSort(EquipShopProbabilityViewModule.PoolInfo a, EquipShopProbabilityViewModule.PoolInfo b)
		{
			return -a.Quality.CompareTo(b.Quality);
		}

		public override void OnClose()
		{
			this.shopSummonTable = null;
			foreach (EquipShopProbabilityViewModule.PoolInfo poolInfo in this.poolInfos.Values)
			{
				foreach (EquipShopProbabilityRewardItem equipShopProbabilityRewardItem in poolInfo.UIItems)
				{
					this.uiItemPool.Release(equipShopProbabilityRewardItem);
				}
				this.poolCtrlPool.Release(poolInfo.PoolCtrl);
			}
			this.poolInfos.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.EquipShopProbabilityViewModule, null);
		}

		public CustomButton buttonMask;

		public CustomButton buttonClose;

		public CustomText textTitle;

		public RectTransform poolParent;

		public EquipShopProbabilityPoolCtrl poolCtrl;

		public EquipShopProbabilityRewardItem RewardItem;

		private Shop_Summon shopSummonTable;

		private Dictionary<int, EquipShopProbabilityViewModule.PoolInfo> poolInfos = new Dictionary<int, EquipShopProbabilityViewModule.PoolInfo>();

		private ObjectPool<EquipShopProbabilityPoolCtrl> poolCtrlPool;

		private ObjectPool<EquipShopProbabilityRewardItem> uiItemPool;

		private Vector3 poolParentDefaultLocalPos;

		private const int PoolQualityGroundAtlasId = 105;

		private string PoolQualityGroundAtlasPath;

		public class PoolInfo
		{
			public int Id;

			public int Rate;

			public string QualityNameId;

			public int Quality = 1;

			public bool ShowQualityIcon;

			public List<Shop_SummonPool> PoolItems = new List<Shop_SummonPool>();

			public EquipShopProbabilityPoolCtrl PoolCtrl;

			public List<EquipShopProbabilityRewardItem> UIItems = new List<EquipShopProbabilityRewardItem>();
		}
	}
}

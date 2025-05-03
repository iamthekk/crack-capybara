using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class EquipSUpSmallRewardPreviewItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.prefabItem.gameObject.SetActive(false);
			this.btnInfo.m_onClick = new Action(this.OnBtnInfoClick);
		}

		protected override void OnDeInit()
		{
			this.btnInfo.m_onClick = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.UpdateTime();
		}

		public void SetData(IAPShopActivityData data)
		{
			this.iapShopActivityData = data;
			this.listData = data.shopActDetailDto.SummonPoolItems.ToList<int>();
			if (this.listData == null)
			{
				this.listData = new List<int>();
			}
			this.UpdateItems();
			this.UpdateTime();
		}

		private void UpdateTime()
		{
			if (this.iapShopActivityData == null)
			{
				this.txtTime.text = "";
				return;
			}
			long endTimestamp = this.iapShopActivityData.endTimestamp;
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			long num = endTimestamp - serverTimestamp;
			if (num > 0L)
			{
				this.txtTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("suppool_ends_in", new object[] { DxxTools.FormatFullTimeWithDay(num) });
				return;
			}
			this.txtTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("suppool_ends_in", new object[] { DxxTools.FormatFullTimeWithDay(0L) });
		}

		private void UpdateItems()
		{
			for (int i = 0; i < this.listData.Count; i++)
			{
				EquipSUpSmallRewardPreviewItem_Item equipSUpSmallRewardPreviewItem_Item;
				if (i < this._items.Count)
				{
					equipSUpSmallRewardPreviewItem_Item = this._items[i];
				}
				else
				{
					equipSUpSmallRewardPreviewItem_Item = Object.Instantiate<EquipSUpSmallRewardPreviewItem_Item>(this.prefabItem, this.prefabItem.transform.parent, false);
					equipSUpSmallRewardPreviewItem_Item.Init();
					this._items.Add(equipSUpSmallRewardPreviewItem_Item);
				}
				equipSUpSmallRewardPreviewItem_Item.gameObject.SetActive(true);
				equipSUpSmallRewardPreviewItem_Item.UpdateData(i, this.listData[i]);
			}
			for (int j = this.listData.Count; j < this._items.Count; j++)
			{
				this._items[j].gameObject.SetActive(false);
			}
		}

		private void OnBtnInfoClick()
		{
			int linkId = this.iapShopActivityData.linkId;
			GameApp.View.OpenView(ViewName.EquipShopProbabilityViewModule, linkId, 1, null, null);
		}

		public RectTransform fg;

		public CustomButton btnInfo;

		public CustomText txtTime;

		public EquipSUpSmallRewardPreviewItem_Item prefabItem;

		private List<EquipSUpSmallRewardPreviewItem_Item> _items = new List<EquipSUpSmallRewardPreviewItem_Item>();

		private List<int> listData = new List<int>();

		private IAPShopActivityData iapShopActivityData;
	}
}

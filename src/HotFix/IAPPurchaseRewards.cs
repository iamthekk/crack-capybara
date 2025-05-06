using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class IAPPurchaseRewards : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.PrefabItem.SetActive(false);
		}

		protected override void OnDeInit()
		{
			this.DestroyAllItem();
		}

		private void DestroyAllItem()
		{
			for (int i = 0; i < this.mItemList.Count; i++)
			{
				this.mItemList[i].DeInit();
			}
			this.mItemList.Clear();
			this.RTFItemRoot.DestroyChildren();
		}

		public void SetData(List<PropData> datalist)
		{
			this.mDataList.Clear();
			this.mDataList.AddRange(datalist);
		}

		public void RefreshUI()
		{
			for (int i = 0; i < this.mDataList.Count; i++)
			{
				UIItem uiitem;
				if (i < this.mItemList.Count)
				{
					uiitem = this.mItemList[i];
				}
				else
				{
					uiitem = Object.Instantiate<GameObject>(this.PrefabItem, this.RTFItemRoot).GetComponent<UIItem>();
					uiitem.onClick = new Action<UIItem, PropData, object>(this.OnClickItem);
					uiitem.Init();
					this.mItemList.Add(uiitem);
				}
				uiitem.SetActive(true);
				uiitem.SetData(this.mDataList[i]);
				uiitem.OnRefresh();
			}
			for (int j = this.mDataList.Count; j < this.mItemList.Count; j++)
			{
				this.mItemList[j].SetActive(false);
			}
		}

		private void OnClickItem(UIItem item, PropData data, object arg3)
		{
			DxxTools.UI.OnItemClick(item, data, arg3);
		}

		public RectTransform RTFItemRoot;

		public GameObject PrefabItem;

		private List<PropData> mDataList = new List<PropData>();

		private List<UIItem> mItemList = new List<UIItem>();
	}
}

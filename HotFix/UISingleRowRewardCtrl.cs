using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UISingleRowRewardCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.copyItem.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.rewardItems.Count; i++)
			{
				this.rewardItems[i].DeInit();
			}
			this.rewardItems.Clear();
		}

		public void SetData(List<ItemData> rewards)
		{
			for (int i = 0; i < this.rewardItems.Count; i++)
			{
				this.rewardItems[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < rewards.Count; j++)
			{
				ItemData itemData = rewards[j];
				UIItem uiitem;
				if (j < this.rewardItems.Count)
				{
					uiitem = this.rewardItems[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(this.rewardLayout, false);
					uiitem = gameObject.GetComponent<UIItem>();
					uiitem.Init();
					this.rewardItems.Add(uiitem);
				}
				if (uiitem != null)
				{
					uiitem.SetData(new PropData
					{
						id = (uint)itemData.ID,
						count = (ulong)itemData.Count
					});
					uiitem.OnRefresh();
					uiitem.gameObject.SetActiveSafe(true);
				}
			}
		}

		public RectTransform rewardLayout;

		public GameObject copyItem;

		private List<UIItem> rewardItems = new List<UIItem>();
	}
}

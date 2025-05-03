using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.Platform;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class UIMiningBonusRewardCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Scroll.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			RectTransform component = this.Scroll.GetComponent<RectTransform>();
			this.initY = component.sizeDelta.y;
			float num = 0f;
			if (Singleton<PlatformHelper>.Instance.IsFringe())
			{
				num = Singleton<PlatformHelper>.Instance.GetBottomHeight();
			}
			component.sizeDelta = new Vector2(component.sizeDelta.x, this.initY - num);
			if (Utility.UI.ScreenRatio > Utility.UI.DesignRatio)
			{
				float num2 = (Utility.UI.ScreenRatio - Utility.UI.DesignRatio + 1f) * this.initY - num;
				component.sizeDelta = new Vector2(component.sizeDelta.x, num2);
			}
			this.Scroll.ScrollRect.horizontal = false;
			this.Scroll.ScrollRect.vertical = false;
		}

		protected override void OnDeInit()
		{
			this.DeInitAllScrollUI();
		}

		public void SetData(List<ItemData> list)
		{
			this.currentList.Clear();
			this.currentList.AddRange(list);
			this.Scroll.SetListItemCount(this.mDataList.Count, false);
			this.Scroll.RefreshAllShownItem();
		}

		public void ShowReward()
		{
			if (this.currentList.Count > 0)
			{
				this.mDataList.Add(this.currentList[0]);
				this.currentList.RemoveAt(0);
				this.Scroll.SetListItemCount(this.mDataList.Count, false);
				this.Scroll.RefreshAllShownItem();
				if (this.showRewardUI != null)
				{
					this.showRewardUI.ShowReward();
				}
			}
		}

		public void ResetReward()
		{
			this.mDataList.Clear();
			this.Scroll.SetListItemCount(this.mDataList.Count, false);
			this.Scroll.RefreshAllShownItem();
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			ItemData itemData = this.mDataList[index];
			LoopGridViewItem loopGridViewItem = view.NewListViewItem("UIBonusRewardItem");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			UIBonusRewardItem uibonusRewardItem = this.TryGetUI(instanceID);
			UIBonusRewardItem component = loopGridViewItem.GetComponent<UIBonusRewardItem>();
			if (uibonusRewardItem == null)
			{
				uibonusRewardItem = this.TryAddUI(instanceID, loopGridViewItem, component);
			}
			uibonusRewardItem.SetData(itemData, index);
			uibonusRewardItem.SetActive(true);
			if (index == this.mDataList.Count - 1)
			{
				this.showRewardUI = uibonusRewardItem;
			}
			return loopGridViewItem;
		}

		private UIBonusRewardItem TryGetUI(int key)
		{
			UIBonusRewardItem uibonusRewardItem;
			if (this.mUICtrlDic.TryGetValue(key, out uibonusRewardItem))
			{
				return uibonusRewardItem;
			}
			return null;
		}

		private UIBonusRewardItem TryAddUI(int key, LoopGridViewItem loopitem, UIBonusRewardItem ui)
		{
			ui.Init();
			UIBonusRewardItem uibonusRewardItem;
			if (this.mUICtrlDic.TryGetValue(key, out uibonusRewardItem))
			{
				if (uibonusRewardItem == null)
				{
					uibonusRewardItem = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, UIBonusRewardItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		public LoopGridView Scroll;

		private List<ItemData> mDataList = new List<ItemData>();

		private List<ItemData> currentList = new List<ItemData>();

		private Dictionary<int, UIBonusRewardItem> mUICtrlDic = new Dictionary<int, UIBonusRewardItem>();

		private UIBonusRewardItem showRewardUI;

		private float initY;
	}
}

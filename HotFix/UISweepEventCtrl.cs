using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Logic.Component;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class UISweepEventCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			LoopListViewInitParam loopListViewInitParam = LoopListViewInitParam.CopyDefaultInitParam();
			loopListViewInitParam.mSmoothDumpRate = 5f;
			this.loopListView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), loopListViewInitParam);
			this.loopListView.ScrollRect.horizontal = false;
			this.loopListView.ScrollRect.vertical = false;
			this.contentRectTrans = this.loopListView.ScrollRect.content.GetComponent<RectTransform>();
			this.scrollHeight = this.loopListView.ScrollRect.GetComponent<RectTransform>().sizeDelta.y;
			for (int i = 0; i < this.helpers.Length; i++)
			{
				this.helpers[i].Init();
			}
		}

		protected override void OnDeInit()
		{
			this.uiDataList.Clear();
			this.itemDic.Clear();
			for (int i = 0; i < this.helpers.Length; i++)
			{
				this.helpers[i].DeInit();
			}
		}

		public void AddEvent(GameEventUIData uiData)
		{
			this.uiDataList.Add(uiData);
			this.loopListView.SetListItemCount(this.uiDataList.Count, false);
		}

		public void ResetEvent()
		{
			this.uiDataList.Clear();
			this.previousIndex = -1;
			this.totalHeight = 0f;
			this.loopListView.SetListItemCount(this.uiDataList.Count, true);
			this.loopListView.UpdateListView();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.uiDataList.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UISweepEventItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UISweepEventItem uisweepEventItem = this.GetUIItem(instanceID);
			if (uisweepEventItem == null)
			{
				uisweepEventItem = this.AddUIItem(loopListViewItem.gameObject);
			}
			uisweepEventItem.Refresh(this.uiDataList[index], index, this.uiDataList.Count);
			if (index == this.uiDataList.Count - 1)
			{
				this.sweepEventItem = uisweepEventItem;
				this.loopListView.OnItemSizeChanged(index);
				if (this.previousIndex < index)
				{
					this.previousIndex = index;
					float height = this.sweepEventItem.GetHeight();
					this.totalHeight += height + loopListViewItem.Padding;
					Sequence sequence = DOTween.Sequence();
					float num = this.totalHeight - this.scrollHeight;
					if (num < 0f)
					{
						num = 0f;
					}
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalMoveY(this.contentRectTrans, num, 0.5f, false));
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						this.loopListView.ScrollRect.StopMovement();
					});
				}
				uisweepEventItem.PlayAni();
			}
			return loopListViewItem;
		}

		private UISweepEventItem GetUIItem(int instanceId)
		{
			UISweepEventItem uisweepEventItem;
			if (this.itemDic.TryGetValue(instanceId, out uisweepEventItem))
			{
				return uisweepEventItem;
			}
			return null;
		}

		private UISweepEventItem AddUIItem(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			int instanceID = obj.GetInstanceID();
			UISweepEventItem uisweepEventItem = this.GetUIItem(instanceID);
			if (uisweepEventItem == null)
			{
				uisweepEventItem = obj.GetComponent<UISweepEventItem>();
				uisweepEventItem.Init();
				this.itemDic.Add(instanceID, uisweepEventItem);
				return uisweepEventItem;
			}
			return uisweepEventItem;
		}

		public UISweepEventItem GetCurrentItem()
		{
			return this.sweepEventItem;
		}

		public LoopListView2 loopListView;

		public UILanguageHelper[] helpers;

		private Dictionary<int, UISweepEventItem> itemDic = new Dictionary<int, UISweepEventItem>();

		private List<GameEventUIData> uiDataList = new List<GameEventUIData>();

		private UISweepEventItem sweepEventItem;

		private RectTransform contentRectTrans;

		private float scrollHeight;

		private int previousIndex = -1;

		private float totalHeight;
	}
}

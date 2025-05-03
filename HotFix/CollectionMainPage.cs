using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class CollectionMainPage : CollectionTabPageBase
	{
		protected override void OnInit()
		{
			this.collectionDataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			this.scrollView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnDeInit()
		{
			foreach (KeyValuePair<int, CollectionItemGroup> keyValuePair in this.nodeDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.nodeDic.Clear();
		}

		public override void OnShow()
		{
			base.SetActive(true);
			this.UpdateView(true);
		}

		public override void UpdateView(bool playAnimation)
		{
			this.RefreshData();
			this.scrollView.SetListItemCount(this.dataList.Count, true);
			this.scrollView.RefreshAllShownItem();
			if (playAnimation)
			{
				List<CollectionItemGroup> list = this.nodeDic.Values.ToList<CollectionItemGroup>();
				list.Sort((CollectionItemGroup a, CollectionItemGroup b) => a.index.CompareTo(b.index));
				Sequence sequence = DOTween.Sequence();
				for (int i = 0; i < list.Count; i++)
				{
					RectTransform rectTransform = list[i].node.transform as RectTransform;
					Vector2 anchoredPosition = rectTransform.anchoredPosition;
					anchoredPosition.x = -1200f;
					rectTransform.anchoredPosition = anchoredPosition;
					Tweener tweener = ShortcutExtensions46.DOAnchorPos(rectTransform, Vector2.zero, 0.2f, false);
					TweenSettingsExtensions.Insert(sequence, 0.1f * (float)i, tweener);
				}
				TweenExtensions.Play<Sequence>(sequence);
			}
		}

		public override void OnHide()
		{
			base.SetActive(false);
		}

		private void RefreshData()
		{
			List<CollectionData> list = new List<CollectionData>();
			List<CollectionData> list2 = new List<CollectionData>();
			List<CollectionData> list3 = new List<CollectionData>();
			List<CollectionData> list4 = new List<CollectionData>();
			List<CollectionData> list5 = new List<CollectionData>();
			foreach (CollectionData collectionData in this.collectionDataModule.collectionDict.Values)
			{
				if (collectionData.IsCanMerge)
				{
					list2.Add(collectionData);
				}
				else if (collectionData.collectionType == 1U)
				{
					list3.Add(collectionData);
				}
				else if (collectionData.collectionType == 2U)
				{
					list4.Add(collectionData);
				}
				else if (collectionData.collectionType == 0U)
				{
					list5.Add(collectionData);
				}
			}
			CollectionHelper.SortCollectionList(list2);
			CollectionHelper.SortCollectionList(list3);
			CollectionHelper.SortCollectionList(list4);
			CollectionHelper.SortCollectionList(list5);
			list.AddRange(list2);
			list.AddRange(list3);
			list.AddRange(list4);
			list.AddRange(list5);
			this.dataList.Clear();
			for (int i = 0; i < list.Count; i += this.columnCount)
			{
				CollectionMainPage.NodeData nodeData = new CollectionMainPage.NodeData();
				nodeData.collectionList = new List<CollectionData>();
				for (int j = 0; j < this.columnCount; j++)
				{
					int num = i + j;
					if (num < list.Count)
					{
						nodeData.collectionList.Add(list[num]);
					}
				}
				this.dataList.Add(nodeData);
			}
			if (this.dataList.Count < 5)
			{
				for (int k = 1; k <= 5; k++)
				{
					if (k > this.dataList.Count)
					{
						this.dataList.Add(null);
					}
				}
			}
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.dataList.Count)
			{
				return null;
			}
			CollectionMainPage.NodeData nodeData = this.dataList[index];
			LoopListViewItem2 loopListViewItem = view.NewListViewItem("CollectionItemGroup");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			CollectionItemGroup collectionItemGroup;
			this.nodeDic.TryGetValue(instanceID, out collectionItemGroup);
			if (collectionItemGroup == null)
			{
				CollectionItemGroup component = loopListViewItem.gameObject.GetComponent<CollectionItemGroup>();
				component.Init();
				component.SetData(index, nodeData, new Func<CollectionData, bool>(this.CalcCollectionItemRedShow));
				this.nodeDic[instanceID] = component;
			}
			else
			{
				CollectionItemGroup collectionItemGroup2 = collectionItemGroup;
				if (collectionItemGroup2 != null)
				{
					collectionItemGroup2.SetData(index, nodeData, new Func<CollectionData, bool>(this.CalcCollectionItemRedShow));
				}
			}
			return loopListViewItem;
		}

		public override void SetTweenFloatingPos(float localPosY)
		{
			foreach (CollectionItemGroup collectionItemGroup in this.nodeDic.Values)
			{
				collectionItemGroup.SetTweenFloatingPos(localPosY);
			}
		}

		private bool CalcCollectionItemRedShow(CollectionData data)
		{
			return data != null && (data.IsCanMerge || data.IsMatchStarUpgradeCondition());
		}

		public LoopListView2 scrollView;

		private Dictionary<int, CollectionItemGroup> nodeDic = new Dictionary<int, CollectionItemGroup>();

		private List<CollectionMainPage.NodeData> dataList = new List<CollectionMainPage.NodeData>();

		private CollectionDataModule collectionDataModule;

		private int columnCount = 4;

		public class NodeData
		{
			public int index;

			public bool isTopSpace;

			public bool isBottomSpace;

			public List<CollectionData> collectionList;
		}
	}
}

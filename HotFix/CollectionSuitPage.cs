using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using SuperScrollView;

namespace HotFix
{
	public class CollectionSuitPage : CollectionTabPageBase
	{
		protected override void OnInit()
		{
			this.scrollView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.collectionDataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
		}

		protected override void OnDeInit()
		{
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.nodeDic)
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

		public override void OnHide()
		{
			base.SetActive(false);
		}

		public override void UpdateView(bool playAnimation)
		{
			this.RefreshData();
			this.scrollView.SetListItemCount(this.dataList.Count, true);
			this.scrollView.RefreshAllShowItems();
		}

		public override void SetTweenFloatingPos(float localPosY)
		{
			foreach (CustomBehaviour customBehaviour in this.nodeDic.Values)
			{
				if (customBehaviour is CollectionSuitItem)
				{
					(customBehaviour as CollectionSuitItem).SetTweenFloatingPos(localPosY);
				}
			}
		}

		private void RefreshData()
		{
			this.dataList = new List<CollectionSuitPage.NodeData>();
			foreach (CollectionSuitData collectionSuitData in this.collectionDataModule.collectionSuitDict.Values)
			{
				CollectionSuitPage.NodeData nodeData = new CollectionSuitPage.NodeData();
				nodeData.collectionSuitData = collectionSuitData;
				this.dataList.Add(nodeData);
			}
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.dataList.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = null;
			CollectionSuitPage.NodeData nodeData = this.dataList[index];
			if (nodeData.collectionSuitData != null)
			{
				loopListViewItem = view.NewListViewItem("SuitItem");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.nodeDic.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					CollectionSuitItem component = loopListViewItem.gameObject.GetComponent<CollectionSuitItem>();
					component.Init();
					component.SetData(nodeData, index, this.scrollView);
					this.nodeDic[instanceID] = component;
				}
				else
				{
					CollectionSuitItem collectionSuitItem = customBehaviour as CollectionSuitItem;
					if (collectionSuitItem != null)
					{
						collectionSuitItem.SetData(nodeData, index, this.scrollView);
					}
				}
				return loopListViewItem;
			}
			return loopListViewItem;
		}

		public LoopListView2 scrollView;

		private List<CollectionSuitPage.NodeData> dataList;

		private CollectionDataModule collectionDataModule;

		private Dictionary<int, CustomBehaviour> nodeDic = new Dictionary<int, CustomBehaviour>();

		public class NodeData
		{
			public int index;

			public bool isTopSpace;

			public bool isBottomSpace;

			public CollectionSuitData collectionSuitData;
		}
	}
}

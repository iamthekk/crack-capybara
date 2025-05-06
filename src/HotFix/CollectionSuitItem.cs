using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class CollectionSuitItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.attributeItem.gameObject.SetActive(false);
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].Init();
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
			}
		}

		public void SetData(CollectionSuitPage.NodeData data, int index, LoopListView2 loopListView2)
		{
			this.data = data;
			this.UpdateView();
			loopListView2.OnItemSizeChanged(index);
		}

		private void UpdateView()
		{
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.data.collectionSuitData.GetSuitName());
			this.UpdateCollectionItems();
			this.UpdateAttributeItems();
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.transform as RectTransform);
		}

		private void UpdateCollectionItems()
		{
			int[] curCollectionIds = this.data.collectionSuitData.GetCurCollectionIds();
			for (int i = 0; i < this.itemList.Count; i++)
			{
				if (i >= curCollectionIds.Length)
				{
					this.itemList[i].gameObject.SetActive(false);
				}
				else
				{
					this.itemList[i].gameObject.SetActive(true);
					CollectionDataModule dataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
					int num = curCollectionIds[i];
					CollectionData collectionData = dataModule.GetCollectionData(num);
					if (collectionData == null)
					{
						HLog.LogError("suit collectionId [" + string.Join<int>(",", curCollectionIds) + "] error, please check");
						return;
					}
					this.itemList[i].SetData(collectionData);
				}
			}
		}

		private void UpdateAttributeItems()
		{
			this.data.collectionSuitData.UpdateSuitData();
			if (this.attributeItemList.Count <= 0)
			{
				for (int i = 0; i < this.data.collectionSuitData.GroupConfigList.Count; i++)
				{
					CollectionSuitAttributeItem collectionSuitAttributeItem = Object.Instantiate<CollectionSuitAttributeItem>(this.attributeItem, this.attributeItem.transform.parent, false);
					this.attributeItemList.Add(collectionSuitAttributeItem);
					collectionSuitAttributeItem.gameObject.SetActive(true);
					collectionSuitAttributeItem.Init();
				}
			}
			for (int j = 0; j < this.attributeItemList.Count; j++)
			{
				CollectionSuitAttributeItem collectionSuitAttributeItem2 = this.attributeItemList[j];
				int curIndex = this.data.collectionSuitData.CurIndex;
				bool flag = curIndex > j || (curIndex == j && this.data.collectionSuitData.CurIndexConditionMatch);
				collectionSuitAttributeItem2.SetData(this.data.collectionSuitData.GroupConfigList[j], flag);
			}
		}

		public void SetTweenFloatingPos(float localPosY)
		{
			Vector2 zero = Vector2.zero;
			zero.y = localPosY;
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].imgIcon.transform.localPosition = zero;
			}
		}

		public List<CollectionItem> itemList = new List<CollectionItem>();

		public CollectionSuitAttributeItem attributeItem;

		public CustomText txtTitle;

		public CustomText labelBasicEffect;

		private List<CollectionSuitAttributeItem> attributeItemList = new List<CollectionSuitAttributeItem>();

		private CollectionSuitPage.NodeData data;
	}
}

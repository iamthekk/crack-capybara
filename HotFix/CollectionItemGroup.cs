using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class CollectionItemGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
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

		public void SetData(int index, CollectionMainPage.NodeData nodeData, Func<CollectionData, bool> calcCollectionItemRedShow)
		{
			this.index = index;
			if (nodeData == null)
			{
				for (int i = 0; i < this.itemList.Count; i++)
				{
					this.itemList[i].gameObject.SetActive(false);
				}
				return;
			}
			for (int j = 0; j < this.itemList.Count; j++)
			{
				if (j < nodeData.collectionList.Count)
				{
					this.itemList[j].gameObject.SetActive(true);
					this.itemList[j].RedCalcAction = calcCollectionItemRedShow;
					this.itemList[j].SetData(nodeData.collectionList[j]);
				}
				else
				{
					this.itemList[j].gameObject.SetActive(false);
				}
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

		public Transform node;

		public List<CollectionItem> itemList = new List<CollectionItem>();

		public int index;
	}
}

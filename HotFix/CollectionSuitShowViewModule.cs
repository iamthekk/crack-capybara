using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class CollectionSuitShowViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.collectionDataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			this.attributeItem.gameObject.SetActive(false);
			for (int i = 0; i < this.cardItemList.Count; i++)
			{
				this.cardItemList[i].Init();
			}
		}

		public override void OnOpen(object data)
		{
			this.data = data as CollectionSuitData;
			this.UpdateView();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.btnBg.m_onClick = new Action(this.OnBtnCloseClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = null;
			this.btnBg.m_onClick = null;
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			this.OnBtnCloseClick();
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.CollectionSuitShowViewModule, null);
		}

		private void UpdateView()
		{
			this.UpdateSuitCards();
			this.UpdateAttributeItems();
		}

		private void UpdateSuitCards()
		{
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.data.GetSuitName());
			int[] curCollectionIds = this.data.GetCurCollectionIds();
			for (int i = 0; i < this.cardItemList.Count; i++)
			{
				if (i < curCollectionIds.Length)
				{
					this.cardItemList[i].gameObject.SetActive(true);
					CollectionData collectionData = this.collectionDataModule.GetCollectionData(curCollectionIds[i]);
					this.cardItemList[i].SetData(collectionData);
				}
				else
				{
					this.cardItemList[i].gameObject.SetActive(false);
				}
			}
		}

		private void UpdateAttributeItems()
		{
			this.data.UpdateSuitData();
			if (this.attributeItemList.Count <= 0)
			{
				for (int i = 0; i < this.data.GroupConfigList.Count; i++)
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
				int curIndex = this.data.CurIndex;
				bool flag = curIndex > j || (curIndex == j && this.data.CurIndexConditionMatch);
				collectionSuitAttributeItem2.SetData(this.data.GroupConfigList[j], flag);
			}
		}

		public UIPopCommon uiPopCommon;

		public CustomButton btnBg;

		public CustomText txtTitle;

		public List<CollectionCardItem> cardItemList = new List<CollectionCardItem>();

		public CollectionSuitAttributeItem attributeItem;

		private CollectionDataModule collectionDataModule;

		private CollectionSuitData data;

		private List<CollectionSuitAttributeItem> attributeItemList = new List<CollectionSuitAttributeItem>();
	}
}

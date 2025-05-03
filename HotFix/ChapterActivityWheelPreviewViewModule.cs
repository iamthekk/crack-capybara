using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Common;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ChapterActivityWheelPreviewViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnBtnCloseClick));
			this.loopListContent.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetContentItemByIndex), null);
		}

		public override void OnOpen(object data)
		{
			this.CreateData();
			this.loopListContent.SetListItemCount(this.contentNodeDatas.Count, true);
			this.loopListContent.RefreshAllShownItem();
			this.loopListContent.MovePanelToItemIndex(this.curContentItemIndex - 1, 0f);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.dataModule == null)
			{
				return;
			}
			if (this.dataModule.WheelInfo == null)
			{
				return;
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			long num = this.dataModule.WheelInfo.EndTime - serverTimestamp;
			if (num > 0L)
			{
				this.textTime.text = Singleton<LanguageManager>.Instance.GetTime(num);
				return;
			}
			this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_end");
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnBtnCloseClick));
			foreach (CustomBehaviour customBehaviour in this.contentItemsDict.Values)
			{
				if (customBehaviour != null)
				{
					customBehaviour.DeInit();
				}
			}
			this.contentItemsDict.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.ChapterActivityWheelPreviewViewModule, null);
		}

		private void CreateData()
		{
			this.contentNodeDatas.Clear();
			List<ChapterActivity_ActvTurntableReward> allRewards = this.dataModule.GetAllRewards();
			ChapterActivity_ActvTurntableReward currentReward = this.dataModule.GetCurrentReward();
			if (allRewards.Count == 0)
			{
				this.OnBtnCloseClick();
				return;
			}
			this.contentNodeDatas.Add(new ChapterActivityWheelPreviewViewModule.ContentNodeData
			{
				isTopSpace = true
			});
			this.curContentItemIndex = allRewards.Count;
			for (int i = 0; i < allRewards.Count; i++)
			{
				ChapterActivityWheelPreviewViewModule.ContentNodeData contentNodeData = new ChapterActivityWheelPreviewViewModule.ContentNodeData();
				contentNodeData.cfg = allRewards[i];
				this.contentNodeDatas.Add(contentNodeData);
				if (currentReward != null && currentReward.id == contentNodeData.cfg.id)
				{
					this.curContentItemIndex = i + 1;
				}
			}
			this.contentNodeDatas.Add(new ChapterActivityWheelPreviewViewModule.ContentNodeData
			{
				isBottomSpace = true
			});
		}

		private LoopListViewItem2 OnGetContentItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.contentNodeDatas.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = null;
			ChapterActivityWheelPreviewViewModule.ContentNodeData contentNodeData = this.contentNodeDatas[index];
			if (contentNodeData.isTopSpace)
			{
				loopListViewItem = view.NewListViewItem("TopSpace");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.contentItemsDict.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.contentItemsDict[instanceID] = component;
				}
			}
			else if (contentNodeData.isBottomSpace)
			{
				loopListViewItem = view.NewListViewItem("BottomSpace");
				int instanceID2 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour2;
				this.contentItemsDict.TryGetValue(instanceID2, out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIItemSpace component2 = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component2.Init();
					this.contentItemsDict[instanceID2] = component2;
				}
			}
			else
			{
				loopListViewItem = view.NewListViewItem("UIRewardPreviewItem");
				int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour3;
				this.contentItemsDict.TryGetValue(instanceID3, out customBehaviour3);
				if (customBehaviour3 == null)
				{
					UIRewardPreviewItem component3 = loopListViewItem.gameObject.GetComponent<UIRewardPreviewItem>();
					component3.Init();
					this.contentItemsDict[instanceID3] = component3;
					customBehaviour3 = component3;
				}
				UIRewardPreviewItem uirewardPreviewItem = customBehaviour3 as UIRewardPreviewItem;
				if (uirewardPreviewItem != null)
				{
					ChapterActiveWheelInfo wheelInfo = this.dataModule.WheelInfo;
					if (wheelInfo != null)
					{
						int score = wheelInfo.Score;
					}
					uirewardPreviewItem.SetData(index, contentNodeData.cfg);
				}
			}
			List<UIRewardPreviewItem> list = new List<UIRewardPreviewItem>();
			foreach (CustomBehaviour customBehaviour4 in this.contentItemsDict.Values)
			{
				if (customBehaviour4 is UIRewardPreviewItem)
				{
					list.Add(customBehaviour4 as UIRewardPreviewItem);
				}
			}
			list.Sort(new Comparison<UIRewardPreviewItem>(this.SortContentItemList));
			int num = int.MaxValue;
			int num2 = int.MinValue;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.SetAsLastSibling();
				if (list[i].isActiveAndEnabled)
				{
					num = Mathf.Min(num, list[i].Index);
					num2 = Mathf.Max(num2, list[i].Index);
				}
			}
			return loopListViewItem;
		}

		private int SortContentItemList(UIRewardPreviewItem a, UIRewardPreviewItem b)
		{
			return b.Index.CompareTo(a.Index);
		}

		public LoopListView2 loopListContent;

		public CustomButton buttonClose;

		public CustomText textTime;

		private List<ChapterActivityWheelPreviewViewModule.ContentNodeData> contentNodeDatas = new List<ChapterActivityWheelPreviewViewModule.ContentNodeData>();

		private Dictionary<int, CustomBehaviour> contentItemsDict = new Dictionary<int, CustomBehaviour>();

		private ChapterActivityWheelDataModule dataModule;

		private int curContentItemIndex;

		public class ContentNodeData
		{
			public bool isTopSpace;

			public bool isBottomSpace;

			public ChapterActivity_ActvTurntableReward cfg;
		}
	}
}

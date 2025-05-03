using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class ChapterActivityRankPreviewViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
			this.buttonClose.m_onClick = new Action(this.OnBtnCloseClick);
			this.loopListContent.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetContentItemByIndex), null);
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.ChapterActivityRankPreviewViewModule, null);
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as ChapterActivityRankPreviewViewModule.OpenData;
			if (this.m_openData == null)
			{
				return;
			}
			this.Obj_TimeUp.SetActiveSafe(false);
			this.Obj_TimeNode.SetActiveSafe(false);
			this.OnRefreshView();
		}

		private void CreateData()
		{
			this.contentNodeDatas.Clear();
			List<ChapterActivity_ChapterObj> list = this.dataModule.OnGetAllRankRewardList(this.m_rankActivityCfg.group);
			ChapterActivity_ChapterObj chapterActivity_ChapterObj = null;
			if (this.m_chapterActivityData != null)
			{
				chapterActivity_ChapterObj = this.m_chapterActivityData.CurrentProgress;
			}
			if (list.Count == 0)
			{
				this.OnBtnCloseClick();
				return;
			}
			this.contentNodeDatas.Add(new ChapterActivityRankPreviewViewModule.ContentNodeData
			{
				isTopSpace = true
			});
			this.curContentItemIndex = list.Count;
			for (int i = 0; i < list.Count; i++)
			{
				ChapterActivityRankPreviewViewModule.ContentNodeData contentNodeData = new ChapterActivityRankPreviewViewModule.ContentNodeData();
				contentNodeData.cfg = list[i];
				this.contentNodeDatas.Add(contentNodeData);
				if (chapterActivity_ChapterObj != null && chapterActivity_ChapterObj.id == contentNodeData.cfg.id)
				{
					this.curContentItemIndex = i + 1;
				}
			}
			this.contentNodeDatas.Add(new ChapterActivityRankPreviewViewModule.ContentNodeData
			{
				isBottomSpace = true
			});
		}

		private void OnRefreshView()
		{
			this.m_rankActivityCfg = GameApp.Table.GetManager().GetChapterActivity_RankActivity((int)this.m_openData.ChapterActivity.ActivityId);
			if (this.m_rankActivityCfg == null)
			{
				return;
			}
			if (this.dataModule.GetRewardActivity(ChapterActivityKind.Rank) != null)
			{
				this.Obj_TimeUp.SetActiveSafe(true);
				this.Obj_TimeNode.SetActiveSafe(false);
			}
			else
			{
				this.m_chapterActivityData = this.dataModule.GetActiveActivityData(ChapterActivityKind.Rank);
				if (this.m_chapterActivityData == null)
				{
					this.Obj_TimeUp.SetActiveSafe(true);
					this.Obj_TimeNode.SetActiveSafe(false);
				}
				else
				{
					this.Obj_TimeNode.SetActiveSafe(!this.m_chapterActivityData.IsEnd());
					this.Obj_TimeUp.SetActiveSafe(this.m_chapterActivityData.IsEnd());
				}
			}
			this.CreateData();
			this.loopListContent.SetListItemCount(this.contentNodeDatas.Count, true);
			this.loopListContent.RefreshAllShownItem();
			this.loopListContent.MovePanelToItemIndex(this.curContentItemIndex - 1, 0f);
		}

		private LoopListViewItem2 OnGetContentItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.contentNodeDatas.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = null;
			ChapterActivityRankPreviewViewModule.ContentNodeData contentNodeData = this.contentNodeDatas[index];
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
					uirewardPreviewItem.SetData(index, contentNodeData.cfg, this.m_chapterActivityData);
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

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_chapterActivityData == null)
			{
				return;
			}
			if (this.m_chapterActivityData.IsEnd())
			{
				this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_end");
				return;
			}
			long num = ChapterActivityDataModule.ServerTime();
			this.textTime.text = Singleton<LanguageManager>.Instance.GetTime(this.m_chapterActivityData.EndTime - num);
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonClose.m_onClick = null;
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

		public LoopListView2 loopListContent;

		public CustomButton buttonClose;

		public GameObject Obj_TimeUp;

		public GameObject Obj_TimeNode;

		public CustomText textTime;

		private List<ChapterActivityRankPreviewViewModule.ContentNodeData> contentNodeDatas = new List<ChapterActivityRankPreviewViewModule.ContentNodeData>();

		private Dictionary<int, CustomBehaviour> contentItemsDict = new Dictionary<int, CustomBehaviour>();

		private ChapterActivityRankPreviewViewModule.OpenData m_openData;

		private ChapterActivity_RankActivity m_rankActivityCfg;

		private ChapterActivityDataModule dataModule;

		private ChapterActivityData m_chapterActivityData;

		private int curContentItemIndex;

		public class OpenData
		{
			public ChapterActivityData ChapterActivity;
		}

		public class ContentNodeData
		{
			public bool isTopSpace;

			public bool isBottomSpace;

			public ChapterActivity_ChapterObj cfg;
		}
	}
}

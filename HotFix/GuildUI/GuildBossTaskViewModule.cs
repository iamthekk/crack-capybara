using System;
using System.Collections.Generic;
using DG.Tweening;
using Dxx.Guild;
using Framework.Logic.UI;
using SuperScrollView;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildBossTaskViewModule : GuildProxy.GuildProxy_BaseView
	{
		private GuildBossInfo guildBossInfo
		{
			get
			{
				return base.SDK.GuildActivity.GuildBoss;
			}
		}

		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.Button_Close.m_onClick = new Action(this.OnClickClose);
			this.Button_Mask.m_onClick = new Action(this.OnClickClose);
			this.Button_Help.Init();
			this.ScrollViewItem.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			this.OnRefreshView();
			DelayCall.Instance.CallLoop(1000, new DelayCall.CallAction(this.RefreshTime));
			GuildSDKManager.Instance.Event.RegisterEvent(203, new GuildHandlerEvent(this.OnUpdateGuildBossTask));
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnViewUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.RefreshTime));
			GuildSDKManager.Instance.Event.UnRegisterEvent(203, new GuildHandlerEvent(this.OnUpdateGuildBossTask));
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
			this.Button_Close.m_onClick = null;
			this.Button_Mask.m_onClick = null;
			this.Button_Help.DeInit();
			this.m_pool.Clear(false);
			foreach (KeyValuePair<int, GuildBossTaskItem> keyValuePair in this.dic)
			{
				keyValuePair.Value.DeInit();
			}
		}

		private void OnClickClose()
		{
			GuildProxy.UI.CloseUIGuildBossTask();
		}

		private void OnRefreshView()
		{
			if (this.guildBossInfo == null)
			{
				return;
			}
			this.RefreshTime();
			this.taskList.Clear();
			this.taskList.AddRange(this.GetSortTaskList());
			this.ScrollViewItem.SetListItemCount(this.taskList.Count, true);
			this.ScrollViewItem.RefreshAllShowItems();
			this.ScrollViewItem.MovePanelToItemIndex(0, 0f);
			this.PlayShowAnim();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.taskList.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UI_GuildBossTaskItem");
			GuildBossTaskItem component;
			this.dic.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<GuildBossTaskItem>();
				component.Init();
				this.dic[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			component.SetData(this.taskList[index]);
			return loopListViewItem;
		}

		private void PlayShowAnim()
		{
			this.m_pool.Clear(false);
			Sequence sequence = this.m_pool.Get();
			for (int i = 0; i < this.ScrollViewItem.ShownItemCount; i++)
			{
				LoopListViewItem2 shownItemByIndex = this.ScrollViewItem.GetShownItemByIndex(i);
				if (!(shownItemByIndex == null))
				{
					RectTransform cachedRectTransform = shownItemByIndex.CachedRectTransform;
					DxxTools.UI.DoMoveRightToScreenAnim(sequence, cachedRectTransform.GetChild(0) as RectTransform, 0f, 0.1f * (float)i, 0.2f, 9);
				}
			}
		}

		private List<GuildBossTask> GetSortTaskList()
		{
			List<GuildBossTask> list = new List<GuildBossTask>();
			List<GuildBossTask> list2 = new List<GuildBossTask>();
			List<GuildBossTask> list3 = new List<GuildBossTask>();
			List<GuildBossTask> taskBossList = this.guildBossInfo.TaskBossList;
			for (int i = 0; i < taskBossList.Count; i++)
			{
				if (taskBossList[i].taskState == GuildBossTask.GuildBossTaskState.Undone)
				{
					list2.Add(taskBossList[i]);
				}
				else if (taskBossList[i].taskState == GuildBossTask.GuildBossTaskState.AllFinish)
				{
					list3.Add(taskBossList[i]);
				}
				else
				{
					list.Add(taskBossList[i]);
				}
			}
			list.AddRange(list2);
			list.AddRange(list3);
			return list;
		}

		private void RefreshTime()
		{
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			long num = this.guildBossInfo.ServerSeasonEndTime - serverTimestamp;
			if (num <= 0L)
			{
				num = 0L;
			}
			this.Text_Time.text = Singleton<LanguageManager>.Instance.GetInfoByID("400185", new object[] { DxxTools.FormatFullTimeWithDay(num) });
		}

		private void OnUpdateGuildBossTask(int type, GuildBaseEvent eventargs)
		{
			this.OnRefreshView();
		}

		public CustomButton Button_Close;

		public CustomButton Button_Mask;

		public UIHelpButton Button_Help;

		public CustomText Text_Time;

		public LoopListView2 ScrollViewItem;

		public RectTransform Rect_Content;

		private Dictionary<int, GuildBossTaskItem> dic = new Dictionary<int, GuildBossTaskItem>();

		private List<GuildBossTask> taskList = new List<GuildBossTask>();

		private SequencePool m_pool = new SequencePool();
	}
}

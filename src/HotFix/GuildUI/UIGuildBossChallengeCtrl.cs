using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.EventSystem;
using Framework.Logic.AttributeExpansion;
using SuperScrollView;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildBossChallengeCtrl : UIGuildBossBaseCtrl
	{
		public override UIGuildBossTag BossTag
		{
			get
			{
				return UIGuildBossTag.Challenge;
			}
		}

		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.loopListView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
		}

		protected override void GuildUI_OnShow()
		{
			GuildProxy.GameEvent.AddReceiver(LocalMessageName.CC_GuildBoss_Refresh, new HandlerEvent(this.OnRefresh));
			base.SDK.Event.RegisterEvent(202, new GuildHandlerEvent(this.OnRefresh));
			base.gameObject.SetActiveSafe(true);
			this.loopListView.ResetListView(true);
			this.Refresh();
		}

		protected override void GuildUI_OnClose()
		{
			GuildProxy.GameEvent.RemoveReceiver(LocalMessageName.CC_GuildBoss_Refresh, new HandlerEvent(this.OnRefresh));
			base.SDK.Event.UnRegisterEvent(202, new GuildHandlerEvent(this.OnRefresh));
			base.gameObject.SetActiveSafe(false);
		}

		private void Refresh()
		{
			this.taskList = this.GetSortTaskList();
			this.loopListView.SetListItemCount(this.taskList.Count, true);
			this.loopListView.RefreshAllShownItem();
			if (this.isShowAni)
			{
				this.isShowAni = false;
				for (int i = 0; i < this.aniList.Count; i++)
				{
					this.aniList[i].ShowAni(i);
				}
				this.aniList.Clear();
			}
		}

		private void OnRefresh(object sender, int type, BaseEventArgs eventArgs)
		{
			this.Refresh();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.taskList.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UIGuildBossChallengeItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIGuildBossChallengeItem uiguildBossChallengeItem = this.GetUIItem(instanceID);
			if (uiguildBossChallengeItem == null)
			{
				uiguildBossChallengeItem = this.AddUIItem(loopListViewItem.gameObject);
			}
			uiguildBossChallengeItem.Refresh(this.taskList[index]);
			uiguildBossChallengeItem.ClearAni();
			if (this.isShowAni && index < 10 && !this.aniList.Contains(uiguildBossChallengeItem))
			{
				this.aniList.Add(uiguildBossChallengeItem);
			}
			return loopListViewItem;
		}

		private UIGuildBossChallengeItem GetUIItem(int instanceId)
		{
			if (this.dic.ContainsKey(instanceId))
			{
				return this.dic[instanceId];
			}
			return null;
		}

		private UIGuildBossChallengeItem AddUIItem(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			int instanceID = obj.GetInstanceID();
			UIGuildBossChallengeItem component = obj.GetComponent<UIGuildBossChallengeItem>();
			component.Init();
			this.dic.Add(instanceID, component);
			return component;
		}

		private void OnRefresh(int type, GuildBaseEvent eventArgs)
		{
			this.Refresh();
		}

		private List<GuildBossTask> GetSortTaskList()
		{
			List<GuildBossTask> list = new List<GuildBossTask>();
			List<GuildBossTask> list2 = new List<GuildBossTask>();
			List<GuildBossTask> list3 = new List<GuildBossTask>();
			List<GuildBossTask> taskBossList = base.SDK.GuildActivity.GuildBoss.TaskBossList;
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

		public LoopListView2 loopListView;

		[Label]
		public bool isinitlooplist;

		private Dictionary<int, UIGuildBossChallengeItem> dic = new Dictionary<int, UIGuildBossChallengeItem>();

		private List<GuildBossTask> taskList = new List<GuildBossTask>();

		private List<UIGuildBossChallengeItem> aniList = new List<UIGuildBossChallengeItem>();
	}
}

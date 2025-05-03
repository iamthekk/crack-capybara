using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class TaskAchievementPanelGroup : BaseTaskPanel
	{
		protected override void OnInit()
		{
			this.m_taskDataModule = GameApp.Data.GetDataModule(DataName.TaskDataModule);
			this.m_scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TaskDataModule_RefreshTaskData, new HandlerEvent(this.OnEventRefreshTaskData));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TaskViewModule_ReceiveAchievementRewardTask, new HandlerEvent(this.OnEventReceiveAchievementRewardTask));
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_RefreshTaskData, new HandlerEvent(this.OnEventRefreshTaskData));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TaskViewModule_ReceiveAchievementRewardTask, new HandlerEvent(this.OnEventReceiveAchievementRewardTask));
			this.m_seqPool.Clear(false);
			this.m_taskDataModule = null;
		}

		public override void OnShow()
		{
			this.OnRefreshUI();
			this.PlayScale();
		}

		public override void OnHide()
		{
		}

		public override void OnRefresh()
		{
			this.OnRefreshUI();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.m_datas.Count + 1)
			{
				return null;
			}
			if (index == 0)
			{
				return listView.NewListViewItem("Space");
			}
			TaskDataModule.AchievementData achievementData = this.m_datas[index - 1];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("Node");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			TaskAchievementNode component;
			this.m_nodes.TryGetValue(instanceID, out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<TaskAchievementNode>();
				component.Init();
				this.m_nodes[instanceID] = component;
			}
			component.OnRefresh(achievementData);
			return loopListViewItem;
		}

		private void OnRefreshUI()
		{
			this.m_datas = this.m_taskDataModule.AchievementDatas;
			this.m_scroll.SetListItemCount(this.m_datas.Count + 1, true);
			this.m_scroll.RefreshAllShownItem();
		}

		private void OnEventRefreshTaskData(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshUI();
		}

		private void OnEventReceiveAchievementRewardTask(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgTaskViewReceiveAchievementRewardTask args = eventargs as EventArgTaskViewReceiveAchievementRewardTask;
			if (args == null)
			{
				return;
			}
			TaskAchievementNode taskAchievementNode = null;
			foreach (KeyValuePair<int, TaskAchievementNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null) && keyValuePair.Value.m_data != null && keyValuePair.Value.m_data.ID == args.m_id)
				{
					taskAchievementNode = keyValuePair.Value;
					break;
				}
			}
			if (taskAchievementNode == null)
			{
				this.OnRefreshUI();
				return;
			}
			taskAchievementNode.PlayScale(Vector3.one, Vector3.zero, delegate
			{
				this.OnRefreshUI();
				TaskAchievementNode taskAchievementNode2 = null;
				foreach (KeyValuePair<int, TaskAchievementNode> keyValuePair2 in this.m_nodes)
				{
					if (!(keyValuePair2.Value == null) && keyValuePair2.Value.m_data != null && keyValuePair2.Value.m_data.ID == args.m_updateID)
					{
						taskAchievementNode2 = keyValuePair2.Value;
						break;
					}
				}
				if (taskAchievementNode2 != null)
				{
					taskAchievementNode2.PlayScale(Vector3.zero, Vector3.one, null);
				}
			});
		}

		private void PlayScale()
		{
			this.m_seqPool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.m_scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		[Header("Context Setting")]
		public LoopListView2 m_scroll;

		private SequencePool m_seqPool = new SequencePool();

		private List<TaskDataModule.AchievementData> m_datas = new List<TaskDataModule.AchievementData>();

		private Dictionary<int, TaskAchievementNode> m_nodes = new Dictionary<int, TaskAchievementNode>();

		private TaskDataModule m_taskDataModule;
	}
}

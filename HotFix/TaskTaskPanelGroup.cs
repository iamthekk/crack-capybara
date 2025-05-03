using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class TaskTaskPanelGroup : BaseTaskPanel
	{
		protected override void OnInit()
		{
			this.m_taskDataModule = GameApp.Data.GetDataModule(DataName.TaskDataModule);
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.m_weeklyActiveGroup.Init();
			this.m_dailyActiveGroup.Init();
			this.m_scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.m_flyCtrl.Init();
			this.m_flyCtrl.m_onFinished = new Action<Transform>(this.OnFinishedFlyCrtl);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TaskDataModule_RefreshTaskData, new HandlerEvent(this.OnEventRefreshTaskData));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveRewardDailyTask, new HandlerEvent(this.OnEventReceiveRewardDailyTask));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveActiveRewardAllTask, new HandlerEvent(this.OnEventRefreshTaskData));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			if (this.m_isPlaying)
			{
				this.m_time += deltaTime;
				if (this.m_time >= this.m_duration)
				{
					this.OnRefreshTime();
					this.m_time = 0f;
				}
			}
			this.m_dailyActiveGroup.OnUpdate(deltaTime, unscaledDeltaTime);
			this.m_weeklyActiveGroup.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_RefreshTaskData, new HandlerEvent(this.OnEventRefreshTaskData));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveRewardDailyTask, new HandlerEvent(this.OnEventReceiveRewardDailyTask));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveActiveRewardAllTask, new HandlerEvent(this.OnEventRefreshTaskData));
			this.m_seqPool.Clear(false);
			this.m_weeklyActiveGroup.DeInit();
			this.m_dailyActiveGroup.DeInit();
			this.m_flyCtrl.m_onFinished = null;
			this.m_flyCtrl.DeInit();
			this.m_taskDataModule = null;
			this.m_loginDataModule = null;
		}

		public override void OnShow()
		{
			this.m_scroll.MovePanelToItemIndex(0, 0f);
			this.OnRefreshUI();
			this.PlayScale();
		}

		public override void OnHide()
		{
			this.m_seqPool.Clear(false);
			if (this.m_flyCtrl != null)
			{
				this.m_flyCtrl.Clear();
			}
		}

		public override void OnRefresh()
		{
			this.OnRefreshUI();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index > this.m_dailyDatas.Count)
			{
				return null;
			}
			if (index == 0)
			{
				return listView.NewListViewItem("Space");
			}
			TaskDataModule.TaskDailyData taskDailyData = this.m_dailyDatas[index - 1];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("Node");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			TaskTaskNode component;
			this.m_nodes.TryGetValue(instanceID, out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<TaskTaskNode>();
				component.Init();
				this.m_nodes[instanceID] = component;
			}
			component.OnRefresh(taskDailyData);
			return loopListViewItem;
		}

		private void OnRefreshUI()
		{
			this.m_weeklyActiveGroup.OnRefreshUI();
			this.m_dailyActiveGroup.OnRefreshUI();
			this.m_dailyDatas = this.m_taskDataModule.DailyDatas;
			this.m_scroll.SetListItemCount((this.m_dailyDatas.Count == 0) ? 0 : (this.m_dailyDatas.Count + 1), true);
			this.m_scroll.RefreshAllShownItem();
			this.OnRefreshTime();
			this.m_isPlaying = true;
			this.m_time = 0f;
		}

		private void OnRefreshTime()
		{
			if (this.m_timeTxt == null)
			{
				return;
			}
			long num = Utility.Math.Max(this.m_taskDataModule.DailyTaskResetTime - this.m_loginDataModule.ServerUTC, 0L);
			if (num <= 0L)
			{
				num = 0L;
				this.m_isPlaying = false;
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TaskDataModule_LoadTaskData, null);
			}
			this.m_timeTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("1254", new object[] { Utility.Math.GetTime3String(num) });
		}

		private void OnEventRefreshTaskData(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshUI();
		}

		private void OnEventReceiveRewardDailyTask(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsTaskDataReceiveRewardDailyTask eventArgsTaskDataReceiveRewardDailyTask = eventargs as EventArgsTaskDataReceiveRewardDailyTask;
			if (eventArgsTaskDataReceiveRewardDailyTask == null)
			{
				return;
			}
			int num = Mathf.Max(this.m_taskDataModule.DailyActive - eventArgsTaskDataReceiveRewardDailyTask.m_lastDailyActive, this.m_taskDataModule.WeeklyActive - eventArgsTaskDataReceiveRewardDailyTask.m_lastWeeklyActive);
			if (num >= 30)
			{
				num = 30;
			}
			if (this.m_flyCtrl != null)
			{
				this.m_flyCtrl.Fly(num);
			}
			float num2 = ((this.m_dailyActiveGroup != null) ? this.m_dailyActiveGroup.GetCurrentActive() : 0f);
			float num3 = ((this.m_weeklyActiveGroup != null) ? this.m_weeklyActiveGroup.GetCurrentActive() : 0f);
			this.OnRefreshUI();
			if (eventArgsTaskDataReceiveRewardDailyTask.m_lastDailyActive != this.m_taskDataModule.DailyActive)
			{
				float num4 = (float)this.m_taskDataModule.DailyActive * 1f / (float)this.m_taskDataModule.DailyMaxActive;
				if (this.m_dailyActiveGroup != null)
				{
					this.m_dailyActiveGroup.PlayProgress(num2, (float)this.m_taskDataModule.DailyActive);
				}
			}
			if (eventArgsTaskDataReceiveRewardDailyTask.m_lastWeeklyActive != this.m_taskDataModule.WeeklyActive)
			{
				float num5 = (float)this.m_taskDataModule.WeeklyActive * 1f / (float)this.m_taskDataModule.WeeklyMaxActive;
				if (this.m_weeklyActiveGroup != null)
				{
					this.m_weeklyActiveGroup.PlayProgress(num3, (float)this.m_taskDataModule.WeeklyActive);
				}
			}
		}

		private void PlayScale()
		{
			this.m_seqPool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.m_scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		private void OnFinishedFlyCrtl(Transform target)
		{
			if (target == null)
			{
				return;
			}
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(target, Vector3.one * 1.3f, 0.1f), 1));
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(target, Vector3.one, 0.1f), 1));
			if (this.m_weeklyActiveGroup.m_icon == target)
			{
				this.m_weeklyActiveGroup.PlayEffect();
			}
			if (this.m_dailyActiveGroup.m_icon == target)
			{
				this.m_dailyActiveGroup.PlayEffect();
			}
		}

		[GameTestMethod("飞行", "", "", 0)]
		private static void Run()
		{
			GameApp.View.GetViewModule(ViewName.TaskViewModule).m_taskGroup.m_flyCtrl.Fly(10);
		}

		[Header("Top Setting")]
		public TaskTaskWeeklyActiveGroup m_weeklyActiveGroup;

		public TaskTaskDailyActiveGroup m_dailyActiveGroup;

		[Header("Center Setting")]
		public CustomText m_timeTxt;

		[Header("Context Setting")]
		public LoopListView2 m_scroll;

		private SequencePool m_seqPool = new SequencePool();

		private List<TaskDataModule.TaskDailyData> m_dailyDatas = new List<TaskDataModule.TaskDailyData>();

		private Dictionary<int, TaskTaskNode> m_nodes = new Dictionary<int, TaskTaskNode>();

		private TaskDataModule m_taskDataModule;

		private LoginDataModule m_loginDataModule;

		[SerializeField]
		[Label]
		private float m_time;

		[SerializeField]
		[Label]
		private float m_duration = 1f;

		[SerializeField]
		[Label]
		private bool m_isPlaying;

		public UIFlyCtrl m_flyCtrl;
	}
}

using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class TaskViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_tabDatas.Clear();
			this.m_tabDatas.Add(new TaskViewModule.TabData
			{
				m_bt = this.m_taskBt,
				m_panel = this.m_taskGroup
			});
			this.m_tabDatas.Add(new TaskViewModule.TabData
			{
				m_bt = this.m_achievementBt,
				m_panel = this.m_achievementGroup
			});
			this.m_taskGroup.Init();
			this.m_achievementGroup.Init();
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as TaskViewModule.OpenData;
			this.m_popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.m_taskBt.onClick.AddListener(new UnityAction(this.OnClickTaskBt));
			this.m_achievementBt.onClick.AddListener(new UnityAction(this.OnClickAchievementBt));
			this.OnRefreshRedPoint();
			this.OnSelectIndex(0);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.m_taskGroup.OnUpdate(deltaTime, unscaledDeltaTime);
			this.m_achievementGroup.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
			this.m_openData = null;
			this.m_selectIndex = -1;
			this.m_popCommon.OnClick = null;
			this.m_taskBt.onClick.RemoveListener(new UnityAction(this.OnClickTaskBt));
			this.m_achievementBt.onClick.RemoveListener(new UnityAction(this.OnClickAchievementBt));
		}

		public override void OnDelete()
		{
			this.m_taskGroup.DeInit();
			this.m_achievementGroup.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TaskDataModule_RefreshTaskData, new HandlerEvent(this.OnEventRefreshRedPoint));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveRewardDailyTask, new HandlerEvent(this.OnEventRefreshRedPoint));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveActiveRewardAllTask, new HandlerEvent(this.OnEventRefreshRedPoint));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveAchievementRewardTask, new HandlerEvent(this.OnEventRefreshRedPoint));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Task_Refresh, new HandlerEvent(this.OnEventUITaskRefresh));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_RefreshTaskData, new HandlerEvent(this.OnEventRefreshRedPoint));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveRewardDailyTask, new HandlerEvent(this.OnEventRefreshRedPoint));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveActiveRewardAllTask, new HandlerEvent(this.OnEventRefreshRedPoint));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveAchievementRewardTask, new HandlerEvent(this.OnEventRefreshRedPoint));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Task_Refresh, new HandlerEvent(this.OnEventUITaskRefresh));
		}

		private void OnSelectIndex(int index)
		{
			if (this.m_selectIndex == index)
			{
				return;
			}
			if (this.m_selectIndex != -1)
			{
				TaskViewModule.TabData tabData = this.m_tabDatas[this.m_selectIndex];
				tabData.m_bt.SetSelect(false);
				tabData.m_panel.OnHide();
				tabData.m_panel.gameObject.SetActive(false);
			}
			if (index >= 0)
			{
				for (int i = 0; i < this.m_tabDatas.Count; i++)
				{
					TaskViewModule.TabData tabData2 = this.m_tabDatas[i];
					if (tabData2 != null)
					{
						if (index != i)
						{
							tabData2.m_bt.SetSelect(false);
							tabData2.m_panel.SetActive(false);
						}
						else
						{
							tabData2.m_bt.SetSelect(true);
							tabData2.m_panel.OnShow();
							tabData2.m_panel.gameObject.SetActive(true);
						}
					}
				}
			}
			this.m_selectIndex = index;
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseBt();
			}
		}

		private void OnClickCloseBt()
		{
			if (this.m_openData != null)
			{
				MoreExtensionViewModule.TryBackOpenView(this.m_openData.srcViewName);
			}
			GameApp.View.CloseView(ViewName.TaskViewModule, null);
		}

		private void OnClickTaskBt()
		{
			this.OnSelectIndex(0);
		}

		private void OnClickAchievementBt()
		{
			this.OnSelectIndex(1);
		}

		private void OnEventUITaskRefresh(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshRedPoint();
			for (int i = 0; i < this.m_tabDatas.Count; i++)
			{
				TaskViewModule.TabData tabData = this.m_tabDatas[i];
				if (tabData != null && tabData.m_panel.gameObject.activeSelf)
				{
					tabData.m_panel.OnRefresh();
				}
			}
		}

		private void OnEventRefreshRedPoint(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshRedPoint();
		}

		private void OnRefreshRedPoint()
		{
			TaskDataModule dataModule = GameApp.Data.GetDataModule(DataName.TaskDataModule);
			bool isCanReceiveForTaskTask = dataModule.GetIsCanReceiveForTaskTask();
			this.m_taskRedPoint.gameObject.SetActive(isCanReceiveForTaskTask);
			bool isCanReceiveForAchievement = dataModule.GetIsCanReceiveForAchievement();
			this.m_achievementRedPoint.gameObject.SetActive(isCanReceiveForAchievement);
		}

		public UIPopCommon m_popCommon;

		public CustomChooseButton m_taskBt;

		public RedNodeOneCtrl m_taskRedPoint;

		public CustomChooseButton m_achievementBt;

		public RedNodeOneCtrl m_achievementRedPoint;

		public TaskTaskPanelGroup m_taskGroup;

		public TaskAchievementPanelGroup m_achievementGroup;

		private TaskViewModule.OpenData m_openData;

		[HideInInspector]
		public int m_selectIndex = -1;

		public List<TaskViewModule.TabData> m_tabDatas = new List<TaskViewModule.TabData>();

		public class OpenData
		{
			public ViewName srcViewName;

			public Action onCloseCallback;
		}

		public class TabData
		{
			public CustomChooseButton m_bt;

			public BaseTaskPanel m_panel;
		}
	}
}

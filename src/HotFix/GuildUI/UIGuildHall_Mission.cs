using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildHall_Mission : UIGuildHall_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.missionRTF = base.gameObject.transform as RectTransform;
			this.missionHeight = (this.missionItemObj.transform as RectTransform).sizeDelta.y;
			this.missionItemObj.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
		}

		protected override void GuildUI_OnShow()
		{
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.RefreshMissionTime));
			DelayCall.Instance.CallLoop(1000, new DelayCall.CallAction(this.RefreshMissionTime));
		}

		protected override void GuildUI_OnClose()
		{
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.RefreshMissionTime));
		}

		public override void OnRefreshUI()
		{
			if (GuildSDKManager.Instance.GuildInfo.GuildData == null)
			{
				return;
			}
			this.ShowMissions();
		}

		private void ShowMissions()
		{
			this.RefreshMissionTime();
			int taskRefreshCount = base.SDK.GuildTask.RefreshData.TaskRefreshCount;
			int taskRefreshMaxCount = base.SDK.GuildTask.RefreshData.TaskRefreshMaxCount;
			this.textMissionRefreshCount.text = GuildProxy.Language.GetInfoByID2("400259", taskRefreshCount, taskRefreshMaxCount);
			List<GuildTaskData> sortTaskList = this.GetSortTaskList();
			for (int i = 0; i < this.missionItemList.Count; i++)
			{
				this.missionItemList[i].gameObject.SetActiveSafe(false);
			}
			float num = 0f;
			num += this.missionSpacing;
			for (int j = 0; j < sortTaskList.Count; j++)
			{
				UIGuildHallMissionItem uiguildHallMissionItem;
				if (j < this.missionItemList.Count)
				{
					uiguildHallMissionItem = this.missionItemList[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.missionItemObj);
					gameObject.SetParentNormal(this.missionLayout.gameObject, false);
					uiguildHallMissionItem = gameObject.GetComponent<UIGuildHallMissionItem>();
					uiguildHallMissionItem.Init();
					this.missionItemList.Add(uiguildHallMissionItem);
				}
				Vector2 vector;
				vector..ctor(0f, -num);
				uiguildHallMissionItem.SetMissionPosition(vector);
				uiguildHallMissionItem.gameObject.SetActiveSafe(true);
				uiguildHallMissionItem.Refresh(sortTaskList[j]);
				num += this.missionHeight;
				num += this.missionSpacing;
			}
			if (sortTaskList.Count > 2)
			{
				num += 5f;
			}
			Vector2 sizeDelta = this.missionRTF.sizeDelta;
			sizeDelta.y = num + 155f;
			this.missionRTF.sizeDelta = sizeDelta;
		}

		private void RefreshMissionTime()
		{
			long num = (long)(base.SDK.GetCustomRefreshTime() - (ulong)GuildProxy.Net.ServerTime());
			num = ((num > 0L) ? num : 0L);
			string time = Singleton<LanguageManager>.Instance.GetTime(num);
			this.textMissionRefreshTime.text = GuildProxy.Language.GetInfoByID1("400185", time);
		}

		private List<GuildTaskData> GetSortTaskList()
		{
			List<GuildTaskData> list = new List<GuildTaskData>();
			List<GuildTaskData> list2 = new List<GuildTaskData>();
			List<GuildTaskData> list3 = new List<GuildTaskData>();
			List<GuildTaskData> taskList = base.SDK.GuildTask.TaskList;
			for (int i = 0; i < taskList.Count; i++)
			{
				if (taskList[i].taskState == GuildTaskData.GuildTaskState.Undone)
				{
					list2.Add(taskList[i]);
				}
				else if (taskList[i].taskState == GuildTaskData.GuildTaskState.AllFinish)
				{
					list3.Add(taskList[i]);
				}
				else
				{
					list.Add(taskList[i]);
				}
			}
			list.AddRange(list2);
			list.AddRange(list3);
			return list;
		}

		public RectTransform missionRTF;

		public GameObject missionItemObj;

		public CustomText textMissionRefreshTime;

		public RectTransform missionLayout;

		public CustomText textMissionRefreshCount;

		[Label]
		public float missionHeight = 200f;

		[Label]
		public float missionSpacing = 10f;

		private List<UIGuildHallMissionItem> missionItemList = new List<UIGuildHallMissionItem>();
	}
}

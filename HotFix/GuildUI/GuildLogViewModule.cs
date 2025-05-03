using System;
using System.Collections.Generic;
using Framework.Logic.UI;
using Proto.Guild;
using SuperScrollView;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildLogViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.scrollView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
		}

		protected override void OnViewOpen(object data)
		{
			this.scrollView.gameObject.SetActive(false);
			this.ReqGuidLog();
		}

		private void ReqGuidLog()
		{
			GuildProxy.UI.SetNetShow(true);
			GuildNetUtil.Guild.DoRequest_GuildLog(delegate(bool isSuccess, GuildLogResponse resp)
			{
				GuildProxy.UI.SetNetShow(false);
				if (isSuccess)
				{
					this.mDataList.Clear();
					for (int i = 0; i < resp.Logs.Count; i++)
					{
						this.mDataList.Add(new GuildLogViewModule.GuildLogData(resp.Logs[i], this.mFirstTime.Ticks));
					}
					this.scrollView.gameObject.SetActive(true);
					this.scrollView.SetListItemCount(this.mDataList.Count, true);
					this.scrollView.RefreshAllShowItems();
				}
			});
		}

		private void OnClickClose()
		{
			GuildProxy.UI.CloseGuildLog();
		}

		protected override void OnViewClose()
		{
		}

		protected override void OnViewDelete()
		{
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			GuildLogViewModule.GuildLogData guildLogData = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = view.NewListViewItem("EventLogItem");
			GuildLogItem component = loopListViewItem.GetComponent<GuildLogItem>();
			if (index == 0)
			{
				component.RefreshData(guildLogData, this.mFirstTime);
			}
			else
			{
				component.RefreshData(guildLogData, this.mDataList[index - 1].time);
			}
			component.SetActive(true);
			return loopListViewItem;
		}

		public LoopListView2 scrollView;

		public CustomButton buttonClose;

		private IList<GuildLogViewModule.GuildLogData> mDataList = new List<GuildLogViewModule.GuildLogData>();

		private DateTime mFirstTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private List<GuildLogItem> itemList = new List<GuildLogItem>();

		public struct GuildLogData
		{
			public GuildLogData(GuildLogDto dto, long startTime)
			{
				this.logType = dto.LogType;
				this.time = new DateTime((long)(dto.TimeStamp * 1000UL * 10000UL + (ulong)startTime), DateTimeKind.Utc).ToLocalTime();
				this.param = dto.LogParam;
				this.serverName = dto.ServerName;
			}

			public int logType;

			public DateTime time;

			public IList<string> param;

			public string serverName;
		}
	}
}

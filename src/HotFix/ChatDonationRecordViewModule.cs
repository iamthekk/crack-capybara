using System;
using System.Collections.Generic;
using Dxx.Chat;
using Dxx.Guild;
using HotFix.ChatUI.ChatDonationRecordUI;
using Proto.Guild;
using SuperScrollView;

namespace HotFix
{
	public class ChatDonationRecordViewModule : ChatProxy.ChatUIBase
	{
		protected override void OnViewCreate()
		{
			this.LoopScroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
		}

		protected override void OnViewOpen(object data)
		{
			this.RefreshUI();
			this.OpenLoadRecord();
		}

		protected override void OnViewClose()
		{
		}

		protected override void OnViewDelete()
		{
			this.uiPopCommon.OnClick = null;
			foreach (KeyValuePair<int, ChatDonationRecordCell> keyValuePair in this.mUIItemDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUIItemDic.Clear();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			GuildDonationRecord guildDonationRecord = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = view.NewListViewItem("ChatDonationRecord");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			ChatDonationRecordCell chatDonationRecordCell = this.TryGetUI(instanceID);
			if (chatDonationRecordCell == null)
			{
				chatDonationRecordCell = this.TryAddUI(instanceID, loopListViewItem, loopListViewItem.GetComponent<ChatDonationRecordCell>());
			}
			chatDonationRecordCell.SetData(guildDonationRecord);
			chatDonationRecordCell.RefreshUI();
			return loopListViewItem;
		}

		private ChatDonationRecordCell TryGetUI(int key)
		{
			ChatDonationRecordCell chatDonationRecordCell;
			if (this.mUIItemDic.TryGetValue(key, out chatDonationRecordCell))
			{
				return chatDonationRecordCell;
			}
			return null;
		}

		private ChatDonationRecordCell TryAddUI(int key, LoopListViewItem2 loopitem, ChatDonationRecordCell ui)
		{
			ui.Init();
			ChatDonationRecordCell chatDonationRecordCell;
			if (this.mUIItemDic.TryGetValue(key, out chatDonationRecordCell))
			{
				if (chatDonationRecordCell == null)
				{
					chatDonationRecordCell = ui;
					this.mUIItemDic[key] = ui;
				}
				return ui;
			}
			this.mUIItemDic.Add(key, ui);
			return ui;
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickThisView();
			}
		}

		private void OnClickThisView()
		{
			ChatProxy.UI.CloseChatDonationRecordView();
		}

		public void RefreshUI()
		{
		}

		private void OpenLoadRecord()
		{
			this.mDataList.Clear();
			ChatProxy.Net.GetGuildDonationGetOperationRecords(delegate(bool result, GuildDonationGetOperationRecordsResponse resp)
			{
				this.RefreshChatDonationList(resp.Records);
			});
		}

		private void RefreshChatDonationList(IList<string> records)
		{
			this.mDataList.Clear();
			if (records != null)
			{
				for (int i = 0; i < records.Count; i++)
				{
					GuildDonationRecord guildDonationRecord = ChatProxy.JSON.ToObject<GuildDonationRecord>(records[i]);
					if (guildDonationRecord != null)
					{
						this.mDataList.Add(guildDonationRecord);
					}
				}
			}
			this.LoopScroll.SetListItemCount(this.mDataList.Count, true);
		}

		public LoopListView2 LoopScroll;

		public UIPopCommon uiPopCommon;

		private List<GuildDonationRecord> mDataList = new List<GuildDonationRecord>();

		private Dictionary<int, ChatDonationRecordCell> mUIItemDic = new Dictionary<int, ChatDonationRecordCell>();
	}
}

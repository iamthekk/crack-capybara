using System;
using System.Collections.Generic;
using Dxx.Chat;
using Dxx.Guild;
using Framework.Logic.UI;
using HotFix.ChatUI.ChatDonationUI;
using Proto.Guild;
using SuperScrollView;

namespace HotFix
{
	public class ChatDonationViewModule : ChatProxy.ChatUIBase
	{
		protected override void OnViewCreate()
		{
			this.LoopGridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.Text_WeekCount.text = "";
			this.Button_Record.m_onClick = new Action(this.OnOpenRecordUI);
			this.Button_Donation.m_onClick = new Action(this.OnOpenDonationUI);
		}

		protected override void OnViewOpen(object data)
		{
			this.RefreshUI();
			this.OpenLoadRecord();
			this.CheckRecvDonation();
		}

		protected override void OnViewClose()
		{
		}

		protected override void OnViewDelete()
		{
			this.uiPopCommon.OnClick = null;
			if (this.Button_Record != null)
			{
				this.Button_Record.m_onClick = null;
			}
			if (this.Button_Donation != null)
			{
				this.Button_Donation.m_onClick = null;
			}
			foreach (KeyValuePair<int, ChatDonationCell> keyValuePair in this.mUIItemDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUIItemDic.Clear();
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			LoopGridViewItem loopGridViewItem = null;
			ChatData chatData = this.mDataList[index];
			ChatDonationCell chatDonationCell = null;
			string text;
			if (chatData.IsMyChat)
			{
				text = "Me";
			}
			else
			{
				text = "Other";
			}
			if (chatData.ShowKind == ChatShowKind.GuildDonation)
			{
				loopGridViewItem = view.NewListViewItem("Chat_Cell_DonationRecord_" + text);
				int instanceID = loopGridViewItem.gameObject.GetInstanceID();
				chatDonationCell = this.TryGetUI(instanceID);
				if (chatDonationCell == null)
				{
					if (chatData.IsMyChat)
					{
						chatDonationCell = this.TryAddUI(instanceID, loopGridViewItem, loopGridViewItem.GetComponent<ChatDonationCellMe>());
					}
					else
					{
						chatDonationCell = this.TryAddUI(instanceID, loopGridViewItem, loopGridViewItem.GetComponent<ChatDonationCellOther>());
					}
				}
			}
			chatDonationCell.SetData(chatData);
			chatDonationCell.RefreshUI();
			return loopGridViewItem;
		}

		private ChatDonationCell TryGetUI(int key)
		{
			ChatDonationCell chatDonationCell;
			if (this.mUIItemDic.TryGetValue(key, out chatDonationCell))
			{
				return chatDonationCell;
			}
			return null;
		}

		private ChatDonationCell TryAddUI(int key, LoopGridViewItem loopitem, ChatDonationCell ui)
		{
			ui.Init();
			ChatDonationCell chatDonationCell;
			if (this.mUIItemDic.TryGetValue(key, out chatDonationCell))
			{
				if (chatDonationCell == null)
				{
					chatDonationCell = ui;
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
			ChatProxy.UI.CloseChatDonationView();
		}

		private void OnOpenRecordUI()
		{
			ChatProxy.UI.OpenChatDonationRecordView(null);
		}

		private void OnOpenDonationUI()
		{
			ChatProxy.UI.OpenChatDonationItemView(null);
			this.OnClickThisView();
		}

		public void RefreshUI()
		{
			GuildDonationInfo donationInfo = ChatProxy.GuildChat.GuildSDK().GuildDonation.DonationInfo;
			int donationItemCount = donationInfo.DonationItemCount;
			int donationMaxItemCount = donationInfo.DonationMaxItemCount;
			this.Text_WeekCount.text = string.Format("{0} / {1}", donationItemCount, donationMaxItemCount);
		}

		private void OpenLoadRecord()
		{
			this.mServerDataPage = 1;
			this.mDataList.Clear();
			ChatProxy.Net.GetGuildDonationGetRecords(0UL, (uint)this.mServerDataPage, delegate(bool result, GuildDonationGetRecordsResponse resp)
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
					string text = records[i];
					ChatData chatData = ChatProxy.JSON.ToObject<ChatData>(text);
					if (chatData != null)
					{
						chatData.CalcData();
						chatData.ShowKind = ChatShowKind.GuildDonation;
						chatData.donation = ChatProxy.JSON.ToObject<ChatDonation>(text);
						if (chatData.donation != null)
						{
							this.mDataList.Add(chatData);
						}
					}
				}
			}
			this.LoopGridView.SetListItemCount(this.mDataList.Count, true);
		}

		private void CheckRecvDonation()
		{
			ChatData myDonationMessage = ChatProxy.GuildChat.GetMyDonationMessage();
			if (myDonationMessage == null || myDonationMessage.donation == null)
			{
				return;
			}
			if (myDonationMessage.donation.receiveCnt < myDonationMessage.donation.count)
			{
				throw new NotImplementedException();
			}
		}

		public LoopGridView LoopGridView;

		public UIPopCommon uiPopCommon;

		public CustomText Text_WeekCount;

		public CustomButton Button_Record;

		public CustomButton Button_Donation;

		private int mServerDataPage = 1;

		private List<ChatData> mDataList = new List<ChatData>();

		private Dictionary<int, ChatDonationCell> mUIItemDic = new Dictionary<int, ChatDonationCell>();
	}
}

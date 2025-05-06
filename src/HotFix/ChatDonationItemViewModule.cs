using System;
using System.Collections.Generic;
using Dxx.Chat;
using HotFix.ChatUI.ChatDonationItemUI;
using SuperScrollView;

namespace HotFix
{
	public class ChatDonationItemViewModule : ChatProxy.ChatUIBase
	{
		protected override void OnViewCreate()
		{
			this.LoopGridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
		}

		protected override void OnViewOpen(object data)
		{
			this.ResetDataList();
			this.LoopGridView.SetListItemCount(this.mDataList.Count, true);
		}

		protected override void OnViewClose()
		{
		}

		protected override void OnViewDelete()
		{
			this.uiPopCommon.OnClick = null;
			foreach (KeyValuePair<int, ChatDonationItemCell> keyValuePair in this.mUIItemDic)
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
			ChatDonationItemData chatDonationItemData = this.mDataList[index];
			LoopGridViewItem loopGridViewItem = this.LoopGridView.NewListViewItem("DonationItem");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			ChatDonationItemCell chatDonationItemCell = this.TryGetUI(instanceID);
			if (chatDonationItemCell == null)
			{
				chatDonationItemCell = this.TryAddUI(instanceID, loopGridViewItem, loopGridViewItem.GetComponent<ChatDonationItemCell>());
			}
			chatDonationItemCell.SetData(chatDonationItemData);
			chatDonationItemCell.RefreshUI();
			return loopGridViewItem;
		}

		private ChatDonationItemCell TryGetUI(int key)
		{
			ChatDonationItemCell chatDonationItemCell;
			if (this.mUIItemDic.TryGetValue(key, out chatDonationItemCell))
			{
				return chatDonationItemCell;
			}
			return null;
		}

		private ChatDonationItemCell TryAddUI(int key, LoopGridViewItem loopitem, ChatDonationItemCell ui)
		{
			ui.Init();
			ChatDonationItemCell chatDonationItemCell;
			if (this.mUIItemDic.TryGetValue(key, out chatDonationItemCell))
			{
				if (chatDonationItemCell == null)
				{
					chatDonationItemCell = ui;
					this.mUIItemDic[key] = ui;
				}
				return ui;
			}
			this.mUIItemDic.Add(key, ui);
			return ui;
		}

		private void ResetDataList()
		{
			this.mDataList = ChatProxy.UI.BuildChatDonationItemDataList();
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
			ChatProxy.UI.CloseChatDonationItemView();
		}

		public LoopGridView LoopGridView;

		public UIPopCommon uiPopCommon;

		private List<ChatDonationItemData> mDataList = new List<ChatDonationItemData>();

		private Dictionary<int, ChatDonationItemCell> mUIItemDic = new Dictionary<int, ChatDonationItemCell>();
	}
}

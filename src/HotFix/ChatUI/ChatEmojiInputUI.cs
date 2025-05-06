using System;
using System.Collections.Generic;
using Dxx.Chat;
using Framework.Logic.AttributeExpansion;
using HotFix.ChatUI.ChatItemUI;
using LocalModels.Bean;
using SuperScrollView;

namespace HotFix.ChatUI
{
	public class ChatEmojiInputUI : ChatProxy.ChatProxy_BaseBehaviour
	{
		protected override void ChatUI_OnInit()
		{
			this.GridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			this.EmojiGroupGroup.Init();
			this.EmojiGroupGroup.OnSwitch = new Action(this.OnSwitchEmojiGroup);
			this.EmojiGroupGroup.InitButtons();
		}

		protected override void ChatUI_OnUnInit()
		{
			this.EmojiGroupGroup.DeInit();
			foreach (KeyValuePair<int, ChatEmojiUI> keyValuePair in this.mUIItemDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUIItemDic.Clear();
		}

		public void OnShow()
		{
		}

		public void OnClose()
		{
		}

		private void OnSwitchEmojiGroup()
		{
			this.InternalSwitchGroup(this.EmojiGroupGroup.CurSelectEmojiGroup);
		}

		public void SwitchGroup(int group)
		{
			this.EmojiGroupGroup.SwitchByKey(group);
		}

		private void InternalSwitchGroup(int group)
		{
			if (this.CurGroup != group || this.mDataList.Count <= 0)
			{
				this.CurGroup = group;
				this.mDataList.Clear();
				foreach (Emoji_Emoji emoji_Emoji in ChatProxy.Table.GetAllEmojiTab())
				{
					if (emoji_Emoji.group == group)
					{
						ChatEmojiData chatEmojiData = new ChatEmojiData();
						chatEmojiData.SetTable(emoji_Emoji);
						this.mDataList.Add(chatEmojiData);
					}
				}
			}
			this.GridView.SetListItemCount(this.mDataList.Count, true);
			this.GridView.RefreshAllShownItem();
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			ChatEmojiData chatEmojiData = this.mDataList[index];
			LoopGridViewItem loopGridViewItem = this.GridView.NewListViewItem("EmojiItem");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			ChatEmojiUI chatEmojiUI = this.TryGetUI(instanceID);
			if (chatEmojiUI == null)
			{
				chatEmojiUI = this.TryAddUI(instanceID, loopGridViewItem, loopGridViewItem.GetComponent<ChatEmojiUI>());
			}
			chatEmojiUI.SetData(chatEmojiData);
			chatEmojiUI.RefreshUI();
			chatEmojiUI.Resize();
			return loopGridViewItem;
		}

		private ChatEmojiUI TryGetUI(int key)
		{
			ChatEmojiUI chatEmojiUI;
			if (this.mUIItemDic.TryGetValue(key, out chatEmojiUI))
			{
				return chatEmojiUI;
			}
			return null;
		}

		private ChatEmojiUI TryAddUI(int key, LoopGridViewItem loopitem, ChatEmojiUI ui)
		{
			ui.Init();
			ui.OnClick = new Action<ChatEmojiUI>(this.OnClickEmoji);
			ChatEmojiUI chatEmojiUI;
			if (this.mUIItemDic.TryGetValue(key, out chatEmojiUI))
			{
				if (chatEmojiUI == null)
				{
					chatEmojiUI = ui;
					this.mUIItemDic[key] = ui;
				}
				return ui;
			}
			this.mUIItemDic.Add(key, ui);
			return ui;
		}

		private void OnClickEmoji(ChatEmojiUI ui)
		{
			if (ui == null || ui.Data == null)
			{
				return;
			}
			Action<ChatEmojiData> onSendEmoji = this.OnSendEmoji;
			if (onSendEmoji == null)
			{
				return;
			}
			onSendEmoji(ui.Data);
		}

		public LoopGridView GridView;

		public ChatEmojiGroupButtonBox EmojiGroupGroup;

		[Label]
		public int CurGroup = 1;

		public Action<ChatEmojiData> OnSendEmoji;

		private List<ChatEmojiData> mDataList = new List<ChatEmojiData>();

		private Dictionary<int, ChatEmojiUI> mUIItemDic = new Dictionary<int, ChatEmojiUI>();
	}
}

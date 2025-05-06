using System;
using System.Collections.Generic;
using Dxx.Chat;
using Framework;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.ChatUI
{
	public class ChatEmojiGroupButtonBox : ChatProxy.ChatProxy_BaseBehaviour
	{
		public ChatEmojiGroupButton CurrentSelect
		{
			get
			{
				return this.mCurrentChoose;
			}
		}

		public string CurrentSelectName
		{
			get
			{
				if (this.mCurrentChoose == null)
				{
					return "";
				}
				return this.mCurrentChoose.ButtonName;
			}
		}

		public int CurSelectIndex
		{
			get
			{
				if (this.mCurrentChoose == null)
				{
					return -1;
				}
				return this.mButtons.IndexOf(this.mCurrentChoose);
			}
		}

		public int CurSelectEmojiGroup
		{
			get
			{
				if (this.mCurrentChoose == null)
				{
					return -1;
				}
				return this.mCurrentChoose.GroupKey;
			}
		}

		public int Count
		{
			get
			{
				return this.mGroupDataList.Count;
			}
		}

		protected override void ChatUI_OnInit()
		{
			this.PrefabBtn.SetActive(false);
			this.PrefabBtn.transform.SetParent(base.transform);
		}

		protected override void ChatUI_OnUnInit()
		{
		}

		public void InitButtons()
		{
			this.mButtons.Clear();
			this.mCurrentChoose = null;
			List<Emoji_Emoji> allEmojiTab = ChatProxy.Table.GetAllEmojiTab();
			Dictionary<int, ChatEmojiGroupButton.EmojiGroupData> dictionary = new Dictionary<int, ChatEmojiGroupButton.EmojiGroupData>();
			for (int i = 0; i < allEmojiTab.Count; i++)
			{
				Emoji_Emoji emoji_Emoji = allEmojiTab[i];
				ChatEmojiGroupButton.EmojiGroupData emojiGroupData;
				if (emoji_Emoji != null && !dictionary.TryGetValue(emoji_Emoji.group, out emojiGroupData))
				{
					emojiGroupData = new ChatEmojiGroupButton.EmojiGroupData();
					emojiGroupData.GroupKey = emoji_Emoji.group;
					emojiGroupData.AtlasID = GameApp.Table.GetAtlasPath(emoji_Emoji.atlasId);
					emojiGroupData.Icon = emoji_Emoji.icon;
					dictionary[emoji_Emoji.group] = emojiGroupData;
					this.mGroupDataList.Add(emojiGroupData);
				}
			}
			this.ButtonRoot.DestroyChildren();
			for (int j = 0; j < this.mGroupDataList.Count; j++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.PrefabBtn, this.ButtonRoot);
				ChatEmojiGroupButton component = gameObject.GetComponent<ChatEmojiGroupButton>();
				gameObject.SetActive(true);
				component.Init();
				component.SetData(this.mGroupDataList[j]);
				component.OnClick = new Action<ChatEmojiGroupButton>(this.OnClickSwitchButton);
				this.mButtons.Add(component);
			}
		}

		public void SwitchByName(string name)
		{
			for (int i = 0; i < this.mButtons.Count; i++)
			{
				if (this.mButtons[i].ButtonName == name)
				{
					this.OnClickSwitchButton(this.mButtons[i]);
					return;
				}
			}
		}

		public void SwitchByIndex(int index)
		{
			if (index < 0 || index >= this.mButtons.Count)
			{
				return;
			}
			if (this.mButtons[index] == null)
			{
				return;
			}
			this.OnClickSwitchButton(this.mButtons[index]);
		}

		public void SwitchByKey(int key)
		{
			for (int i = 0; i < this.mButtons.Count; i++)
			{
				if (this.mButtons[i].GroupKey == key)
				{
					this.OnClickSwitchButton(this.mButtons[i]);
					return;
				}
			}
		}

		public ChatEmojiGroupButton GetButtonByName(string name)
		{
			for (int i = 0; i < this.mButtons.Count; i++)
			{
				if (this.mButtons[i].ButtonName == name)
				{
					return this.mButtons[i];
				}
			}
			return null;
		}

		public ChatEmojiGroupButton GetButtonByIndex(int index)
		{
			if (index < 0 || index >= this.mButtons.Count)
			{
				return null;
			}
			if (this.mButtons[index] == null)
			{
				return null;
			}
			return this.mButtons[index];
		}

		private void OnClickSwitchButton(ChatEmojiGroupButton btn)
		{
			if (btn == null)
			{
				return;
			}
			if (btn == this.mCurrentChoose)
			{
				return;
			}
			this.mCurrentChoose = btn;
			for (int i = 0; i < this.mButtons.Count; i++)
			{
				ChatEmojiGroupButton chatEmojiGroupButton = this.mButtons[i];
				chatEmojiGroupButton.SetChoose(chatEmojiGroupButton == this.mCurrentChoose);
			}
			Action onSwitch = this.OnSwitch;
			if (onSwitch == null)
			{
				return;
			}
			onSwitch();
		}

		public RectTransform ButtonRoot;

		public GameObject PrefabBtn;

		public Action OnSwitch;

		private List<ChatEmojiGroupButton.EmojiGroupData> mGroupDataList = new List<ChatEmojiGroupButton.EmojiGroupData>();

		private List<ChatEmojiGroupButton> mButtons = new List<ChatEmojiGroupButton>();

		private ChatEmojiGroupButton mCurrentChoose;
	}
}

using System;
using Dxx.Chat;
using Framework.Logic.UI;
using HotFix.ChatUI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class ChatMiniItem : ChatProxy.ChatProxy_BaseBehaviour
	{
		protected override void ChatUI_OnInit()
		{
		}

		protected override void ChatUI_OnUnInit()
		{
		}

		public void SetData(string groupNameKey, ChatData chatData)
		{
			this.curChatData = chatData;
			base.gameObject.SetActive(true);
			this.HideEmoji();
			string text = string.Empty;
			if (!string.IsNullOrEmpty(groupNameKey))
			{
				text = text + "<color=#F4883F>[" + Singleton<LanguageManager>.Instance.GetInfoByID(groupNameKey) + "] </color>";
			}
			text = text + "<color=#F4883F>" + chatData.GetShowNick() + ": </color>";
			switch (chatData.ShowKind)
			{
			case ChatShowKind.CustomText:
			case ChatShowKind.SystemTips:
			case ChatShowKind.MultipleSystemTips:
				text += chatData.GetShowContent();
				goto IL_00AA;
			case ChatShowKind.Emoji:
				this.ShowEmoji();
				goto IL_00AA;
			}
			base.gameObject.SetActive(false);
			IL_00AA:
			this.infoText.text = text;
			this.emojiRoot.position = this.GetLastCharWorldPos(this.infoText, this.emojiRoot.rect.width / 2f);
		}

		private void ShowEmoji()
		{
			this.HideEmoji();
			int creatEmojiID = this.curChatData.EmojiID;
			ChatEmojiPool.Instance.CreateEmoji(creatEmojiID, delegate(GameObject obj)
			{
				if (this.gameObject == null || !this.gameObject.activeSelf || this.curChatData == null || creatEmojiID != this.curChatData.EmojiID)
				{
					ChatEmojiPool.Instance.ReleaseEmoji(obj);
					return;
				}
				this.curObjEmoji = obj;
				if (this.curObjEmoji != null)
				{
					this.curObjEmoji.SetParentNormal(this.emojiRoot, false);
					RectTransform rectTransform = this.curObjEmoji.transform as RectTransform;
					if (rectTransform != null)
					{
						rectTransform.sizeDelta = this.emojiRoot.sizeDelta;
					}
				}
			});
		}

		private void HideEmoji()
		{
			if (this.curObjEmoji != null)
			{
				ChatEmojiPool.Instance.ReleaseEmoji(this.curObjEmoji);
				this.curObjEmoji = null;
			}
		}

		private Vector3 GetLastCharWorldPos(Text text, float offsetX)
		{
			Vector2 anchoredPosition = text.rectTransform.anchoredPosition;
			anchoredPosition.x = text.preferredWidth + offsetX;
			return text.rectTransform.TransformPoint(anchoredPosition);
		}

		[SerializeField]
		private CustomText infoText;

		[SerializeField]
		private RectTransform emojiRoot;

		private GameObject curObjEmoji;

		private ChatData curChatData;
	}
}

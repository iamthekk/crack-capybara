using System;
using Dxx.Chat;
using Framework.Logic.UI;
using Proto.Chat;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public class ChatItem_CustomChat : ChatItem_UserChatBase
	{
		public bool IsEmoji
		{
			get
			{
				return this.Data.ShowKind == ChatShowKind.Emoji;
			}
		}

		public float MinContentTextSizeWidth
		{
			get
			{
				return 60f;
			}
		}

		public float MaxContentSizeTextWidth
		{
			get
			{
				return (float)this.RootMaxWidth - this.ContentWidthPadding - this.mContentTextWidthPaddingX - this.mTranslateWidth;
			}
		}

		public float MinContentTextSizeHeight
		{
			get
			{
				return 60f;
			}
		}

		public string GetShowContent()
		{
			if (this.Data == null)
			{
				return "";
			}
			if (!this.ForceShowRevertContent && this.Data.TranslateLanguageKind >= 0 && !string.IsNullOrEmpty(this.Data.TranslateContent))
			{
				return this.Data.TranslateContent;
			}
			return this.Data.chatContent;
		}

		protected override void ChatUI_OnInit()
		{
			base.ChatUI_OnInit();
			this.TranslateUI.OnTranslate = new Action(this.OnTranslateContent);
			this.TranslateUI.OnRevert = new Action(this.OnTranslateRevertContent);
			this.TranslateUI.Init();
		}

		protected override void ChatUI_OnUnInit()
		{
			this.TranslateUI.DeInit();
			base.ChatUI_OnUnInit();
		}

		public override void SetData(ChatData data)
		{
			if (this.Data != null && !this.Data.IsSameMessageID(data))
			{
				this.ForceShowRevertContent = false;
			}
			this.Data = data;
		}

		public override void RefreshUIContent()
		{
			if (this.Data.IsShowTranslate())
			{
				this.mTranslateWidth = 100f;
				this.TranslateUI.SetActive(true);
				if (this.Data.IsTranslated())
				{
					if (this.ForceShowRevertContent)
					{
						this.TranslateUI.SwitchState(ChatTranslateState.Revert);
					}
					else
					{
						this.TranslateUI.SwitchState(ChatTranslateState.Translated);
					}
				}
				else
				{
					this.TranslateUI.SwitchState(ChatTranslateState.NotTranslate);
				}
			}
			else
			{
				this.mTranslateWidth = 0f;
				this.TranslateUI.SetActive(false);
			}
			if (this.IsEmoji)
			{
				this.RTFEmoji.gameObject.SetActive(true);
				this.TextContent.gameObject.SetActive(false);
				this.RefreshEmoji();
				return;
			}
			this.RTFEmoji.gameObject.SetActive(false);
			this.TextContent.gameObject.SetActive(true);
			this.TextContent.text = this.GetShowContent();
		}

		private void RefreshEmoji()
		{
			this.RefreshSizeAsEmoji();
			if (this.ObjEmoji != null)
			{
				ChatEmojiPool.Instance.ReleaseEmoji(this.ObjEmoji);
				this.ObjEmoji = null;
				this.objEmojiId = -1;
			}
			int emojiid = this.Data.EmojiID;
			ChatEmojiPool.Instance.CreateEmoji(this.Data.EmojiID, delegate(GameObject obj)
			{
				if (this.gameObject == null || !this.gameObject.activeSelf || this.Data == null || emojiid != this.Data.EmojiID)
				{
					ChatEmojiPool.Instance.ReleaseEmoji(obj);
					return;
				}
				if (this.ObjEmoji != null)
				{
					if (this.objEmojiId == this.Data.EmojiID)
					{
						ChatEmojiPool.Instance.ReleaseEmoji(obj);
						return;
					}
					ChatEmojiPool.Instance.ReleaseEmoji(this.ObjEmoji);
					this.ObjEmoji = null;
					this.objEmojiId = -1;
				}
				this.ObjEmoji = obj;
				if (this.ObjEmoji != null)
				{
					this.objEmojiId = emojiid;
					this.ObjEmoji.SetParentNormal(this.RTFEmoji, false);
					this.RefreshSizeAsEmoji();
					return;
				}
				this.objEmojiId = -1;
			});
		}

		public override void RefreshUIAsNull()
		{
			base.RefreshUIAsNull();
			this.TextContent.text = "";
		}

		public override void RefreshSize()
		{
			if (this.IsEmoji)
			{
				this.RefreshSizeAsEmoji();
				return;
			}
			this.RefreshSizeAsText();
		}

		private void RefreshSizeAsText()
		{
			Vector2 sizeDelta = base.RTFRoot.sizeDelta;
			RectTransform rectTransform = this.TextContent.rectTransform;
			float maxContentSizeTextWidth = this.MaxContentSizeTextWidth;
			float minContentTextSizeWidth = this.MinContentTextSizeWidth;
			Vector2 vector;
			vector..ctor(maxContentSizeTextWidth, minContentTextSizeWidth);
			rectTransform.sizeDelta = vector;
			this.TextContent.text = this.TextContent.text;
			float num = this.TextContent.preferredWidth;
			float num2 = this.TextContent.preferredHeight;
			if (num > maxContentSizeTextWidth)
			{
				num = maxContentSizeTextWidth;
			}
			else
			{
				num2 = minContentTextSizeWidth;
			}
			if (num < this.MinContentTextSizeWidth)
			{
				num = this.MinContentTextSizeWidth;
			}
			if (num2 < this.MinContentTextSizeHeight)
			{
				num2 = this.MinContentTextSizeHeight;
			}
			vector..ctor(num, num2);
			rectTransform.sizeDelta = vector;
			vector..ctor(num + this.mContentTextWidthPaddingX, num2 + this.mContentTextWidthPaddingY);
			this.RTFContent.sizeDelta = vector;
			vector..ctor(base.RTFRoot.sizeDelta.x, vector.y + this.ContentHeightPadding);
			if (this.ExtureHeight > 0)
			{
				vector.y += (float)this.ExtureHeight;
			}
			base.RTFRoot.sizeDelta = vector;
			this.RootSizeHeight = (int)vector.y;
			if (sizeDelta != vector)
			{
				Action<ChatItemBase> onSizeChange = this.OnSizeChange;
				if (onSizeChange == null)
				{
					return;
				}
				onSizeChange(this);
			}
		}

		private void RefreshSizeAsEmoji()
		{
			Vector2 sizeDelta = base.RTFRoot.sizeDelta;
			Vector2 vector;
			vector..ctor(this.EmojiSize, this.EmojiSize);
			if (this.ObjEmoji != null)
			{
				RectTransform rectTransform = this.ObjEmoji.transform as RectTransform;
				if (rectTransform != null)
				{
					rectTransform.sizeDelta = vector;
				}
			}
			this.RTFEmoji.sizeDelta = vector;
			vector += new Vector2(this.mContentTextWidthPaddingX, this.mContentTextWidthPaddingY);
			this.RTFContent.sizeDelta = vector;
			vector..ctor(base.RTFRoot.sizeDelta.x, vector.y + this.ContentHeightPadding);
			if (this.ExtureHeight > 0)
			{
				vector.y += (float)this.ExtureHeight;
			}
			base.RTFRoot.sizeDelta = vector;
			this.RootSizeHeight = (int)vector.y;
			if (sizeDelta != vector)
			{
				Action<ChatItemBase> onSizeChange = this.OnSizeChange;
				if (onSizeChange == null)
				{
					return;
				}
				onSizeChange(this);
			}
		}

		private void OnTranslateContent()
		{
			if (this.Data == null)
			{
				return;
			}
			long chatmsgid = this.Data.msgId;
			if (this.Data.IsTranslated())
			{
				this.ForceShowRevertContent = false;
				this.RefreshUI();
				this.RefreshSize();
				return;
			}
			this.TranslateUI.SwitchState(ChatTranslateState.Translating);
			ChatNetwork.Chat.DoRequest_ChatTranslate(this.ChatGroupType, this.ChatGroupID, this.Data, delegate(bool result, ChatTranslateResponse resp)
			{
				if (result && this.Data != null && chatmsgid == this.Data.msgId)
				{
					this.ForceShowRevertContent = false;
					this.RefreshUI();
					this.RefreshSize();
				}
			});
		}

		private void OnTranslateRevertContent()
		{
			this.ForceShowRevertContent = true;
			this.RefreshUI();
			this.RefreshSize();
		}

		public CustomText TextContent;

		public RectTransform RTFContent;

		[Header("翻译")]
		public ChatTranslateUI TranslateUI;

		[Header("表情")]
		public RectTransform RTFEmoji;

		[HideInInspector]
		public GameObject ObjEmoji;

		private int objEmojiId;

		public float EmojiSize = 160f;

		[Header("显示控制")]
		public bool ForceShowRevertContent;

		[Tooltip("最大内容文本宽度相对于Item宽度的差值")]
		public float ContentWidthPadding = 220f;

		[Tooltip("最大内容文本高度相对于Item高度的差值")]
		public float ContentHeightPadding = 70f;

		private float mContentTextWidthPaddingX = 70f;

		private float mContentTextWidthPaddingY = 45f;

		private float mTranslateWidth;
	}
}

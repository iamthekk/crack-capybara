using System;
using System.Text;
using Dxx.Chat;
using Framework;
using Framework.Logic.UI;
using Framework.Logic.UI.Chat;
using Proto.Chat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.ChatUI
{
	public class ChatInputUI : ChatProxy.ChatProxy_BaseBehaviour
	{
		public float SendInterval { get; internal set; } = 1f;

		public int MaxSendCharCount { get; internal set; } = 100;

		protected override void ChatUI_OnInit()
		{
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.InputCtrl.OnEndEdit = new Action(this.OnEndEdit);
			this.InputCtrl.OnKeyboardHeightChange = new Action<float>(this.OnKeyboardHeightChange);
			this.InputCtrl.OnInputFocus = new Action<bool>(this.OnInputFocus);
			this.ButtonSend.m_onClick = new Action(this.OnClickSendMessage);
			this.ButtonEmoji.onClick.AddListener(new UnityAction(this.OnShowEmoji));
			this.EmojiCtrl.Init();
			this.EmojiCtrl.OnSendEmoji = new Action<ChatEmojiData>(this.OnClickSendEmoji);
			this.MoreInputCtrl.Init();
			Singleton<ChatManager>.Instance.OnQuitGroup += this.InstanceOnQuitGroup;
		}

		private long GetCurTimestamp()
		{
			return this.loginDataModule.ServerUTC;
		}

		protected override void ChatUI_OnUnInit()
		{
			if (this.ButtonSend != null)
			{
				this.ButtonSend.m_onClick = new Action(this.OnClickSendMessage);
			}
			if (this.ButtonEmoji != null)
			{
				this.ButtonEmoji.onClick.RemoveListener(new UnityAction(this.OnShowEmoji));
			}
			if (this.EmojiCtrl != null)
			{
				this.EmojiCtrl.DeInit();
			}
			Singleton<ChatManager>.Instance.OnQuitGroup -= this.InstanceOnQuitGroup;
		}

		public void OnShow()
		{
			this.Input.text = "";
			this.EmojiCtrl.OnShow();
			this.MoreInputCtrl.OnShow();
			this.ShowNormal();
		}

		public void OnClose()
		{
			this.EmojiCtrl.OnClose();
			this.MoreInputCtrl.OnClose();
		}

		private void InstanceOnQuitGroup(SocketGroupType arg1, string arg2)
		{
			if (this.ChatGroup != null && this.ChatGroup.ChatGroupType == arg1 && this.ChatGroup.ChatGroupID == arg2)
			{
				this.ChatGroup = null;
			}
		}

		public void OnSwitchChannel(SocketGroupType groupType, string groupId)
		{
			if (this.ChatGroup != null && this.ChatGroup.ChatGroupType == groupType && this.ChatGroup.ChatGroupID == groupId)
			{
				return;
			}
			this.ChatGroup = Singleton<ChatManager>.Instance.GetGroup(groupType, groupId);
		}

		private string ChatContentTruncation(string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				return "";
			}
			if (content.Length * 3 <= this.MaxSendCharCount)
			{
				return content;
			}
			int num = this.MaxSendCharCount;
			Encoding @default = Encoding.Default;
			int num2 = 0;
			for (int i = 0; i < content.Length; i++)
			{
				if (@default.GetBytes(content.Substring(i, 1)).Length > 1)
				{
					num -= 3;
				}
				else
				{
					num--;
				}
				if (num == 0)
				{
					num2 = i;
					break;
				}
				if (num < 0)
				{
					break;
				}
				num2 = i;
			}
			if (num2 + 1 == content.Length)
			{
				return content;
			}
			return content.Substring(0, num2 + 1);
		}

		private int GetLanType()
		{
			return ChatProxy.Language.GetCurrentLanguage();
		}

		private void OnClickSendMessage()
		{
			if (this.ChatGroup == null)
			{
				HLog.LogError("ChatGroup is null when send chat message!");
				this.Input.text = "";
				return;
			}
			string content = this.Input.text;
			if (string.IsNullOrWhiteSpace(content) || string.IsNullOrEmpty(content))
			{
				this.Input.text = "";
				return;
			}
			bool flag;
			if (this.ChatGroup.ChatGroupType == SocketGroupType.GUILD)
			{
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Chat_Union_UnLock, true);
			}
			else
			{
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Chat_Server_UnLock, true);
			}
			if (flag)
			{
				long curTimestamp = this.GetCurTimestamp();
				if (curTimestamp - this.mStartSendTime > 5L && !this.mSendFinish)
				{
					this.mSendFinish = true;
				}
				if ((float)(curTimestamp - this.mLastSendTime) < this.SendInterval || !this.mSendFinish)
				{
					ChatProxy.Language.TipsChatSendTooFrequently();
					return;
				}
				content = this.ChatContentTruncation(content);
				this.Input.text = "";
				this.mStartSendTime = this.GetCurTimestamp();
				this.mSendFinish = false;
			}
			if (this.ChatGroup.ChatGroupType == SocketGroupType.GUILD)
			{
				if (flag)
				{
					ChatNetwork.Chat.DoRequest_SendChatGuild(content, this.GetLanType(), delegate(bool result, ChatGuildResponse resp)
					{
						this.LogSendChatResult(result, content, (resp != null) ? resp.Code : 0);
					});
					return;
				}
			}
			else if (flag)
			{
				ChatNetwork.Chat.DoRequest_SendChat(this.ChatGroup.ChatGroupType, this.ChatGroup.ChatGroupID, content, this.GetLanType(), delegate(bool result, ChatCommonResponse resp)
				{
					this.LogSendChatResult(result, content, (resp != null) ? resp.Code : 0);
				});
			}
		}

		private void OnClickSendEmoji(ChatEmojiData emoji)
		{
			this.ShowNormal();
			if (this.ChatGroup == null)
			{
				HLog.LogError("ChatGroup is null when send chat message!");
				this.Input.text = "";
				return;
			}
			string content = "";
			bool flag = false;
			if (this.ChatGroup.ChatGroupType == SocketGroupType.GUILD)
			{
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Chat_Union_UnLock, true);
			}
			else if (this.ChatGroup.ChatGroupType == SocketGroupType.Server)
			{
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Chat_Server_UnLock, true);
			}
			else if (this.ChatGroup.ChatGroupType == SocketGroupType.SCENE)
			{
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Chat_Local_UnLock, true);
			}
			if (flag)
			{
				long curTimestamp = this.GetCurTimestamp();
				if (curTimestamp - this.mStartSendTime > 5L && !this.mSendFinish)
				{
					this.mSendFinish = true;
				}
				if ((float)(curTimestamp - this.mLastSendTime) < this.SendInterval || !this.mSendFinish)
				{
					ChatProxy.Language.TipsChatSendTooFrequently();
					return;
				}
				content = emoji.MakeEmojiContent();
				this.mStartSendTime = this.GetCurTimestamp();
				this.mSendFinish = false;
			}
			if (this.ChatGroup.ChatGroupType == SocketGroupType.GUILD)
			{
				if (flag)
				{
					ChatNetwork.Chat.DoRequest_SendChatGuild(content, this.GetLanType(), delegate(bool result, ChatGuildResponse resp)
					{
						this.LogSendChatResult(result, content, (resp != null) ? resp.Code : 0);
					});
					return;
				}
			}
			else if (flag)
			{
				ChatNetwork.Chat.DoRequest_SendChat(this.ChatGroup.ChatGroupType, this.ChatGroup.ChatGroupID, content, this.GetLanType(), delegate(bool result, ChatCommonResponse resp)
				{
					this.LogSendChatResult(result, content, (resp != null) ? resp.Code : 0);
				});
			}
		}

		private void LogSendChatResult(bool result, string content, int errorCode)
		{
			this.mSendFinish = true;
			long curTimestamp = this.GetCurTimestamp();
			this.mLastSendTime = curTimestamp;
			if (!result)
			{
				HLog.LogError(string.Format("Send message {0} error !!! errorCode: {1}", content, errorCode));
			}
		}

		private void OnShowEmoji()
		{
			if (this.EmojiCtrl.IsActive())
			{
				this.OnInputFocus(true);
				this.EmojiCtrl.SetActive(false);
				return;
			}
			this.OnInputFocus(false);
			this.CloseKeyboard();
			this.EmojiCtrl.SetActive(true);
			this.EmojiCtrl.SwitchGroup(1);
			this.MoreInputCtrl.SwitchToHide();
		}

		private void OnInputFocus(bool active)
		{
			if (active)
			{
				this.EmojiCtrl.SetActive(false);
				this.mCurKeyboard = TouchScreenKeyboard.Open(this.Input.text, 9, false, false, false);
			}
			if (active)
			{
				this.MoreInputCtrl.SwitchToHide();
			}
		}

		private void CloseKeyboard()
		{
			if (this.mCurKeyboard != null)
			{
				this.mCurKeyboard.active = false;
			}
		}

		private void OnEndEdit()
		{
			if (this.Input == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.Input.text))
			{
				return;
			}
			this.OnClickSendMessage();
		}

		private void OnKeyboardHeightChange(float height)
		{
		}

		public void ShowNormal()
		{
			this.EmojiCtrl.SetActive(false);
		}

		public void OnCalcleInput()
		{
			this.EmojiCtrl.SetActive(false);
			this.OnInputFocus(false);
			this.CloseKeyboard();
			this.MoreInputCtrl.SwitchToHide();
		}

		public InputField Input;

		public ChatInputCtrl InputCtrl;

		public CustomButton ButtonSend;

		public CustomButton ButtonEmoji;

		public ChatEmojiInputUI EmojiCtrl;

		public ChatMoreInputCtrl MoreInputCtrl;

		public ChatGroupData ChatGroup;

		private bool mSendFinish = true;

		private long mStartSendTime;

		private long mLastSendTime;

		private TouchScreenKeyboard mCurKeyboard;

		private LoginDataModule loginDataModule;
	}
}

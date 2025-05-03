using System;
using System.Text.RegularExpressions;
using HotFix;
using Proto.Common;

namespace Dxx.Chat
{
	public class ChatData
	{
		public string GetShowItemName()
		{
			return "";
		}

		public bool IsMyChat { get; private set; }

		public int EmojiID { get; private set; }

		public static string MakeEmojiContent(int emojiid)
		{
			return string.Format("[#{0}]", emojiid);
		}

		public static bool GetEmoji(string content, out int emojiid)
		{
			emojiid = 0;
			return !string.IsNullOrEmpty(content) && content.Length >= 4 && content[0] == '[' && (content[0] == '#' || content[content.Length - 1] == ']') && int.TryParse(content.Substring(2, content.Length - 3), out emojiid) && ChatProxy.Table.GetEmojiTab(emojiid) != null;
		}

		public static int Sort(ChatData x, ChatData y)
		{
			return x.msgId.CompareTo(y.msgId);
		}

		public static int SortByTime(ChatData x, ChatData y)
		{
			return x.timestamp.CompareTo(y.timestamp);
		}

		public void CalcData()
		{
			this.IsMyChat = this.userId == ChatProxy.User.GetMyUserID();
			if (this.ShowKind == ChatShowKind.Undefine)
			{
				this.ShowKind = ChatShowKind.CustomText;
			}
			int num;
			if (ChatData.GetEmoji(this.chatContent, out num))
			{
				this.EmojiID = num;
				this.ShowKind = ChatShowKind.Emoji;
			}
			if (this.itemType != 0 && this.itemType == 1)
			{
				this.equipment = ChatProxy.JSON.ToObject<EquipmentDto>(this.itemProto);
				this.ShowKind = ChatShowKind.ItemShare;
			}
		}

		public string GetShowContent()
		{
			if (!string.IsNullOrEmpty(this.TranslateContent))
			{
				return this.TranslateContent;
			}
			return this.chatContent;
		}

		public string GetShowNick()
		{
			ChatShowKind showKind = this.ShowKind;
			if (showKind == ChatShowKind.SystemTips || showKind == ChatShowKind.MultipleSystemTips)
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("420006");
			}
			return ChatProxy.Language.GetNickName(this.nickName, this.userId);
		}

		public string GetShowTitle()
		{
			if (this.userPosition <= 0)
			{
				return string.Empty;
			}
			return "[" + ChatProxy.Language.GetInfoByID((this.userPosition + 400010).ToString()) + "]";
		}

		public string GetTimeShow()
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double)this.timestamp).ToLocalTime();
			DateTime now = DateTime.Now;
			if (now.Year == dateTime.Year && now.Month == dateTime.Month && now.Day == dateTime.Day)
			{
				return dateTime.ToString("t");
			}
			if (now.Year == dateTime.Year)
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID(dateTime.ToString("M")) + " " + dateTime.ToString("t");
			}
			return dateTime.ToString("U");
		}

		public bool IsSameMessageID(ChatData chatdata)
		{
			if (chatdata == null)
			{
				return false;
			}
			if (this.UIBindMsgID != 0L)
			{
				if (chatdata.UIBindMsgID != 0L)
				{
					return chatdata.UIBindMsgID == this.UIBindMsgID;
				}
				return chatdata.msgId == this.UIBindMsgID;
			}
			else
			{
				if (chatdata.UIBindMsgID != 0L)
				{
					return chatdata.UIBindMsgID == this.msgId;
				}
				return chatdata.msgId == this.msgId;
			}
		}

		public bool IsShowTranslate()
		{
			return this.ShowKind == ChatShowKind.CustomText && !string.IsNullOrEmpty(this.chatContent) && !this.IsMyChat && this.GetCurrentUseLanguageID() != this.languageId && !Regex.IsMatch(this.chatContent, "^[0-9]+$");
		}

		private int GetCurrentUseLanguageID()
		{
			return ChatProxy.Language.GetCurrentLanguage();
		}

		public bool IsTranslated()
		{
			int currentUseLanguageID = this.GetCurrentUseLanguageID();
			return !string.IsNullOrEmpty(this.TranslateContent) && currentUseLanguageID == this.TranslateLanguageKind;
		}

		public static ChatData TestCreateTimePartData(ChatData binddata, long timestamp)
		{
			if (binddata == null)
			{
				return null;
			}
			if ((float)binddata.timestamp >= (float)timestamp + ChatData.MessagePartTimeSec)
			{
				return new ChatData
				{
					UIBindMsgID = binddata.msgId,
					ShowKind = ChatShowKind.TimeShow,
					timestamp = binddata.timestamp
				};
			}
			return null;
		}

		public long msgId;

		public long msgSeq;

		public ulong userId;

		public string nickName;

		public int avatar;

		public int avatarFrame;

		public int title;

		public string chatContent;

		public int languageId;

		public long timestamp;

		public int userPosition;

		public int itemType;

		public string itemProto;

		public EquipmentDto equipment;

		public ChatDonation donation;

		public int TranslateLanguageKind = -1;

		public string TranslateContent;

		public string[] ContentList;

		public ChatShowKind ShowKind;

		public const string NumberPattern = "^[0-9]+$";

		public static float MessagePartTimeSec = 60f;

		public long UIBindMsgID;
	}
}

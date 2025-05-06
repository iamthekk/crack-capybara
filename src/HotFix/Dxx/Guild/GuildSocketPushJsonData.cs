using System;
using System.Collections.Generic;
using System.Text;
using HotFix;
using Proto.Guild;

namespace Dxx.Guild
{
	public class GuildSocketPushJsonData
	{
		public static GuildSocketPushJsonData.GuildBaseJsonData TryGetData(int type, string json)
		{
			switch (type)
			{
			case 101:
				return GuildProxy.JSON.ToObject<GuildSocketPushJsonData.BeApprovedJoinGuild>(json);
			case 102:
				return GuildProxy.JSON.ToObject<GuildSocketPushJsonData.SomeOneJoinGuild>(json);
			case 103:
				return GuildProxy.JSON.ToObject<GuildSocketPushJsonData.SomeOneLeaveGuild>(json);
			case 104:
				return GuildProxy.JSON.ToObject<GuildSocketPushJsonData.BeChangeGuildPosition>(json);
			case 105:
				return GuildProxy.JSON.ToObject<GuildSocketPushJsonData.BeKickOutGuild>(json);
			case 106:
				return GuildProxy.JSON.ToObject<GuildSocketPushJsonData.SomeOneBeKickOut>(json);
			case 107:
				return GuildProxy.JSON.ToObject<GuildSocketPushJsonData.GuildInfoUpdate>(json);
			case 111:
				return GuildProxy.JSON.ToObject<GuildSocketPushJsonData.SomeOneApplyJoin>(json);
			}
			return null;
		}

		private static GuildSDKManager GuildSDK
		{
			get
			{
				return GuildSDKManager.Instance;
			}
		}

		public static void TestJson2Data()
		{
		}

		public abstract class GuildBaseJsonData
		{
			public abstract void HandleMessage();

			public abstract string MakeLogString();

			public int msgId;

			public long timestamp;
		}

		public class BeApprovedJoinGuild : GuildSocketPushJsonData.GuildBaseJsonData
		{
			public override void HandleMessage()
			{
				GuildNetUtil.Guild.DoRequest_GuildLoginRequest(delegate(bool result, GuildGetInfoResponse resp)
				{
					GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID1("400205", this.guildName));
				});
			}

			public override string MakeLogString()
			{
				return GuildProxy.Language.GetInfoByID1("420801", GuildProxy.GameUser.MyNick());
			}

			public ulong guildId;

			public string guildName;
		}

		public class SomeOneJoinGuild : GuildSocketPushJsonData.GuildBaseJsonData
		{
			public override void HandleMessage()
			{
				if (GuildSocketPushJsonData.GuildSDK.GuildInfo.HasGuild)
				{
					GuildNetUtil.Guild.DoRequest_GetGuildMemberList(GuildSocketPushJsonData.GuildSDK.GuildInfo.GuildID, null, false);
				}
			}

			public override string MakeLogString()
			{
				if (this.joinUsers == null || this.joinUsers.Length == 0)
				{
					return "";
				}
				StringBuilder stringBuilder = new StringBuilder(100);
				for (int i = 0; i < this.joinUsers.Length; i++)
				{
					GuildSocketPushJsonData.SomeOneJoinGuild.member member = this.joinUsers[i];
					if (member != null)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(';');
						}
						string text = member.nickName;
						if (string.IsNullOrEmpty(text))
						{
							text = GuildProxy.GameUser.GetPlayerDefaultNick(member.userId);
						}
						stringBuilder.Append(GuildProxy.Language.GetInfoByID1("420801", text));
					}
				}
				return stringBuilder.ToString();
			}

			public GuildSocketPushJsonData.SomeOneJoinGuild.member[] joinUsers;

			[Serializable]
			public class member
			{
				public long userId;

				public string nickName;

				public uint avatar;

				public uint avatarFrame;

				public uint level;

				public ulong activeTime;

				public uint position;

				public uint chapterId;

				public long atk;

				public long hp;

				public long battlePower;

				public ulong applyTime;

				public ulong joinTime;

				public uint dailyActive;

				public uint weekActive;
			}
		}

		public class SomeOneLeaveGuild : GuildSocketPushJsonData.GuildBaseJsonData
		{
			public override void HandleMessage()
			{
				if (GuildSocketPushJsonData.GuildSDK.GuildInfo.HasGuild)
				{
					GuildNetUtil.Guild.DoRequest_GetGuildMemberList(GuildSocketPushJsonData.GuildSDK.GuildInfo.GuildID, delegate(bool result, GuildGetMemberListResponse resp)
					{
						if (result)
						{
							GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = new GuildEvent_GuildMemberChange();
							guildEvent_GuildMemberChange.GuildID = GuildSocketPushJsonData.GuildSDK.GuildInfo.GuildID;
							guildEvent_GuildMemberChange.DeleteUser.Add(this.leaveUserId);
							GuildSocketPushJsonData.GuildSDK.Event.DispatchNow(14, guildEvent_GuildMemberChange);
						}
					}, false);
				}
			}

			public override string MakeLogString()
			{
				if (!string.IsNullOrEmpty(this.leaveUserNickName))
				{
					return GuildProxy.Language.GetInfoByID1("420802", this.leaveUserNickName);
				}
				return GuildProxy.Language.GetInfoByID1("420802", GuildProxy.GameUser.GetPlayerDefaultNick(this.leaveUserId));
			}

			public long leaveUserId;

			public string leaveUserNickName;
		}

		public class BeChangeGuildPosition : GuildSocketPushJsonData.GuildBaseJsonData
		{
			public override void HandleMessage()
			{
				if (this.toUserId != GuildProxy.GameUser.UserID())
				{
					return;
				}
				string positionLanguageByPos = GuildUserShareDataEx.GetPositionLanguageByPos(this.fromUserPosition);
				string positionLanguageByPos2 = GuildUserShareDataEx.GetPositionLanguageByPos(this.upPosition);
				string text = (string.IsNullOrEmpty(this.fromUserNickName) ? GuildProxy.GameUser.GetPlayerDefaultNick(this.fromUserId) : this.fromUserNickName);
				string text2;
				if (this.oldPosition > this.upPosition)
				{
					text2 = GuildProxy.Language.GetInfoByID3("400206", positionLanguageByPos, text, positionLanguageByPos2);
				}
				else
				{
					text2 = GuildProxy.Language.GetInfoByID3("400207", positionLanguageByPos, text, positionLanguageByPos2);
				}
				if (!string.IsNullOrEmpty(text2))
				{
					GuildProxy.UI.ShowTips(text2);
				}
				if (GuildSocketPushJsonData.GuildSDK.GuildInfo.HasGuild)
				{
					GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = new GuildEvent_GuildMemberChange();
					guildEvent_GuildMemberChange.GuildID = GuildSocketPushJsonData.GuildSDK.GuildInfo.GuildID;
					GuildUserShareData guildUserShareData = new GuildUserShareData();
					guildUserShareData.CloneFrom(GuildSocketPushJsonData.GuildSDK.User.MyUserData);
					guildUserShareData.GuildPosition = (GuildPositionType)this.upPosition;
					guildEvent_GuildMemberChange.UserList.Add(guildUserShareData);
					if (this.upPosition == 1)
					{
						IList<GuildUserShareData> memberList = GuildSocketPushJsonData.GuildSDK.GuildInfo.GetMemberList();
						GuildUserShareData guildUserShareData2 = null;
						for (int i = 0; i < memberList.Count; i++)
						{
							if (memberList[i].UserID == this.fromUserId)
							{
								guildUserShareData2 = memberList[i];
							}
						}
						if (guildUserShareData2 != null)
						{
							GuildUserShareData guildUserShareData3 = new GuildUserShareData();
							guildUserShareData3.CloneFrom(guildUserShareData2);
							guildUserShareData3.GuildPosition = GuildPositionType.Member;
							guildEvent_GuildMemberChange.UserList.Add(guildUserShareData3);
						}
					}
					GuildSocketPushJsonData.GuildSDK.Event.DispatchNow(14, guildEvent_GuildMemberChange);
				}
			}

			public override string MakeLogString()
			{
				string positionLanguageByPos = GuildUserShareDataEx.GetPositionLanguageByPos(this.fromUserPosition);
				string positionLanguageByPos2 = GuildUserShareDataEx.GetPositionLanguageByPos(this.upPosition);
				string text = (string.IsNullOrEmpty(this.fromUserNickName) ? GuildProxy.GameUser.GetPlayerDefaultNick(this.fromUserId) : this.fromUserNickName);
				string text2;
				if (this.toUserId == GuildProxy.GameUser.UserID())
				{
					text2 = GuildProxy.GameUser.MyNick();
				}
				else
				{
					text2 = (string.IsNullOrEmpty(this.toUserNickName) ? GuildProxy.GameUser.GetPlayerDefaultNick(this.toUserId) : this.toUserNickName);
				}
				string text3;
				if (this.oldPosition > this.upPosition)
				{
					text3 = GuildProxy.Language.GetInfoByID4("420803", positionLanguageByPos, text, text2, positionLanguageByPos2);
				}
				else
				{
					text3 = GuildProxy.Language.GetInfoByID4("420804", positionLanguageByPos, text, text2, positionLanguageByPos2);
				}
				return text3;
			}

			public long fromUserId;

			public string fromUserNickName;

			public int fromUserPosition;

			public int upPosition;

			public int oldPosition;

			public long toUserId;

			public string toUserNickName;
		}

		public class BeKickOutGuild : GuildSocketPushJsonData.GuildBaseJsonData
		{
			public override void HandleMessage()
			{
				GuildShareData guildData = GuildSocketPushJsonData.GuildSDK.GuildInfo.GuildData;
				GuildEvent_BeKickOutSetData e = new GuildEvent_BeKickOutSetData();
				e.Info = new GuildBeKickOutInfo();
				if (guildData != null)
				{
					e.Info.GuildID = guildData.GuildID;
					e.Info.GuildName = guildData.GuildShowName;
				}
				e.Info.KickUserID = this.fromUserId;
				e.Info.KickUserNick = this.fromUserNickName;
				e.Info.KickUserPos = (GuildPositionType)this.fromUserPosition;
				GuildNetUtil.Guild.DoRequest_GuildLoginRequest(delegate(bool result, GuildGetInfoResponse resp)
				{
					if (result && !resp.IsJoined)
					{
						GuildSocketPushJsonData.GuildSDK.Event.DispatchNow(22, e);
						GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_Leave);
					}
				});
			}

			public override string MakeLogString()
			{
				return "";
			}

			public long fromUserId;

			public string fromUserNickName;

			public int fromUserPosition;
		}

		public class SomeOneBeKickOut : GuildSocketPushJsonData.GuildBaseJsonData
		{
			public override void HandleMessage()
			{
				if (GuildSocketPushJsonData.GuildSDK.GuildInfo.HasGuild)
				{
					GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = new GuildEvent_GuildMemberChange();
					guildEvent_GuildMemberChange.GuildID = GuildSocketPushJsonData.GuildSDK.GuildInfo.GuildID;
					guildEvent_GuildMemberChange.DeleteUser.Add(this.beKickedUserId);
					GuildSocketPushJsonData.GuildSDK.Event.DispatchNow(14, guildEvent_GuildMemberChange);
				}
			}

			public override string MakeLogString()
			{
				string positionLanguageByPos = GuildUserShareDataEx.GetPositionLanguageByPos(this.fromUserPosition);
				string text = (string.IsNullOrEmpty(this.fromUserNickName) ? GuildProxy.GameUser.GetPlayerDefaultNick(this.fromUserId) : this.fromUserNickName);
				string text2 = (string.IsNullOrEmpty(this.beKickedUserNickName) ? GuildProxy.GameUser.GetPlayerDefaultNick(this.beKickedUserId) : this.beKickedUserNickName);
				return GuildProxy.Language.GetInfoByID3("420805", text2, positionLanguageByPos, text);
			}

			public long fromUserId;

			public string fromUserNickName;

			public int fromUserPosition;

			public long beKickedUserId;

			public string beKickedUserNickName;
		}

		public class GuildInfoUpdate : GuildSocketPushJsonData.GuildBaseJsonData
		{
			public override void HandleMessage()
			{
				if (GuildSocketPushJsonData.GuildSDK.GuildInfo.HasGuild)
				{
					GuildShareData guildData = GuildSocketPushJsonData.GuildSDK.GuildInfo.GuildData;
					GuildEvent_GuildInfoDataChange e = new GuildEvent_GuildInfoDataChange();
					e.ChangeInfo = new GuildChangeInfo();
					e.ChangeInfo.guildName = this.guildName;
					e.ChangeInfo.guildIcon = this.guildIcon;
					e.ChangeInfo.guildIconBg = this.guildIconBg;
					e.ChangeInfo.guildNotice = this.guildNotice;
					e.ChangeInfo.guildIntro = this.guildIntro;
					e.ChangeInfo.guildLevel = this.guildLevel;
					e.ChangeInfo.guildExp = this.guildExp;
					if (guildData.GuildLevel != this.guildLevel)
					{
						GuildNetUtil.Guild.DoRequest_GuildLoginRequest(delegate(bool result, GuildGetInfoResponse resp)
						{
							GuildSocketPushJsonData.GuildSDK.Event.DispatchNow(23, e);
						});
						return;
					}
					GuildSocketPushJsonData.GuildSDK.Event.DispatchNow(23, e);
				}
			}

			public override string MakeLogString()
			{
				return "";
			}

			public string guildName;

			public int guildIcon;

			public int guildIconBg;

			public string guildNotice;

			public string guildIntro;

			public int guildLevel;

			public int guildExp;
		}

		public class SomeOneApplyJoin : GuildSocketPushJsonData.GuildBaseJsonData
		{
			public override void HandleMessage()
			{
				if (GuildSocketPushJsonData.GuildSDK.GuildInfo.HasGuild)
				{
					GuildNetUtil.Guild.DoRequest_GetApplyJoinGuildUserList(delegate(bool result, GuildGetApplyListResponse resp)
					{
						GuildEvent_ApplyJoinCount guildEvent_ApplyJoinCount = new GuildEvent_ApplyJoinCount();
						guildEvent_ApplyJoinCount.ApplyJoinCount = (int)this.totalCount;
						GuildSocketPushJsonData.GuildSDK.Event.DispatchNow(24, guildEvent_ApplyJoinCount);
					}, false);
				}
			}

			public override string MakeLogString()
			{
				return "";
			}

			public ulong guildId;

			public uint totalCount;

			public GuildSocketPushJsonData.SomeOneApplyJoin.applyuserinfo[] dataList;

			[Serializable]
			public class applyuserinfo
			{
				public ulong userId;

				public string nickName;

				public uint avatar;

				public uint avatarFrame;
			}
		}
	}
}

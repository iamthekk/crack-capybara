using System;
using System.Collections.Generic;
using Dxx.Chat;
using Dxx.Guild;
using Framework;
using Google.Protobuf;
using Google.Protobuf.Collections;
using HotFix;
using Proto.Guild;
using Proto.GuildRace;
using Proto.User;

public static class GuildNetUtil
{
	public class Guild
	{
		private static GuildSDKManager GuildSDK
		{
			get
			{
				return GuildSDKManager.Instance;
			}
		}

		public static void LoginSetUserData(string deviceid, UserLoginResponse loginresp)
		{
			GuildEvent_SetSelfUserData guildEvent_SetSelfUserData = new GuildEvent_SetSelfUserData();
			guildEvent_SetSelfUserData.LanguageID = GuildProxy.Language.GetCurrentLanguage();
			guildEvent_SetSelfUserData.UserID = loginresp.UserId;
			guildEvent_SetSelfUserData.DeviceID = deviceid;
			GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(9, guildEvent_SetSelfUserData);
		}

		public static bool IsGuildError(int code)
		{
			return GuildProxy.Net.IsGuildError(code);
		}

		private static ulong GetULongGuildID(string guildid)
		{
			ulong num;
			if (ulong.TryParse(guildid, out num))
			{
				return num;
			}
			return 0UL;
		}

		public static void DoRequest_GuildLoginRequest(Action<bool, GuildGetInfoResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildGetInfoRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildGetInfoResponse guildGetInfoResponse = response as GuildGetInfoResponse;
				if (guildGetInfoResponse != null && guildGetInfoResponse.Code == 0)
				{
					GuildEvent_LoginSuccess guildEvent_LoginSuccess = new GuildEvent_LoginSuccess();
					guildEvent_LoginSuccess.IsJoin = guildGetInfoResponse.IsJoined;
					if (guildGetInfoResponse.IsJoined)
					{
						guildEvent_LoginSuccess.IsLevelUp = guildGetInfoResponse.IsLevelUp;
						guildEvent_LoginSuccess.SetDataFromServer(guildGetInfoResponse.GuildDetailInfoDto, guildGetInfoResponse.GuildFeaturesDto);
					}
					if (guildGetInfoResponse.BeKickedOutDto != null && guildGetInfoResponse.BeKickedOutDto.GuildId > 0UL)
					{
						guildEvent_LoginSuccess.BeKickOutInfo = guildGetInfoResponse.BeKickedOutDto.ToGuildBeKickOutInfo();
					}
					guildEvent_LoginSuccess.IsLevelUp = guildGetInfoResponse.IsLevelUp;
					guildEvent_LoginSuccess.QuitGuildTimeStamp = guildGetInfoResponse.QuitGuildTimeStamp;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(10, guildEvent_LoginSuccess);
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(202, guildEvent_LoginSuccess);
					GuildProxy.RedPoint.CalcRedPoint("Guild", true);
					Action<bool, GuildGetInfoResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildGetInfoResponse);
					return;
				}
				else
				{
					Action<bool, GuildGetInfoResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildGetInfoResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GetGuildHallList(int searchtype, string value, bool onlyjoinable, Action<bool, GuildSearchResponse> callback)
		{
			GuildSearchRequest guildSearchRequest = new GuildSearchRequest();
			guildSearchRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			guildSearchRequest.Type = (uint)searchtype;
			guildSearchRequest.Value = value;
			guildSearchRequest.IsOnlyJoinable = onlyjoinable;
			if (guildSearchRequest.Type != 1U)
			{
				GuildListGroup listGroup = GuildNetUtil.Guild.GuildSDK.GuildList.GetListGroup(0);
				guildSearchRequest.ExcludeGuildIds.AddRange(listGroup.GetGuildIDULongList(true));
			}
			GuildProxy.Net.TrySendMessage(guildSearchRequest, delegate(IMessage response)
			{
				GuildSearchResponse guildSearchResponse = response as GuildSearchResponse;
				if (guildSearchResponse != null && guildSearchResponse.Code == 0)
				{
					GuildEvent_GetGuildList guildEvent_GetGuildList = new GuildEvent_GetGuildList();
					guildEvent_GetGuildList.GroupID = 0;
					guildEvent_GetGuildList.GuildList.Clear();
					guildEvent_GetGuildList.GuildList.AddRange(guildSearchResponse.GuildInfoDtos.ToGuidList());
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(11, guildEvent_GetGuildList);
					Action<bool, GuildSearchResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildSearchResponse);
					return;
				}
				else
				{
					Action<bool, GuildSearchResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildSearchResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GetGuildGroupList(int pageindex, int searchtype, string value, Action<bool, GuildSearchResponse> callback)
		{
			if (pageindex <= 0)
			{
				HLog.LogError("DoRequest_GetGuildGroupList 该接口不支持 pageindex <= 0 !!!");
				return;
			}
			GuildSearchRequest guildSearchRequest = new GuildSearchRequest();
			guildSearchRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			guildSearchRequest.Type = (uint)searchtype;
			if (string.IsNullOrEmpty(value))
			{
				value = "";
			}
			guildSearchRequest.Value = value;
			guildSearchRequest.IsOnlyJoinable = false;
			guildSearchRequest.PageIndex = (uint)pageindex;
			GuildProxy.Net.TrySendMessage(guildSearchRequest, delegate(IMessage response)
			{
				GuildSearchResponse guildSearchResponse = response as GuildSearchResponse;
				if (guildSearchResponse != null && guildSearchResponse.Code == 0)
				{
					Action<bool, GuildSearchResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildSearchResponse);
					return;
				}
				else
				{
					Action<bool, GuildSearchResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildSearchResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GuildContribute(Action<bool, GuildContributeResponse> callBack)
		{
			GuildProxy.Net.TrySendMessage(new GuildContributeRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildContributeResponse guildContributeResponse = response as GuildContributeResponse;
				if (guildContributeResponse != null && guildContributeResponse.Code == 0)
				{
					GuildEvent_GuildContribute guildEvent_GuildContribute = new GuildEvent_GuildContribute();
					guildEvent_GuildContribute.DayContributeTimes = guildContributeResponse.DayContributeTimes;
					guildEvent_GuildContribute.DayAllContributeTimes = guildContributeResponse.DayALLContributeTimes;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(305, guildEvent_GuildContribute);
					Action<bool, GuildContributeResponse> callBack2 = callBack;
					if (callBack2 != null)
					{
						callBack2(true, guildContributeResponse);
					}
					GuildProxy.RedPoint.CalcRedPoint("Guild.Hall.Donation", true);
					return;
				}
				Action<bool, GuildContributeResponse> callBack3 = callBack;
				if (callBack3 == null)
				{
					return;
				}
				callBack3(false, guildContributeResponse);
			}, true, false, true);
		}

		public static void DoRequest_GetGuildDetailInfo(string guildID, Action<bool, GuildGetDetailResponse> callback, bool showmask = false)
		{
			uint num;
			if (!uint.TryParse(guildID, out num))
			{
				num = 0U;
			}
			if (num != 0U)
			{
				GuildProxy.Net.TrySendMessage(new GuildGetDetailRequest
				{
					CommonParams = GuildProxy.Net.GetCommonParams(),
					GuildId = (ulong)num
				}, delegate(IMessage response)
				{
					GuildGetDetailResponse guildGetDetailResponse = response as GuildGetDetailResponse;
					if (guildGetDetailResponse != null && guildGetDetailResponse.Code == 0)
					{
						GuildEvent_GetGuildDetailInfo guildEvent_GetGuildDetailInfo = new GuildEvent_GetGuildDetailInfo();
						guildEvent_GetGuildDetailInfo.GuildData = guildGetDetailResponse.GuildDetailInfoDto.ToGuildDetailData();
						GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(12, guildEvent_GetGuildDetailInfo);
						Action<bool, GuildGetDetailResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(true, guildGetDetailResponse);
						return;
					}
					else
					{
						Action<bool, GuildGetDetailResponse> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, guildGetDetailResponse);
						return;
					}
				}, showmask, false, true);
				return;
			}
			Action<bool, GuildGetDetailResponse> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(false, null);
		}

		public static void DoRequest_GetGuildMemberList(string guildID, Action<bool, GuildGetMemberListResponse> callback, bool showmask = true)
		{
			uint num;
			if (!uint.TryParse(guildID, out num))
			{
				num = 0U;
			}
			if (num != 0U)
			{
				GuildProxy.Net.TrySendMessage(new GuildGetMemberListRequest
				{
					CommonParams = GuildProxy.Net.GetCommonParams(),
					GuildId = (ulong)num
				}, delegate(IMessage response)
				{
					GuildGetMemberListResponse guildGetMemberListResponse = response as GuildGetMemberListResponse;
					if (guildGetMemberListResponse != null && guildGetMemberListResponse.Code == 0)
					{
						if (guildID == GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildID)
						{
							GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = new GuildEvent_GuildMemberChange();
							guildEvent_GuildMemberChange.GuildID = guildID;
							guildEvent_GuildMemberChange.UserList.AddRange(guildGetMemberListResponse.GuildMemberInfoDtos.ToGuidUserList());
							guildEvent_GuildMemberChange.MemberCount = guildGetMemberListResponse.GuildMemberInfoDtos.Count;
							GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(14, guildEvent_GuildMemberChange);
						}
						Action<bool, GuildGetMemberListResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(true, guildGetMemberListResponse);
						return;
					}
					else
					{
						Action<bool, GuildGetMemberListResponse> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, guildGetMemberListResponse);
						return;
					}
				}, showmask, false, true);
				return;
			}
			Action<bool, GuildGetMemberListResponse> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(false, null);
		}

		public static void DoRequest_GuildGetFeaturesInfoRequest(Action<bool, GuildGetFeaturesInfoResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildGetFeaturesInfoRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildGetFeaturesInfoResponse guildGetFeaturesInfoResponse = response as GuildGetFeaturesInfoResponse;
				if (guildGetFeaturesInfoResponse != null && guildGetFeaturesInfoResponse.Code == 0)
				{
					GuildEvent_SetMyGuildFeaturesInfo guildEvent_SetMyGuildFeaturesInfo = new GuildEvent_SetMyGuildFeaturesInfo();
					guildEvent_SetMyGuildFeaturesInfo.SetDataFromServer(guildGetFeaturesInfoResponse.GuildFeaturesDto);
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(16, guildEvent_SetMyGuildFeaturesInfo);
					GuildProxy.RedPoint.CalcRedPoint("Guild", true);
					Action<bool, GuildGetFeaturesInfoResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildGetFeaturesInfoResponse);
					return;
				}
				else
				{
					Action<bool, GuildGetFeaturesInfoResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildGetFeaturesInfoResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_CreateGuild(GuildCreateData createdata, Action<bool, GuildCreateResponse> callback)
		{
			GuildCreateRequest guildCreateRequest = new GuildCreateRequest();
			guildCreateRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			guildCreateRequest.GuildName = createdata.GuildShowName;
			guildCreateRequest.GuildIntro = createdata.GuildSlogan;
			if (string.IsNullOrEmpty(guildCreateRequest.GuildIntro))
			{
				guildCreateRequest.GuildIntro = "";
			}
			if (string.IsNullOrEmpty(guildCreateRequest.GuildNotice))
			{
				guildCreateRequest.GuildNotice = "";
			}
			guildCreateRequest.GuildIcon = (uint)createdata.GuildLogo;
			guildCreateRequest.GuildIconBg = (uint)createdata.GuildLogoBG;
			guildCreateRequest.ApplyType = (uint)createdata.JoinKind;
			guildCreateRequest.ApplyCondition = (uint)createdata.JoinCondition_Level;
			guildCreateRequest.Language = (uint)createdata.Language;
			GuildProxy.Net.TrySendMessage(guildCreateRequest, delegate(IMessage response)
			{
				GuildCreateResponse guildCreateResponse = response as GuildCreateResponse;
				if (guildCreateResponse != null && guildCreateResponse.Code == 0)
				{
					GuildEvent_LoginSuccess guildEvent_LoginSuccess = new GuildEvent_LoginSuccess();
					guildEvent_LoginSuccess.IsJoin = true;
					guildEvent_LoginSuccess.SetDataFromServer(guildCreateResponse.GuildDetailInfoDto, guildCreateResponse.GuildFeaturesDto);
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(10, guildEvent_LoginSuccess);
					GuildProxy.TGA.OnGuildCreate();
					GuildProxy.RedPoint.CalcRedPoint("Guild", true);
					Action<bool, GuildCreateResponse> callback2 = callback;
					if (callback2 != null)
					{
						callback2(true, guildCreateResponse);
					}
					GameApp.SDK.Analyze.Track_GuildActivity("create", TGA_GuildActivityData.Create(), null);
					return;
				}
				Action<bool, GuildCreateResponse> callback3 = callback;
				if (callback3 == null)
				{
					return;
				}
				callback3(false, guildCreateResponse);
			}, true, false, true);
		}

		public static void DoRequest_DismissGuild(Action<bool, GuildDismissResponse> callback)
		{
			GuildDismissRequest guildDismissRequest = new GuildDismissRequest();
			guildDismissRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			GuildShareData guilddata = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildData;
			int mypos = (int)GuildNetUtil.Guild.GuildSDK.Permission.MyGuildPosition;
			TGA_GuildActivityData tgaData = TGA_GuildActivityData.Create();
			GuildProxy.Net.TrySendMessage(guildDismissRequest, delegate(IMessage response)
			{
				GuildDismissResponse guildDismissResponse = response as GuildDismissResponse;
				if (guildDismissResponse != null && guildDismissResponse.Code == 0)
				{
					GuildEvent_LoginSuccess guildEvent_LoginSuccess = new GuildEvent_LoginSuccess();
					guildEvent_LoginSuccess.IsJoin = false;
					guildEvent_LoginSuccess.MyGuildShareDetail = null;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(10, guildEvent_LoginSuccess);
					if (guilddata != null)
					{
						GuildProxy.TGA.OnGuildQuit(guilddata.GuildID, guilddata.GuildLevel, mypos);
					}
					Action<bool, GuildDismissResponse> callback2 = callback;
					if (callback2 != null)
					{
						callback2(true, guildDismissResponse);
					}
					GameApp.SDK.Analyze.Track_GuildActivity("quit", tgaData, null);
					return;
				}
				Action<bool, GuildDismissResponse> callback3 = callback;
				if (callback3 == null)
				{
					return;
				}
				callback3(false, guildDismissResponse);
			}, true, false, true);
		}

		public static void DoRequest_ModifyGuildInfo(GuildCreateData createdata, Action<bool, GuildModifyResponse> callback)
		{
			GuildModifyRequest guildModifyRequest = new GuildModifyRequest();
			guildModifyRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			guildModifyRequest.GuildName = createdata.GuildShowName;
			guildModifyRequest.GuildIntro = createdata.GuildSlogan;
			guildModifyRequest.GuildIcon = (uint)createdata.GuildLogo;
			guildModifyRequest.GuildIconBg = (uint)createdata.GuildLogoBG;
			guildModifyRequest.ApplyType = (uint)createdata.JoinKind;
			guildModifyRequest.ApplyCondition = (uint)createdata.JoinCondition_Level;
			guildModifyRequest.Language = (uint)createdata.Language;
			guildModifyRequest.IsModifyGuildIntro = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildSlogan != guildModifyRequest.GuildIntro;
			guildModifyRequest.GuildNotice = createdata.GuildNotice;
			guildModifyRequest.IsModifyGuildNotice = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildNotice != guildModifyRequest.GuildNotice;
			GuildProxy.Net.TrySendMessage(guildModifyRequest, delegate(IMessage response)
			{
				GuildModifyResponse guildModifyResponse = response as GuildModifyResponse;
				if (guildModifyResponse != null && guildModifyResponse.Code == 0)
				{
					GuildEvent_GetGuildDetailInfo guildEvent_GetGuildDetailInfo = new GuildEvent_GetGuildDetailInfo();
					guildEvent_GetGuildDetailInfo.GuildData = new GuildShareDetailData();
					guildEvent_GetGuildDetailInfo.GuildData.GuildID = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildID;
					guildEvent_GetGuildDetailInfo.GuildData.ShareData = guildModifyResponse.GuildInfoDto.ToGuid();
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(15, guildEvent_GetGuildDetailInfo);
					Action<bool, GuildModifyResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildModifyResponse);
					return;
				}
				else
				{
					Action<bool, GuildModifyResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildModifyResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GuildAutoJoin(Action<bool, int, GuildAutoJoinResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildAutoJoinRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildAutoJoinResponse guildAutoJoinResponse = response as GuildAutoJoinResponse;
				if (guildAutoJoinResponse != null && guildAutoJoinResponse.Code == 0)
				{
					int num;
					if (guildAutoJoinResponse.GuildDetailInfoDto != null)
					{
						GuildEvent_LoginSuccess guildEvent_LoginSuccess = new GuildEvent_LoginSuccess();
						guildEvent_LoginSuccess.IsJoin = true;
						guildEvent_LoginSuccess.SetDataFromServer(guildAutoJoinResponse.GuildDetailInfoDto, guildAutoJoinResponse.GuildFeaturesDto);
						GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(10, guildEvent_LoginSuccess);
						num = 1;
						GuildProxy.TGA.OnGuildJoin(3, GuildProxy.GameUser.UserID(), 0L);
						GameApp.SDK.Analyze.Track_GuildActivity("join_auto", TGA_GuildActivityData.Create(), null);
					}
					else
					{
						num = 2;
					}
					Action<bool, int, GuildAutoJoinResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, num, guildAutoJoinResponse);
					return;
				}
				else
				{
					Action<bool, int, GuildAutoJoinResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, 0, guildAutoJoinResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_RequireJoinGuild(string guildID, int language, Action<bool, int, GuildApplyJoinResponse> callback)
		{
			uint num;
			if (!uint.TryParse(guildID, out num))
			{
				num = 0U;
			}
			if (num != 0U)
			{
				GuildProxy.Net.TrySendMessage(new GuildApplyJoinRequest
				{
					CommonParams = GuildProxy.Net.GetCommonParams(),
					GuildId = (ulong)num,
					Language = (uint)language
				}, delegate(IMessage response)
				{
					GuildApplyJoinResponse guildApplyJoinResponse = response as GuildApplyJoinResponse;
					if (guildApplyJoinResponse != null && guildApplyJoinResponse.Code == 0)
					{
						int num2;
						if (guildApplyJoinResponse.GuildDetailInfoDto != null)
						{
							GuildEvent_LoginSuccess guildEvent_LoginSuccess = new GuildEvent_LoginSuccess();
							guildEvent_LoginSuccess.IsJoin = true;
							guildEvent_LoginSuccess.SetDataFromServer(guildApplyJoinResponse.GuildDetailInfoDto, guildApplyJoinResponse.GuildFeaturesDto);
							GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(10, guildEvent_LoginSuccess);
							num2 = 1;
							GuildProxy.TGA.OnGuildJoin(2, GuildProxy.GameUser.UserID(), 0L);
							GameApp.SDK.Analyze.Track_GuildActivity("join_auto", TGA_GuildActivityData.Create(), null);
						}
						else
						{
							num2 = 2;
						}
						Action<bool, int, GuildApplyJoinResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(true, num2, guildApplyJoinResponse);
						return;
					}
					else
					{
						Action<bool, int, GuildApplyJoinResponse> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, 0, guildApplyJoinResponse);
						return;
					}
				}, true, false, true);
				return;
			}
			Action<bool, int, GuildApplyJoinResponse> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(false, 0, null);
		}

		public static void DoRequest_RequireCancelJoinGuild(string guildID, Action<bool, GuildApplyJoinResponse> callback)
		{
			uint num;
			if (!uint.TryParse(guildID, out num))
			{
				num = 0U;
			}
			if (num != 0U)
			{
				GuildProxy.Net.TrySendMessage(new GuildCancelApplyRequest
				{
					CommonParams = GuildProxy.Net.GetCommonParams()
				}, delegate(IMessage response)
				{
					GuildApplyJoinResponse guildApplyJoinResponse = response as GuildApplyJoinResponse;
					if (guildApplyJoinResponse != null && guildApplyJoinResponse.Code == 0)
					{
						Action<bool, GuildApplyJoinResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(true, guildApplyJoinResponse);
						return;
					}
					else
					{
						Action<bool, GuildApplyJoinResponse> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, guildApplyJoinResponse);
						return;
					}
				}, true, false, true);
				return;
			}
			Action<bool, GuildApplyJoinResponse> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(false, null);
		}

		public static void DoRequest_GetApplyJoinGuildUserList(Action<bool, GuildGetApplyListResponse> callback, bool showMask = false)
		{
			GuildProxy.Net.TrySendMessage(new GuildGetApplyListRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildGetApplyListResponse guildGetApplyListResponse = response as GuildGetApplyListResponse;
				if (guildGetApplyListResponse != null && guildGetApplyListResponse.Code == 0)
				{
					GuildEvent_ApplyJoinCount guildEvent_ApplyJoinCount = new GuildEvent_ApplyJoinCount();
					guildEvent_ApplyJoinCount.ApplyJoinCount = guildGetApplyListResponse.ApplyList.Count;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(24, guildEvent_ApplyJoinCount);
					GuildNetUtil.Guild.GuildSDK.GuildInfo.SetPlayerDataApplyJoin(guildGetApplyListResponse.ApplyList.ToGuidUserList());
					Action<bool, GuildGetApplyListResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildGetApplyListResponse);
					return;
				}
				else
				{
					Action<bool, GuildGetApplyListResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildGetApplyListResponse);
					return;
				}
			}, showMask, false, true);
		}

		public static void DoRequest_AgreeJoinGuild(List<long> userids, Action<bool, GuildAgreeJoinResponse> callback)
		{
			GuildAgreeJoinRequest guildAgreeJoinRequest = new GuildAgreeJoinRequest();
			guildAgreeJoinRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			guildAgreeJoinRequest.UserIds.AddRange(userids);
			GuildProxy.Net.TrySendMessage(guildAgreeJoinRequest, delegate(IMessage response)
			{
				GuildAgreeJoinResponse guildAgreeJoinResponse = response as GuildAgreeJoinResponse;
				if (guildAgreeJoinResponse != null && guildAgreeJoinResponse.Code == 0)
				{
					GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = new GuildEvent_GuildMemberChange();
					guildEvent_GuildMemberChange.GuildID = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildID;
					guildEvent_GuildMemberChange.UserList.AddRange(guildAgreeJoinResponse.GuildMemberInfoDto.ToGuidUserList());
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(14, guildEvent_GuildMemberChange);
					GuildEvent_ApplyJoinCount guildEvent_ApplyJoinCount = new GuildEvent_ApplyJoinCount();
					guildEvent_ApplyJoinCount.ApplyJoinCount = (int)guildAgreeJoinResponse.ApplyCount;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(24, guildEvent_ApplyJoinCount);
					if (guildAgreeJoinResponse.JoinOtherGuildUserIds.Count > 0)
					{
						GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("server_err_321093"));
					}
					if (guildAgreeJoinResponse.TimeLimitGuildUserIds.Count > 0)
					{
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("server_err_706"));
					}
					RepeatedField<GuildMemberInfoDto> guildMemberInfoDto = guildAgreeJoinResponse.GuildMemberInfoDto;
					for (int i = 0; i < guildMemberInfoDto.Count; i++)
					{
						GuildProxy.TGA.OnGuildJoin(1, guildMemberInfoDto[i].UserId, GuildProxy.GameUser.UserID());
					}
					Action<bool, GuildAgreeJoinResponse> callback2 = callback;
					if (callback2 != null)
					{
						callback2(true, guildAgreeJoinResponse);
					}
					List<long> list = new List<long>();
					for (int j = 0; j < guildMemberInfoDto.Count; j++)
					{
						list.Add(guildMemberInfoDto[j].UserId);
					}
					GameApp.SDK.Analyze.Track_GuildActivity("approval", TGA_GuildActivityData.Create(), list);
					return;
				}
				Action<bool, GuildAgreeJoinResponse> callback3 = callback;
				if (callback3 == null)
				{
					return;
				}
				callback3(false, guildAgreeJoinResponse);
			}, true, false, true);
		}

		public static void DoRequest_RefuseJoinGuild(List<long> userids, Action<bool, GuildRefuseJoinResponse> callback)
		{
			GuildRefuseJoinRequest guildRefuseJoinRequest = new GuildRefuseJoinRequest();
			guildRefuseJoinRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			guildRefuseJoinRequest.UserIds.AddRange(userids);
			GuildProxy.Net.TrySendMessage(guildRefuseJoinRequest, delegate(IMessage response)
			{
				GuildRefuseJoinResponse guildRefuseJoinResponse = response as GuildRefuseJoinResponse;
				if (guildRefuseJoinResponse != null && guildRefuseJoinResponse.Code == 0)
				{
					GuildEvent_ApplyJoinCount guildEvent_ApplyJoinCount = new GuildEvent_ApplyJoinCount();
					guildEvent_ApplyJoinCount.ApplyJoinCount = (int)guildRefuseJoinResponse.ApplyCount;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(24, guildEvent_ApplyJoinCount);
					Action<bool, GuildRefuseJoinResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildRefuseJoinResponse);
					return;
				}
				else
				{
					Action<bool, GuildRefuseJoinResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildRefuseJoinResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GuildKickUser(long userid, Action<bool, GuildKickOutResponse> callback)
		{
			GuildKickOutRequest guildKickOutRequest = new GuildKickOutRequest();
			guildKickOutRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			guildKickOutRequest.UserId = userid;
			IList<GuildUserShareData> memberList = GuildNetUtil.Guild.GuildSDK.GuildInfo.GetMemberList();
			GuildUserShareData kickuser = null;
			for (int i = 0; i < memberList.Count; i++)
			{
				if (memberList[i].UserID == userid)
				{
					kickuser = memberList[i];
					break;
				}
			}
			GuildProxy.Net.TrySendMessage(guildKickOutRequest, delegate(IMessage response)
			{
				GuildKickOutResponse guildKickOutResponse = response as GuildKickOutResponse;
				if (guildKickOutResponse != null && guildKickOutResponse.Code == 0)
				{
					GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = new GuildEvent_GuildMemberChange();
					guildEvent_GuildMemberChange.GuildID = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildID;
					guildEvent_GuildMemberChange.DeleteUser.Add(userid);
					guildEvent_GuildMemberChange.MemberCount = (int)guildKickOutResponse.Members;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(14, guildEvent_GuildMemberChange);
					int myGuildPosition = (int)GuildNetUtil.Guild.GuildSDK.Permission.MyGuildPosition;
					GuildProxy.TGA.OnGuildKicked((int)userid, (int)((kickuser != null) ? kickuser.GuildPosition : ((GuildPositionType)0)), myGuildPosition);
					GameApp.SDK.Analyze.Track_GuildActivity("kick", TGA_GuildActivityData.Create(), new List<long> { userid });
					Action<bool, GuildKickOutResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildKickOutResponse);
					return;
				}
				else
				{
					Action<bool, GuildKickOutResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildKickOutResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_LeaveGuild(Action<bool, GuildLeaveResponse> callback)
		{
			GuildLeaveRequest guildLeaveRequest = new GuildLeaveRequest();
			guildLeaveRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			GuildShareData guilddata = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildData;
			int mypos = (int)GuildNetUtil.Guild.GuildSDK.Permission.MyGuildPosition;
			TGA_GuildActivityData tgaData = TGA_GuildActivityData.Create();
			GuildProxy.Net.TrySendMessage(guildLeaveRequest, delegate(IMessage response)
			{
				GuildLeaveResponse guildLeaveResponse = response as GuildLeaveResponse;
				if (guildLeaveResponse != null && guildLeaveResponse.Code == 0)
				{
					GuildEvent_LoginSuccess guildEvent_LoginSuccess = new GuildEvent_LoginSuccess();
					guildEvent_LoginSuccess.IsJoin = false;
					guildEvent_LoginSuccess.MyGuildShareDetail = null;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(10, guildEvent_LoginSuccess);
					if (guilddata != null)
					{
						GuildProxy.TGA.OnGuildQuit(guilddata.GuildID, guilddata.GuildLevel, mypos);
					}
					GameApp.SDK.Analyze.Track_GuildActivity("quit", tgaData, null);
					Action<bool, GuildLeaveResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildLeaveResponse);
					return;
				}
				else
				{
					Action<bool, GuildLeaveResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildLeaveResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_ChangePosition(long userid, int position, Action<bool, GuildUpPositionResponse> callback)
		{
			GuildUpPositionRequest guildUpPositionRequest = new GuildUpPositionRequest();
			guildUpPositionRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			guildUpPositionRequest.UserId = userid;
			guildUpPositionRequest.Position = (uint)position;
			IList<GuildUserShareData> memberList = GuildNetUtil.Guild.GuildSDK.GuildInfo.GetMemberList();
			GuildUserShareData kickuser = null;
			for (int i = 0; i < memberList.Count; i++)
			{
				if (memberList[i].UserID == userid)
				{
					kickuser = memberList[i];
					break;
				}
			}
			GuildProxy.Net.TrySendMessage(guildUpPositionRequest, delegate(IMessage response)
			{
				GuildUpPositionResponse guildUpPositionResponse = response as GuildUpPositionResponse;
				if (guildUpPositionResponse != null && guildUpPositionResponse.Code == 0)
				{
					GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = new GuildEvent_GuildMemberChange();
					guildEvent_GuildMemberChange.GuildID = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildID;
					guildEvent_GuildMemberChange.UserList.Add(guildUpPositionResponse.GuildMemberInfoDto.ToGuildUser());
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(14, guildEvent_GuildMemberChange);
					int myGuildPosition = (int)GuildNetUtil.Guild.GuildSDK.Permission.MyGuildPosition;
					int num = (int)((kickuser != null) ? kickuser.GuildPosition : ((GuildPositionType)0));
					GuildProxy.TGA.OnGuildJobChange((int)userid, num, position, myGuildPosition);
					Action<bool, GuildUpPositionResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildUpPositionResponse);
					return;
				}
				else
				{
					Action<bool, GuildUpPositionResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildUpPositionResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GuildTransfer(long userid, Action<bool, GuildTransferPresidentResponse> callback)
		{
			GuildTransferPresidentRequest guildTransferPresidentRequest = new GuildTransferPresidentRequest();
			guildTransferPresidentRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			guildTransferPresidentRequest.UserId = userid;
			IList<GuildUserShareData> memberList = GuildNetUtil.Guild.GuildSDK.GuildInfo.GetMemberList();
			GuildUserShareData kickuser = null;
			for (int i = 0; i < memberList.Count; i++)
			{
				if (memberList[i].UserID == userid)
				{
					kickuser = memberList[i];
					break;
				}
			}
			GuildProxy.Net.TrySendMessage(guildTransferPresidentRequest, delegate(IMessage response)
			{
				GuildTransferPresidentResponse guildTransferPresidentResponse = response as GuildTransferPresidentResponse;
				if (guildTransferPresidentResponse != null && guildTransferPresidentResponse.Code == 0)
				{
					GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = new GuildEvent_GuildMemberChange();
					guildEvent_GuildMemberChange.GuildID = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildID;
					guildEvent_GuildMemberChange.UserList.AddRange(guildTransferPresidentResponse.GuildMemberInfoDtos.ToGuidUserList());
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(14, guildEvent_GuildMemberChange);
					int num = (int)((kickuser != null) ? kickuser.GuildPosition : ((GuildPositionType)0));
					GuildProxy.TGA.OnGuildJobChange((int)userid, num, 1, 1);
					Action<bool, GuildTransferPresidentResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildTransferPresidentResponse);
					return;
				}
				else
				{
					Action<bool, GuildTransferPresidentResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildTransferPresidentResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GuildLevelUp(Action<bool, GuildLevelUpResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildLevelUpRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildLevelUpResponse guildLevelUpResponse = response as GuildLevelUpResponse;
				if (guildLevelUpResponse != null && guildLevelUpResponse.Code == 0)
				{
					GuildEvent_GuildLevelUpSetData guildEvent_GuildLevelUpSetData = new GuildEvent_GuildLevelUpSetData();
					guildEvent_GuildLevelUpSetData.GuildUpdateInfo = guildLevelUpResponse.GuildUpdateInfo.ToGuildUpdateData();
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(17, guildEvent_GuildLevelUpSetData);
					if (guildLevelUpResponse.Tasks.Count > 0)
					{
						GuildEvent_GuildTaskSetData guildEvent_GuildTaskSetData = new GuildEvent_GuildTaskSetData();
						if (guildLevelUpResponse.Tasks != null && guildLevelUpResponse.Tasks.Count > 0)
						{
							guildEvent_GuildTaskSetData.TaskList.AddRange(guildLevelUpResponse.Tasks.ToGuidTaskList());
						}
						GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(20, guildEvent_GuildTaskSetData);
					}
					if (guildLevelUpResponse.DailyShop.Count > 0)
					{
						new GuildEvent_GuildShopSetData
						{
							ShopType = 1,
							ShopDataList = new List<GuildShopData>()
						}.ShopDataList.AddRange(guildLevelUpResponse.DailyShop.ToGuildShopList());
						GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(19, guildEvent_GuildLevelUpSetData);
					}
					if (guildLevelUpResponse.WeeklyShop.Count > 0)
					{
						new GuildEvent_GuildShopSetData
						{
							ShopType = 2,
							ShopDataList = new List<GuildShopData>()
						}.ShopDataList.AddRange(guildLevelUpResponse.WeeklyShop.ToGuildShopList());
						GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(19, guildEvent_GuildLevelUpSetData);
					}
					Action<bool, GuildLevelUpResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildLevelUpResponse);
					return;
				}
				else
				{
					Action<bool, GuildLevelUpResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildLevelUpResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GuildSign(Action<bool, GuildSignInResponse> callback)
		{
			GuildSignInRequest guildSignInRequest = new GuildSignInRequest();
			guildSignInRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			GuildSignData guildSignData = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildSignData;
			int signcount = 0;
			int costid = 0;
			int costcount = 0;
			if (guildSignData != null)
			{
				signcount = guildSignData.SignCount + 1;
				if (guildSignData.SignCost != null)
				{
					costid = guildSignData.SignCost.id;
					costcount = guildSignData.SignCost.count;
				}
			}
			GuildProxy.Net.TrySendMessage(guildSignInRequest, delegate(IMessage response)
			{
				GuildSignInResponse guildSignInResponse = response as GuildSignInResponse;
				if (guildSignInResponse != null && guildSignInResponse.Code == 0)
				{
					GuildEvent_GuildSignSetData guildEvent_GuildSignSetData = new GuildEvent_GuildSignSetData();
					guildEvent_GuildSignSetData.SignData = guildSignInResponse.SignInDto.ToGuildSignData();
					guildEvent_GuildSignSetData.UserDailyActive = (int)guildSignInResponse.UserDailyActive;
					guildEvent_GuildSignSetData.UserWeeklyActive = (int)guildSignInResponse.UserWeeklyActive;
					guildEvent_GuildSignSetData.GuildUpdateInfo = guildSignInResponse.GuildUpdateInfo.ToGuildUpdateData();
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(18, guildEvent_GuildSignSetData);
					if (guildSignInResponse.Tasks.Count > 0)
					{
						GuildEvent_GuildTaskSetData guildEvent_GuildTaskSetData = new GuildEvent_GuildTaskSetData();
						if (guildSignInResponse.Tasks != null && guildSignInResponse.Tasks.Count > 0)
						{
							guildEvent_GuildTaskSetData.TaskList.AddRange(guildSignInResponse.Tasks.ToGuidTaskList());
						}
						GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(20, guildEvent_GuildTaskSetData);
					}
					GuildProxy.TGA.OnGuildSign(signcount, costid, costcount);
					GuildProxy.RedPoint.CalcRedPoint("Guild.Hall.Sign", false);
					Action<bool, GuildSignInResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildSignInResponse);
					return;
				}
				else
				{
					Action<bool, GuildSignInResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildSignInResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GuildShopBuy(GuildShopType type, int shopid, Action<bool, GuildShopBuyResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildShopBuyRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				Type = (uint)type,
				ShopId = (uint)shopid
			}, delegate(IMessage response)
			{
				GuildShopBuyResponse guildShopBuyResponse = response as GuildShopBuyResponse;
				if (guildShopBuyResponse != null && guildShopBuyResponse.Code == 0)
				{
					GuildEvent_GuildShopSetData guildEvent_GuildShopSetData = new GuildEvent_GuildShopSetData();
					guildEvent_GuildShopSetData.ShopType = (int)type;
					guildEvent_GuildShopSetData.ShopDataList = new List<GuildShopData>();
					guildEvent_GuildShopSetData.ShopDataList.Add(guildShopBuyResponse.GuildShopDto.ToGuildShopData());
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(19, guildEvent_GuildShopSetData);
					if (guildShopBuyResponse.Tasks.Count > 0)
					{
						GuildEvent_GuildTaskSetData guildEvent_GuildTaskSetData = new GuildEvent_GuildTaskSetData();
						if (guildShopBuyResponse.Tasks != null && guildShopBuyResponse.Tasks.Count > 0)
						{
							guildEvent_GuildTaskSetData.TaskList.AddRange(guildShopBuyResponse.Tasks.ToGuidTaskList());
						}
						GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(20, guildEvent_GuildTaskSetData);
					}
					Action<bool, GuildShopBuyResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildShopBuyResponse);
					return;
				}
				else
				{
					Action<bool, GuildShopBuyResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildShopBuyResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GuildShopRefresh(GuildShopType type, Action<bool, GuildShopRefreshResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildShopRefreshRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				Type = (uint)type
			}, delegate(IMessage response)
			{
				GuildShopRefreshResponse guildShopRefreshResponse = response as GuildShopRefreshResponse;
				if (guildShopRefreshResponse != null && guildShopRefreshResponse.Code == 0)
				{
					GuildEvent_GuildShopSetData guildEvent_GuildShopSetData = new GuildEvent_GuildShopSetData();
					guildEvent_GuildShopSetData.ShopType = (int)type;
					guildEvent_GuildShopSetData.ShopDataList = guildShopRefreshResponse.ShopDto.ToGuildShopList();
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(19, guildEvent_GuildShopSetData);
					Action<bool, GuildShopRefreshResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildShopRefreshResponse);
					return;
				}
				else
				{
					Action<bool, GuildShopRefreshResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildShopRefreshResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GetGuildTaskReward(int taskid, Action<bool, GuildTaskRewardResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildTaskRewardRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				TaskId = (uint)taskid
			}, delegate(IMessage response)
			{
				GuildTaskRewardResponse guildTaskRewardResponse = response as GuildTaskRewardResponse;
				if (guildTaskRewardResponse != null && guildTaskRewardResponse.Code == 0)
				{
					GuildEvent_GuildTaskSetData guildEvent_GuildTaskSetData = new GuildEvent_GuildTaskSetData();
					if (guildTaskRewardResponse.Tasks != null && guildTaskRewardResponse.Tasks.Count > 0)
					{
						guildEvent_GuildTaskSetData.TaskList.AddRange(guildTaskRewardResponse.Tasks.ToGuidTaskList());
					}
					if (guildTaskRewardResponse.UpdateTaskDto != null)
					{
						guildEvent_GuildTaskSetData.TaskList.Add(guildTaskRewardResponse.UpdateTaskDto.ToGuidTaskData());
					}
					guildEvent_GuildTaskSetData.DeleteTaskID = (int)guildTaskRewardResponse.DeleteTaskDtoId;
					guildEvent_GuildTaskSetData.UserDailyActive = (int)guildTaskRewardResponse.UserDailyActive;
					guildEvent_GuildTaskSetData.UserWeeklyActive = (int)guildTaskRewardResponse.UserWeeklyActive;
					if (guildTaskRewardResponse.GuildUpdateInfo != null)
					{
						guildEvent_GuildTaskSetData.UpdateInfo = guildTaskRewardResponse.GuildUpdateInfo.ToGuildUpdateData();
					}
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(20, guildEvent_GuildTaskSetData);
					string text = "";
					if (guildTaskRewardResponse.CommonData.Reward != null)
					{
						text = guildTaskRewardResponse.CommonData.Reward.ToStringBuilder().ToString();
					}
					GuildProxy.TGA.OnGuildTaskGetRewards(taskid, text);
					Action<bool, GuildTaskRewardResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildTaskRewardResponse);
					return;
				}
				else
				{
					Action<bool, GuildTaskRewardResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildTaskRewardResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GuildTaskRefresh(int taskid, Action<bool, GuildTaskRefreshResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildTaskRefreshRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				TaskId = (uint)taskid
			}, delegate(IMessage response)
			{
				GuildTaskRefreshResponse guildTaskRefreshResponse = response as GuildTaskRefreshResponse;
				if (guildTaskRefreshResponse != null && guildTaskRefreshResponse.Code == 0)
				{
					GuildEvent_GuildTaskSetData guildEvent_GuildTaskSetData = new GuildEvent_GuildTaskSetData();
					guildEvent_GuildTaskSetData.TaskList.Add(guildTaskRefreshResponse.GuildTask.ToGuidTaskData());
					guildEvent_GuildTaskSetData.DeleteTaskID = 0;
					guildEvent_GuildTaskSetData.UserDailyActive = -1;
					guildEvent_GuildTaskSetData.UserWeeklyActive = -1;
					guildEvent_GuildTaskSetData.UpdateInfo = null;
					guildEvent_GuildTaskSetData.RefreshData = new GuildTaskRefreshData
					{
						TaskRefreshMaxCount = -1,
						TaskRefreshCount = (int)guildTaskRefreshResponse.TaskRefreshCount,
						RefreshCost = new GuildItemData
						{
							id = -1,
							count = (int)guildTaskRefreshResponse.TaskRefreshCost
						}
					};
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(20, guildEvent_GuildTaskSetData);
					GuildProxy.TGA.OnGuildTaskRefresh(taskid);
					Action<bool, GuildTaskRefreshResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildTaskRefreshResponse);
					return;
				}
				else
				{
					Action<bool, GuildTaskRefreshResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildTaskRefreshResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GetGuildBossInfo(Action<bool, GuildBossGetInfoResponse> callback, bool showMask = true)
		{
			GuildProxy.Net.TrySendMessage(new GuildBossGetInfoRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildBossGetInfoResponse guildBossGetInfoResponse = response as GuildBossGetInfoResponse;
				if (guildBossGetInfoResponse != null && guildBossGetInfoResponse.Code == 0)
				{
					GuildEvent_SetGuildBossInfo guildEvent_SetGuildBossInfo = new GuildEvent_SetGuildBossInfo();
					guildEvent_SetGuildBossInfo.Info = guildBossGetInfoResponse.GuildBossInfo.ToGuildBossInfo();
					guildEvent_SetGuildBossInfo.Info.IsFullChallengeRecords = true;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(202, guildEvent_SetGuildBossInfo);
					GuildProxy.RedPoint.CalcRedPoint("Guild.Boss.Challenge", true);
					GuildProxy.RedPoint.CalcRedPoint("Guild.Boss.Task", true);
					GuildProxy.RedPoint.CalcRedPoint("Guild.Boss.BoxReward", true);
					Action<bool, GuildBossGetInfoResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildBossGetInfoResponse);
					return;
				}
				else
				{
					Action<bool, GuildBossGetInfoResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildBossGetInfoResponse);
					return;
				}
			}, showMask, false, true);
		}

		public static void DoRequest_GuildBossStart(Action<bool, GuildBossStartResponse> callback)
		{
			GuildBossStartRequest guildBossStartRequest = new GuildBossStartRequest();
			guildBossStartRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			ulong transid = guildBossStartRequest.CommonParams.TransId;
			GuildProxy.Net.TrySendMessage(guildBossStartRequest, delegate(IMessage response)
			{
				GuildBossStartResponse guildBossStartResponse = response as GuildBossStartResponse;
				if (guildBossStartResponse != null && guildBossStartResponse.Code == 0)
				{
					GuildEvent_GuildBossSetTransID guildEvent_GuildBossSetTransID = new GuildEvent_GuildBossSetTransID();
					guildEvent_GuildBossSetTransID.BossBattleTransID = transid;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(205, guildEvent_GuildBossSetTransID);
					if (guildBossStartResponse.Tasks.Count > 0)
					{
						GuildEvent_GuildTaskSetData guildEvent_GuildTaskSetData = new GuildEvent_GuildTaskSetData();
						if (guildBossStartResponse.Tasks != null && guildBossStartResponse.Tasks.Count > 0)
						{
							guildEvent_GuildTaskSetData.TaskList.AddRange(guildBossStartResponse.Tasks.ToGuidTaskList());
						}
						GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(20, guildEvent_GuildTaskSetData);
					}
					GuildEvent_GuildBossCount guildEvent_GuildBossCount = new GuildEvent_GuildBossCount();
					guildEvent_GuildBossCount.ChallengeCount = (int)guildBossStartResponse.ChallengeCnt;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(201, guildEvent_GuildBossCount);
					Action<bool, GuildBossStartResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildBossStartResponse);
					return;
				}
				else
				{
					Action<bool, GuildBossStartResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildBossStartResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GuildBossBuyCount(GuildBossBuyKind buykind, Action<bool, GuildBossBuyCntResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildBossBuyCntRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				Type = (uint)buykind
			}, delegate(IMessage response)
			{
				GuildBossBuyCntResponse guildBossBuyCntResponse = response as GuildBossBuyCntResponse;
				if (guildBossBuyCntResponse != null && guildBossBuyCntResponse.Code == 0)
				{
					GuildEvent_GuildBossCount guildEvent_GuildBossCount = new GuildEvent_GuildBossCount();
					guildEvent_GuildBossCount.ChallengeCount = (int)guildBossBuyCntResponse.ChallengeCnt;
					guildEvent_GuildBossCount.SetGoldBuyCount((int)guildBossBuyCntResponse.BuyCntByCoins, (int)guildBossBuyCntResponse.BuyCntCostByCoins, -1);
					guildEvent_GuildBossCount.SetDiamondsBuyCount((int)guildBossBuyCntResponse.BuyCntByDiamonds, (int)guildBossBuyCntResponse.BuyCntCostByDiamonds, -1);
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(201, guildEvent_GuildBossCount);
					Action<bool, GuildBossBuyCntResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildBossBuyCntResponse);
					return;
				}
				else
				{
					Action<bool, GuildBossBuyCntResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildBossBuyCntResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GetGuildBossTaskReward(int taskid, Action<bool, GuildBossTaskRewardResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildBossTaskRewardRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				TaskId = (uint)taskid
			}, delegate(IMessage response)
			{
				GuildBossTaskRewardResponse guildBossTaskRewardResponse = response as GuildBossTaskRewardResponse;
				if (guildBossTaskRewardResponse != null && guildBossTaskRewardResponse.Code == 0)
				{
					GuildEvent_SetGuildBossTaskData guildEvent_SetGuildBossTaskData = new GuildEvent_SetGuildBossTaskData();
					guildEvent_SetGuildBossTaskData.TaskList = new List<GuildBossTask> { guildBossTaskRewardResponse.GuildBossTaskDto.ToGuildBossTask() };
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(203, guildEvent_SetGuildBossTaskData);
					GuildProxy.RedPoint.CalcRedPoint("Guild.Boss.Task", true);
					Action<bool, GuildBossTaskRewardResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildBossTaskRewardResponse);
					return;
				}
				else
				{
					Action<bool, GuildBossTaskRewardResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildBossTaskRewardResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GetGuildBossBoxReward(int boxid, Action<bool, GuildBossBoxRewardResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildBossBoxRewardRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				BoxId = (uint)boxid
			}, delegate(IMessage response)
			{
				GuildBossBoxRewardResponse guildBossBoxRewardResponse = response as GuildBossBoxRewardResponse;
				if (guildBossBoxRewardResponse != null && guildBossBoxRewardResponse.Code == 0)
				{
					GuildEvent_SetGuildBossBoxData guildEvent_SetGuildBossBoxData = new GuildEvent_SetGuildBossBoxData();
					guildEvent_SetGuildBossBoxData.BoxList = new List<GuildBossKillBox> { guildBossBoxRewardResponse.GuildBossKillBoxDto.ToGuildBossKillData() };
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(204, guildEvent_SetGuildBossBoxData);
					Action<bool, GuildBossBoxRewardResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildBossBoxRewardResponse);
					return;
				}
				else
				{
					Action<bool, GuildBossBoxRewardResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildBossBoxRewardResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GetGuildBossChallengeList(Action<bool, GuildBossGetChallengeListResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildBossGetChallengeListRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildBossGetChallengeListResponse guildBossGetChallengeListResponse = response as GuildBossGetChallengeListResponse;
				if (guildBossGetChallengeListResponse != null && guildBossGetChallengeListResponse.Code == 0)
				{
					GuildEvent_SetGuildUserBossRankList guildEvent_SetGuildUserBossRankList = new GuildEvent_SetGuildUserBossRankList();
					guildEvent_SetGuildUserBossRankList.RankList = guildBossGetChallengeListResponse.BossRankList.ToGuildUserBossRankList();
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(206, guildEvent_SetGuildUserBossRankList);
					Action<bool, GuildBossGetChallengeListResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildBossGetChallengeListResponse);
					return;
				}
				else
				{
					Action<bool, GuildBossGetChallengeListResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildBossGetChallengeListResponse);
					return;
				}
			}, true, false, true);
		}

		public static void DoRequest_GuildBossStartBattle(Action<bool, GuildBossStartBattleResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildBossStartBattleRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildBossStartBattleResponse guildBossStartBattleResponse = response as GuildBossStartBattleResponse;
				if (guildBossStartBattleResponse != null && guildBossStartBattleResponse.Code == 0)
				{
					GameApp.Data.GetDataModule(DataName.BattleGuildBossDataModule).SetBattleSkillSeed(guildBossStartBattleResponse.Seed);
					Action<bool, GuildBossStartBattleResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildBossStartBattleResponse);
					return;
				}
				else
				{
					Action<bool, GuildBossStartBattleResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildBossStartBattleResponse);
					return;
				}
			}, false, false, false);
		}

		public static void DoRequest_GuildBossEndBattle(List<int> skillBuilds, Action<bool, GuildBossEndBattleResponse> callback)
		{
			GuildBossEndBattleRequest guildBossEndBattleRequest = new GuildBossEndBattleRequest();
			guildBossEndBattleRequest.CommonParams = GuildProxy.Net.GetCommonParams();
			for (int i = 0; i < skillBuilds.Count; i++)
			{
				guildBossEndBattleRequest.RoundSkillList.Add(skillBuilds[i]);
			}
			int bossID = GuildSDKManager.Instance.GuildActivity.GuildBoss.BossData.BossID;
			int bossLevel = GuildSDKManager.Instance.GuildActivity.GuildBoss.BossData.BossStep;
			GuildProxy.Net.TrySendMessage(guildBossEndBattleRequest, delegate(IMessage response)
			{
				GuildBossEndBattleResponse guildBossEndBattleResponse = response as GuildBossEndBattleResponse;
				if (guildBossEndBattleResponse != null && guildBossEndBattleResponse.Code == 0)
				{
					EventArgsBattleGuildBossEnter eventArgsBattleGuildBossEnter = new EventArgsBattleGuildBossEnter();
					eventArgsBattleGuildBossEnter.Response = guildBossEndBattleResponse;
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_GuildBoss_BattleGuildBossEnter, eventArgsBattleGuildBossEnter);
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(206, new GuildEvent_SetGuildUserBossRankList());
					GuildEvent_GuildBossCount guildEvent_GuildBossCount = new GuildEvent_GuildBossCount();
					guildEvent_GuildBossCount.ChallengeCount = (int)guildBossEndBattleResponse.ChallengeCnt;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(201, guildEvent_GuildBossCount);
					GuildEvent_SetGuildBossInfo guildEvent_SetGuildBossInfo = new GuildEvent_SetGuildBossInfo();
					guildEvent_SetGuildBossInfo.Info = guildBossEndBattleResponse.GuildBossInfo.ToGuildBossInfo();
					guildEvent_SetGuildBossInfo.Info.IsFullChallengeRecords = true;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(202, guildEvent_SetGuildBossInfo);
					GuildProxy.RedPoint.CalcRedPoint("Guild.Boss.Challenge", true);
					GuildProxy.RedPoint.CalcRedPoint("Guild.Boss.Task", true);
					GuildProxy.RedPoint.CalcRedPoint("Guild.Boss.BoxReward", true);
					Action<bool, GuildBossEndBattleResponse> callback2 = callback;
					if (callback2 != null)
					{
						callback2(true, guildBossEndBattleResponse);
					}
					GameApp.SDK.Analyze.Track_GuildBossBattleEnd(bossID, bossLevel, guildBossEndBattleResponse.Damage);
					return;
				}
				Action<bool, GuildBossEndBattleResponse> callback3 = callback;
				if (callback3 == null)
				{
					return;
				}
				callback3(false, guildBossEndBattleResponse);
			}, false, false, true);
		}

		public static void DoRequest_GetGuildBossGuildRankList(int page, bool isNextPage, bool isShowMask, int type, Action<int, bool, bool, GuildBossBattleGRankResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildBossBattleGRankRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				Page = page,
				OptType = type
			}, delegate(IMessage response)
			{
				GuildBossBattleGRankResponse guildBossBattleGRankResponse = response as GuildBossBattleGRankResponse;
				if (guildBossBattleGRankResponse != null && guildBossBattleGRankResponse.Code == 0)
				{
					GuildEvent_SetGuildBossGuildRankList guildEvent_SetGuildBossGuildRankList = new GuildEvent_SetGuildBossGuildRankList();
					guildEvent_SetGuildBossGuildRankList.RankList = guildBossBattleGRankResponse.Dtos.ToGuildBossGuildRankList();
					guildEvent_SetGuildBossGuildRankList.MyRank = guildBossBattleGRankResponse.MyRank;
					guildEvent_SetGuildBossGuildRankList.MyRankData = guildBossBattleGRankResponse.MyGuildDto.ToGuildBossGuildRankData((int)guildBossBattleGRankResponse.MyRank);
					guildEvent_SetGuildBossGuildRankList.Type = type;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(207, guildEvent_SetGuildBossGuildRankList);
					Action<int, bool, bool, GuildBossBattleGRankResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(page, isNextPage, true, guildBossBattleGRankResponse);
					return;
				}
				else
				{
					Action<int, bool, bool, GuildBossBattleGRankResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(page, isNextPage, false, guildBossBattleGRankResponse);
					return;
				}
			}, isShowMask, false, true);
		}

		public static void DoRequest_GetGuildBossBoxKillReward(Action<bool, GuildBossKilledRewardResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildBossKilledRewardRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildBossKilledRewardResponse guildBossKilledRewardResponse = response as GuildBossKilledRewardResponse;
				if (guildBossKilledRewardResponse != null && guildBossKilledRewardResponse.Code == 0)
				{
					GuildEvent_SetGuildBossBoxRewardData guildEvent_SetGuildBossBoxRewardData = new GuildEvent_SetGuildBossBoxRewardData();
					guildEvent_SetGuildBossBoxRewardData.KillRewardList = new List<int>();
					guildEvent_SetGuildBossBoxRewardData.KillRewardList.AddRange(guildBossKilledRewardResponse.KilledBossList);
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(210, guildEvent_SetGuildBossBoxRewardData);
					GuildProxy.RedPoint.CalcRedPoint("Guild.Boss.BoxReward", true);
					Action<bool, GuildBossKilledRewardResponse> callback2 = callback;
					if (callback2 != null)
					{
						callback2(true, guildBossKilledRewardResponse);
					}
					GameApp.SDK.Analyze.Track_GuildBossReward(guildBossKilledRewardResponse.CommonData.Reward);
					return;
				}
				Action<bool, GuildBossKilledRewardResponse> callback3 = callback;
				if (callback3 == null)
				{
					return;
				}
				callback3(false, guildBossKilledRewardResponse);
			}, true, false, true);
		}

		public static void DoRequest_GuildGetMessageRecords(long msgid, Action<bool, GuildGetMessageRecordsResponse> callback)
		{
			if (GuildProxy.Net.SocketNet.Connected)
			{
				GuildGetMessageRecordsRequest guildGetMessageRecordsRequest = new GuildGetMessageRecordsRequest();
				guildGetMessageRecordsRequest.CommonParams = GuildProxy.Net.GetCommonParams();
				guildGetMessageRecordsRequest.MsgId = (ulong)msgid;
				string text = (GuildNetUtil.Guild.GuildSDK.GuildInfo.HasGuild ? GuildNetUtil.Guild.GuildSDK.GuildInfo.IMGroupId : "");
				ChatGroupData group = GuildProxy.Chat.GetGuildChatGroup(text);
				guildGetMessageRecordsRequest.PageIndex = (uint)group.HistoryPage;
				GuildProxy.Net.TrySendMessage(guildGetMessageRecordsRequest, delegate(IMessage response)
				{
					GuildGetMessageRecordsResponse guildGetMessageRecordsResponse = response as GuildGetMessageRecordsResponse;
					if (guildGetMessageRecordsResponse != null && guildGetMessageRecordsResponse.Code == 0)
					{
						GuildProxy.Chat.OnRecvGuildChatRecords(group, guildGetMessageRecordsResponse);
						Action<bool, GuildGetMessageRecordsResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(true, guildGetMessageRecordsResponse);
						return;
					}
					else
					{
						Action<bool, GuildGetMessageRecordsResponse> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, guildGetMessageRecordsResponse);
						return;
					}
				}, false, false, true);
				return;
			}
			GuildProxy.Net.SocketNet.CheckReconnect("Guild chat");
			Action<bool, GuildGetMessageRecordsResponse> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(false, null);
		}

		public static void DoRequest_GuildLog(Action<bool, GuildLogResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildLogRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildLogResponse guildLogResponse = response as GuildLogResponse;
				if (guildLogResponse != null && guildLogResponse.Code == 0)
				{
					Action<bool, GuildLogResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildLogResponse);
					return;
				}
				else
				{
					Action<bool, GuildLogResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildLogResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GuildRaceGuildApplyRequest(int type, Action<bool, GuildRaceGuildApplyResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildRaceGuildApplyRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				GuildId = GuildNetUtil.Guild.GetULongGuildID(GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildID),
				Type = (uint)type
			}, delegate(IMessage response)
			{
				GuildRaceGuildApplyResponse guildRaceGuildApplyResponse = response as GuildRaceGuildApplyResponse;
				if (guildRaceGuildApplyResponse != null && guildRaceGuildApplyResponse.Code == 0)
				{
					GuildEvent_RaceApply guildEvent_RaceApply = new GuildEvent_RaceApply();
					guildEvent_RaceApply.IsGuildApply = 1;
					guildEvent_RaceApply.IsUserApply = -1;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(402, guildEvent_RaceApply);
					GuildProxy.TGA.OnGuildRaceGuildApply(type);
					Action<bool, GuildRaceGuildApplyResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildRaceGuildApplyResponse);
					return;
				}
				else
				{
					Action<bool, GuildRaceGuildApplyResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildRaceGuildApplyResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GuildRaceUserApplyRequest(Action<bool, GuildRaceUserApplyResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildRaceUserApplyRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildRaceUserApplyResponse guildRaceUserApplyResponse = response as GuildRaceUserApplyResponse;
				if (guildRaceUserApplyResponse != null && guildRaceUserApplyResponse.Code == 0)
				{
					GuildEvent_RaceApply guildEvent_RaceApply = new GuildEvent_RaceApply();
					guildEvent_RaceApply.IsGuildApply = -1;
					guildEvent_RaceApply.IsUserApply = 1;
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(402, guildEvent_RaceApply);
					GuildProxy.TGA.OnGuildRaceUserApply();
					Action<bool, GuildRaceUserApplyResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildRaceUserApplyResponse);
					return;
				}
				else
				{
					Action<bool, GuildRaceUserApplyResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildRaceUserApplyResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GuildRaceEditSeqRequest(uint position, long userid, Action<bool, GuildRaceEditSeqResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildRaceEditSeqRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				OpSeq = position,
				TargetUserId = userid
			}, delegate(IMessage response)
			{
				GuildRaceEditSeqResponse guildRaceEditSeqResponse = response as GuildRaceEditSeqResponse;
				if (guildRaceEditSeqResponse != null && guildRaceEditSeqResponse.Code == 0)
				{
					Action<bool, GuildRaceEditSeqResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildRaceEditSeqResponse);
					return;
				}
				else
				{
					Action<bool, GuildRaceEditSeqResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildRaceEditSeqResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GuildRaceInfoRequest(Action<bool, GuildRaceInfoResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildRaceInfoRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				GuildId = GuildNetUtil.Guild.GetULongGuildID(GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildID)
			}, delegate(IMessage response)
			{
				GuildRaceInfoResponse guildRaceInfoResponse = response as GuildRaceInfoResponse;
				if (guildRaceInfoResponse != null && guildRaceInfoResponse.Code == 0)
				{
					GuildProxy.Table.GuildRaceDan = (int)guildRaceInfoResponse.LastDon;
					string guildID = GuildNetUtil.Guild.GuildSDK.GuildInfo.GuildID;
					GuildEvent_RaceSetBaseInfo guildEvent_RaceSetBaseInfo = new GuildEvent_RaceSetBaseInfo();
					guildEvent_RaceSetBaseInfo.RaceStage = (int)guildRaceInfoResponse.Stage;
					guildEvent_RaceSetBaseInfo.SeasonID = (int)guildRaceInfoResponse.Type;
					guildEvent_RaceSetBaseInfo.SeasonStartTime = guildRaceInfoResponse.InitTime;
					guildEvent_RaceSetBaseInfo.IsUserApply = guildRaceInfoResponse.UserApply;
					guildEvent_RaceSetBaseInfo.MyGuildInfo = guildRaceInfoResponse.RaceGuild.ToGuildRaceGuild();
					guildEvent_RaceSetBaseInfo.AllGuild = guildRaceInfoResponse.GroupRaceGuilds.ToGuildRaceGuildList();
					List<GuildRaceGuild> allGuild = guildEvent_RaceSetBaseInfo.AllGuild;
					for (int i = 0; i < allGuild.Count; i++)
					{
						GuildRaceGuild guildRaceGuild = allGuild[i];
						if (guildRaceGuild.GuildID == guildID)
						{
							guildEvent_RaceSetBaseInfo.MyGuildInfo = guildRaceGuild;
							break;
						}
					}
					if (guildRaceInfoResponse.LastDon != 0U)
					{
						guildEvent_RaceSetBaseInfo.MyGuildInfo.RaceDan = (int)guildRaceInfoResponse.LastDon;
					}
					int raceSeasonTotalTimeMinute = GuildProxy.Table.GetRaceSeasonTotalTimeMinute(guildEvent_RaceSetBaseInfo.SeasonID);
					ulong num = (ulong)GuildProxy.Net.ServerTime();
					guildEvent_RaceSetBaseInfo.CheckRealSeasonStartTime((ulong)((long)(raceSeasonTotalTimeMinute * 60)), num);
					GuildNetUtil.Guild.GuildSDK.Event.DispatchNow(401, guildEvent_RaceSetBaseInfo);
					Action<bool, GuildRaceInfoResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildRaceInfoResponse);
					return;
				}
				else
				{
					Action<bool, GuildRaceInfoResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildRaceInfoResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GuildRaceOwnerUserApplyListRequest(Action<bool, GuildRaceOwnerUserApplyListResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildRaceOwnerUserApplyListRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildRaceOwnerUserApplyListResponse guildRaceOwnerUserApplyListResponse = response as GuildRaceOwnerUserApplyListResponse;
				if (guildRaceOwnerUserApplyListResponse != null && guildRaceOwnerUserApplyListResponse.Code == 0)
				{
					Action<bool, GuildRaceOwnerUserApplyListResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildRaceOwnerUserApplyListResponse);
					return;
				}
				else
				{
					Action<bool, GuildRaceOwnerUserApplyListResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildRaceOwnerUserApplyListResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GuildRaceOppUserApplyListRequest(Action<bool, GuildRaceOppUserApplyListResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildRaceOppUserApplyListRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams()
			}, delegate(IMessage response)
			{
				GuildRaceOppUserApplyListResponse guildRaceOppUserApplyListResponse = response as GuildRaceOppUserApplyListResponse;
				if (guildRaceOppUserApplyListResponse != null && guildRaceOppUserApplyListResponse.Code == 0)
				{
					Action<bool, GuildRaceOppUserApplyListResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildRaceOppUserApplyListResponse);
					return;
				}
				else
				{
					Action<bool, GuildRaceOppUserApplyListResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildRaceOppUserApplyListResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GuildRaceUserInfoRequest(long userId, Action<bool, GuildRaceUserInfoResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildRaceUserInfoRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				UserId = userId
			}, delegate(IMessage response)
			{
				GuildRaceUserInfoResponse guildRaceUserInfoResponse = response as GuildRaceUserInfoResponse;
				if (guildRaceUserInfoResponse != null && guildRaceUserInfoResponse.Code == 0)
				{
					Action<bool, GuildRaceUserInfoResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildRaceUserInfoResponse);
					return;
				}
				else
				{
					Action<bool, GuildRaceUserInfoResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildRaceUserInfoResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GuildRaceGuildRecordRequest(int day, Action<bool, GuildRaceGuildRecordResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildRaceGuildRecordRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				Day = (uint)day
			}, delegate(IMessage response)
			{
				GuildRaceGuildRecordResponse guildRaceGuildRecordResponse = response as GuildRaceGuildRecordResponse;
				if (guildRaceGuildRecordResponse != null && guildRaceGuildRecordResponse.Code == 0)
				{
					Action<bool, GuildRaceGuildRecordResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildRaceGuildRecordResponse);
					return;
				}
				else
				{
					Action<bool, GuildRaceGuildRecordResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildRaceGuildRecordResponse);
					return;
				}
			}, false, false, true);
		}

		public static void DoRequest_GuildRacePVPRecordRequest(ulong recordid, Action<bool, GuildRacePVPRecordResponse> callback)
		{
			GuildProxy.Net.TrySendMessage(new GuildRacePVPRecordRequest
			{
				CommonParams = GuildProxy.Net.GetCommonParams(),
				RecordId = recordid
			}, delegate(IMessage response)
			{
				GuildRacePVPRecordResponse guildRacePVPRecordResponse = response as GuildRacePVPRecordResponse;
				if (guildRacePVPRecordResponse != null && guildRacePVPRecordResponse.Code == 0)
				{
					Action<bool, GuildRacePVPRecordResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, guildRacePVPRecordResponse);
					return;
				}
				else
				{
					Action<bool, GuildRacePVPRecordResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, guildRacePVPRecordResponse);
					return;
				}
			}, false, false, true);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Dxx.Chat;
using Framework;
using Framework.Logic.Platform;
using Google.Protobuf;
using Google.Protobuf.Collections;
using HabbySDK.WebGame;
using HotFix.EventArgs;
using LocalModels.Bean;
using Proto.Activity;
using Proto.Actor;
using Proto.ActTime;
using Proto.Artifact;
using Proto.Chapter;
using Proto.Chat;
using Proto.Collection;
using Proto.Common;
using Proto.Conquer;
using Proto.CrossArena;
using Proto.Dungeon;
using Proto.Equip;
using Proto.IntegralShop;
using Proto.Item;
using Proto.LeaderBoard;
using Proto.Mining;
using Proto.Mission;
using Proto.Mount;
using Proto.NewWorld;
using Proto.Pay;
using Proto.Pet;
using Proto.Relic;
using Proto.ServerList;
using Proto.SevenDayTask;
using Proto.SignIn;
using Proto.Social;
using Proto.Talents;
using Proto.Task;
using Proto.Tower;
using Proto.User;
using Server;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public class NetworkUtils
	{
		public static void ChapterBattleLogRequest(string logData, Action<bool, ChapterBattleLogResponse> callback)
		{
			ChapterBattleLogRequest chapterBattleLogRequest = new ChapterBattleLogRequest();
			chapterBattleLogRequest.CommonParams = NetworkUtils.GetCommonParams();
			chapterBattleLogRequest.ChapterId = 1;
			chapterBattleLogRequest.Day = 1;
			chapterBattleLogRequest.BossId = 1;
			chapterBattleLogRequest.LogData = logData;
			HLog.LogError(logData);
			GameApp.NetWork.Send(chapterBattleLogRequest, delegate(IMessage response)
			{
				ChapterBattleLogResponse chapterBattleLogResponse = response as ChapterBattleLogResponse;
				if (chapterBattleLogResponse != null && chapterBattleLogResponse.Code == 0)
				{
					Action<bool, ChapterBattleLogResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, chapterBattleLogResponse);
					return;
				}
				else
				{
					Action<bool, ChapterBattleLogResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, chapterBattleLogResponse);
					return;
				}
			}, false, false, string.Empty, true);
		}

		public static CommonParams GetCommonParams()
		{
			CommonParams commonParams = new CommonParams();
			commonParams.Version = (uint)GameApp.NetWork.Version;
			commonParams.AccountId = GameApp.NetWork.m_account;
			commonParams.DeviceId = GameApp.NetWork.m_deviceID;
			commonParams.TransId = GameApp.NetWork.m_transId;
			commonParams.ServerId = GameApp.NetWork.m_serverID;
			commonParams.ClientVersion = (uint)Singleton<BattleServerVersionMgr>.Instance.BattleServerVersion;
			commonParams.PackageVersion = GameApp.Config.GetString("Version");
			commonParams.HotFixVersion = Singleton<HotfixVersionMgr>.Instance.ResVersion;
			commonParams.LanguageMark = Application.systemLanguage.ToString();
			if (!string.IsNullOrEmpty(NetworkUtils.m_paramToken))
			{
				commonParams.AccessToken = NetworkUtils.m_paramToken;
			}
			return commonParams;
		}

		public static void SetCommonData(CommonData commonData)
		{
		}

		public static void HandleResponse_CommonDataInternal(CommonData data)
		{
			if (data != null)
			{
				NetworkUtils.HandleResponse_UpdateUserCurrency(data.UpdateUserCurrency, true);
				NetworkUtils.HandleResponse_UpdateUserTicket(data.UserTickets);
				NetworkUtils.HandleResponse_UpdateUserLevel(data.UpdateUserLevel, true);
				NetworkUtils.HandleResponse_UpdateUserVIPLevel(data.UpdateUserVipLevel, true);
				NetworkUtils.HandleResponse_UpdateProps(data.Items);
				NetworkUtils.HandleResponse_UpdateEquips(data.Equipment);
				NetworkUtils.HandleResponse_UpdatePets(data.PetDto);
				NetworkUtils.HandleResponse_UpdateHeros(data.Heros);
				NetworkUtils.HandleResponse_UpdateTasks(data.UpdateTasks);
				NetworkUtils.HandleResponse_UpdateBattlePassScore(data.BattlePassScore);
				NetworkUtils.HandleResponse_UpdateRelics(data.Relics);
				NetworkUtils.HandleResponse_UpdateHeroLevelUp(data.Actor);
				NetworkUtils.HandleResponse_UpdateSevenDayCarnival(data.UpdateTaskDto);
				NetworkUtils.HandleResponse_UpdateCollection(data.CollectionDto);
				NetworkUtils.HandleResponse_UpdateUserStatisticInfo(data.UserStatisticInfo);
				NetworkUtils.HandleResponse_UpdatePushGift(data.PushIapDto);
				NetworkUtils.HandleResponse_UpdateUserInfo(data.UserInfoDto);
				NetworkUtils.HandleResponse_UpdateConsumeData(data.ConsumeData);
				NetworkUtils.HandleResponse_UpdateTurnTableTaskDto(data.TurnTableTasks);
				NetworkUtils.HandleResponse_UpdateChainPackActDto(data);
				NetworkUtils.HandleResponse_UpdateChainPackPushActDto(data);
				if (data.SetCloseFunction)
				{
					NetworkUtils.HandleResponse_UpdateCloseFunction(data.CloseFunctionId);
				}
			}
		}

		public static void HandleResponse_UpdateUserCurrency(UpdateUserCurrency currency, bool isUpdate)
		{
			if (currency != null && currency.IsChange)
			{
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				EventArgsCurrencyChanged instance = Singleton<EventArgsCurrencyChanged>.Instance;
				instance.Clear();
				instance.OldCurrency = dataModule.userCurrency;
				dataModule.UpdateCurrency(currency);
				if (isUpdate)
				{
					instance.NewCurrency = dataModule.userCurrency;
					GameApp.Event.DispatchNow(null, 122, instance);
				}
			}
		}

		private static void HandleResponse_UpdateUserLevel(UpdateUserLevel updateUserLevel, bool isUpdate)
		{
			if (updateUserLevel != null && updateUserLevel.IsChange)
			{
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				UserLevel userLevel = dataModule.userLevel;
				dataModule.UpdateUserLevel(updateUserLevel);
				if (isUpdate)
				{
					EventUserLevelUp eventUserLevelUp = new EventUserLevelUp();
					eventUserLevelUp.OldLevel = userLevel;
					eventUserLevelUp.NewLevel = updateUserLevel.UserLevel;
					eventUserLevelUp.Rewards = updateUserLevel.LevelUpReward;
					GameApp.Event.DispatchNow(null, 128, eventUserLevelUp);
				}
			}
		}

		private static void HandleResponse_UpdateUserTicket(IList<UserTicket> userTickets)
		{
			if (userTickets == null || userTickets.Count == 0)
			{
				return;
			}
			TicketDataModule dataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			for (int i = 0; i < userTickets.Count; i++)
			{
				dataModule.UpdateUserTicket(userTickets[i]);
			}
		}

		private static void HandleResponse_UpdateRelics(RepeatedField<RelicDto> dataRelics)
		{
			if (dataRelics == null || dataRelics.Count <= 0)
			{
				return;
			}
			EventArgsUpdateRelicData instance = Singleton<EventArgsUpdateRelicData>.Instance;
			instance.SetData(dataRelics);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_UpdateRelicsData, instance);
		}

		private static void HandleResponse_UpdateUserVIPLevel(UpdateUserVipLevel updateUserVipLevel, bool isUpdate)
		{
			if (updateUserVipLevel != null && updateUserVipLevel.IsChange)
			{
				UserVipLevel userVipLevel = null;
				if (isUpdate)
				{
					userVipLevel = GameApp.Data.GetDataModule(DataName.VIPDataModule).UserVip;
				}
				EventArgsUserVIPLevelUpdata eventArgsUserVIPLevelUpdata = new EventArgsUserVIPLevelUpdata();
				eventArgsUserVIPLevelUpdata.SetData(userVipLevel, updateUserVipLevel.UserVipLevel);
				GameApp.Event.DispatchNow(null, 129, eventArgsUserVIPLevelUpdata);
			}
		}

		public static void HandleResponse_UpdateProps(MapField<ulong, ItemDto> mapItems)
		{
			if (mapItems == null || mapItems.Count == 0)
			{
				return;
			}
			GameApp.Data.GetDataModule(DataName.PropDataModule).UpdateItemsForUse(mapItems);
			RedPointHelper.ItemChangeCheck(mapItems);
		}

		public static void HandleResponse_UpdateEquips(RepeatedField<EquipmentDto> equips)
		{
			if (equips == null || equips.Count == 0)
			{
				return;
			}
			EventArgsUpdateEquipDatas eventArgsUpdateEquipDatas = new EventArgsUpdateEquipDatas();
			eventArgsUpdateEquipDatas.SetData(equips);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_EquipDataModule_UpdateEquipDatas, eventArgsUpdateEquipDatas);
			RedPointHelper.EquipChangeCheck(equips);
		}

		public static void HandleResponse_UpdatePets(RepeatedField<PetDto> pets)
		{
			if (pets == null || pets.Count == 0)
			{
				return;
			}
			EventArgsUpdatePetData eventArgsUpdatePetData = new EventArgsUpdatePetData();
			eventArgsUpdatePetData.SetData(pets);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetDataModule_UpdatePetData, eventArgsUpdatePetData);
			RedPointHelper.PetChangeCheck(pets);
		}

		public static void HandleResponse_UpdateCollection(RepeatedField<CollectionDto> dtos)
		{
			if (dtos == null || dtos.Count <= 0)
			{
				return;
			}
			EventArgsCollectionUpdate eventArgsCollectionUpdate = new EventArgsCollectionUpdate();
			eventArgsCollectionUpdate.SetData(dtos);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_CollectionDataModule_CollectionUpdate, eventArgsCollectionUpdate);
			RedPointHelper.CollectionChangeCheck(dtos);
		}

		public static void HandleResponse_UpdatePushGift(PushIapDto pushIapDto)
		{
			if (pushIapDto == null)
			{
				return;
			}
			GameApp.Data.GetDataModule(DataName.PushGiftDataModule).UpdateData(pushIapDto);
		}

		public static void HandleResponse_UpdateUserStatisticInfo(UserStatisticInfo userStatisticInfo)
		{
			if (userStatisticInfo == null || userStatisticInfo.DataMap == null || userStatisticInfo.DataMap.Count <= 0)
			{
				return;
			}
			GameApp.Data.GetDataModule(DataName.CollectionDataModule).UpdateUserStatisticInfo(userStatisticInfo);
		}

		public static void HandleResponse_UpdateHeros(RepeatedField<HeroDto> heros)
		{
			if (heros == null || heros.Count <= 0)
			{
				return;
			}
			HLog.LogError("未完成：通用返回，处理英雄列表");
		}

		public static void HandleResponse_UpdateTasks(RepeatedField<TaskDto> updateTasks)
		{
			if (updateTasks == null || updateTasks.Count <= 0)
			{
				return;
			}
			GameApp.Data.GetDataModule(DataName.TaskDataModule).RefreshDataFromServer(updateTasks);
			RedPointController.Instance.ReCalc("Main.Mission", true);
		}

		public static void HandleResponse_UpdateBattlePassScore(uint score)
		{
			if (score <= 0U)
			{
				return;
			}
			EventArgsRefreshIAPBattlePassScore instance = Singleton<EventArgsRefreshIAPBattlePassScore>.Instance;
			instance.SetData((int)score);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPBattlePassScore, instance);
		}

		private static void HandleResponse_UpdateHeroLevelUp(ActorDto dto)
		{
			if (dto == null)
			{
				return;
			}
			HeroLevelUpDataModule dataModule = GameApp.Data.GetDataModule(DataName.HeroLevelUpDataModule);
			if (dataModule == null)
			{
				return;
			}
			if ((long)dataModule.Level == (long)((ulong)dto.Level))
			{
				return;
			}
			EventArgsRefreshHeroLevelUpData eventArgsRefreshHeroLevelUpData = new EventArgsRefreshHeroLevelUpData();
			eventArgsRefreshHeroLevelUpData.SetData((int)dto.Level);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_HeroLevelUpDataModule_RefreshData, eventArgsRefreshHeroLevelUpData);
		}

		private static void HandleResponse_UpdateSevenDayCarnival(RepeatedField<SevenDayTaskDto> datas)
		{
			if (datas == null || datas.Count <= 0)
			{
				return;
			}
			GameApp.Data.GetDataModule(DataName.SevenDayCarnivalDataModule).UpdateTaskDatas(datas);
		}

		private static void HandleResponse_UpdateUserInfo(UserInfoDto userInfoDto)
		{
			if (userInfoDto == null)
			{
				return;
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			if (dataModule == null)
			{
				return;
			}
			dataModule.ServerSetUserInfo(userInfoDto, false);
		}

		private static void HandleResponse_UpdateConsumeData(RepeatedField<Consume> consumeData)
		{
			if (consumeData == null || consumeData.Count <= 0)
			{
				return;
			}
			ActivityWeekDataModule dataModule = GameApp.Data.GetDataModule(DataName.ActivityWeekDataModule);
			if (dataModule == null)
			{
				return;
			}
			dataModule.CommonUpdateConsumeData(consumeData);
		}

		private static void HandleResponse_UpdateTurnTableTaskDto(RepeatedField<TurnTableTaskDto> data)
		{
			if (data == null || data.Count <= 0)
			{
				return;
			}
			ActivitySlotTrainDataModule dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
			if (dataModule == null)
			{
				return;
			}
			dataModule.CommonUpdateTurnTableTaskDto(data);
		}

		private static void HandleResponse_UpdateChainPackActDto(CommonData commonData)
		{
			if (commonData.IsChainActv != 1U || commonData.ChainActvDto == null)
			{
				return;
			}
			ChainPacksDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChainPacksDataModule);
			if (dataModule == null)
			{
				return;
			}
			dataModule.CommonUpdateChainPacksInfo(commonData.ChainActvDto);
		}

		private static void HandleResponse_UpdateChainPackPushActDto(CommonData commonData)
		{
			if (commonData.PushChainDto == null)
			{
				return;
			}
			if (commonData.PushChainDto.Count <= 0)
			{
				return;
			}
			ChainPacksPushDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChainPacksPushDataModule);
			if (dataModule != null)
			{
				dataModule.OnRefreshData(commonData.PushChainDto, false);
			}
			ChainPacksDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.ChainPacksDataModule);
			if (dataModule2 == null)
			{
				return;
			}
			dataModule2.OnAddChainPacksInfo();
		}

		private static void HandleResponse_UpdateCloseFunction(RepeatedField<int> closeFunctionIds)
		{
			FunctionDataModule dataModule = GameApp.Data.GetDataModule(DataName.FunctionDataModule);
			if (dataModule == null || closeFunctionIds == null)
			{
				return;
			}
			dataModule.CommonUpdateServerCloseFunction(closeFunctionIds);
		}

		public static bool TryRankTypeToLeaderBoardType(RankType rankType, out LeaderBoardType leaderBoardType)
		{
			leaderBoardType = LeaderBoardType.Default;
			if (rankType == RankType.WorldBoss)
			{
				leaderBoardType = LeaderBoardType.WorldBoss;
				return true;
			}
			if (rankType != RankType.NewWorld)
			{
				return false;
			}
			leaderBoardType = LeaderBoardType.NewWorld;
			return true;
		}

		public static void DoRankRequest(RankType rankType, int page, bool isNextPage, bool showMask, Action<int, bool, bool, LeaderBoardResponse> callback)
		{
			LeaderBoardType leaderBoardType;
			if (!NetworkUtils.TryRankTypeToLeaderBoardType(rankType, out leaderBoardType))
			{
				HLog.LogError(string.Format("未知的排行榜类型 {0}", rankType));
				return;
			}
			LeaderBoardRequest leaderBoardRequest = new LeaderBoardRequest();
			leaderBoardRequest.CommonParams = NetworkUtils.GetCommonParams();
			leaderBoardRequest.Page = page;
			leaderBoardRequest.Type = leaderBoardType;
			leaderBoardRequest.IncludeSelf = true;
			GameApp.NetWork.Send(leaderBoardRequest, delegate(IMessage response)
			{
				LeaderBoardResponse leaderBoardResponse = response as LeaderBoardResponse;
				if (leaderBoardResponse == null || leaderBoardResponse.Code != 0)
				{
					Action<int, bool, bool, LeaderBoardResponse> callback2 = callback;
					if (callback2 != null)
					{
						callback2(page, isNextPage, false, leaderBoardResponse);
					}
					HLog.LogError(string.Format("排行榜 {0} 失败 {1}", rankType, (leaderBoardResponse != null) ? leaderBoardResponse.Code : (-999)));
					return;
				}
				if (rankType == RankType.WorldBoss)
				{
					GameApp.Data.GetDataModule(DataName.WorldBossDataModule).UpdateRankData(leaderBoardResponse);
				}
				else if (rankType == RankType.NewWorld)
				{
					GameApp.Data.GetDataModule(DataName.NewWorldDataModule).UpdateRankData(leaderBoardResponse);
					GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnUpdateRankData(leaderBoardResponse);
				}
				GameApp.Data.GetDataModule(DataName.RankDataModule).SetLastTop3(rankType, leaderBoardResponse.Top3);
				Action<int, bool, bool, LeaderBoardResponse> callback3 = callback;
				if (callback3 == null)
				{
					return;
				}
				callback3(page, isNextPage, true, leaderBoardResponse);
			}, showMask, false, string.Empty, true);
		}

		public static void UserGetLastLoginRequest(Action<bool, UserGetLastLoginResponse> callback, bool isShowMask)
		{
			UserGetLastLoginRequest userGetLastLoginRequest = new UserGetLastLoginRequest();
			userGetLastLoginRequest.CommonParams = NetworkUtils.GetCommonParams();
			GameApp.NetWork.Send(userGetLastLoginRequest, delegate(IMessage response)
			{
				UserGetLastLoginResponse userGetLastLoginResponse = response as UserGetLastLoginResponse;
				if (userGetLastLoginResponse != null && userGetLastLoginResponse.Code == 0)
				{
					Action<bool, UserGetLastLoginResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, userGetLastLoginResponse);
					return;
				}
				else
				{
					Action<bool, UserGetLastLoginResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, userGetLastLoginResponse);
					return;
				}
			}, false, false, string.Empty, true);
		}

		public static void FindServerListRequest(uint groupId, Action<bool, FindServerListResponse> callback)
		{
			FindServerListRequest findServerListRequest = new FindServerListRequest();
			findServerListRequest.CommonParams = NetworkUtils.GetCommonParams();
			findServerListRequest.GroupId = groupId;
			GameApp.NetWork.Send(findServerListRequest, delegate(IMessage response)
			{
				FindServerListResponse findServerListResponse = response as FindServerListResponse;
				if (findServerListResponse != null && findServerListResponse.Code == 0)
				{
					Action<bool, FindServerListResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, findServerListResponse);
					return;
				}
				else
				{
					Action<bool, FindServerListResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, findServerListResponse);
					return;
				}
			}, false, false, string.Empty, true);
		}

		public static void WatchTVRequest(string videoId, bool isShowMask = true, Action<bool, int> callback = null)
		{
			GMVideoRequest gmvideoRequest = new GMVideoRequest
			{
				CommonParams = NetworkUtils.GetCommonParams(),
				VideoId = videoId
			};
			GameApp.NetWork.Send(gmvideoRequest, delegate(IMessage response)
			{
				GMVideoResponse gmvideoResponse = response as GMVideoResponse;
				if (gmvideoResponse != null)
				{
					if (gmvideoResponse.Code == 0)
					{
						if (gmvideoResponse.CommonData != null && gmvideoResponse.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(gmvideoResponse.CommonData.Reward, null, true);
						}
						GameApp.Data.GetDataModule(DataName.TVRewardDataModule).OnNetResponse_WatchTV(gmvideoResponse.GmVideoDtos);
						Action<bool, int> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, gmvideoResponse.Code);
						return;
					}
					else
					{
						Action<bool, int> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, gmvideoResponse.Code);
						return;
					}
				}
				else
				{
					Action<bool, int> callback4 = callback;
					if (callback4 == null)
					{
						return;
					}
					callback4(false, -106);
					return;
				}
			}, isShowMask, false, string.Empty, false);
		}

		private static string m_paramToken = string.Empty;

		public class PushGift
		{
			public static void DoClosePushGift(int configId, Action<bool, IapPushRemoveResponse> callback)
			{
				IapPushRemoveRequest iapPushRemoveRequest = new IapPushRemoveRequest();
				iapPushRemoveRequest.ConfigId = configId;
				iapPushRemoveRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(iapPushRemoveRequest, delegate(IMessage response)
				{
					IapPushRemoveResponse iapPushRemoveResponse = response as IapPushRemoveResponse;
					if (iapPushRemoveResponse == null || iapPushRemoveResponse.Code != 0)
					{
						Action<bool, IapPushRemoveResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, iapPushRemoveResponse);
						}
						HLog.LogError(string.Format("发送关闭推送面板消息失败,ConfigId:{0}", configId));
						return;
					}
					Action<bool, IapPushRemoveResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, iapPushRemoveResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoRefreshPushGiftData(Action<bool, GetIapPushDtoResponse> callBack)
			{
				GetIapPushDtoRequest getIapPushDtoRequest = new GetIapPushDtoRequest();
				getIapPushDtoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(getIapPushDtoRequest, delegate(IMessage response)
				{
					GetIapPushDtoResponse getIapPushDtoResponse = response as GetIapPushDtoResponse;
					if (getIapPushDtoResponse == null || getIapPushDtoResponse.Code != 0)
					{
						Action<bool, GetIapPushDtoResponse> callBack2 = callBack;
						if (callBack2 != null)
						{
							callBack2(false, getIapPushDtoResponse);
						}
						HLog.LogError("发送刷新推送面板消息失败");
						return;
					}
					Action<bool, GetIapPushDtoResponse> callBack3 = callBack;
					if (callBack3 == null)
					{
						return;
					}
					callBack3(true, getIapPushDtoResponse);
				}, true, false, string.Empty, true);
			}
		}

		public class ActivityCommon
		{
			public static void ActivityGetListRequest(bool isShowMask = true, Action<bool, int> callback = null)
			{
				ActivityGetListRequest activityGetListRequest = new ActivityGetListRequest
				{
					CommonParams = NetworkUtils.GetCommonParams()
				};
				GameApp.NetWork.Send(activityGetListRequest, delegate(IMessage response)
				{
					ActivityGetListResponse activityGetListResponse = response as ActivityGetListResponse;
					if (activityGetListResponse != null)
					{
						if (activityGetListResponse.Code == 0)
						{
							EventArgsActivityCommonData instance = Singleton<EventArgsActivityCommonData>.Instance;
							instance.SetData(activityGetListResponse);
							GameApp.Event.DispatchNow(null, 252, instance);
							Action<bool, int> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, activityGetListResponse.Code);
							return;
						}
						else
						{
							Action<bool, int> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, activityGetListResponse.Code);
							return;
						}
					}
					else
					{
						Action<bool, int> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, -106);
						return;
					}
				}, isShowMask, false, string.Empty, false);
			}
		}

		public class ActivitySlotTrain
		{
			public static void RequestTurnTableGetInfo(bool isShowMask = true, Action<bool, int> callback = null)
			{
				TurnTableGetInfoRequest turnTableGetInfoRequest = new TurnTableGetInfoRequest();
				turnTableGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(turnTableGetInfoRequest, delegate(IMessage response)
				{
					TurnTableGetInfoResponse turnTableGetInfoResponse = response as TurnTableGetInfoResponse;
					if (turnTableGetInfoResponse != null)
					{
						if (turnTableGetInfoResponse.Code == 0)
						{
							EventArgsActivitySlotTrainData instance = Singleton<EventArgsActivitySlotTrainData>.Instance;
							instance.SetData(turnTableGetInfoResponse);
							GameApp.Event.DispatchNow(null, LocalMessageName.CC_ActivitySlotTrain, instance);
							Action<bool, int> callback2 = callback;
							if (callback2 != null)
							{
								callback2(true, turnTableGetInfoResponse.Code);
							}
						}
						else
						{
							GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).Clear();
							Action<bool, int> callback3 = callback;
							if (callback3 != null)
							{
								callback3(false, turnTableGetInfoResponse.Code);
							}
						}
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_ActivitySlotTrain_StageChanged, null);
						return;
					}
					Action<bool, int> callback4 = callback;
					if (callback4 == null)
					{
						return;
					}
					callback4(false, -106);
				}, isShowMask, false, string.Empty, true);
			}

			public static void RequestTurnTableExtract(int extractNum, Action<bool, TurnTableExtractResponse> callback = null)
			{
				TurnTableExtractRequest turnTableExtractRequest = new TurnTableExtractRequest();
				turnTableExtractRequest.CommonParams = NetworkUtils.GetCommonParams();
				turnTableExtractRequest.ExtractNum = extractNum;
				GameApp.NetWork.Send(turnTableExtractRequest, delegate(IMessage response)
				{
					TurnTableExtractResponse turnTableExtractResponse = response as TurnTableExtractResponse;
					if (turnTableExtractResponse != null)
					{
						if (turnTableExtractResponse.Code == 0)
						{
							EventArgsTurnTableExtractData instance = Singleton<EventArgsTurnTableExtractData>.Instance;
							instance.SetData(turnTableExtractResponse);
							GameApp.Event.DispatchNow(null, 257, instance);
							Action<bool, TurnTableExtractResponse> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, turnTableExtractResponse);
							return;
						}
						else
						{
							if (turnTableExtractResponse.Code == 7001)
							{
								GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).Clear();
							}
							Action<bool, TurnTableExtractResponse> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, turnTableExtractResponse);
							return;
						}
					}
					else
					{
						Action<bool, TurnTableExtractResponse> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, null);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void RequestTurnTableReceiveCumulativeReward(int turntableRewardConfigId, Action<bool, TurnTableReceiveCumulativeRewardResponse> callback)
			{
				TurnTableReceiveCumulativeRewardRequest turnTableReceiveCumulativeRewardRequest = new TurnTableReceiveCumulativeRewardRequest();
				turnTableReceiveCumulativeRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				turnTableReceiveCumulativeRewardRequest.TurntableRewardConfigId = turntableRewardConfigId;
				GameApp.NetWork.Send(turnTableReceiveCumulativeRewardRequest, delegate(IMessage response)
				{
					TurnTableReceiveCumulativeRewardResponse turnTableReceiveCumulativeRewardResponse = response as TurnTableReceiveCumulativeRewardResponse;
					if (turnTableReceiveCumulativeRewardResponse != null)
					{
						if (turnTableReceiveCumulativeRewardResponse.Code == 0)
						{
							if (turnTableReceiveCumulativeRewardResponse.CommonData != null && turnTableReceiveCumulativeRewardResponse.CommonData.Reward != null && turnTableReceiveCumulativeRewardResponse.CommonData.Reward.Count > 0)
							{
								DxxTools.UI.OpenRewardCommon(turnTableReceiveCumulativeRewardResponse.CommonData.Reward, null, true);
							}
							EventArgsTurnTableReceiveCumulativeRewardData instance = Singleton<EventArgsTurnTableReceiveCumulativeRewardData>.Instance;
							instance.SetData(turnTableReceiveCumulativeRewardResponse);
							GameApp.Event.DispatchNow(null, 258, instance);
							Action<bool, TurnTableReceiveCumulativeRewardResponse> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, turnTableReceiveCumulativeRewardResponse);
							return;
						}
						else
						{
							if (turnTableReceiveCumulativeRewardResponse.Code == 7001)
							{
								GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).Clear();
							}
							Action<bool, TurnTableReceiveCumulativeRewardResponse> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, turnTableReceiveCumulativeRewardResponse);
							return;
						}
					}
					else
					{
						Action<bool, TurnTableReceiveCumulativeRewardResponse> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, null);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void RequestTurnPayAd(int cfgId, int adID)
			{
				TurnPayAdRequest turnPayAdRequest = new TurnPayAdRequest();
				turnPayAdRequest.CommonParams = NetworkUtils.GetCommonParams();
				turnPayAdRequest.ConfigId = cfgId;
				GameApp.NetWork.Send(turnPayAdRequest, delegate(IMessage response)
				{
					TurnPayAdResponse turnPayAdResponse = response as TurnPayAdResponse;
					if (turnPayAdResponse != null && turnPayAdResponse.Code == 0)
					{
						ActivitySlotTrainDataModule dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
						if (turnPayAdResponse.CommonData != null)
						{
							if (turnPayAdResponse.CommonData.AdData != null)
							{
								GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(turnPayAdResponse.CommonData.AdData);
							}
							if (turnPayAdResponse.CommonData.Reward != null && turnPayAdResponse.CommonData.Reward.Count > 0)
							{
								DxxTools.UI.OpenRewardCommon(turnPayAdResponse.CommonData.Reward, null, true);
							}
						}
						dataModule.CommonUpdateTurntablePayCount(turnPayAdResponse.TurntablePayCount);
						GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(adID), "REWARD ", "", turnPayAdResponse.CommonData.Reward, null);
					}
				}, true, false, string.Empty, true);
			}

			public static void RequestTurnTableTaskReceiveReward(int taskId)
			{
				TurnTableTaskReceiveRewardRequest turnTableTaskReceiveRewardRequest = new TurnTableTaskReceiveRewardRequest();
				turnTableTaskReceiveRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				turnTableTaskReceiveRewardRequest.TaskId = taskId;
				GameApp.NetWork.Send(turnTableTaskReceiveRewardRequest, delegate(IMessage response)
				{
					TurnTableTaskReceiveRewardResponse turnTableTaskReceiveRewardResponse = response as TurnTableTaskReceiveRewardResponse;
					if (turnTableTaskReceiveRewardResponse != null)
					{
						if (turnTableTaskReceiveRewardResponse.Code == 0)
						{
							if (turnTableTaskReceiveRewardResponse.CommonData != null && turnTableTaskReceiveRewardResponse.CommonData.Reward != null && turnTableTaskReceiveRewardResponse.CommonData.Reward.Count > 0)
							{
								DxxTools.UI.OpenRewardCommon(turnTableTaskReceiveRewardResponse.CommonData.Reward, null, true);
								ActivitySlotTrainDataModule dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
								GameApp.SDK.Analyze.Track_TaskFinish("卡皮机", dataModule.TurntableId, taskId, turnTableTaskReceiveRewardResponse.CommonData.Reward);
							}
							EventArgsTurnTableTaskReceiveRewardData instance = Singleton<EventArgsTurnTableTaskReceiveRewardData>.Instance;
							instance.SetData(turnTableTaskReceiveRewardResponse);
							GameApp.Event.DispatchNow(null, 259, instance);
							return;
						}
						if (turnTableTaskReceiveRewardResponse.Code == 7001)
						{
							GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).Clear();
						}
					}
				}, true, false, string.Empty, true);
			}

			public static void RequestTurnTableSelectBigGuaranteeItem(int itemId, Action<bool> callback = null)
			{
				TurnTableSelectBigGuaranteeItemRequest turnTableSelectBigGuaranteeItemRequest = new TurnTableSelectBigGuaranteeItemRequest();
				turnTableSelectBigGuaranteeItemRequest.CommonParams = NetworkUtils.GetCommonParams();
				turnTableSelectBigGuaranteeItemRequest.ItemId = itemId;
				GameApp.NetWork.Send(turnTableSelectBigGuaranteeItemRequest, delegate(IMessage response)
				{
					TurnTableSelectBigGuaranteeItemResponse turnTableSelectBigGuaranteeItemResponse = response as TurnTableSelectBigGuaranteeItemResponse;
					if (turnTableSelectBigGuaranteeItemResponse != null)
					{
						if (turnTableSelectBigGuaranteeItemResponse.Code == 0)
						{
							EventArgsTurnTableSelectBigGuaranteeItemData instance = Singleton<EventArgsTurnTableSelectBigGuaranteeItemData>.Instance;
							instance.SetData(turnTableSelectBigGuaranteeItemResponse);
							GameApp.Event.DispatchNow(null, 260, instance);
							Action<bool> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true);
							return;
						}
						else
						{
							if (turnTableSelectBigGuaranteeItemResponse.Code == 7001)
							{
								GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).Clear();
							}
							Action<bool> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false);
							return;
						}
					}
					else
					{
						Action<bool> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class ActivityWeek
		{
			public static void RequestActTimeActivityList(bool showMask = true)
			{
				ActTimeInfoRequest actTimeInfoRequest = new ActTimeInfoRequest();
				actTimeInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(actTimeInfoRequest, delegate(IMessage response)
				{
					ActTimeInfoResponse actTimeInfoResponse = response as ActTimeInfoResponse;
					if (actTimeInfoResponse != null && actTimeInfoResponse.Code == 0)
					{
						EventArgsActTimeInfoData instance = Singleton<EventArgsActTimeInfoData>.Instance;
						instance.SetData(actTimeInfoResponse);
						GameApp.Event.DispatchNow(null, 253, instance);
					}
				}, showMask, false, string.Empty, false);
			}

			public static void RequestActTimeRank(int page, bool isNextPage, int actId, bool isShowMask, Action<int, bool, bool, ActTimeRankResponse> callback)
			{
				ActTimeRankRequest actTimeRankRequest = new ActTimeRankRequest();
				actTimeRankRequest.CommonParams = NetworkUtils.GetCommonParams();
				actTimeRankRequest.ActId = actId;
				actTimeRankRequest.Page = page;
				GameApp.NetWork.Send(actTimeRankRequest, delegate(IMessage response)
				{
					ActTimeRankResponse actTimeRankResponse = response as ActTimeRankResponse;
					if (actTimeRankResponse != null && actTimeRankResponse.Code == 0)
					{
						EventArgsActTimeRankData instance = Singleton<EventArgsActTimeRankData>.Instance;
						instance.SetData(actTimeRankResponse);
						GameApp.Event.DispatchNow(null, 254, instance);
						if (callback != null)
						{
							callback(page, isNextPage, true, actTimeRankResponse);
							return;
						}
					}
					else if (callback != null)
					{
						callback(page, isNextPage, false, actTimeRankResponse);
					}
				}, isShowMask, false, string.Empty, true);
			}

			public static void RequestActiveShopBug(int actId, int id, Action<bool, ActShopBuyResponse> callback)
			{
				ActShopBuyRequest actShopBuyRequest = new ActShopBuyRequest();
				actShopBuyRequest.Id = id;
				actShopBuyRequest.ActId = actId;
				actShopBuyRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(actShopBuyRequest, delegate(IMessage response)
				{
					ActShopBuyResponse actShopBuyResponse = response as ActShopBuyResponse;
					if (actShopBuyResponse != null && actShopBuyResponse.Code == 0)
					{
						if (actShopBuyResponse.CommonData != null && actShopBuyResponse.CommonData.Reward != null && actShopBuyResponse.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(actShopBuyResponse.CommonData.Reward, null, true);
						}
						NetworkUtils.ActivityWeek.RequestActTimeActivityList(true);
						if (callback != null)
						{
							callback(true, actShopBuyResponse);
							return;
						}
					}
					else if (callback != null)
					{
						callback(false, actShopBuyResponse);
					}
				}, true, false, string.Empty, true);
			}

			public static void RequestActiveDropBuy(int actId, int id, Action<bool, ActDropBuyResponse> callback)
			{
				ActDropBuyRequest actDropBuyRequest = new ActDropBuyRequest();
				actDropBuyRequest.Id = id;
				actDropBuyRequest.ActId = actId;
				actDropBuyRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(actDropBuyRequest, delegate(IMessage response)
				{
					ActDropBuyResponse actDropBuyResponse = response as ActDropBuyResponse;
					if (actDropBuyResponse != null && actDropBuyResponse.Code == 0)
					{
						if (actDropBuyResponse.CommonData != null && actDropBuyResponse.CommonData.Reward != null && actDropBuyResponse.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(actDropBuyResponse.CommonData.Reward, null, true);
						}
						NetworkUtils.ActivityWeek.RequestActTimeActivityList(true);
						if (callback != null)
						{
							callback(true, actDropBuyResponse);
							return;
						}
					}
					else if (callback != null)
					{
						callback(false, actDropBuyResponse);
					}
				}, true, false, string.Empty, true);
			}

			public static void RequestActTimePayFreeReward(int actId, int cfgId, Action<bool, ActTimePayFreeRewardResponse> callback)
			{
				ActTimePayFreeRewardRequest actTimePayFreeRewardRequest = new ActTimePayFreeRewardRequest();
				actTimePayFreeRewardRequest.ActId = actId;
				actTimePayFreeRewardRequest.ConfigId = cfgId;
				actTimePayFreeRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(actTimePayFreeRewardRequest, delegate(IMessage response)
				{
					ActTimePayFreeRewardResponse actTimePayFreeRewardResponse = response as ActTimePayFreeRewardResponse;
					if (actTimePayFreeRewardResponse != null && actTimePayFreeRewardResponse.Code == 0)
					{
						if (actTimePayFreeRewardResponse.CommonData != null && actTimePayFreeRewardResponse.CommonData.Reward != null && actTimePayFreeRewardResponse.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(actTimePayFreeRewardResponse.CommonData.Reward, null, true);
						}
						NetworkUtils.ActivityWeek.RequestActTimeActivityList(true);
						if (callback != null)
						{
							callback(true, actTimePayFreeRewardResponse);
							return;
						}
					}
					else if (callback != null)
					{
						callback(false, actTimePayFreeRewardResponse);
					}
				}, true, false, string.Empty, true);
			}

			public static void RequestActTimeReward(int actId, int opt, int taskId, Action<bool, ActTimeRewardResponse> callback)
			{
				ActTimeRewardRequest actTimeRewardRequest = new ActTimeRewardRequest();
				actTimeRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				actTimeRewardRequest.ActId = actId;
				actTimeRewardRequest.Opt = opt;
				actTimeRewardRequest.TaskId = taskId;
				GameApp.NetWork.Send(actTimeRewardRequest, delegate(IMessage response)
				{
					ActTimeRewardResponse actTimeRewardResponse = response as ActTimeRewardResponse;
					if (actTimeRewardResponse != null && actTimeRewardResponse.Code == 0)
					{
						if (actTimeRewardResponse.CommonData != null && actTimeRewardResponse.CommonData.Reward != null && actTimeRewardResponse.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(actTimeRewardResponse.CommonData.Reward, null, true);
							GameApp.SDK.Analyze.Track_TaskFinish("周活动", actId, taskId, actTimeRewardResponse.CommonData.Reward);
						}
						NetworkUtils.ActivityWeek.RequestActTimeActivityList(true);
						if (callback != null)
						{
							callback(true, actTimeRewardResponse);
							return;
						}
					}
					else if (callback != null)
					{
						callback(false, actTimeRewardResponse);
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Actor
		{
			public static void DoActorLevelUpRequest(Action<bool, ActorLevelUpResponse> callback)
			{
				ActorLevelUpRequest actorLevelUpRequest = new ActorLevelUpRequest();
				actorLevelUpRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(actorLevelUpRequest, delegate(IMessage response)
				{
					ActorLevelUpResponse actorLevelUpResponse = response as ActorLevelUpResponse;
					if (actorLevelUpResponse != null && actorLevelUpResponse.Code == 0)
					{
						Action<bool, ActorLevelUpResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, actorLevelUpResponse);
						return;
					}
					else
					{
						Action<bool, ActorLevelUpResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, actorLevelUpResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoActorAdvanceUpRequest(Action<bool, ActorAdvanceUpResponse> callback)
			{
				ActorAdvanceUpRequest actorAdvanceUpRequest = new ActorAdvanceUpRequest();
				actorAdvanceUpRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(actorAdvanceUpRequest, delegate(IMessage response)
				{
					ActorAdvanceUpResponse actorAdvanceUpResponse = response as ActorAdvanceUpResponse;
					if (actorAdvanceUpResponse != null && actorAdvanceUpResponse.Code == 0)
					{
						Action<bool, ActorAdvanceUpResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, actorAdvanceUpResponse);
						return;
					}
					else
					{
						Action<bool, ActorAdvanceUpResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, actorAdvanceUpResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Artifact
		{
			public static void DoArtifactUpgradeRequest(Action<bool, ArtifactUpgradeResponse> callback)
			{
				ArtifactUpgradeRequest artifactUpgradeRequest = new ArtifactUpgradeRequest();
				artifactUpgradeRequest.CommonParams = NetworkUtils.GetCommonParams();
				ArtifactInfo artifactInfo = GameApp.Data.GetDataModule(DataName.ArtifactDataModule).ArtifactInfo;
				int preStage = (int)artifactInfo.Stage;
				int preLevel = (int)artifactInfo.Level;
				GameApp.NetWork.Send(artifactUpgradeRequest, delegate(IMessage response)
				{
					ArtifactUpgradeResponse artifactUpgradeResponse = response as ArtifactUpgradeResponse;
					if (artifactUpgradeResponse != null && artifactUpgradeResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ArtifactDataModule).UpdateArtifactInfo(artifactUpgradeResponse.ArtifactInfo);
						RedPointController.Instance.ReCalc("Equip.Artifact.UpgradeTag", true);
						Action<bool, ArtifactUpgradeResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, artifactUpgradeResponse);
						}
						GameApp.SDK.Analyze.Track_LegendUpgrade(artifactUpgradeResponse.ArtifactInfo, artifactUpgradeResponse.CommonData.CostDto, preStage, preLevel);
						return;
					}
					Action<bool, ArtifactUpgradeResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, artifactUpgradeResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoArtifactUpgradeAllRequest(Action<bool, ArtifactUpgradeAllResponse> callback)
			{
				ArtifactUpgradeAllRequest artifactUpgradeAllRequest = new ArtifactUpgradeAllRequest();
				artifactUpgradeAllRequest.CommonParams = NetworkUtils.GetCommonParams();
				ArtifactInfo artifactInfo = GameApp.Data.GetDataModule(DataName.ArtifactDataModule).ArtifactInfo;
				int preStage = (int)artifactInfo.Stage;
				int preLevel = (int)artifactInfo.Level;
				GameApp.NetWork.Send(artifactUpgradeAllRequest, delegate(IMessage response)
				{
					ArtifactUpgradeAllResponse artifactUpgradeAllResponse = response as ArtifactUpgradeAllResponse;
					if (artifactUpgradeAllResponse != null && artifactUpgradeAllResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ArtifactDataModule).UpdateArtifactInfo(artifactUpgradeAllResponse.ArtifactInfo);
						RedPointController.Instance.ReCalc("Equip.Artifact.UpgradeTag", true);
						Action<bool, ArtifactUpgradeAllResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, artifactUpgradeAllResponse);
						}
						GameApp.SDK.Analyze.Track_LegendUpgrade(artifactUpgradeAllResponse.ArtifactInfo, artifactUpgradeAllResponse.CommonData.CostDto, preStage, preLevel);
						return;
					}
					Action<bool, ArtifactUpgradeAllResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, artifactUpgradeAllResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoArtifactItemStarRequest(int tableId, Action<bool, ArtifactItemStarResponse> callback)
			{
				ArtifactItemStarRequest artifactItemStarRequest = new ArtifactItemStarRequest();
				artifactItemStarRequest.CommonParams = NetworkUtils.GetCommonParams();
				artifactItemStarRequest.ConfigId = tableId;
				GameApp.NetWork.Send(artifactItemStarRequest, delegate(IMessage response)
				{
					ArtifactItemStarResponse artifactItemStarResponse = response as ArtifactItemStarResponse;
					if (artifactItemStarResponse != null && artifactItemStarResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ArtifactDataModule).UpdateArtifactItemDto(artifactItemStarResponse.ArtifactItemDto);
						RedPointController.Instance.ReCalc("Equip.Artifact.AdvanceTag", true);
						Action<bool, ArtifactItemStarResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, artifactItemStarResponse);
						}
						GameApp.SDK.Analyze.Track_RareLegendUpgrade(tableId, artifactItemStarResponse.ArtifactItemDto, artifactItemStarResponse.CommonData.CostDto);
						return;
					}
					Action<bool, ArtifactItemStarResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, artifactItemStarResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoArtifactApplySkillRequest(int advanceId, int optType, Action<bool, ArtifactApplySkillResponse> callback)
			{
				ArtifactApplySkillRequest artifactApplySkillRequest = new ArtifactApplySkillRequest();
				artifactApplySkillRequest.CommonParams = NetworkUtils.GetCommonParams();
				artifactApplySkillRequest.AdvanceId = advanceId;
				artifactApplySkillRequest.OptType = optType;
				GameApp.NetWork.Send(artifactApplySkillRequest, delegate(IMessage response)
				{
					ArtifactApplySkillResponse artifactApplySkillResponse = response as ArtifactApplySkillResponse;
					if (artifactApplySkillResponse != null && artifactApplySkillResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ArtifactDataModule).UpdateArtifactInfo(artifactApplySkillResponse.ArtifactInfo);
						Action<bool, ArtifactApplySkillResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, artifactApplySkillResponse);
						return;
					}
					else
					{
						Action<bool, ArtifactApplySkillResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, artifactApplySkillResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoArtifactUnlockRequest(int configId, Action<bool, ArtifactUnlockResponse> callback)
			{
				ArtifactUnlockRequest artifactUnlockRequest = new ArtifactUnlockRequest();
				artifactUnlockRequest.CommonParams = NetworkUtils.GetCommonParams();
				artifactUnlockRequest.ConfigId = configId;
				GameApp.NetWork.Send(artifactUnlockRequest, delegate(IMessage response)
				{
					ArtifactUnlockResponse artifactUnlockResponse = response as ArtifactUnlockResponse;
					if (artifactUnlockResponse != null && artifactUnlockResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ArtifactDataModule).UpdateArtifactItemDtos(artifactUnlockResponse.ArtifactItemDto);
						RedPointController.Instance.ReCalc("Equip.Artifact.AdvanceTag", true);
						Action<bool, ArtifactUnlockResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, artifactUnlockResponse);
						}
						GameApp.SDK.Analyze.Track_UnlockRareLegend(configId);
						return;
					}
					Action<bool, ArtifactUnlockResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, artifactUnlockResponse);
				}, true, false, string.Empty, true);
			}
		}

		public class ChainPacks
		{
			public static void DoChainPacksTimeRequest(bool isLoginRequest = false, bool isShowMask = true, Action<bool, int> callback = null, bool showError = true)
			{
				if (isLoginRequest)
				{
					isShowMask = false;
					showError = false;
				}
				getChainPacksTimeRequest getChainPacksTimeRequest = new getChainPacksTimeRequest
				{
					CommonParams = NetworkUtils.GetCommonParams()
				};
				GameApp.NetWork.Send(getChainPacksTimeRequest, delegate(IMessage response)
				{
					getChainPacksTimeResponse getChainPacksTimeResponse = response as getChainPacksTimeResponse;
					if (getChainPacksTimeResponse != null)
					{
						if (getChainPacksTimeResponse.Code == 0)
						{
							GameApp.Data.GetDataModule(DataName.ChainPacksDataModule).NetUpdateChainPacksInfo(getChainPacksTimeResponse.ChainActvDto);
							Action<bool, int> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, getChainPacksTimeResponse.Code);
							return;
						}
						else
						{
							Action<bool, int> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, getChainPacksTimeResponse.Code);
							return;
						}
					}
					else
					{
						Action<bool, int> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, -106);
						return;
					}
				}, isShowMask, false, string.Empty, showError);
			}

			public static void DoFreePickChainPacksRewardRequest(int actId, int pkId, bool isShowMask = true, Action<bool, int> callback = null)
			{
				takeChainPacksRewardRequest takeChainPacksRewardRequest = new takeChainPacksRewardRequest
				{
					Id = pkId,
					CommonParams = NetworkUtils.GetCommonParams(),
					ActId = actId
				};
				GameApp.NetWork.Send(takeChainPacksRewardRequest, delegate(IMessage response)
				{
					takeChainPacksRewardResponse takeChainPacksRewardResponse = response as takeChainPacksRewardResponse;
					if (takeChainPacksRewardResponse != null)
					{
						if (takeChainPacksRewardResponse.Code == 0)
						{
							if (takeChainPacksRewardResponse.CommonData != null && takeChainPacksRewardResponse.CommonData.Reward != null && takeChainPacksRewardResponse.CommonData.Reward.Count > 0)
							{
								DxxTools.UI.OpenRewardCommon(takeChainPacksRewardResponse.CommonData.Reward, null, true);
							}
							GameApp.Data.GetDataModule(DataName.ChainPacksDataModule).NetPickedReward(actId, pkId);
							Action<bool, int> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, takeChainPacksRewardResponse.Code);
							return;
						}
						else
						{
							Action<bool, int> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, takeChainPacksRewardResponse.Code);
							return;
						}
					}
					else
					{
						Action<bool, int> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, -106);
						return;
					}
				}, isShowMask, false, string.Empty, true);
			}

			public static void DoFreePickChainPacksPushRewardRequest(int actId, int pkId, bool isShowMask = true, Action<bool, int> callback = null)
			{
				takePushChainRewardRequest takePushChainRewardRequest = new takePushChainRewardRequest
				{
					Id = pkId,
					CommonParams = NetworkUtils.GetCommonParams(),
					ActId = actId
				};
				GameApp.NetWork.Send(takePushChainRewardRequest, delegate(IMessage response)
				{
					takePushChainRewardResponse takePushChainRewardResponse = response as takePushChainRewardResponse;
					if (takePushChainRewardResponse != null)
					{
						if (takePushChainRewardResponse.Code == 0)
						{
							if (takePushChainRewardResponse.CommonData != null && takePushChainRewardResponse.CommonData.Reward != null && takePushChainRewardResponse.CommonData.Reward.Count > 0)
							{
								DxxTools.UI.OpenRewardCommon(takePushChainRewardResponse.CommonData.Reward, null, true);
							}
							GameApp.Data.GetDataModule(DataName.ChainPacksDataModule).NetPickedReward(actId, pkId);
							Action<bool, int> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, takePushChainRewardResponse.Code);
							return;
						}
						else
						{
							Action<bool, int> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, takePushChainRewardResponse.Code);
							return;
						}
					}
					else
					{
						Action<bool, int> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, -106);
						return;
					}
				}, isShowMask, false, string.Empty, true);
			}
		}

		public class Chapter
		{
			public static void DoStartChapterRequest(int chapterID, Action<bool, StartChapterResponse> callback)
			{
				StartChapterRequest startChapterRequest = new StartChapterRequest();
				startChapterRequest.CommonParams = NetworkUtils.GetCommonParams();
				startChapterRequest.ChapterId = chapterID;
				GameApp.NetWork.Send(startChapterRequest, delegate(IMessage response)
				{
					StartChapterResponse startChapterResponse = response as StartChapterResponse;
					if (startChapterResponse != null && startChapterResponse.Code == 0)
					{
						EventArgServerData eventArgServerData = new EventArgServerData();
						eventArgServerData.SetData(startChapterResponse.ChapterSeed, startChapterResponse.EventMap, startChapterResponse.ActiveMap, startChapterResponse.BattleKey, startChapterResponse.BattleTimes);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameEventData_SetServerData, eventArgServerData);
						Action<bool, StartChapterResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, startChapterResponse);
						}
						GameAFTools.Ins.OnStartChapter(startChapterResponse.ChapterId);
						GameApp.SDK.Analyze.Track_ChapterStart(startChapterResponse.ChapterId, startChapterResponse.BattleKey, 0, 0, (int)startChapterResponse.BattleTimes);
						return;
					}
					Action<bool, StartChapterResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, startChapterResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoEndChapterRequest(int chapterID, int stage, int result, string pveData, string battleKey, List<RewardDto> eventReward, List<RewardDto> battleReward, List<int> skills, Action<bool, EndChapterResponse> callback)
			{
				EndChapterRequest endChapterRequest = new EndChapterRequest();
				endChapterRequest.CommonParams = NetworkUtils.GetCommonParams();
				endChapterRequest.ChapterId = chapterID;
				endChapterRequest.WaveIndex = stage;
				endChapterRequest.Result = result;
				endChapterRequest.FightData = pveData;
				for (int i = 0; i < skills.Count; i++)
				{
					endChapterRequest.SkillIds.Add(skills[i]);
				}
				ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
				int battleTime = dataModule.ChapterBattleTimes;
				GameApp.NetWork.Send(endChapterRequest, delegate(IMessage response)
				{
					Singleton<EventRecordController>.Instance.DeleteChapterRecord();
					EndChapterResponse endChapterResponse = response as EndChapterResponse;
					if (endChapterResponse != null && endChapterResponse.Code == 0)
					{
						Action<bool, EndChapterResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, endChapterResponse);
						}
						RedPointController.Instance.ReCalc("Main.ChapterReward", true);
						RedPointController.Instance.ReCalc("Main.NewWorld", true);
						if (endChapterResponse.CommonData.UpdateUserCurrency != null && endChapterResponse.CommonData.UpdateUserCurrency.UserCurrency != null)
						{
							long coins = endChapterResponse.CommonData.UpdateUserCurrency.UserCurrency.Coins;
						}
						bool flag = endChapterResponse.ChapterId > chapterID;
						GameAFTools.Ins.OnEndChapter(flag, chapterID);
						BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
						int num = 0;
						if (GameTGATools.Ins.ChapterEndQuitType == 0)
						{
							num = (flag ? 0 : 1);
						}
						else if (GameTGATools.Ins.ChapterEndQuitType == 1)
						{
							num = 2;
						}
						else if (GameTGATools.Ins.ChapterEndQuitType == 2)
						{
							num = 3;
						}
						int battleTime2 = battleTime;
						if (playerData != null)
						{
							GameApp.SDK.Analyze.Track_ChapterEnd(chapterID, battleKey, 0, playerData.Attack.GetValue(), playerData.HpMax.GetValue(), playerData.Defence.GetValue(), battleTime2, playerData.GetPlayerSkillBuildList(), num, Singleton<GameEventController>.Instance.GetCurrentStage(), 0, 0, endChapterResponse.CommonData.Reward, playerData.Chips.mVariable, new List<NodeScoreParam>());
							return;
						}
						List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData>();
						if (skills != null)
						{
							foreach (int num2 in skills)
							{
								GameSkillBuild_skillBuild elementById = GameApp.Table.GetManager().GetGameSkillBuild_skillBuildModelInstance().GetElementById(num2);
								if (elementById != null)
								{
									GameEventSkillBuildData gameEventSkillBuildData = new GameEventSkillBuildData();
									gameEventSkillBuildData.SetTable(elementById);
									list.Add(gameEventSkillBuildData);
								}
							}
						}
						GameApp.SDK.Analyze.Track_ChapterEnd(chapterID, battleKey, 0, 0L, 0L, 0L, battleTime2, list, num, Singleton<GameEventController>.Instance.GetCurrentStage(), 0, 0, endChapterResponse.CommonData.Reward, 0, new List<NodeScoreParam>());
						return;
					}
					else
					{
						Action<bool, EndChapterResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, endChapterResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoGetWaveRewardRequest(int chapterID, int stage, Action<bool, GetWaveRewardResponse> callback)
			{
				GetWaveRewardRequest getWaveRewardRequest = new GetWaveRewardRequest();
				getWaveRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				getWaveRewardRequest.ChapterId = chapterID;
				getWaveRewardRequest.WaveIndex = stage;
				GameApp.NetWork.Send(getWaveRewardRequest, delegate(IMessage response)
				{
					GetWaveRewardResponse getWaveRewardResponse = response as GetWaveRewardResponse;
					if (getWaveRewardResponse != null && getWaveRewardResponse.Code == 0)
					{
						EventArgsRefreshChapterRewardData eventArgsRefreshChapterRewardData = new EventArgsRefreshChapterRewardData();
						eventArgsRefreshChapterRewardData.SetData(getWaveRewardResponse.CanRewardList);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChapterData_RefreshChapterRewardData, eventArgsRefreshChapterRewardData);
						Action<bool, GetWaveRewardResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, getWaveRewardResponse);
						}
						RedPointController.Instance.ReCalc("Main.ChapterReward", true);
						GameApp.SDK.Analyze.Track_ChapterReward(chapterID, stage, getWaveRewardResponse.CommonData.Reward);
						return;
					}
					Action<bool, GetWaveRewardResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, getWaveRewardResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoChapterActRewardRequest(int stage, List<ulong> rowIds, Action<bool, ChapterActRewardResponse> callback)
			{
				GameApp.SDK.Analyze.Track_StagetClickTest(null);
				ChapterActRewardRequest chapterActRewardRequest = new ChapterActRewardRequest();
				chapterActRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				chapterActRewardRequest.Day = stage;
				for (int i = 0; i < rowIds.Count; i++)
				{
					chapterActRewardRequest.RowIds.Add(rowIds[i]);
				}
				ChapterActivityDataModule chapterActivityDataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
				Dictionary<ulong, int> preScores = new Dictionary<ulong, int>();
				if (rowIds.Count > 0)
				{
					foreach (ulong num in rowIds)
					{
						ChapterActivityData activityData = chapterActivityDataModule.GetActivityData(num);
						preScores[num] = (int)activityData.TotalScore;
					}
				}
				GameApp.NetWork.Send(chapterActRewardRequest, delegate(IMessage response)
				{
					ChapterActRewardResponse chapterActRewardResponse = response as ChapterActRewardResponse;
					if (chapterActRewardResponse != null && chapterActRewardResponse.Code == 0)
					{
						Singleton<EventRecordController>.Instance.EventGroupEnd();
						EventArgsChapterActivityRefreshScore eventArgsChapterActivityRefreshScore = new EventArgsChapterActivityRefreshScore();
						eventArgsChapterActivityRefreshScore.SetData(chapterActRewardResponse.Score, chapterActRewardResponse.CommonData.Reward.ToItemDatas());
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChapterActivity_RefreshScore, eventArgsChapterActivityRefreshScore);
						Action<bool, ChapterActRewardResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, chapterActRewardResponse);
						}
						if (rowIds.Count <= 0)
						{
							return;
						}
						ChapterActivityData activityData2 = chapterActivityDataModule.GetActivityData(rowIds[0]);
						if (activityData2 != null)
						{
							string text = GameTGATools.GetSourceName(11010) + Singleton<LanguageManager>.Instance.GetInfoByID(2, activityData2.ActivityTitleId);
							GameApp.SDK.Analyze.Track_Get(text, chapterActRewardResponse.CommonData.Reward, null, null, null, null);
						}
						Dictionary<ulong, int> dictionary = new Dictionary<ulong, int>();
						foreach (ulong num2 in rowIds)
						{
							ChapterActivityData activityData3 = chapterActivityDataModule.GetActivityData(num2);
							dictionary[num2] = (int)activityData3.TotalScore;
						}
						using (Dictionary<ulong, int>.Enumerator enumerator3 = dictionary.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								KeyValuePair<ulong, int> keyValuePair = enumerator3.Current;
								int num3;
								if (preScores.ContainsKey(keyValuePair.Key))
								{
									num3 = keyValuePair.Value - preScores[keyValuePair.Key];
								}
								else
								{
									num3 = keyValuePair.Value;
								}
								if (num3 > 0)
								{
									string activityTitleId = chapterActivityDataModule.GetActivityData(keyValuePair.Key).ActivityTitleId;
									string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(2, activityTitleId);
									int value = keyValuePair.Value;
									GameApp.SDK.Analyze.Track_ActivityPoint(infoByID, num3, value);
								}
							}
							return;
						}
					}
					Action<bool, ChapterActRewardResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, chapterActRewardResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoStartChapterSweepRequest(int rate, Action<bool, StartChapterSweepResponse> callback)
			{
				StartChapterSweepRequest startChapterSweepRequest = new StartChapterSweepRequest();
				startChapterSweepRequest.CommonParams = NetworkUtils.GetCommonParams();
				startChapterSweepRequest.Rate = (uint)rate;
				GameApp.NetWork.Send(startChapterSweepRequest, delegate(IMessage response)
				{
					StartChapterSweepResponse startChapterSweepResponse = response as StartChapterSweepResponse;
					if (startChapterSweepResponse != null && startChapterSweepResponse.Code == 0)
					{
						Action<bool, StartChapterSweepResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, startChapterSweepResponse);
						}
						GameApp.SDK.Analyze.Track_ChapterStart(startChapterSweepResponse.ChapterId, "", 1, (int)startChapterSweepResponse.Rate, 0);
						return;
					}
					Action<bool, StartChapterSweepResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, startChapterSweepResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoEndChapterSweepRequest(int chapterID, int rate, int stage, List<RewardDto> eventReward, List<RewardDto> battleReward, Action<bool, EndChapterSweepResponse> callback)
			{
				EndChapterSweepRequest endChapterSweepRequest = new EndChapterSweepRequest();
				endChapterSweepRequest.CommonParams = NetworkUtils.GetCommonParams();
				endChapterSweepRequest.ChapterId = chapterID;
				endChapterSweepRequest.WaveIndex = stage;
				GameApp.NetWork.Send(endChapterSweepRequest, delegate(IMessage response)
				{
					Singleton<EventRecordController>.Instance.DeleteSweepRecord();
					EndChapterSweepResponse endChapterSweepResponse = response as EndChapterSweepResponse;
					if (endChapterSweepResponse != null && endChapterSweepResponse.Code == 0)
					{
						Action<bool, EndChapterSweepResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, endChapterSweepResponse);
						}
						if (endChapterSweepResponse.CommonData.UpdateUserCurrency != null && endChapterSweepResponse.CommonData.UpdateUserCurrency.UserCurrency != null)
						{
							long coins = endChapterSweepResponse.CommonData.UpdateUserCurrency.UserCurrency.Coins;
						}
						GameApp.SDK.Analyze.Track_ChapterEnd(chapterID, "", 1, 0L, 0L, 0L, 0, null, 0, GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetElementById(chapterID)
							.journeyStage, rate, 0, endChapterSweepResponse.CommonData.Reward, 0, new List<NodeScoreParam>());
						NetworkUtils.Chapter.DoGetHangUpInfoRequest(null);
						return;
					}
					Action<bool, EndChapterSweepResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, endChapterSweepResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoChapterActRankRequest(uint actId, Action<bool, ChapterActRankResponse> callback)
			{
				ChapterActRankRequest chapterActRankRequest = new ChapterActRankRequest();
				chapterActRankRequest.CommonParams = NetworkUtils.GetCommonParams();
				chapterActRankRequest.ActId = (int)actId;
				GameApp.NetWork.Send(chapterActRankRequest, delegate(IMessage response)
				{
					ChapterActRankResponse chapterActRankResponse = response as ChapterActRankResponse;
					if (chapterActRankResponse != null && chapterActRankResponse.Code == 0)
					{
						EventArgsChapterActivityRankReward eventArgsChapterActivityRankReward = new EventArgsChapterActivityRankReward();
						eventArgsChapterActivityRankReward.SetData(chapterActRankResponse.RewardInfo);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChapterActivity_RankReward, eventArgsChapterActivityRankReward);
						Action<bool, ChapterActRankResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, chapterActRankResponse);
						return;
					}
					else
					{
						Action<bool, ChapterActRankResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, chapterActRankResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void DoChapterRankRewardRequest(Action<bool, ChapterRankRewardResponse> callback)
			{
				ChapterRankRewardRequest chapterRankRewardRequest = new ChapterRankRewardRequest();
				chapterRankRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(chapterRankRewardRequest, delegate(IMessage response)
				{
					ChapterRankRewardResponse chapterRankRewardResponse = response as ChapterRankRewardResponse;
					if (chapterRankRewardResponse != null && chapterRankRewardResponse.Code == 0)
					{
						Action<bool, ChapterRankRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, chapterRankRewardResponse);
						return;
					}
					else
					{
						Action<bool, ChapterRankRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, chapterRankRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoEndChapterCheckRequest(int chapterId, int stage, string attribute, List<int> skills, List<int> monsterCfgIds, long hp, int reviveCount, Action<bool, EndChapterCheckResponse> callback)
			{
				EndChapterCheckRequest request = new EndChapterCheckRequest();
				request.CommonParams = NetworkUtils.GetCommonParams();
				request.ChapterId = chapterId;
				request.WaveIndex = stage;
				request.Attributes = attribute;
				for (int i = 0; i < skills.Count; i++)
				{
					request.SkillIds.Add(skills[i]);
				}
				for (int j = 0; j < monsterCfgIds.Count; j++)
				{
					request.MonsterCfgId.Add(monsterCfgIds[j]);
				}
				request.CurHp = (ulong)hp;
				request.ReviveCount = reviveCount;
				request.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
				GameApp.NetWork.Send(request, delegate(IMessage response)
				{
					EndChapterCheckResponse endChapterCheckResponse = response as EndChapterCheckResponse;
					if (endChapterCheckResponse != null && endChapterCheckResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ChapterDataModule).SetBossBattleCheckInfo(request, endChapterCheckResponse);
						Action<bool, EndChapterCheckResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, endChapterCheckResponse);
						return;
					}
					else
					{
						if (endChapterCheckResponse != null && (endChapterCheckResponse.Code == 6004 || endChapterCheckResponse.Code == 6005))
						{
							string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("battle_check_fail"), Array.Empty<object>());
							string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("battle_check_ok");
							DxxTools.UI.OpenPopCommon(text, delegate(int id)
							{
								GameApp.View.CloseView(ViewName.GameEventViewModule, null);
								GameApp.View.CloseAllView(new int[] { 214, 101, 102, 106 });
								GameApp.State.ActiveState(StateName.LoginState);
							}, string.Empty, infoByID, string.Empty, false, 2);
							BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
							GameApp.SDK.Analyze.Track_ChapterBattleCheat(chapterId, stage, playerData.Attack.GetValue(), playerData.Defence.GetValue(), playerData.CurrentHp.GetValue(), playerData.HpMax.GetValue(), playerData.GetPlayerSkillBuildList());
						}
						Action<bool, EndChapterCheckResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, endChapterCheckResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoGetHangUpInfoRequest(Action<bool, GetHangUpInfoResponse> callback)
			{
				GetHangUpInfoRequest getHangUpInfoRequest = new GetHangUpInfoRequest();
				getHangUpInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(getHangUpInfoRequest, delegate(IMessage response)
				{
					GetHangUpInfoResponse getHangUpInfoResponse = response as GetHangUpInfoResponse;
					if (getHangUpInfoResponse != null && getHangUpInfoResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.HangUpDataModule).UpdateHangUpInfo(getHangUpInfoResponse.HungUpInfoDto);
						RedPointController.Instance.ReCalc("Main.HangUp", true);
						Action<bool, GetHangUpInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, getHangUpInfoResponse);
						return;
					}
					else
					{
						Action<bool, GetHangUpInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, getHangUpInfoResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void DoGetHangUpRewardRequest(bool isAd, Action<bool, GetHangUpRewardResponse> callback)
			{
				GetHangUpRewardRequest getHangUpRewardRequest = new GetHangUpRewardRequest();
				getHangUpRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				getHangUpRewardRequest.Advert = (isAd ? 1 : 0);
				GameApp.NetWork.Send(getHangUpRewardRequest, delegate(IMessage response)
				{
					GetHangUpRewardResponse getHangUpRewardResponse = response as GetHangUpRewardResponse;
					if (getHangUpRewardResponse != null)
					{
						GameApp.Data.GetDataModule(DataName.HangUpDataModule).UpdateHangUpInfo(getHangUpRewardResponse.HungUpInfoDto);
					}
					if (getHangUpRewardResponse != null && getHangUpRewardResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(getHangUpRewardResponse.CommonData.AdData);
						RedPointController.Instance.ReCalc("Main.HangUp", true);
						Action<bool, GetHangUpRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, getHangUpRewardResponse);
						return;
					}
					else
					{
						Action<bool, GetHangUpRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, getHangUpRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoChapterBattlePassScoreRequest(int stage, long rowId, Action<bool, ChapterBattlePassScoreResponse> callback)
			{
				ChapterBattlePassScoreRequest chapterBattlePassScoreRequest = new ChapterBattlePassScoreRequest();
				chapterBattlePassScoreRequest.CommonParams = NetworkUtils.GetCommonParams();
				chapterBattlePassScoreRequest.Day = stage;
				chapterBattlePassScoreRequest.RowId = rowId;
				GameApp.NetWork.Send(chapterBattlePassScoreRequest, delegate(IMessage response)
				{
					ChapterBattlePassScoreResponse chapterBattlePassScoreResponse = response as ChapterBattlePassScoreResponse;
					if (chapterBattlePassScoreResponse != null && chapterBattlePassScoreResponse.Code == 0)
					{
						Singleton<EventRecordController>.Instance.EventGroupEnd();
						GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule).UpdateScore(chapterBattlePassScoreResponse.Score, chapterBattlePassScoreResponse.RowId);
						Action<bool, ChapterBattlePassScoreResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, chapterBattlePassScoreResponse);
						return;
					}
					else
					{
						Action<bool, ChapterBattlePassScoreResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, chapterBattlePassScoreResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoGetChapterBattlePassRewardRequest(List<int> rewardIdList, Action<bool, GetChapterBattlePassRewardResponse> callback)
			{
				GetChapterBattlePassRewardRequest getChapterBattlePassRewardRequest = new GetChapterBattlePassRewardRequest();
				getChapterBattlePassRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				for (int i = 0; i < rewardIdList.Count; i++)
				{
					getChapterBattlePassRewardRequest.RewardIdList.Add(rewardIdList[i]);
				}
				GameApp.NetWork.Send(getChapterBattlePassRewardRequest, delegate(IMessage response)
				{
					GetChapterBattlePassRewardResponse getChapterBattlePassRewardResponse = response as GetChapterBattlePassRewardResponse;
					if (getChapterBattlePassRewardResponse != null && getChapterBattlePassRewardResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule).UpdateData(getChapterBattlePassRewardResponse.ChapterBattlePassDto);
						Action<bool, GetChapterBattlePassRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, getChapterBattlePassRewardResponse);
						return;
					}
					else
					{
						Action<bool, GetChapterBattlePassRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, getChapterBattlePassRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoChapterBattlePassOpenBoxRequest(Action<bool, ChapterBattlePassOpenBoxResponse> callback)
			{
				ChapterBattlePassOpenBoxRequest chapterBattlePassOpenBoxRequest = new ChapterBattlePassOpenBoxRequest();
				chapterBattlePassOpenBoxRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(chapterBattlePassOpenBoxRequest, delegate(IMessage response)
				{
					ChapterBattlePassOpenBoxResponse chapterBattlePassOpenBoxResponse = response as ChapterBattlePassOpenBoxResponse;
					if (chapterBattlePassOpenBoxResponse != null && chapterBattlePassOpenBoxResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule).UpdateFinalRewardCount(chapterBattlePassOpenBoxResponse.FinalRewardCount);
						Action<bool, ChapterBattlePassOpenBoxResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, chapterBattlePassOpenBoxResponse);
						return;
					}
					else
					{
						Action<bool, ChapterBattlePassOpenBoxResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, chapterBattlePassOpenBoxResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoChapterWheelScoreRequest(int stage, long rowId, Action<bool, ChapterWheelScoreResponse> callback)
			{
				ChapterWheelScoreRequest chapterWheelScoreRequest = new ChapterWheelScoreRequest();
				chapterWheelScoreRequest.CommonParams = NetworkUtils.GetCommonParams();
				chapterWheelScoreRequest.Day = stage;
				chapterWheelScoreRequest.RowId = rowId;
				GameApp.NetWork.Send(chapterWheelScoreRequest, delegate(IMessage response)
				{
					ChapterWheelScoreResponse chapterWheelScoreResponse = response as ChapterWheelScoreResponse;
					if (chapterWheelScoreResponse != null && chapterWheelScoreResponse.Code == 0)
					{
						ChapterActivityWheelDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
						int num = ((dataModule.WheelInfo != null) ? dataModule.WheelInfo.Score : 0);
						dataModule.UpdateScore(chapterWheelScoreResponse.RowId, chapterWheelScoreResponse.Score);
						Action<bool, ChapterWheelScoreResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, chapterWheelScoreResponse);
						}
						int num2 = ((dataModule.WheelInfo != null) ? dataModule.WheelInfo.Score : 0);
						int num3 = num2 - num;
						if (num3 > 0)
						{
							string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(2, "activity_wheel_title");
							GameApp.SDK.Analyze.Track_ActivityPoint(infoByID, num3, num2);
							return;
						}
					}
					else
					{
						Action<bool, ChapterWheelScoreResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, chapterWheelScoreResponse);
					}
				}, true, false, string.Empty, true);
			}

			public static void DoChapterWheelSpineRequest(int rate, Action<bool, ChapterWheelSpineResponse> callback)
			{
				ChapterWheelSpineRequest chapterWheelSpineRequest = new ChapterWheelSpineRequest();
				chapterWheelSpineRequest.CommonParams = NetworkUtils.GetCommonParams();
				chapterWheelSpineRequest.Rate = rate;
				GameApp.NetWork.Send(chapterWheelSpineRequest, delegate(IMessage response)
				{
					ChapterWheelSpineResponse chapterWheelSpineResponse = response as ChapterWheelSpineResponse;
					if (chapterWheelSpineResponse != null && chapterWheelSpineResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule).UpdateSpinInfo(chapterWheelSpineResponse);
						Action<bool, ChapterWheelSpineResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, chapterWheelSpineResponse);
						}
						if (chapterWheelSpineResponse.CommonData != null)
						{
							GameApp.SDK.Analyze.Track_Turntable(rate, chapterWheelSpineResponse.Score, chapterWheelSpineResponse.PlayTimes, chapterWheelSpineResponse.CommonData.Reward);
							return;
						}
					}
					else
					{
						Action<bool, ChapterWheelSpineResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, chapterWheelSpineResponse);
					}
				}, true, false, string.Empty, true);
			}

			public static void DoChapterWheelInfoRequest(Action<bool, ChapterWheelInfoResponse> callback)
			{
				ChapterWheelInfoRequest chapterWheelInfoRequest = new ChapterWheelInfoRequest();
				chapterWheelInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(chapterWheelInfoRequest, delegate(IMessage response)
				{
					ChapterWheelInfoResponse chapterWheelInfoResponse = response as ChapterWheelInfoResponse;
					if (chapterWheelInfoResponse != null && chapterWheelInfoResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule).UpdateInfo(chapterWheelInfoResponse.Info);
						Action<bool, ChapterWheelInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, chapterWheelInfoResponse);
						return;
					}
					else
					{
						Action<bool, ChapterWheelInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, chapterWheelInfoResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Chat
		{
			public static void DoRequest_GetMessageRecords(long msgId, SocketGroupType groupType, string groupId, Action<bool, ChatGetMessageRecordsResponse> callback)
			{
				if (GameApp.SocketNet.Connected)
				{
					ChatGroupData group = Singleton<ChatManager>.Instance.GetGroup(groupType, groupId);
					ChatGetMessageRecordsRequest chatGetMessageRecordsRequest = new ChatGetMessageRecordsRequest
					{
						CommonParams = NetworkUtils.GetCommonParams(),
						MsgId = (ulong)msgId,
						PageIndex = (uint)group.HistoryPage,
						GroupType = (uint)groupType,
						GroupId = groupId
					};
					GameApp.NetWork.Send(chatGetMessageRecordsRequest, delegate(IMessage response)
					{
						ChatGetMessageRecordsResponse chatGetMessageRecordsResponse = response as ChatGetMessageRecordsResponse;
						if (chatGetMessageRecordsResponse != null && chatGetMessageRecordsResponse.Code == 0)
						{
							ChatProxy.Common.OnRecvGuildChatRecords(group, chatGetMessageRecordsResponse);
							Action<bool, ChatGetMessageRecordsResponse> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(true, chatGetMessageRecordsResponse);
							return;
						}
						else
						{
							Action<bool, ChatGetMessageRecordsResponse> callback4 = callback;
							if (callback4 == null)
							{
								return;
							}
							callback4(false, chatGetMessageRecordsResponse);
							return;
						}
					}, false, false, string.Empty, false);
					return;
				}
				GameApp.SocketNet.CheckReconnect(string.Format("{0} chat", groupType));
				Action<bool, ChatGetMessageRecordsResponse> callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2(false, null);
			}
		}

		public class Chest
		{
			public static void ChestUseRequest(ulong rowId, int count, Action<bool, ChestUseResponse> callback = null)
			{
				ChestUseRequest chestUseRequest = new ChestUseRequest();
				chestUseRequest.CommonParams = NetworkUtils.GetCommonParams();
				chestUseRequest.RowId = rowId;
				chestUseRequest.Count = (uint)count;
				GameApp.NetWork.Send(chestUseRequest, delegate(IMessage response)
				{
					ChestUseResponse chestUseResponse = response as ChestUseResponse;
					if (chestUseResponse != null && chestUseResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.ChestDataModule).UpdateCurrentScore();
						Action<bool, ChestUseResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, chestUseResponse);
						return;
					}
					else
					{
						Action<bool, ChestUseResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, chestUseResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void ChestRewardRequest(bool isGetAll, Action<bool, ChestRewardResponse> callback = null)
			{
				ChestRewardRequest chestRewardRequest = new ChestRewardRequest();
				chestRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				chestRewardRequest.OptType = (isGetAll ? 1 : 0);
				ChestDataModule chestDataModule = GameApp.Data.GetDataModule(DataName.ChestDataModule);
				long preScore = chestDataModule.GetCurScore();
				GameApp.NetWork.Send(chestRewardRequest, delegate(IMessage response)
				{
					ChestRewardResponse chestRewardResponse = response as ChestRewardResponse;
					if (chestRewardResponse != null && chestRewardResponse.Code == 0)
					{
						chestDataModule.UpdateChestInfo(chestRewardResponse.ChestInfo);
						DxxTools.UI.OpenRewardCommon(chestRewardResponse.CommonData.Reward, delegate
						{
							GameApp.Event.Dispatch(null, LocalMessageName.CC_Chest_ScoreRewardChange, null);
							GameApp.Event.Dispatch(null, LocalMessageName.CC_Chest_ChestChange, null);
						}, true);
						Action<bool, ChestRewardResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, chestRewardResponse);
						}
						GameApp.SDK.Analyze.Track_TreasurePoint_Reward(isGetAll, preScore - chestDataModule.GetCurScore(), chestRewardResponse.CommonData.Reward);
						return;
					}
					Action<bool, ChestRewardResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, chestRewardResponse);
				}, true, false, string.Empty, true);
			}
		}

		public class Collection
		{
			public static void CollectionMergeRequest(uint rowId)
			{
				CollecComposeRequest collecComposeRequest = new CollecComposeRequest();
				collecComposeRequest.CommonParams = NetworkUtils.GetCommonParams();
				collecComposeRequest.RowId = (ulong)rowId;
				GameApp.NetWork.Send(collecComposeRequest, delegate(IMessage response)
				{
					CollecComposeResponse collecComposeResponse = response as CollecComposeResponse;
					if (collecComposeResponse != null && collecComposeResponse.Code == 0)
					{
						GameApp.Sound.PlayClip(630, 1f);
						GameApp.Data.GetDataModule(DataName.CollectionDataModule).TryCalcAttribute();
						EventArgsCollectionMerge eventArgsCollectionMerge = new EventArgsCollectionMerge();
						eventArgsCollectionMerge.SetData(collecComposeResponse.CollectionDto.ToList<CollectionDto>());
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_CollectionDataModule_CollectionMerge, eventArgsCollectionMerge);
						NetworkUtils.Collection.CheckSpecialCollectionChange(collecComposeResponse.CollectionDto);
						DxxTools.UI.OpenRewardCommon(collecComposeResponse.CommonData.Reward, delegate
						{
							GameApp.Event.DispatchNow(null, 390, null);
						}, true);
						RedPointController.Instance.ReCalc("Equip.Collection.Main", true);
						RedPointController.Instance.ReCalc("Equip.Collection.StarUpgrade", true);
					}
				}, true, false, string.Empty, true);
			}

			public static void CollectionLevelUpRequest(uint rowId, Action<bool, CollecStrengthResponse> callback = null)
			{
				CollecStrengthRequest collecStrengthRequest = new CollecStrengthRequest();
				collecStrengthRequest.CommonParams = NetworkUtils.GetCommonParams();
				collecStrengthRequest.RowId = (ulong)rowId;
				GameApp.NetWork.Send(collecStrengthRequest, delegate(IMessage response)
				{
					CollecStrengthResponse collecStrengthResponse = response as CollecStrengthResponse;
					if (collecStrengthResponse != null && collecStrengthResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.CollectionDataModule).TryCalcAttribute();
						EventArgsCollectionLevelUp eventArgsCollectionLevelUp = new EventArgsCollectionLevelUp();
						eventArgsCollectionLevelUp.SetData(collecStrengthResponse.CollectionDto);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_CollectionDataModule_CollectionLevelUp, eventArgsCollectionLevelUp);
						Action<bool, CollecStrengthResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, collecStrengthResponse);
						}
						RedPointController.Instance.ReCalc("Equip.Collection.Main", true);
						return;
					}
					Action<bool, CollecStrengthResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, collecStrengthResponse);
				}, true, false, string.Empty, true);
			}

			public static void CollectionStarUpgradeRequest(uint rowId, Action<bool, CollecStarResponse> callback = null)
			{
				CollecStarRequest collecStarRequest = new CollecStarRequest();
				collecStarRequest.CommonParams = NetworkUtils.GetCommonParams();
				collecStarRequest.RowId = (ulong)rowId;
				GameApp.NetWork.Send(collecStarRequest, delegate(IMessage response)
				{
					CollecStarResponse collecStarResponse = response as CollecStarResponse;
					if (collecStarResponse != null && collecStarResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.CollectionDataModule).TryCalcAttribute();
						EventArgsCollectionStarUpgrade eventArgsCollectionStarUpgrade = new EventArgsCollectionStarUpgrade();
						eventArgsCollectionStarUpgrade.SetData(collecStarResponse.CollectionDto.ToList<CollectionDto>());
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_CollectionDataModule_CollectionStarUpgrade, eventArgsCollectionStarUpgrade);
						Action<bool, CollecStarResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, collecStarResponse);
						}
						RedPointController.Instance.ReCalc("Equip.Collection.Main", true);
						RedPointController.Instance.ReCalc("Equip.Collection.StarUpgrade", true);
						NetworkUtils.Collection.CheckSpecialCollectionChange(collecStarResponse.CollectionDto);
						return;
					}
					Action<bool, CollecStarResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, collecStarResponse);
				}, true, false, string.Empty, true);
			}

			private static void CheckSpecialCollectionChange(RepeatedField<CollectionDto> collectionDtos)
			{
				if (collectionDtos == null || collectionDtos.Count == 0)
				{
					return;
				}
				bool flag = false;
				for (int i = 0; i < collectionDtos.Count; i++)
				{
					CollectionDto collectionDto = collectionDtos[i];
					Collection_collection elementById = GameApp.Table.GetManager().GetCollection_collectionModelInstance().GetElementById((int)collectionDto.ConfigId);
					if (elementById != null && elementById.passiveType == 12)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					NetworkUtils.Chapter.DoGetHangUpInfoRequest(null);
				}
			}
		}

		public class Conquer
		{
			public static void DoConquerListRequest(long userID, Action<bool, ConquerListResponse> callback)
			{
				ConquerListRequest conquerListRequest = new ConquerListRequest();
				conquerListRequest.CommonParams = NetworkUtils.GetCommonParams();
				conquerListRequest.UserId = userID;
				GameApp.NetWork.Send(conquerListRequest, delegate(IMessage response)
				{
					ConquerListResponse conquerListResponse = response as ConquerListResponse;
					if (conquerListResponse != null && conquerListResponse.Code == 0)
					{
						Action<bool, ConquerListResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, conquerListResponse);
						return;
					}
					else
					{
						Action<bool, ConquerListResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, conquerListResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void DoConquerBattleRequest(long userID, Action<bool, ConquerBattleResponse> callback)
			{
				ConquerBattleRequest conquerBattleRequest = new ConquerBattleRequest();
				conquerBattleRequest.CommonParams = NetworkUtils.GetCommonParams();
				conquerBattleRequest.UserId = userID;
				GameApp.NetWork.Send(conquerBattleRequest, delegate(IMessage response)
				{
					ConquerBattleResponse conquerBattleResponse = response as ConquerBattleResponse;
					if (conquerBattleResponse != null && conquerBattleResponse.Code == 0)
					{
						EventArgsBattleConquerEnter instance = Singleton<EventArgsBattleConquerEnter>.Instance;
						instance.SetData(conquerBattleResponse.Record, false);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleConquer_BattleConquerEnter, instance);
						EventArgsRefreshLordAddSlaveData instance2 = Singleton<EventArgsRefreshLordAddSlaveData>.Instance;
						instance2.Clear();
						instance2.SetData(conquerBattleResponse.Lord, (int)conquerBattleResponse.SlaveCount);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, instance2);
						Action<bool, ConquerBattleResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, conquerBattleResponse);
						return;
					}
					else
					{
						Action<bool, ConquerBattleResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, conquerBattleResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoConquerRevoltRequest(long userID, Action<bool, ConquerRevoltResponse> callback)
			{
				ConquerRevoltRequest conquerRevoltRequest = new ConquerRevoltRequest();
				conquerRevoltRequest.CommonParams = NetworkUtils.GetCommonParams();
				conquerRevoltRequest.UserId = userID;
				GameApp.NetWork.Send(conquerRevoltRequest, delegate(IMessage response)
				{
					ConquerRevoltResponse conquerRevoltResponse = response as ConquerRevoltResponse;
					if (conquerRevoltResponse != null && conquerRevoltResponse.Code == 0)
					{
						EventArgsBattleConquerEnter instance = Singleton<EventArgsBattleConquerEnter>.Instance;
						instance.SetData(conquerRevoltResponse.Record, false);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleConquer_BattleConquerEnter, instance);
						EventArgsRefreshLordAddSlaveData instance2 = Singleton<EventArgsRefreshLordAddSlaveData>.Instance;
						instance2.Clear();
						instance2.SetData(conquerRevoltResponse.Lord, (int)conquerRevoltResponse.SlaveCount);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, instance2);
						Action<bool, ConquerRevoltResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, conquerRevoltResponse);
						return;
					}
					else
					{
						Action<bool, ConquerRevoltResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, conquerRevoltResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoConquerLootRequest(long userID, Action<bool, ConquerLootResponse> callback)
			{
				ConquerLootRequest conquerLootRequest = new ConquerLootRequest();
				conquerLootRequest.CommonParams = NetworkUtils.GetCommonParams();
				conquerLootRequest.UserId = userID;
				GameApp.NetWork.Send(conquerLootRequest, delegate(IMessage response)
				{
					ConquerLootResponse conquerLootResponse = response as ConquerLootResponse;
					if (conquerLootResponse != null && conquerLootResponse.Code == 0)
					{
						EventArgsBattleConquerEnter instance = Singleton<EventArgsBattleConquerEnter>.Instance;
						instance.SetData(conquerLootResponse.Record, false);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleConquer_BattleConquerEnter, instance);
						EventArgsRefreshLordAddSlaveData instance2 = Singleton<EventArgsRefreshLordAddSlaveData>.Instance;
						instance2.Clear();
						instance2.SetData(conquerLootResponse.Lord, (int)conquerLootResponse.SlaveCount);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, instance2);
						Action<bool, ConquerLootResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, conquerLootResponse);
						return;
					}
					else
					{
						Action<bool, ConquerLootResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, conquerLootResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoConquerPardonRequest(long userID, Action<bool, ConquerPardonResponse> callback)
			{
				ConquerPardonRequest conquerPardonRequest = new ConquerPardonRequest();
				conquerPardonRequest.CommonParams = NetworkUtils.GetCommonParams();
				conquerPardonRequest.UserId = userID;
				GameApp.NetWork.Send(conquerPardonRequest, delegate(IMessage response)
				{
					ConquerPardonResponse conquerPardonResponse = response as ConquerPardonResponse;
					if (conquerPardonResponse != null && conquerPardonResponse.Code == 0)
					{
						EventArgsRefreshLordAddSlaveData instance = Singleton<EventArgsRefreshLordAddSlaveData>.Instance;
						instance.Clear();
						instance.SetData(conquerPardonResponse.Lord, (int)conquerPardonResponse.SlaveCount);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, instance);
						Action<bool, ConquerPardonResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, conquerPardonResponse);
						return;
					}
					else
					{
						Action<bool, ConquerPardonResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, conquerPardonResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class CrossArena
		{
			private static CrossArenaDataModule _DataModule
			{
				get
				{
					return GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
				}
			}

			public static void DoCrossArenaGetInfoRequest(Action<bool, CrossArenaGetInfoResponse> callback)
			{
				CrossArenaGetInfoRequest crossArenaGetInfoRequest = new CrossArenaGetInfoRequest();
				crossArenaGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(crossArenaGetInfoRequest, delegate(IMessage response)
				{
					CrossArenaGetInfoResponse crossArenaGetInfoResponse = response as CrossArenaGetInfoResponse;
					if (crossArenaGetInfoResponse == null || crossArenaGetInfoResponse.Code != 0)
					{
						Action<bool, CrossArenaGetInfoResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, crossArenaGetInfoResponse);
						}
						HLog.LogError(string.Format("跨服竞技场 获取基本数据 失败 {0}", (crossArenaGetInfoResponse != null) ? crossArenaGetInfoResponse.Code : (-999)));
						return;
					}
					EventArgsSetCrossArenaInfo eventArgsSetCrossArenaInfo = new EventArgsSetCrossArenaInfo();
					eventArgsSetCrossArenaInfo.SetData(crossArenaGetInfoResponse.Dan, crossArenaGetInfoResponse.Rank, crossArenaGetInfoResponse.Score, crossArenaGetInfoResponse.TeamCount, crossArenaGetInfoResponse.CurSeason);
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_CrossArena_SetInfo, eventArgsSetCrossArenaInfo);
					Action<bool, CrossArenaGetInfoResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, crossArenaGetInfoResponse);
				}, false, false, string.Empty, true);
			}

			public static void DoCrossArenaChallengeListRequest(bool forcerefresh, Action<bool, CrossArenaChallengeListResponse> callback)
			{
				CrossArenaChallengeListRequest crossArenaChallengeListRequest = new CrossArenaChallengeListRequest();
				crossArenaChallengeListRequest.CommonParams = NetworkUtils.GetCommonParams();
				crossArenaChallengeListRequest.Refresh = forcerefresh;
				GameApp.NetWork.Send(crossArenaChallengeListRequest, delegate(IMessage response)
				{
					CrossArenaChallengeListResponse crossArenaChallengeListResponse = response as CrossArenaChallengeListResponse;
					if (crossArenaChallengeListResponse == null || crossArenaChallengeListResponse.Code != 0)
					{
						Action<bool, CrossArenaChallengeListResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, crossArenaChallengeListResponse);
						}
						HLog.LogError(string.Format("跨服竞技场-挑战列表 失败 {0}", (crossArenaChallengeListResponse != null) ? crossArenaChallengeListResponse.Code : (-999)));
						return;
					}
					EventArgsCrossArenaRefreshOppList eventArgsCrossArenaRefreshOppList = new EventArgsCrossArenaRefreshOppList();
					eventArgsCrossArenaRefreshOppList.Members = CrossArenaRankMember.ToListWithRank(crossArenaChallengeListResponse.OppList);
					eventArgsCrossArenaRefreshOppList.RefreshCount = (int)crossArenaChallengeListResponse.RefreshCount;
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_CrossArena_RefreshChallengeList, eventArgsCrossArenaRefreshOppList);
					Action<bool, CrossArenaChallengeListResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, crossArenaChallengeListResponse);
				}, false, false, string.Empty, true);
			}

			public static void DoCrossArenaChallengeRequest(long userid, Action<bool, CrossArenaChallengeResponse> callback)
			{
				CrossArenaChallengeRequest crossArenaChallengeRequest = new CrossArenaChallengeRequest();
				crossArenaChallengeRequest.CommonParams = NetworkUtils.GetCommonParams();
				crossArenaChallengeRequest.UserId = userid;
				crossArenaChallengeRequest.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
				GameApp.NetWork.Send(crossArenaChallengeRequest, delegate(IMessage response)
				{
					CrossArenaChallengeResponse crossArenaChallengeResponse = response as CrossArenaChallengeResponse;
					if (crossArenaChallengeResponse == null || crossArenaChallengeResponse.Code != 0)
					{
						Action<bool, CrossArenaChallengeResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, crossArenaChallengeResponse);
						}
						HLog.LogError(string.Format("跨服竞技场-挑战 失败 {0}", (crossArenaChallengeResponse != null) ? crossArenaChallengeResponse.Code : (-999)));
						return;
					}
					GameApp.Data.GetDataModule(DataName.CrossArenaDataModule).RefreshOppListCount = 0;
					Action<bool, CrossArenaChallengeResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, crossArenaChallengeResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoCrossArenaRankRequest(int page, Action<bool, CrossArenaRankResponse> callback)
			{
				CrossArenaRankRequest crossArenaRankRequest = new CrossArenaRankRequest();
				crossArenaRankRequest.CommonParams = NetworkUtils.GetCommonParams();
				crossArenaRankRequest.Page = (uint)page;
				GameApp.NetWork.Send(crossArenaRankRequest, delegate(IMessage response)
				{
					CrossArenaRankResponse crossArenaRankResponse = response as CrossArenaRankResponse;
					if (crossArenaRankResponse == null || crossArenaRankResponse.Code != 0)
					{
						Action<bool, CrossArenaRankResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, crossArenaRankResponse);
						}
						HLog.LogError(string.Format("跨服竞技场-排行榜 失败 {0}", (crossArenaRankResponse != null) ? crossArenaRankResponse.Code : (-999)));
						return;
					}
					if (crossArenaRankResponse.Rank.Count > 0)
					{
						long userId = GameApp.Data.GetDataModule(DataName.LoginDataModule).userId;
						CrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
						for (int i = 0; i < crossArenaRankResponse.Rank.Count; i++)
						{
							if (crossArenaRankResponse.Rank[i] != null && crossArenaRankResponse.Rank[i].UserId == userId)
							{
								dataModule.UpdateMyRankInfo(crossArenaRankResponse.Rank[i]);
							}
						}
					}
					Action<bool, CrossArenaRankResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, crossArenaRankResponse);
				}, false, false, string.Empty, true);
			}

			public static void DoCrossArenaRecordRequest(Action<bool, CrossArenaRecordResponse> callback)
			{
				CrossArenaRecordRequest crossArenaRecordRequest = new CrossArenaRecordRequest();
				crossArenaRecordRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(crossArenaRecordRequest, delegate(IMessage response)
				{
					CrossArenaRecordResponse crossArenaRecordResponse = response as CrossArenaRecordResponse;
					if (crossArenaRecordResponse == null || crossArenaRecordResponse.Code != 0)
					{
						Action<bool, CrossArenaRecordResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, crossArenaRecordResponse);
						}
						HLog.LogError(string.Format("跨服竞技场-对战记录 失败 {0}", (crossArenaRecordResponse != null) ? crossArenaRecordResponse.Code : (-999)));
						return;
					}
					Action<bool, CrossArenaRecordResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, crossArenaRecordResponse);
				}, false, false, string.Empty, true);
			}

			public static void DoCrossArenaEnterRequest(Action<bool, CrossArenaEnterResponse> callback)
			{
				CrossArenaEnterRequest crossArenaEnterRequest = new CrossArenaEnterRequest();
				crossArenaEnterRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(crossArenaEnterRequest, delegate(IMessage response)
				{
					CrossArenaEnterResponse crossArenaEnterResponse = response as CrossArenaEnterResponse;
					if (crossArenaEnterResponse == null || crossArenaEnterResponse.Code != 0)
					{
						Action<bool, CrossArenaEnterResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, crossArenaEnterResponse);
						}
						HLog.LogError(string.Format("跨服竞技场-进入 失败 {0}", (crossArenaEnterResponse != null) ? crossArenaEnterResponse.Code : (-999)));
						return;
					}
					EventArgsSetCrossArenaInfo eventArgsSetCrossArenaInfo = new EventArgsSetCrossArenaInfo();
					eventArgsSetCrossArenaInfo.SetData(crossArenaEnterResponse.Dan, crossArenaEnterResponse.Rank, crossArenaEnterResponse.Score, crossArenaEnterResponse.TeamCount, crossArenaEnterResponse.CurSeason, crossArenaEnterResponse.GroupId);
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_CrossArena_SetInfo, eventArgsSetCrossArenaInfo);
					Action<bool, CrossArenaEnterResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, crossArenaEnterResponse);
				}, false, false, string.Empty, true);
			}
		}

		public class Dungeon
		{
			public static void DoStartDungeonRequest(int dungeonId, int levelId, bool isSweep, Action<bool, StartDungeonResponse> callback)
			{
				StartDungeonRequest startDungeonRequest = new StartDungeonRequest();
				startDungeonRequest.CommonParams = NetworkUtils.GetCommonParams();
				startDungeonRequest.DungeonId = dungeonId;
				startDungeonRequest.LevelId = levelId;
				startDungeonRequest.OptionType = (isSweep ? 1 : 0);
				startDungeonRequest.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
				GameApp.NetWork.Send(startDungeonRequest, delegate(IMessage response)
				{
					StartDungeonResponse startDungeonResponse = response as StartDungeonResponse;
					if (startDungeonResponse != null && startDungeonResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.DungeonDataModule).UpdateData(startDungeonResponse);
						Action<bool, StartDungeonResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, startDungeonResponse);
						}
						GameApp.SDK.Analyze.Track_Raid(dungeonId, levelId, isSweep, (int)startDungeonResponse.Result, startDungeonResponse.CommonData.CostDto, startDungeonResponse.CommonData.Reward);
						return;
					}
					Action<bool, StartDungeonResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, startDungeonResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoDungeonAdGetItemRequest(int dungeonId, Action<bool, DungeonAdGetItemResponse> callback)
			{
				DungeonAdGetItemRequest dungeonAdGetItemRequest = new DungeonAdGetItemRequest();
				dungeonAdGetItemRequest.CommonParams = NetworkUtils.GetCommonParams();
				dungeonAdGetItemRequest.DungeonId = (uint)dungeonId;
				GameApp.NetWork.Send(dungeonAdGetItemRequest, delegate(IMessage response)
				{
					DungeonAdGetItemResponse dungeonAdGetItemResponse = response as DungeonAdGetItemResponse;
					if (dungeonAdGetItemResponse != null && dungeonAdGetItemResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(dungeonAdGetItemResponse.AdData);
						Action<bool, DungeonAdGetItemResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, dungeonAdGetItemResponse);
						return;
					}
					else
					{
						Action<bool, DungeonAdGetItemResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, dungeonAdGetItemResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Equip
		{
			public static void EquipStrengthRequest(Action<bool, EquipStrengthResponse> callback, ulong rwoid, List<ulong> equipRowIds, Dictionary<uint, uint> useItems)
			{
				EquipStrengthRequest equipStrengthRequest = new EquipStrengthRequest();
				equipStrengthRequest.CommonParams = NetworkUtils.GetCommonParams();
				equipStrengthRequest.RowId = rwoid;
				equipStrengthRequest.UseItems.Add(useItems);
				equipStrengthRequest.EquipRowIds.AddRange(equipRowIds);
				GameApp.NetWork.Send(equipStrengthRequest, delegate(IMessage response)
				{
					EquipStrengthResponse equipStrengthResponse = response as EquipStrengthResponse;
					if (response != null && equipStrengthResponse.Code == 0)
					{
						GameApp.Event.DispatchNow(null, 145, null);
						Action<bool, EquipStrengthResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, equipStrengthResponse);
						return;
					}
					else
					{
						Action<bool, EquipStrengthResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, equipStrengthResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoEquipDress(List<ulong> rwoids, Action<bool, EquipDressResponse> callback = null)
			{
				EquipDressRequest equipDressRequest = new EquipDressRequest();
				equipDressRequest.CommonParams = NetworkUtils.GetCommonParams();
				equipDressRequest.RowIds.AddRange(rwoids);
				GameApp.NetWork.Send(equipDressRequest, delegate(IMessage response)
				{
					EquipDressResponse equipDressResponse = response as EquipDressResponse;
					if (response != null && equipDressResponse.Code == 0)
					{
						EventArgsRefreshEquipDressRowIds eventArgsRefreshEquipDressRowIds = new EventArgsRefreshEquipDressRowIds();
						eventArgsRefreshEquipDressRowIds.SetData(equipDressResponse.RowIds);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, eventArgsRefreshEquipDressRowIds);
						GameApp.Event.DispatchNow(null, 145, null);
						RedPointController.Instance.ReCalc("Equip.Hero", true);
						Action<bool, EquipDressResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, equipDressResponse);
						return;
					}
					else
					{
						Action<bool, EquipDressResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, equipDressResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoEquipUpgradeRequest(ulong rowID, uint count, Action<bool, EquipUpgradeResponse> callback = null)
			{
				EquipUpgradeRequest equipUpgradeRequest = new EquipUpgradeRequest();
				equipUpgradeRequest.CommonParams = NetworkUtils.GetCommonParams();
				equipUpgradeRequest.RowId = rowID;
				equipUpgradeRequest.Count = count;
				GameApp.NetWork.Send(equipUpgradeRequest, delegate(IMessage response)
				{
					EquipUpgradeResponse equipUpgradeResponse = response as EquipUpgradeResponse;
					if (response != null && equipUpgradeResponse.Code == 0)
					{
						RedPointController.Instance.ReCalc("Equip.Hero", true);
						GameApp.Event.DispatchNow(null, 145, null);
						Action<bool, EquipUpgradeResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, equipUpgradeResponse);
						}
						GameApp.SDK.Analyze.Track_EquipmentLevel(equipUpgradeResponse.CommonData.Equipment);
						return;
					}
					Action<bool, EquipUpgradeResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, equipUpgradeResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoEquipEvolutionRequest(ulong rowID, Action<bool, EquipEvolutionResponse> callback = null)
			{
				EquipEvolutionRequest equipEvolutionRequest = new EquipEvolutionRequest();
				equipEvolutionRequest.CommonParams = NetworkUtils.GetCommonParams();
				equipEvolutionRequest.RowId = rowID;
				GameApp.NetWork.Send(equipEvolutionRequest, delegate(IMessage response)
				{
					EquipEvolutionResponse equipEvolutionResponse = response as EquipEvolutionResponse;
					if (equipEvolutionResponse == null)
					{
						HLog.LogError("EquipEvolutionResponse is null, please check");
						return;
					}
					if (response != null && equipEvolutionResponse.Code == 0)
					{
						RedPointController.Instance.ReCalc("Equip.Hero", true);
						GameApp.Event.DispatchNow(null, 145, null);
						Action<bool, EquipEvolutionResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, equipEvolutionResponse);
						}
						GameApp.SDK.Analyze.Track_EquipmentLevel(equipEvolutionResponse.CommonData.Equipment);
						return;
					}
					Action<bool, EquipEvolutionResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, equipEvolutionResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoEquipComposeRequest(List<EquipComposeData> composeData, bool isAuto, Action<bool, EquipComposeResponse> callback = null)
			{
				EquipComposeRequest equipComposeRequest = new EquipComposeRequest();
				equipComposeRequest.CommonParams = NetworkUtils.GetCommonParams();
				equipComposeRequest.ComposeData.AddRange(composeData);
				GameApp.NetWork.Send(equipComposeRequest, delegate(IMessage response)
				{
					EquipComposeResponse equipComposeResponse = response as EquipComposeResponse;
					if (response != null && equipComposeResponse.Code == 0)
					{
						GameApp.SDK.Analyze.Track_EquipmentMerge(equipComposeResponse.CommonData.Equipment, equipComposeResponse.DelEquipRowId, isAuto);
						EventArgsRemoveEquipDatas instance = Singleton<EventArgsRemoveEquipDatas>.Instance;
						instance.m_datas = equipComposeResponse.DelEquipRowId;
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_EquipDataModule_RemoveEquipDatas, instance);
						List<ulong> equipDressRowIds = GameApp.Data.GetDataModule(DataName.EquipDataModule).m_equipDressRowIds;
						bool flag = false;
						if (equipDressRowIds.Count != equipComposeResponse.RowIds.Count)
						{
							flag = true;
						}
						else
						{
							for (int i = 0; i < equipDressRowIds.Count; i++)
							{
								if (equipDressRowIds[i] != equipComposeResponse.RowIds[i])
								{
									flag = true;
									break;
								}
							}
						}
						if (flag)
						{
							EventArgsRefreshEquipDressRowIds eventArgsRefreshEquipDressRowIds = new EventArgsRefreshEquipDressRowIds();
							eventArgsRefreshEquipDressRowIds.SetData(equipComposeResponse.RowIds);
							GameApp.Event.DispatchNow(null, LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, eventArgsRefreshEquipDressRowIds);
						}
						GameApp.Event.DispatchNow(null, 145, null);
						RedPointController.Instance.ReCalc("Equip.Hero", true);
						Action<bool, EquipComposeResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, equipComposeResponse);
						return;
					}
					else
					{
						Action<bool, EquipComposeResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, equipComposeResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoEquipResetRequest(ulong rowID, Action<bool, EquipLevelResetResponse> callback = null)
			{
				EquipLevelResetRequest equipLevelResetRequest = new EquipLevelResetRequest();
				equipLevelResetRequest.CommonParams = NetworkUtils.GetCommonParams();
				equipLevelResetRequest.RowIds.Add(rowID);
				GameApp.NetWork.Send(equipLevelResetRequest, delegate(IMessage response)
				{
					EquipLevelResetResponse equipLevelResetResponse = response as EquipLevelResetResponse;
					if (response != null && equipLevelResetResponse.Code == 0)
					{
						GameApp.Event.DispatchNow(null, 145, null);
						if (equipLevelResetResponse.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(equipLevelResetResponse.CommonData.Reward, null, true);
						}
						RedPointController.Instance.ReCalc("Equip.Hero", true);
						Action<bool, EquipLevelResetResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, equipLevelResetResponse);
						return;
					}
					else
					{
						Action<bool, EquipLevelResetResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, equipLevelResetResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoEquipDecomposeRequest(ulong rowID, Action<bool, EquipDecomposeResponse> callback = null)
			{
				EquipDecomposeRequest equipDecomposeRequest = new EquipDecomposeRequest();
				equipDecomposeRequest.CommonParams = NetworkUtils.GetCommonParams();
				equipDecomposeRequest.RowId = rowID;
				GameApp.NetWork.Send(equipDecomposeRequest, delegate(IMessage response)
				{
					EquipDecomposeResponse equipDecomposeResponse = response as EquipDecomposeResponse;
					if (response != null && equipDecomposeResponse.Code == 0)
					{
						GameApp.Event.DispatchNow(null, 145, null);
						if (equipDecomposeResponse.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(equipDecomposeResponse.CommonData.Reward, null, true);
						}
						RedPointController.Instance.ReCalc("Equip.Hero", true);
						Action<bool, EquipDecomposeResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, equipDecomposeResponse);
						return;
					}
					else
					{
						Action<bool, EquipDecomposeResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, equipDecomposeResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Item
		{
			public static void SendItemUseRequest(ulong rowId, uint count, uint index, Action<bool, ItemUseResponse> callback)
			{
				ItemUseRequest itemUseRequest = new ItemUseRequest();
				itemUseRequest.CommonParams = NetworkUtils.GetCommonParams();
				itemUseRequest.RowId = rowId;
				itemUseRequest.Count = count;
				itemUseRequest.Index = index;
				GameApp.NetWork.Send(itemUseRequest, delegate(IMessage response)
				{
					ItemUseResponse itemUseResponse = response as ItemUseResponse;
					if (itemUseResponse != null && itemUseResponse.Code == 0)
					{
						Action<bool, ItemUseResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, itemUseResponse);
						return;
					}
					else
					{
						Action<bool, ItemUseResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, itemUseResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Login
		{
			public static void UserLoginRequest(Action<bool, UserLoginResponse> callback)
			{
				UserLoginRequest userLoginRequest = new UserLoginRequest();
				userLoginRequest.CommonParams = NetworkUtils.GetCommonParams();
				userLoginRequest.CommonParams.ServerId = SelectServerDataModule.GetJumpServerId(GameApp.NetWork.m_account, GameApp.NetWork.m_account2, GameApp.NetWork.m_deviceID);
				userLoginRequest.ChannelId = 1U;
				userLoginRequest.AccountId2 = GameApp.NetWork.m_account2;
				GameApp.NetWork.Send(userLoginRequest, delegate(IMessage response)
				{
					UserLoginResponse userLoginResponse = response as UserLoginResponse;
					if (userLoginResponse != null && userLoginResponse.Code == 0)
					{
						SelectServerDataModule.ClearJumpServerData();
						NetworkUtils.m_paramToken = userLoginResponse.AccessToken;
						GameApp.NetWork.m_userID = userLoginResponse.UserId.ToString();
						GameApp.NetWork.m_serverID = userLoginResponse.ServerId;
						Action<bool, UserLoginResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userLoginResponse);
						return;
					}
					else
					{
						HLog.LogError("NetworkUtils.Login.DoUserLoginResponse.resp == null");
						Action<bool, UserLoginResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userLoginResponse);
						return;
					}
				}, false, false, string.Empty, false);
			}

			public static void OnLoginGetActivityCommonInfo(Action<int, int> onLoginFailure = null)
			{
				NetworkUtils.ActivityCommon.ActivityGetListRequest(false, delegate(bool isOk, int code)
				{
					if (!isOk)
					{
						Action<int, int> onLoginFailure2 = onLoginFailure;
						if (onLoginFailure2 == null)
						{
							return;
						}
						onLoginFailure2(11401, code);
					}
				});
			}

			public static void OnLoginGetActivitySlotTrainInfo(Action<int, int> onLoginFailure = null)
			{
				NetworkUtils.ActivitySlotTrain.RequestTurnTableGetInfo(false, delegate(bool isOk, int code)
				{
					if (!isOk)
					{
						Action<int, int> onLoginFailure2 = onLoginFailure;
						if (onLoginFailure2 == null)
						{
							return;
						}
						onLoginFailure2(11701, code);
					}
				});
			}

			public static void OnLoginGetWorldBossInfo(Action<int, int> onLoginFailure = null)
			{
				NetworkUtils.WorldBoss.DoGetWorldBossInfo(false, delegate(bool isOk, int code)
				{
					if (!isOk)
					{
						Action<int, int> onLoginFailure2 = onLoginFailure;
						if (onLoginFailure2 == null)
						{
							return;
						}
						onLoginFailure2(10415, code);
					}
				});
			}
		}

		public class MainCity
		{
			public static void DoCityGoldmineLevelUpRequest(Action<bool, CityGoldmineLevelUpResponse> callback)
			{
				CityGoldmineLevelUpRequest cityGoldmineLevelUpRequest = new CityGoldmineLevelUpRequest();
				cityGoldmineLevelUpRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(cityGoldmineLevelUpRequest, delegate(IMessage response)
				{
					CityGoldmineLevelUpResponse cityGoldmineLevelUpResponse = response as CityGoldmineLevelUpResponse;
					if (cityGoldmineLevelUpResponse != null && cityGoldmineLevelUpResponse.Code == 0)
					{
						EventArgsRefreshMainCityGoldData instance = Singleton<EventArgsRefreshMainCityGoldData>.Instance;
						instance.SetData(cityGoldmineLevelUpResponse.Level, 0L);
						GameApp.Event.DispatchNow(null, 142, instance);
						Action<bool, CityGoldmineLevelUpResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, cityGoldmineLevelUpResponse);
						return;
					}
					else
					{
						Action<bool, CityGoldmineLevelUpResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, cityGoldmineLevelUpResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoCityGoldmineHangRewardRequest(Action<bool, CityGoldmineHangRewardResponse> callback)
			{
				CityGoldmineHangRewardRequest cityGoldmineHangRewardRequest = new CityGoldmineHangRewardRequest();
				cityGoldmineHangRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(cityGoldmineHangRewardRequest, delegate(IMessage response)
				{
					CityGoldmineHangRewardResponse cityGoldmineHangRewardResponse = response as CityGoldmineHangRewardResponse;
					if (cityGoldmineHangRewardResponse != null && cityGoldmineHangRewardResponse.Code == 0)
					{
						MainCityDataModule dataModule = GameApp.Data.GetDataModule(DataName.MainCityDataModule);
						ulong lastGoldmineRewardTime = cityGoldmineHangRewardResponse.LastGoldmineRewardTime;
						long goldTimeSpan = dataModule.m_goldTimeSpan;
						EventArgsRefreshMainCityGoldData instance = Singleton<EventArgsRefreshMainCityGoldData>.Instance;
						instance.SetData(0, (long)cityGoldmineHangRewardResponse.LastGoldmineRewardTime);
						GameApp.Event.DispatchNow(null, 142, instance);
						Action<bool, CityGoldmineHangRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, cityGoldmineHangRewardResponse);
						return;
					}
					else
					{
						Action<bool, CityGoldmineHangRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, cityGoldmineHangRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoCityGetChestInfoRequest(Action<bool, CityGetChestInfoResponse> callback)
			{
				CityGetChestInfoRequest cityGetChestInfoRequest = new CityGetChestInfoRequest();
				cityGetChestInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(cityGetChestInfoRequest, delegate(IMessage response)
				{
					CityGetChestInfoResponse cityGetChestInfoResponse = response as CityGetChestInfoResponse;
					if (cityGetChestInfoResponse != null && cityGetChestInfoResponse.Code == 0)
					{
						EventArgsRefreshMainCityBoxData instance = Singleton<EventArgsRefreshMainCityBoxData>.Instance;
						instance.SetData(cityGetChestInfoResponse.CityChest, cityGetChestInfoResponse.StartTime, cityGetChestInfoResponse.RefreshTime);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainCityData_RefreshMainCityBoxData, instance);
						Action<bool, CityGetChestInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, cityGetChestInfoResponse);
						return;
					}
					else
					{
						Action<bool, CityGetChestInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, cityGetChestInfoResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void DoCityOpenChestRequest(List<ulong> rowIDs, Action<bool, CityOpenChestResponse> callback, bool isShowMask = true)
			{
				CityOpenChestRequest cityOpenChestRequest = new CityOpenChestRequest();
				cityOpenChestRequest.CommonParams = NetworkUtils.GetCommonParams();
				cityOpenChestRequest.RowId.AddRange(rowIDs);
				GameApp.NetWork.Send(cityOpenChestRequest, delegate(IMessage response)
				{
					CityOpenChestResponse cityOpenChestResponse = response as CityOpenChestResponse;
					if (cityOpenChestResponse != null && cityOpenChestResponse.Code == 0)
					{
						EventArgsRefreshMainCityBoxData instance = Singleton<EventArgsRefreshMainCityBoxData>.Instance;
						instance.SetData(cityOpenChestResponse.RowId, cityOpenChestResponse.RefreshTime, cityOpenChestResponse.Score);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainCityData_RefreshMainCityBoxData, instance);
						RedPointController.Instance.ReCalc("Main.Box", true);
						Action<bool, CityOpenChestResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, cityOpenChestResponse);
						return;
					}
					else
					{
						Action<bool, CityOpenChestResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, cityOpenChestResponse);
						return;
					}
				}, isShowMask, false, string.Empty, true);
			}

			public static void DoCityTakeScoreRewardRequest(Action<bool, CityTakeScoreRewardResponse> callback)
			{
				CityTakeScoreRewardRequest cityTakeScoreRewardRequest = new CityTakeScoreRewardRequest();
				cityTakeScoreRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(cityTakeScoreRewardRequest, delegate(IMessage response)
				{
					CityTakeScoreRewardResponse cityTakeScoreRewardResponse = response as CityTakeScoreRewardResponse;
					if (cityTakeScoreRewardResponse != null && cityTakeScoreRewardResponse.Code == 0)
					{
						EventArgsRefreshMainCityBoxData instance = Singleton<EventArgsRefreshMainCityBoxData>.Instance;
						instance.SetData(cityTakeScoreRewardResponse.RefreshTime, cityTakeScoreRewardResponse.Score);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainCityData_RefreshMainCityBoxData, instance);
						RedPointController.Instance.ReCalc("Main.Box", true);
						Action<bool, CityTakeScoreRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, cityTakeScoreRewardResponse);
						return;
					}
					else
					{
						Action<bool, CityTakeScoreRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, cityTakeScoreRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoCitySyncPowerRequest(long power)
			{
				NetworkUtils.MainCity.newPower = power;
				if (NetworkUtils.MainCity.inSendingPower)
				{
					return;
				}
				NetworkUtils.MainCity.inSendingPower = true;
				DelayCall.Instance.CallOnce(2000, delegate
				{
					NetworkUtils.MainCity.inSendingPower = false;
					CitySyncPowerRequest citySyncPowerRequest = new CitySyncPowerRequest();
					citySyncPowerRequest.CommonParams = NetworkUtils.GetCommonParams();
					citySyncPowerRequest.Power = (long)((int)power);
					citySyncPowerRequest.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
					GameApp.NetWork.Send(citySyncPowerRequest, delegate(IMessage response)
					{
						CitySyncPowerResponse citySyncPowerResponse = response as CitySyncPowerResponse;
						if (citySyncPowerResponse != null)
						{
							int code = citySyncPowerResponse.Code;
						}
					}, false, false, string.Empty, false);
				});
			}

			public static void DoCityGetInfoRequest(Action<bool, CityGetInfoResponse> callback)
			{
				CityGetInfoRequest cityGetInfoRequest = new CityGetInfoRequest();
				cityGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(cityGetInfoRequest, delegate(IMessage response)
				{
					CityGetInfoResponse cityGetInfoResponse = response as CityGetInfoResponse;
					if (cityGetInfoResponse != null && cityGetInfoResponse.Code == 0)
					{
						Action<bool, CityGetInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, cityGetInfoResponse);
						return;
					}
					else
					{
						Action<bool, CityGetInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, cityGetInfoResponse);
						return;
					}
				}, false, false, string.Empty, false);
			}

			private static long newPower;

			private static bool inSendingPower;
		}

		public class Mining
		{
			public static void DoGetMiningInfoRequest(Action<bool, GetMiningInfoResponse> callback)
			{
				GetMiningInfoRequest getMiningInfoRequest = new GetMiningInfoRequest();
				getMiningInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(getMiningInfoRequest, delegate(IMessage response)
				{
					GetMiningInfoResponse getMiningInfoResponse = response as GetMiningInfoResponse;
					if (getMiningInfoResponse == null || getMiningInfoResponse.Code != 0)
					{
						Action<bool, GetMiningInfoResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, getMiningInfoResponse);
						}
						return;
					}
					MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
					dataModule.UpdateMiningInfo(getMiningInfoResponse.MiningInfoDto);
					dataModule.UpdateMiningDrawInfo(getMiningInfoResponse.MiningDrawDto);
					Action<bool, GetMiningInfoResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, getMiningInfoResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoMiningRequest(uint optType, List<int> posList, Action<bool, DoMiningResponse> callback)
			{
				DoMiningRequest doMiningRequest = new DoMiningRequest();
				doMiningRequest.CommonParams = NetworkUtils.GetCommonParams();
				doMiningRequest.OptType = (ulong)optType;
				for (int i = 0; i < posList.Count; i++)
				{
					doMiningRequest.Pos.Add(posList[i]);
				}
				GameApp.NetWork.Send(doMiningRequest, delegate(IMessage response)
				{
					DoMiningResponse doMiningResponse = response as DoMiningResponse;
					if (doMiningResponse != null)
					{
						GameApp.Data.GetDataModule(DataName.MiningDataModule).UpdateMiningInfo(doMiningResponse.MiningInfoDto);
						if (doMiningResponse.Code == 0)
						{
							Action<bool, DoMiningResponse> callback2 = callback;
							if (callback2 != null)
							{
								callback2(true, doMiningResponse);
							}
							GameApp.SDK.Analyze.Track_MiningMine_Mine(optType, doMiningResponse.CommonData.CostDto);
							return;
						}
						Action<bool, DoMiningResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, doMiningResponse);
					}
				}, optType == 1U, false, string.Empty, optType != 2U);
			}

			public static void DoOpenBombRequest(int pos, Action<bool, OpenBombResponse> callback)
			{
				OpenBombRequest openBombRequest = new OpenBombRequest();
				openBombRequest.CommonParams = NetworkUtils.GetCommonParams();
				openBombRequest.Pos = pos;
				GameApp.NetWork.Send(openBombRequest, delegate(IMessage response)
				{
					OpenBombResponse openBombResponse = response as OpenBombResponse;
					if (openBombResponse != null)
					{
						GameApp.Data.GetDataModule(DataName.MiningDataModule).UpdateMiningInfo(openBombResponse.MiningInfoDto);
						if (openBombResponse.Code == 0)
						{
							Action<bool, OpenBombResponse> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, openBombResponse);
							return;
						}
						else
						{
							Action<bool, OpenBombResponse> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, openBombResponse);
						}
					}
				}, false, false, string.Empty, true);
			}

			public static void DoGetMiningRewardRequest(Action<bool, GetMiningRewardResponse> callback, Action closeRewardCallback = null)
			{
				GetMiningRewardRequest getMiningRewardRequest = new GetMiningRewardRequest();
				getMiningRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(getMiningRewardRequest, delegate(IMessage response)
				{
					GetMiningRewardResponse getMiningRewardResponse = response as GetMiningRewardResponse;
					if (getMiningRewardResponse != null)
					{
						MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
						dataModule.UpdateMiningInfo(getMiningRewardResponse.MiningInfoDto);
						GameApp.Event.DispatchNow(null, 445, null);
						if (getMiningRewardResponse.Code == 0)
						{
							if (getMiningRewardResponse.CommonData.Reward != null && getMiningRewardResponse.CommonData.Reward.Count > 0)
							{
								dataModule.CacheTicket();
								DxxTools.UI.OpenRewardCommon(getMiningRewardResponse.CommonData.Reward, closeRewardCallback, true);
							}
							Action<bool, GetMiningRewardResponse> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, getMiningRewardResponse);
							return;
						}
						else
						{
							Action<bool, GetMiningRewardResponse> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, getMiningRewardResponse);
						}
					}
				}, true, false, string.Empty, true);
			}

			public static void DoOpenNextDoorRequest(Action<bool, OpenNextDoorResponse> callback)
			{
				OpenNextDoorRequest openNextDoorRequest = new OpenNextDoorRequest();
				openNextDoorRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(openNextDoorRequest, delegate(IMessage response)
				{
					OpenNextDoorResponse openNextDoorResponse = response as OpenNextDoorResponse;
					if (openNextDoorResponse != null)
					{
						MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
						dataModule.UpdateMiningInfo(openNextDoorResponse.MiningInfoDto);
						dataModule.CacheTicket();
						if (openNextDoorResponse.Code == 0)
						{
							Action<bool, OpenNextDoorResponse> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, openNextDoorResponse);
							return;
						}
						else
						{
							Action<bool, OpenNextDoorResponse> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, openNextDoorResponse);
						}
					}
				}, true, false, string.Empty, true);
			}

			public static void DoSetMiningOptionRequest(uint autoOpt, Action<bool, SetMiningOptionResponse> callback)
			{
				SetMiningOptionRequest setMiningOptionRequest = new SetMiningOptionRequest();
				setMiningOptionRequest.CommonParams = NetworkUtils.GetCommonParams();
				setMiningOptionRequest.AutoOpt = (ulong)autoOpt;
				GameApp.NetWork.Send(setMiningOptionRequest, delegate(IMessage response)
				{
					SetMiningOptionResponse setMiningOptionResponse = response as SetMiningOptionResponse;
					if (setMiningOptionResponse != null)
					{
						GameApp.Data.GetDataModule(DataName.MiningDataModule).SetAutoOpt(setMiningOptionResponse.AutoOpt);
						if (setMiningOptionResponse.Code == 0)
						{
							Action<bool, SetMiningOptionResponse> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, setMiningOptionResponse);
							return;
						}
						else
						{
							Action<bool, SetMiningOptionResponse> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, setMiningOptionResponse);
						}
					}
				}, false, false, string.Empty, true);
			}

			public static void DoBonusDrawRequest(int rate, Action<bool, BounDrawResponse> callback)
			{
				BounDrawRequest bounDrawRequest = new BounDrawRequest();
				bounDrawRequest.CommonParams = NetworkUtils.GetCommonParams();
				bounDrawRequest.Rate = rate;
				MiningDataModule miningDataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
				bool isFree = miningDataModule.MiningDraw.FreeTimes > 0;
				int continueFreeTimes = miningDataModule.MiningDraw.ContinueFreeTimes;
				GameApp.NetWork.Send(bounDrawRequest, delegate(IMessage response)
				{
					BounDrawResponse bounDrawResponse = response as BounDrawResponse;
					if (bounDrawResponse != null)
					{
						miningDataModule.UpdateMiningDrawInfo(bounDrawResponse.DrawInfo);
						miningDataModule.CacheTicket();
						if (bounDrawResponse.Code == 0)
						{
							Action<bool, BounDrawResponse> callback2 = callback;
							if (callback2 != null)
							{
								callback2(true, bounDrawResponse);
							}
							GameApp.SDK.Analyze.Track_MiningTrain(isFree, continueFreeTimes, bounDrawResponse.RewardsIndex.Count, bounDrawResponse.CommonData.Reward);
							return;
						}
						Action<bool, BounDrawResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, bounDrawResponse);
					}
				}, true, false, string.Empty, true);
			}

			public static void DoMiningBoxUpgradeRewardRequest(Action<bool, MiningBoxUpgradeRewardResponse> callback)
			{
				MiningBoxUpgradeRewardRequest miningBoxUpgradeRewardRequest = new MiningBoxUpgradeRewardRequest();
				miningBoxUpgradeRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(miningBoxUpgradeRewardRequest, delegate(IMessage response)
				{
					MiningBoxUpgradeRewardResponse miningBoxUpgradeRewardResponse = response as MiningBoxUpgradeRewardResponse;
					if (miningBoxUpgradeRewardResponse != null)
					{
						MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
						dataModule.UpdateMiningInfo(miningBoxUpgradeRewardResponse.MiningInfoDto);
						dataModule.CacheTicket();
						dataModule.ClearCachePos();
						if (miningBoxUpgradeRewardResponse.Code == 0)
						{
							Action<bool, MiningBoxUpgradeRewardResponse> callback2 = callback;
							if (callback2 != null)
							{
								callback2(true, miningBoxUpgradeRewardResponse);
							}
							GameApp.SDK.Analyze.Track_MiningMine_Capy(miningBoxUpgradeRewardResponse.MiningInfoDto.TreasureUpGradeInfo, miningBoxUpgradeRewardResponse.CommonData.Reward);
							return;
						}
						Action<bool, MiningBoxUpgradeRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, miningBoxUpgradeRewardResponse);
					}
				}, true, false, string.Empty, true);
			}

			public static void DoMiningAdGetItemRequest(Action<bool, MiningAdGetItemResponse> callback)
			{
				MiningAdGetItemRequest miningAdGetItemRequest = new MiningAdGetItemRequest();
				miningAdGetItemRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(miningAdGetItemRequest, delegate(IMessage response)
				{
					MiningAdGetItemResponse miningAdGetItemResponse = response as MiningAdGetItemResponse;
					if (miningAdGetItemResponse != null)
					{
						GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(miningAdGetItemResponse.AdData);
						GameApp.Data.GetDataModule(DataName.MiningDataModule).CacheTicket();
						if (miningAdGetItemResponse.Code == 0)
						{
							Action<bool, MiningAdGetItemResponse> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, miningAdGetItemResponse);
							return;
						}
						else
						{
							Action<bool, MiningAdGetItemResponse> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, miningAdGetItemResponse);
						}
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Mount
		{
			public static void DoMountUpgradeRequest(Action<bool, MountUpgradeResponse> callback)
			{
				MountUpgradeRequest mountUpgradeRequest = new MountUpgradeRequest();
				mountUpgradeRequest.CommonParams = NetworkUtils.GetCommonParams();
				MountInfo mountInfo = GameApp.Data.GetDataModule(DataName.MountDataModule).MountInfo;
				int preStage = (int)mountInfo.Stage;
				int preLevel = (int)mountInfo.Level;
				GameApp.NetWork.Send(mountUpgradeRequest, delegate(IMessage response)
				{
					MountUpgradeResponse mountUpgradeResponse = response as MountUpgradeResponse;
					if (mountUpgradeResponse != null && mountUpgradeResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.MountDataModule).UpdateMountInfo(mountUpgradeResponse.MountInfo);
						RedPointController.Instance.ReCalc("Equip.Mount.UpgradeTag", true);
						Action<bool, MountUpgradeResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, mountUpgradeResponse);
						}
						GameApp.SDK.Analyze.Track_MountLevel(mountUpgradeResponse.MountInfo, mountUpgradeResponse.CommonData.CostDto, preStage, preLevel);
						return;
					}
					Action<bool, MountUpgradeResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, mountUpgradeResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoMountUpgradeAllRequest(Action<bool, MountUpgradeAllResponse> callback)
			{
				MountUpgradeAllRequest mountUpgradeAllRequest = new MountUpgradeAllRequest();
				mountUpgradeAllRequest.CommonParams = NetworkUtils.GetCommonParams();
				MountInfo mountInfo = GameApp.Data.GetDataModule(DataName.MountDataModule).MountInfo;
				int preStage = (int)mountInfo.Stage;
				int preLevel = (int)mountInfo.Level;
				GameApp.NetWork.Send(mountUpgradeAllRequest, delegate(IMessage response)
				{
					MountUpgradeAllResponse mountUpgradeAllResponse = response as MountUpgradeAllResponse;
					if (mountUpgradeAllResponse != null && mountUpgradeAllResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.MountDataModule).UpdateMountInfo(mountUpgradeAllResponse.MountInfo);
						RedPointController.Instance.ReCalc("Equip.Mount.UpgradeTag", true);
						Action<bool, MountUpgradeAllResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, mountUpgradeAllResponse);
						}
						GameApp.SDK.Analyze.Track_MountLevel(mountUpgradeAllResponse.MountInfo, mountUpgradeAllResponse.CommonData.CostDto, preStage, preLevel);
						return;
					}
					Action<bool, MountUpgradeAllResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, mountUpgradeAllResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoMountItemStarRequest(int tableId, Action<bool, MountItemStarResponse> callback)
			{
				MountItemStarRequest mountItemStarRequest = new MountItemStarRequest();
				mountItemStarRequest.CommonParams = NetworkUtils.GetCommonParams();
				mountItemStarRequest.ConfigId = tableId;
				GameApp.NetWork.Send(mountItemStarRequest, delegate(IMessage response)
				{
					MountItemStarResponse mountItemStarResponse = response as MountItemStarResponse;
					if (mountItemStarResponse != null && mountItemStarResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.MountDataModule).UpdateMountItemDto(mountItemStarResponse.MountItemDto);
						RedPointController.Instance.ReCalc("Equip.Mount.AdvanceTag", true);
						Action<bool, MountItemStarResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, mountItemStarResponse);
						}
						GameApp.SDK.Analyze.Track_RareMountUpgrade(tableId, mountItemStarResponse.MountItemDto, mountItemStarResponse.CommonData.CostDto);
						return;
					}
					Action<bool, MountItemStarResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, mountItemStarResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoMountApplySkillRequest(int advanceId, int optType, Action<bool, MountApplySkillResponse> callback)
			{
				MountApplySkillRequest mountApplySkillRequest = new MountApplySkillRequest();
				mountApplySkillRequest.CommonParams = NetworkUtils.GetCommonParams();
				mountApplySkillRequest.AdvanceId = advanceId;
				mountApplySkillRequest.OptType = optType;
				GameApp.NetWork.Send(mountApplySkillRequest, delegate(IMessage response)
				{
					MountApplySkillResponse mountApplySkillResponse = response as MountApplySkillResponse;
					if (mountApplySkillResponse != null && mountApplySkillResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.MountDataModule).UpdateMountInfo(mountApplySkillResponse.MountInfo);
						Action<bool, MountApplySkillResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, mountApplySkillResponse);
						return;
					}
					else
					{
						Action<bool, MountApplySkillResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, mountApplySkillResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoMountDressRequest(int type, int configId, int optType, Action<bool, MountDressResponse> callback)
			{
				MountDressRequest mountDressRequest = new MountDressRequest();
				mountDressRequest.CommonParams = NetworkUtils.GetCommonParams();
				mountDressRequest.ConfigType = type;
				mountDressRequest.ConfigId = configId;
				mountDressRequest.OptType = optType;
				if (optType != -1)
				{
					PlayerPrefsKeys.SetMountRideRed("1");
				}
				GameApp.NetWork.Send(mountDressRequest, delegate(IMessage response)
				{
					MountDressResponse mountDressResponse = response as MountDressResponse;
					if (mountDressResponse != null && mountDressResponse.Code == 0)
					{
						MountDataModule dataModule = GameApp.Data.GetDataModule(DataName.MountDataModule);
						uint configType = dataModule.MountInfo.ConfigType;
						uint configId2 = dataModule.MountInfo.ConfigId;
						dataModule.UpdateMountInfo(mountDressResponse.MountInfo);
						RedPointController.Instance.ReCalc("Equip.Mount.RideTag", true);
						if (configType != mountDressResponse.MountInfo.ConfigType || configId2 != mountDressResponse.MountInfo.ConfigId)
						{
							GameApp.Event.DispatchNow(null, LocalMessageName.CC_UIMount_ChangeRide, null);
						}
						Action<bool, MountDressResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, mountDressResponse);
						}
						GameApp.SDK.Analyze.Track_EquipMount(mountDressResponse.MountInfo);
						return;
					}
					Action<bool, MountDressResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, mountDressResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoMountUnlockRequest(int configId, Action<bool, MountUnlockResponse> callback)
			{
				MountUnlockRequest mountUnlockRequest = new MountUnlockRequest();
				mountUnlockRequest.CommonParams = NetworkUtils.GetCommonParams();
				mountUnlockRequest.ConfigId = configId;
				GameApp.NetWork.Send(mountUnlockRequest, delegate(IMessage response)
				{
					MountUnlockResponse mountUnlockResponse = response as MountUnlockResponse;
					if (mountUnlockResponse != null && mountUnlockResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.MountDataModule).UpdateMountItemDtos(mountUnlockResponse.MountItemDtos);
						RedPointController.Instance.ReCalc("Equip.Mount.AdvanceTag", true);
						Action<bool, MountUnlockResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, mountUnlockResponse);
						}
						GameApp.SDK.Analyze.Track_MountUnlock(configId);
						return;
					}
					Action<bool, MountUnlockResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, mountUnlockResponse);
				}, true, false, string.Empty, true);
			}
		}

		public class NewWorld
		{
			public static void DoEnterRequest(Action<bool, EnterResponse> callback)
			{
				EnterRequest enterRequest = new EnterRequest();
				enterRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(enterRequest, delegate(IMessage response)
				{
					EnterResponse enterResponse = response as EnterResponse;
					if (enterResponse != null && enterResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.NewWorldDataModule).EnterNewWorld();
						Action<bool, EnterResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, enterResponse);
						return;
					}
					else
					{
						Action<bool, EnterResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, enterResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoLikeRequest(int index, Action<bool, LikeResponse> callback)
			{
				LikeRequest likeRequest = new LikeRequest();
				likeRequest.CommonParams = NetworkUtils.GetCommonParams();
				likeRequest.RankIndex = index;
				GameApp.NetWork.Send(likeRequest, delegate(IMessage response)
				{
					LikeResponse likeResponse = response as LikeResponse;
					if (likeResponse != null && likeResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.NewWorldDataModule).UpdateLikeInfo(likeResponse);
						Action<bool, LikeResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, likeResponse);
						return;
					}
					else
					{
						Action<bool, LikeResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, likeResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoNewWorldInfoRequest(Action<bool, NewWorldInfoResponse> callback)
			{
				NewWorldInfoRequest newWorldInfoRequest = new NewWorldInfoRequest();
				newWorldInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(newWorldInfoRequest, delegate(IMessage response)
				{
					NewWorldInfoResponse newWorldInfoResponse = response as NewWorldInfoResponse;
					if (newWorldInfoResponse != null && newWorldInfoResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.NewWorldDataModule).UpdateInfo(newWorldInfoResponse);
						Action<bool, NewWorldInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, newWorldInfoResponse);
						return;
					}
					else
					{
						Action<bool, NewWorldInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, newWorldInfoResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoNewWorldTaskRewardRequest(int tableId, Action<bool, NewWorldTaskRewardResponse> callback)
			{
				NewWorldTaskRewardRequest newWorldTaskRewardRequest = new NewWorldTaskRewardRequest();
				newWorldTaskRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				newWorldTaskRewardRequest.ConfigId = tableId;
				GameApp.NetWork.Send(newWorldTaskRewardRequest, delegate(IMessage response)
				{
					NewWorldTaskRewardResponse newWorldTaskRewardResponse = response as NewWorldTaskRewardResponse;
					if (newWorldTaskRewardResponse != null && newWorldTaskRewardResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.NewWorldDataModule).UpdateTaskData(newWorldTaskRewardResponse);
						Action<bool, NewWorldTaskRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, newWorldTaskRewardResponse);
						return;
					}
					else
					{
						Action<bool, NewWorldTaskRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, newWorldTaskRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Pet
		{
			public static void PetInfoRefreshRequest(Action<bool, UserRefDataResponse> callback)
			{
				UserRefDataRequest userRefDataRequest = new UserRefDataRequest();
				userRefDataRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(userRefDataRequest, delegate(IMessage response)
				{
					UserRefDataResponse userRefDataResponse = response as UserRefDataResponse;
					if (userRefDataResponse != null && userRefDataResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.PetDataModule).UpdatePetInfo(userRefDataResponse.PetInfo);
						RedPointController.Instance.ReCalc("Equip.Pet", true);
						GameApp.Event.DispatchNow(null, 153, null);
						Action<bool, UserRefDataResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userRefDataResponse);
						return;
					}
					else
					{
						Action<bool, UserRefDataResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, null);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void PetDrawRequest(int petBoxType, Action<PetDrawResponse> successCallback)
			{
				PetDrawRequest petDrawRequest = new PetDrawRequest();
				petDrawRequest.CommonParams = NetworkUtils.GetCommonParams();
				petDrawRequest.Type = petBoxType;
				GameApp.NetWork.Send(petDrawRequest, delegate(IMessage response)
				{
					PetDrawResponse petDrawResponse = response as PetDrawResponse;
					if (petDrawResponse != null && petDrawResponse.Code == 0)
					{
						PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
						List<PetDto> list = new List<PetDto>();
						if (petDrawResponse.ShowPet != null)
						{
							for (int i = 0; i < petDrawResponse.ShowPet.Count; i++)
							{
								PetDto petDto = petDrawResponse.ShowPet[i];
								petDto.RowId = (ulong)((long)i + 1L);
								if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)petDto.ConfigId)
									.quality >= 6)
								{
									list.Add(petDto);
								}
							}
							if (petDrawResponse.ShowPet.Count > 0 && list.Count == 0)
							{
								int num = 0;
								int num2 = -1;
								int num3 = -1;
								for (int j = 0; j < petDrawResponse.ShowPet.Count; j++)
								{
									PetDto petDto2 = petDrawResponse.ShowPet[j];
									Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById((int)petDto2.ConfigId);
									if (elementById != null && elementById.quality > num2 && elementById.id > num3)
									{
										num2 = elementById.quality;
										num = j;
										num3 = (int)petDto2.ConfigId;
									}
								}
								list.Add(petDrawResponse.ShowPet[num]);
							}
						}
						dataModule.UpdatePetDrawData(petDrawResponse);
						RedPointController.Instance.ReCalc("Equip.Pet", true);
						List<ItemData> list2 = new List<ItemData>();
						for (int k = 0; k < petDrawResponse.AddPet.Count; k++)
						{
							PetDto petDto3 = petDrawResponse.AddPet[k];
							ItemData itemData = new ItemData((int)petDto3.ConfigId, (long)((ulong)petDto3.PetCount));
							list2.Add(itemData);
						}
						PetOpenEggViewModule.OpenData openData = new PetOpenEggViewModule.OpenData();
						openData.petBoxType = petBoxType;
						openData.petList = list;
						openData.rewardList = list2;
						openData.newPetRowIds = new List<ulong>();
						if (!GameApp.View.IsOpened(ViewName.PetOpenEggViewModule))
						{
							GameApp.View.OpenView(ViewName.PetOpenEggViewModule, openData, 1, null, delegate
							{
								GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetDataModule_RefreshPetDrawInfo, null);
							});
						}
						else
						{
							EventArgsDrawPetResultData eventArgsDrawPetResultData = new EventArgsDrawPetResultData();
							eventArgsDrawPetResultData.openData = openData;
							GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetOpenEggViewModule_RefreshData, eventArgsDrawPetResultData);
							GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetDataModule_RefreshPetDrawInfo, null);
						}
						if (petBoxType == 11)
						{
							GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(8), "REWARD ", "", null, petDrawResponse.ShowPet);
						}
						Action<PetDrawResponse> successCallback2 = successCallback;
						if (successCallback2 == null)
						{
							return;
						}
						successCallback2(petDrawResponse);
					}
				}, true, false, string.Empty, true);
			}

			public static void PetComposeRequest(ulong rowId, Action<bool, PetComposeResponse> callback)
			{
				PetComposeRequest petComposeRequest = new PetComposeRequest();
				petComposeRequest.CommonParams = NetworkUtils.GetCommonParams();
				petComposeRequest.RowId = rowId;
				GameApp.NetWork.Send(petComposeRequest, delegate(IMessage response)
				{
					PetComposeResponse petComposeResponse = response as PetComposeResponse;
					if (petComposeResponse != null && petComposeResponse.Code == 0)
					{
						EventArgsFragmentMergePet instance = Singleton<EventArgsFragmentMergePet>.Instance;
						instance.addPets = petComposeResponse.PetDto;
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetDataModule_FragmentMergePet, instance);
						RedPointController.Instance.ReCalc("Equip.Pet", true);
						Action<bool, PetComposeResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, petComposeResponse);
						}
						if (petComposeResponse.PetDto.Count > 0)
						{
							PetStarUpgradeViewModule.OpenData openData = new PetStarUpgradeViewModule.OpenData();
							openData.petData = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetData(petComposeResponse.PetDto[0].RowId);
							GameApp.View.OpenView(ViewName.PetStarUpgradeViewModule, openData, 1, null, null);
							return;
						}
					}
					else
					{
						Action<bool, PetComposeResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, petComposeResponse);
					}
				}, true, false, string.Empty, true);
			}

			public static void PetStarUpgradeRequest(ulong rowId, Action<bool, PetStarResponse> callback)
			{
				PetStarRequest petStarRequest = new PetStarRequest();
				petStarRequest.CommonParams = NetworkUtils.GetCommonParams();
				petStarRequest.RowId = rowId;
				GameApp.NetWork.Send(petStarRequest, delegate(IMessage response)
				{
					PetStarResponse petStarResponse = response as PetStarResponse;
					if (petStarResponse != null && petStarResponse.Code == 0)
					{
						NetworkUtils.HandleResponse_UpdatePets(petStarResponse.PetDto);
						PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
						for (int i = 0; i < petStarResponse.PetDto.Count; i++)
						{
							if (dataModule.IsDeploy(petStarResponse.PetDto[i].RowId))
							{
								dataModule.MathAddAttributeData();
								GameApp.Event.DispatchNow(null, 145, null);
								break;
							}
						}
						Action<bool, PetStarResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, petStarResponse);
						return;
					}
					else
					{
						Action<bool, PetStarResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, petStarResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void PetLevelUpRequest(ulong rwoId, bool isQuickLevelUp, Action<bool, PetStrengthResponse> callback)
			{
				PetStrengthRequest petStrengthRequest = new PetStrengthRequest();
				petStrengthRequest.CommonParams = NetworkUtils.GetCommonParams();
				petStrengthRequest.RowId = rwoId;
				if (isQuickLevelUp)
				{
					petStrengthRequest.OptType = 1;
				}
				GameApp.NetWork.Send(petStrengthRequest, delegate(IMessage response)
				{
					PetStrengthResponse petStrengthResponse = response as PetStrengthResponse;
					if (petStrengthResponse != null && petStrengthResponse.Code == 0)
					{
						RepeatedField<PetDto> repeatedField = new RepeatedField<PetDto>();
						repeatedField.Add(petStrengthResponse.PetDto);
						NetworkUtils.HandleResponse_UpdatePets(repeatedField);
						GameApp.Data.GetDataModule(DataName.PetDataModule).MathAddAttributeData();
						GameApp.Event.DispatchNow(null, 145, null);
						Action<bool, PetStrengthResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, petStrengthResponse);
						}
						GameApp.SDK.Analyze.Track_PetLevel(petStrengthResponse.PetDto, petStrengthResponse.CommonData.CostDto);
						return;
					}
					Action<bool, PetStrengthResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, petStrengthResponse);
				}, true, false, string.Empty, true);
			}

			public static void PetResetRequest(ulong rwoId, Action<bool, PetResetResponse> callback)
			{
				PetResetRequest petResetRequest = new PetResetRequest();
				petResetRequest.CommonParams = NetworkUtils.GetCommonParams();
				petResetRequest.RowId = rwoId;
				GameApp.NetWork.Send(petResetRequest, delegate(IMessage response)
				{
					PetResetResponse petResetResponse = response as PetResetResponse;
					if (petResetResponse != null && petResetResponse.Code == 0)
					{
						NetworkUtils.HandleResponse_UpdatePets(petResetResponse.PetDto);
						GameApp.Data.GetDataModule(DataName.PetDataModule).MathAddAttributeData();
						GameApp.Event.DispatchNow(null, 145, null);
						if (petResetResponse.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(petResetResponse.CommonData.Reward, null, true);
						}
						Action<bool, PetResetResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, petResetResponse);
						return;
					}
					else
					{
						Action<bool, PetResetResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, petResetResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void PetShowRequest(List<ulong> rowIds, Action<bool, PetShowResponse> callback, bool isShowMask = true)
			{
				PetShowRequest petShowRequest = new PetShowRequest();
				petShowRequest.CommonParams = NetworkUtils.GetCommonParams();
				petShowRequest.RowId.AddRange(rowIds);
				GameApp.NetWork.Send(petShowRequest, delegate(IMessage response)
				{
					PetShowResponse petShowResponse = response as PetShowResponse;
					if (petShowResponse != null && petShowResponse.Code == 0)
					{
						NetworkUtils.HandleResponse_UpdatePets(petShowResponse.PetDtos);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetDataModule_ShowIdsChange, null);
						Action<bool, PetShowResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, petShowResponse);
						return;
					}
					else
					{
						Action<bool, PetShowResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, petShowResponse);
						return;
					}
				}, isShowMask, false, string.Empty, true);
			}

			public static void PetFormatPosRequest(List<ulong> rowIds, Action<bool, PetFormationResponse> callback)
			{
				PetFormationRequest petFormationRequest = new PetFormationRequest();
				petFormationRequest.CommonParams = NetworkUtils.GetCommonParams();
				petFormationRequest.RowId.AddRange(rowIds);
				GameApp.NetWork.Send(petFormationRequest, delegate(IMessage response)
				{
					PetFormationResponse petFormationResponse = response as PetFormationResponse;
					if (petFormationResponse != null && petFormationResponse.Code == 0)
					{
						NetworkUtils.HandleResponse_UpdatePets(petFormationResponse.PetDtos);
						PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
						dataModule.UpdatePetFormationData();
						dataModule.MathAddAttributeData();
						GameApp.Event.DispatchNow(null, 145, null);
						Action<bool, PetFormationResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, petFormationResponse);
						return;
					}
					else
					{
						Action<bool, PetFormationResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, petFormationResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void PetFetterActiveRequest(int configId, Action<bool, PetFetterActiveResponse> callback)
			{
				PetFetterActiveRequest petFetterActiveRequest = new PetFetterActiveRequest();
				petFetterActiveRequest.CommonParams = NetworkUtils.GetCommonParams();
				petFetterActiveRequest.ConfigId = configId;
				GameApp.NetWork.Send(petFetterActiveRequest, delegate(IMessage response)
				{
					PetFetterActiveResponse petFetterActiveResponse = response as PetFetterActiveResponse;
					if (petFetterActiveResponse != null && petFetterActiveResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.PetDataModule).UpdateCollectionData(petFetterActiveResponse.ConfigId);
						GameApp.Data.GetDataModule(DataName.PetDataModule).MathAddAttributeData();
						GameApp.Event.DispatchNow(null, 145, null);
						RedPointController.Instance.ReCalc("Equip.Pet", true);
						Action<bool, PetFetterActiveResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, petFetterActiveResponse);
						return;
					}
					else
					{
						Action<bool, PetFetterActiveResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, petFetterActiveResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void PetTrainRequest(ulong rowId, List<int> lockList, Action<bool, PetTrainResponse> callback)
			{
				PetTrainRequest petTrainRequest = new PetTrainRequest();
				petTrainRequest.CommonParams = NetworkUtils.GetCommonParams();
				petTrainRequest.RowId = rowId;
				if (lockList != null)
				{
					for (int i = 0; i < lockList.Count; i++)
					{
						petTrainRequest.LockIndex.Add(lockList[i]);
					}
				}
				GameApp.NetWork.Send(petTrainRequest, delegate(IMessage response)
				{
					PetTrainResponse petTrainResponse = response as PetTrainResponse;
					if (petTrainResponse != null && petTrainResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.PetDataModule).UpdateTrainingData(petTrainResponse.TrainingLevel, petTrainResponse.TrainingExp);
						RepeatedField<PetDto> repeatedField = new RepeatedField<PetDto>();
						repeatedField.Add(petTrainResponse.PetDto);
						NetworkUtils.HandleResponse_UpdatePets(repeatedField);
						Action<bool, PetTrainResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, petTrainResponse);
						return;
					}
					else
					{
						Action<bool, PetTrainResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, petTrainResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void PetTrainSureRequest(ulong rowId, Action<bool, PetTrainSureResponse> callback)
			{
				PetTrainSureRequest petTrainSureRequest = new PetTrainSureRequest();
				petTrainSureRequest.CommonParams = NetworkUtils.GetCommonParams();
				petTrainSureRequest.RowId = rowId;
				GameApp.NetWork.Send(petTrainSureRequest, delegate(IMessage response)
				{
					PetTrainSureResponse petTrainSureResponse = response as PetTrainSureResponse;
					if (petTrainSureResponse != null && petTrainSureResponse.Code == 0)
					{
						RepeatedField<PetDto> repeatedField = new RepeatedField<PetDto>();
						repeatedField.Add(petTrainSureResponse.PetDto);
						NetworkUtils.HandleResponse_UpdatePets(repeatedField);
						PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
						if (dataModule.IsDeploy(petTrainSureResponse.PetDto.RowId))
						{
							dataModule.MathAddAttributeData();
							GameApp.Event.DispatchNow(null, 145, null);
						}
						Action<bool, PetTrainSureResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, petTrainSureResponse);
						return;
					}
					else
					{
						Action<bool, PetTrainSureResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, petTrainSureResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class PlayerData
		{
			public static void RequestUnlockUserAvatar(int itemType, int itemId, Action<bool, UnlockUserAvatarResponse> callback, bool isShowMask = true)
			{
				UnlockUserAvatarRequest unlockUserAvatarRequest = new UnlockUserAvatarRequest();
				unlockUserAvatarRequest.CommonParams = NetworkUtils.GetCommonParams();
				unlockUserAvatarRequest.ItemType = (uint)itemType;
				unlockUserAvatarRequest.ItemId = (uint)itemId;
				GameApp.NetWork.Send(unlockUserAvatarRequest, delegate(IMessage response)
				{
					UnlockUserAvatarResponse unlockUserAvatarResponse = response as UnlockUserAvatarResponse;
					if (unlockUserAvatarResponse != null && unlockUserAvatarResponse.Code == 0)
					{
						LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
						dataModule.ServerSetUserInfo(unlockUserAvatarResponse.UserInfoDto, false);
						dataModule.UpdateUnlockAllAvatarClotheScene();
						if (itemType < 3 || itemType == 7)
						{
							RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Avatar", true);
						}
						else if (itemType == 6)
						{
							RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Scene", true);
						}
						else
						{
							RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Clothes", true);
						}
						RedPointController.Instance.ReCalc("Equip.Fashion", true);
						Action<bool, UnlockUserAvatarResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, unlockUserAvatarResponse);
						}
						NetworkUtils.PlayerData.SetUserInfoToChat();
						return;
					}
					Action<bool, UnlockUserAvatarResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, unlockUserAvatarResponse);
				}, isShowMask, false, string.Empty, true);
			}

			public static void RequestUpdateUserAvatar(int partType, int changeTableId, Action<bool, UpdateUserAvatarResponse> callback, bool isShowMask = true)
			{
				LoginDataModule loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				ClothesData selfClothesData = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfClothesData;
				AvatarClothesData avatarClothesData = new AvatarClothesData();
				avatarClothesData.AvatarIconId = loginDataModule.Avatar;
				avatarClothesData.AvatarFrameId = loginDataModule.AvatarFrame;
				avatarClothesData.SceneSkinId = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfSceneSkinData.CurSkinId;
				avatarClothesData.ClothesData = new ClothesData(selfClothesData.HeadId, selfClothesData.BodyId, selfClothesData.AccessoryId);
				if (partType == 1)
				{
					avatarClothesData.AvatarIconId = changeTableId;
				}
				else if (partType == 2)
				{
					avatarClothesData.AvatarFrameId = changeTableId;
				}
				else if (partType == 7)
				{
					avatarClothesData.AvatarTitleId = changeTableId;
				}
				else if (partType == 3)
				{
					avatarClothesData.ClothesData.DressPart(SkinType.Body, changeTableId);
				}
				else if (partType == 4)
				{
					avatarClothesData.ClothesData.DressPart(SkinType.Head, changeTableId);
				}
				else if (partType == 5)
				{
					avatarClothesData.ClothesData.DressPart(SkinType.Back, changeTableId);
				}
				else if (partType == 6)
				{
					avatarClothesData.SceneSkinId = changeTableId;
				}
				UpdateUserAvatarRequest updateUserAvatarRequest = new UpdateUserAvatarRequest();
				updateUserAvatarRequest.CommonParams = NetworkUtils.GetCommonParams();
				updateUserAvatarRequest.AvatarId = (uint)avatarClothesData.AvatarIconId;
				updateUserAvatarRequest.AvatarFrameId = (uint)avatarClothesData.AvatarFrameId;
				updateUserAvatarRequest.TitleId = (uint)avatarClothesData.AvatarTitleId;
				updateUserAvatarRequest.SkinHeaddressId = (uint)avatarClothesData.ClothesData.HeadId;
				updateUserAvatarRequest.SkinBodyId = (uint)avatarClothesData.ClothesData.BodyId;
				updateUserAvatarRequest.SkinAccessoryId = (uint)avatarClothesData.ClothesData.AccessoryId;
				updateUserAvatarRequest.BackGround = (uint)avatarClothesData.SceneSkinId;
				GameApp.NetWork.Send(updateUserAvatarRequest, delegate(IMessage response)
				{
					UpdateUserAvatarResponse updateUserAvatarResponse = response as UpdateUserAvatarResponse;
					if (updateUserAvatarResponse != null && updateUserAvatarResponse.Code == 0)
					{
						loginDataModule.ServerSetUserInfo(updateUserAvatarResponse.UserInfoDto, false);
						Action<bool, UpdateUserAvatarResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, updateUserAvatarResponse);
						}
						NetworkUtils.PlayerData.SetUserInfoToChat();
						return;
					}
					Action<bool, UpdateUserAvatarResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, updateUserAvatarResponse);
				}, isShowMask, false, string.Empty, true);
			}

			public static void DoUserUpdateInfoRequest(string nickName, uint avatar, uint avatarframe, Action<bool, UserUpdateInfoResponse> callback, bool isShowMask = false)
			{
				UserUpdateInfoRequest userUpdateInfoRequest = new UserUpdateInfoRequest();
				userUpdateInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				LoginDataModule ldm = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				if (string.IsNullOrEmpty(nickName))
				{
					userUpdateInfoRequest.NickName = ldm.ServerSetNickName;
				}
				else
				{
					userUpdateInfoRequest.NickName = nickName;
				}
				if (avatar <= 0U)
				{
					userUpdateInfoRequest.Avatar = (uint)ldm.Avatar;
				}
				else
				{
					userUpdateInfoRequest.Avatar = avatar;
				}
				if (avatarframe <= 0U)
				{
					userUpdateInfoRequest.AvatarFrame = (uint)ldm.AvatarFrame;
				}
				else
				{
					userUpdateInfoRequest.AvatarFrame = avatarframe;
				}
				GameApp.NetWork.Send(userUpdateInfoRequest, delegate(IMessage response)
				{
					UserUpdateInfoResponse userUpdateInfoResponse = response as UserUpdateInfoResponse;
					if (userUpdateInfoResponse != null && userUpdateInfoResponse.Code == 0)
					{
						ldm.ServerSetUserInfo(userUpdateInfoResponse.UserInfoDto, false);
						Action<bool, UserUpdateInfoResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, userUpdateInfoResponse);
						}
						NetworkUtils.PlayerData.SetUserInfoToChat();
						return;
					}
					HLog.LogError(string.Format("玩家信息-同步昵称失败 nickName = {0},Code = {1}", nickName, userUpdateInfoResponse.Code));
					Action<bool, UserUpdateInfoResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, userUpdateInfoResponse);
				}, isShowMask, false, string.Empty, true);
			}

			public static void UserGetPlayerInfoRequest(Action<bool, UserGetPlayerInfoResponse> callback, long userid)
			{
				UserGetPlayerInfoRequest userGetPlayerInfoRequest = new UserGetPlayerInfoRequest();
				userGetPlayerInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				userGetPlayerInfoRequest.PlayerUserId = userid;
				GameApp.NetWork.Send(userGetPlayerInfoRequest, delegate(IMessage response)
				{
					UserGetPlayerInfoResponse userGetPlayerInfoResponse = response as UserGetPlayerInfoResponse;
					if (userGetPlayerInfoResponse != null && userGetPlayerInfoResponse.Code == 0)
					{
						Action<bool, UserGetPlayerInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userGetPlayerInfoResponse);
						return;
					}
					else
					{
						Action<bool, UserGetPlayerInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userGetPlayerInfoResponse);
						return;
					}
				}, false, false, string.Empty, false);
			}

			private static void SetUserInfoToChat()
			{
			}

			public static void TipSendUserGetInfoRequest(string contextKey, Action success = null)
			{
				if (NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting)
				{
					return;
				}
				NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting = true;
				string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID(contextKey), Array.Empty<object>());
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("17");
				Action<bool, UserGetIapInfoResponse> <>9__1;
				DxxTools.UI.OpenPopCommon(text, delegate(int id)
				{
					Action<bool, UserGetIapInfoResponse> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(bool isOk, UserGetIapInfoResponse response)
						{
							NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting = false;
							Action success2 = success;
							if (success2 == null)
							{
								return;
							}
							success2();
						});
					}
					NetworkUtils.PlayerData.SendUserGetIapInfoRequest(action);
				}, string.Empty, infoByID, string.Empty, false, 2);
			}

			public static void SendUserGetIapInfoRequest(Action success)
			{
				if (NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting)
				{
					return;
				}
				NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting = true;
				NetworkUtils.PlayerData.SendUserGetIapInfoRequest(delegate(bool isOk, UserGetIapInfoResponse response)
				{
					NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting = false;
					Action success2 = success;
					if (success2 == null)
					{
						return;
					}
					success2();
				});
			}

			private static void SendUserGetIapInfoRequest(Action<bool, UserGetIapInfoResponse> callback)
			{
				UserGetIapInfoRequest userGetIapInfoRequest = new UserGetIapInfoRequest();
				userGetIapInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(userGetIapInfoRequest, delegate(IMessage response)
				{
					UserGetIapInfoResponse userGetIapInfoResponse = response as UserGetIapInfoResponse;
					if (userGetIapInfoResponse != null && userGetIapInfoResponse.Code == 0)
					{
						EventArgsRefreshIAPInfoData eventArgsRefreshIAPInfoData = new EventArgsRefreshIAPInfoData();
						eventArgsRefreshIAPInfoData.SetData(userGetIapInfoResponse.IapInfo);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, eventArgsRefreshIAPInfoData);
						Action<bool, UserGetIapInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userGetIapInfoResponse);
						return;
					}
					else
					{
						Action<bool, UserGetIapInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userGetIapInfoResponse);
						return;
					}
				}, false, false, string.Empty, false);
			}

			public static void UserOpenModelRequest(List<uint> functionIds, Action<bool, UserOpenModelResponse> callback)
			{
				UserOpenModelRequest userOpenModelRequest = new UserOpenModelRequest();
				userOpenModelRequest.CommonParams = NetworkUtils.GetCommonParams();
				userOpenModelRequest.ModelIds.AddRange(functionIds);
				GameApp.NetWork.Send(userOpenModelRequest, delegate(IMessage response)
				{
					UserOpenModelResponse userOpenModelResponse = response as UserOpenModelResponse;
					if (userOpenModelResponse != null && userOpenModelResponse.Code == 0)
					{
						Action<bool, UserOpenModelResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userOpenModelResponse);
						return;
					}
					else
					{
						Action<bool, UserOpenModelResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userOpenModelResponse);
						return;
					}
				}, false, false, string.Empty, false);
			}

			public static void SendUserGetAllPanelInfoRequest(Action<bool, UserGetAllPanelInfoResponse> callback)
			{
				UserGetAllPanelInfoRequest userGetAllPanelInfoRequest = new UserGetAllPanelInfoRequest();
				userGetAllPanelInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(userGetAllPanelInfoRequest, delegate(IMessage response)
				{
					UserGetAllPanelInfoResponse userGetAllPanelInfoResponse = response as UserGetAllPanelInfoResponse;
					if (userGetAllPanelInfoResponse != null && userGetAllPanelInfoResponse.Code == 0)
					{
						Action<bool, UserGetAllPanelInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userGetAllPanelInfoResponse);
						return;
					}
					else
					{
						Action<bool, UserGetAllPanelInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userGetAllPanelInfoResponse);
						return;
					}
				}, false, false, string.Empty, false);
			}

			private static bool isTipAndSendUserGetInfoRequesting;
		}

		public class Purchase
		{
			public static void SendPayInAppPurchaseRequest(int purchaseId, string receipt, int extraType, string extraInfo, ulong preOrderID, Action<bool, PayInAppPurchaseResponse> callback)
			{
				PayInAppPurchaseRequest payInAppPurchaseRequest = new PayInAppPurchaseRequest();
				payInAppPurchaseRequest.CommonParams = NetworkUtils.GetCommonParams();
				payInAppPurchaseRequest.ProductId = purchaseId.ToString();
				payInAppPurchaseRequest.ExtraInfo = extraInfo;
				payInAppPurchaseRequest.ReceiptData = receipt;
				payInAppPurchaseRequest.PreOrderId = preOrderID;
				payInAppPurchaseRequest.ExtraType = (uint)extraType;
				payInAppPurchaseRequest.PlatformIndex = (uint)Singleton<PlatformHelper>.Instance.GetPlatformIndex();
				payInAppPurchaseRequest.ChannelId = (uint)Singleton<PlatformHelper>.Instance.GetChannelIndex();
				GameApp.NetWork.Send(payInAppPurchaseRequest, delegate(IMessage response)
				{
					PayInAppPurchaseResponse payInAppPurchaseResponse = response as PayInAppPurchaseResponse;
					if (payInAppPurchaseResponse != null && payInAppPurchaseResponse.Code == 0)
					{
						EventArgsRefreshIAPCommonData instance = Singleton<EventArgsRefreshIAPCommonData>.Instance;
						instance.SetData(payInAppPurchaseResponse.RechargeIds, false);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshCommonData, instance);
						EventArgsRefreshIAPInfoData instance2 = Singleton<EventArgsRefreshIAPInfoData>.Instance;
						instance2.SetData(payInAppPurchaseResponse.IapInfo);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, instance2);
						Action<bool, PayInAppPurchaseResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, payInAppPurchaseResponse);
						return;
					}
					else
					{
						Action<bool, PayInAppPurchaseResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, payInAppPurchaseResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void SendPayInAppPurchaseRequest(int purchaseId, string receipt, Action<bool, PayInAppPurchaseResponse> callback)
			{
				PayInAppPurchaseRequest payInAppPurchaseRequest = new PayInAppPurchaseRequest();
				payInAppPurchaseRequest.CommonParams = NetworkUtils.GetCommonParams();
				payInAppPurchaseRequest.ProductId = purchaseId.ToString();
				payInAppPurchaseRequest.ReceiptData = receipt;
				payInAppPurchaseRequest.PlatformIndex = 1U;
				payInAppPurchaseRequest.ChannelId = 2U;
				GameApp.NetWork.Send(payInAppPurchaseRequest, delegate(IMessage response)
				{
					PayInAppPurchaseResponse payInAppPurchaseResponse = response as PayInAppPurchaseResponse;
					if (payInAppPurchaseResponse != null && payInAppPurchaseResponse.Code == 0)
					{
						EventArgsRefreshIAPCommonData instance = Singleton<EventArgsRefreshIAPCommonData>.Instance;
						instance.SetData(payInAppPurchaseResponse.RechargeIds, false);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshCommonData, instance);
						EventArgsRefreshIAPInfoData instance2 = Singleton<EventArgsRefreshIAPInfoData>.Instance;
						instance2.SetData(payInAppPurchaseResponse.IapInfo);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, instance2);
						Action<bool, PayInAppPurchaseResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, payInAppPurchaseResponse);
						return;
					}
					else
					{
						Action<bool, PayInAppPurchaseResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, payInAppPurchaseResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void SendPayPreOrderRequest(int purchaseId, string productId, int extraType, string extraInfo, long serverTime, Action<bool, PayPreOrderResponse> callback)
			{
				PayPreOrderRequest payPreOrderRequest = new PayPreOrderRequest();
				payPreOrderRequest.CommonParams = NetworkUtils.GetCommonParams();
				payPreOrderRequest.ProductId = purchaseId.ToString();
				payPreOrderRequest.ExtraInfo = extraInfo;
				payPreOrderRequest.PreOrderId = (ulong)serverTime;
				payPreOrderRequest.ExtraType = (uint)extraType;
				GameApp.NetWork.Send(payPreOrderRequest, delegate(IMessage response)
				{
					PayPreOrderResponse payPreOrderResponse = response as PayPreOrderResponse;
					if (payPreOrderResponse == null || payPreOrderResponse.Code != 0)
					{
						Action<bool, PayPreOrderResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, payPreOrderResponse);
						}
						if (payPreOrderResponse != null)
						{
							int code = payPreOrderResponse.Code;
						}
						return;
					}
					Action<bool, PayPreOrderResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, payPreOrderResponse);
				}, true, false, string.Empty, true);
			}

			public static void BattlePassRewardRequest(List<uint> idlist, Action<bool, BattlePassRewardResponse> callback)
			{
				BattlePassRewardRequest battlePassRewardRequest = new BattlePassRewardRequest();
				battlePassRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				battlePassRewardRequest.BattlePassRewardIdList.AddRange(idlist);
				GameApp.NetWork.Send(battlePassRewardRequest, delegate(IMessage response)
				{
					BattlePassRewardResponse battlePassRewardResponse = response as BattlePassRewardResponse;
					if (battlePassRewardResponse != null && battlePassRewardResponse.BattlePassDto != null)
					{
						EventArgsRefreshIAPBattlePassData instance = Singleton<EventArgsRefreshIAPBattlePassData>.Instance;
						instance.SetData(battlePassRewardResponse.BattlePassDto);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPBattlePassData, instance);
					}
					if (battlePassRewardResponse != null && battlePassRewardResponse.Code == 0)
					{
						Action<bool, BattlePassRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, battlePassRewardResponse);
						return;
					}
					else
					{
						Action<bool, BattlePassRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, battlePassRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void BattlePassBuyScoreRequest(uint addscore, Action<bool, BattlePassChangeScoreResponse> callback)
			{
				BattlePassChangeScoreRequest battlePassChangeScoreRequest = new BattlePassChangeScoreRequest();
				battlePassChangeScoreRequest.CommonParams = NetworkUtils.GetCommonParams();
				battlePassChangeScoreRequest.AddScore = addscore;
				GameApp.NetWork.Send(battlePassChangeScoreRequest, delegate(IMessage response)
				{
					BattlePassChangeScoreResponse battlePassChangeScoreResponse = response as BattlePassChangeScoreResponse;
					if (battlePassChangeScoreResponse != null && battlePassChangeScoreResponse.Code == 0)
					{
						EventArgsRefreshIAPBattlePassData instance = Singleton<EventArgsRefreshIAPBattlePassData>.Instance;
						instance.SetData(battlePassChangeScoreResponse.BattlePassDto);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPBattlePassData, instance);
						Action<bool, BattlePassChangeScoreResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, battlePassChangeScoreResponse);
						return;
					}
					else
					{
						Action<bool, BattlePassChangeScoreResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, battlePassChangeScoreResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void BattlePassFinalRewardRequest(Action<bool, BattlePassFinalRewardResponse> callback)
			{
				BattlePassFinalRewardRequest battlePassFinalRewardRequest = new BattlePassFinalRewardRequest();
				battlePassFinalRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(battlePassFinalRewardRequest, delegate(IMessage response)
				{
					BattlePassFinalRewardResponse battlePassFinalRewardResponse = response as BattlePassFinalRewardResponse;
					if (battlePassFinalRewardResponse != null && battlePassFinalRewardResponse.Code == 0)
					{
						EventArgsRefreshIAPBattlePassData instance = Singleton<EventArgsRefreshIAPBattlePassData>.Instance;
						instance.SetData(battlePassFinalRewardResponse.BattlePassDto);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPBattlePassData, instance);
						Action<bool, BattlePassFinalRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, battlePassFinalRewardResponse);
						return;
					}
					else
					{
						Action<bool, BattlePassFinalRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, battlePassFinalRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void LevelFundRewardRequest(List<int> ids, Action<bool, LevelFundRewardResponse> callback)
			{
				LevelFundRewardRequest levelFundRewardRequest = new LevelFundRewardRequest();
				levelFundRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				levelFundRewardRequest.LevelFundRewardId.AddRange(ids);
				GameApp.NetWork.Send(levelFundRewardRequest, delegate(IMessage response)
				{
					LevelFundRewardResponse levelFundRewardResponse = response as LevelFundRewardResponse;
					if (levelFundRewardResponse != null && levelFundRewardResponse.Code == 0)
					{
						EventArgsRefreshIAPLevelFundRewards instance = Singleton<EventArgsRefreshIAPLevelFundRewards>.Instance;
						instance.SetData(levelFundRewardResponse.LevelFundReward, levelFundRewardResponse.FreeLevelFundReward);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPLevelFundRewards, instance);
						Action<bool, LevelFundRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, levelFundRewardResponse);
						return;
					}
					else
					{
						Action<bool, LevelFundRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, levelFundRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void ShopGetInfoRequest(Action<bool, ShopGetInfoResponse> callback)
			{
				ShopGetInfoRequest shopGetInfoRequest = new ShopGetInfoRequest();
				shopGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(shopGetInfoRequest, delegate(IMessage response)
				{
					ShopGetInfoResponse shopGetInfoResponse = response as ShopGetInfoResponse;
					if (shopGetInfoResponse != null && shopGetInfoResponse.Code == 0)
					{
						Action<bool, ShopGetInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, shopGetInfoResponse);
						return;
					}
					else
					{
						Action<bool, ShopGetInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, shopGetInfoResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void SendMonthCardGetRewardRequest(int tableID, Action<bool, MonthCardGetRewardResponse> callback)
			{
				MonthCardGetRewardRequest monthCardGetRewardRequest = new MonthCardGetRewardRequest();
				monthCardGetRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				monthCardGetRewardRequest.MonthCardId = (uint)tableID;
				GameApp.NetWork.Send(monthCardGetRewardRequest, delegate(IMessage response)
				{
					MonthCardGetRewardResponse monthCardGetRewardResponse = response as MonthCardGetRewardResponse;
					if (monthCardGetRewardResponse != null && monthCardGetRewardResponse.Code == 0)
					{
						EventArgsRefreshIAPInfoData instance = Singleton<EventArgsRefreshIAPInfoData>.Instance;
						instance.SetData(monthCardGetRewardResponse.IapInfo);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, instance);
						Action<bool, MonthCardGetRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, monthCardGetRewardResponse);
						return;
					}
					else
					{
						Action<bool, MonthCardGetRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, monthCardGetRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void SendBuyShopFreeIAPItemRequest(int purchaseID, int extraType, string extraInfo, Action<bool, ShopFreeIAPItemResponse> callback)
			{
				ShopFreeIAPItemRequest shopFreeIAPItemRequest = new ShopFreeIAPItemRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					ConfigId = (uint)purchaseID,
					ExtraType = (uint)extraType,
					ExtraInfo = extraInfo
				};
				GameApp.NetWork.Send(shopFreeIAPItemRequest, delegate(IMessage response)
				{
					ShopFreeIAPItemResponse shopFreeIAPItemResponse = response as ShopFreeIAPItemResponse;
					if (shopFreeIAPItemResponse != null && shopFreeIAPItemResponse.Code == 0)
					{
						EventArgsRefreshIAPCommonData instance = Singleton<EventArgsRefreshIAPCommonData>.Instance;
						instance.SetData(shopFreeIAPItemResponse.RechargeIds, false);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshCommonData, instance);
						EventArgsRefreshIAPInfoData instance2 = Singleton<EventArgsRefreshIAPInfoData>.Instance;
						instance2.SetData(shopFreeIAPItemResponse.IapInfo);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, instance2);
						Action<bool, ShopFreeIAPItemResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, shopFreeIAPItemResponse);
						return;
					}
					else
					{
						Action<bool, ShopFreeIAPItemResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, shopFreeIAPItemResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void SendFirstRechargeRewardRequest(Action<bool, FirstRechargeRewardResponse> callback)
			{
				FirstRechargeRewardRequest firstRechargeRewardRequest = new FirstRechargeRewardRequest
				{
					CommonParams = NetworkUtils.GetCommonParams()
				};
				GameApp.NetWork.Send(firstRechargeRewardRequest, delegate(IMessage response)
				{
					FirstRechargeRewardResponse firstRechargeRewardResponse = response as FirstRechargeRewardResponse;
					if (firstRechargeRewardResponse != null && firstRechargeRewardResponse.Code == 0)
					{
						EventArgsRefreshIAPFirstGiftData instance = Singleton<EventArgsRefreshIAPFirstGiftData>.Instance;
						instance.SetData(firstRechargeRewardResponse.FirstRechargeReward, firstRechargeRewardResponse.TotalRecharge);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPFirstGiftData, instance);
						Action<bool, FirstRechargeRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, firstRechargeRewardResponse);
						return;
					}
					else
					{
						Action<bool, FirstRechargeRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, firstRechargeRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void SendVIPLevelRewardRequest(int viplevel, Action<bool, VIPLevelRewardResponse> callback)
			{
				VIPLevelRewardRequest viplevelRewardRequest = new VIPLevelRewardRequest();
				viplevelRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				viplevelRewardRequest.Level = (uint)viplevel;
				GameApp.NetWork.Send(viplevelRewardRequest, delegate(IMessage response)
				{
					VIPLevelRewardResponse viplevelRewardResponse = response as VIPLevelRewardResponse;
					if (viplevelRewardResponse != null && viplevelRewardResponse.Code == 0)
					{
						Action<bool, VIPLevelRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, viplevelRewardResponse);
						return;
					}
					else
					{
						Action<bool, VIPLevelRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, viplevelRewardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoFirstRechargeRewardV1Request(int tableId, int day, Action<bool, FirstRechargeRewardV1Response> callback)
			{
				FirstRechargeRewardV1Request firstRechargeRewardV1Request = new FirstRechargeRewardV1Request
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					Configid = tableId,
					Products = day
				};
				GameApp.NetWork.Send(firstRechargeRewardV1Request, delegate(IMessage response)
				{
					FirstRechargeRewardV1Response firstRechargeRewardV1Response = response as FirstRechargeRewardV1Response;
					if (firstRechargeRewardV1Response != null && firstRechargeRewardV1Response.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.IAPDataModule).MeetingGift.UpdateRewardSign(firstRechargeRewardV1Response.FirstChargeGiftReward);
						Action<bool, FirstRechargeRewardV1Response> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, firstRechargeRewardV1Response);
						return;
					}
					else
					{
						Action<bool, FirstRechargeRewardV1Response> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, firstRechargeRewardV1Response);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class MiniPurchase
		{
			public static void SendPayInAppPurchaseRequest(long preOrderID, int channelId, Action<bool, PayInAppPurchaseResponse> callback, bool ordercheck = false)
			{
				NetworkUtils.MiniPurchase.SendPayInAppPurchaseRequestInternal(preOrderID, channelId, 0, callback, ordercheck);
			}

			private static void SendPayInAppPurchaseRequestInternal(long preOrderID, int channelId, int attemptsLeft, Action<bool, PayInAppPurchaseResponse> callback, bool ordercheck = false)
			{
				PayInAppPurchaseRequest payInAppPurchaseRequest = new PayInAppPurchaseRequest();
				payInAppPurchaseRequest.PreOrderId = (ulong)preOrderID;
				payInAppPurchaseRequest.ChannelId = (uint)channelId;
				payInAppPurchaseRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(payInAppPurchaseRequest, delegate(IMessage response)
				{
					PayInAppPurchaseResponse payInAppPurchaseResponse = response as PayInAppPurchaseResponse;
					if (payInAppPurchaseResponse != null && payInAppPurchaseResponse.Code == 0)
					{
						EventArgsRefreshIAPCommonData instance = Singleton<EventArgsRefreshIAPCommonData>.Instance;
						instance.SetData(payInAppPurchaseResponse.RechargeIds, false);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshCommonData, instance);
						EventArgsRefreshIAPInfoData instance2 = Singleton<EventArgsRefreshIAPInfoData>.Instance;
						instance2.SetData(payInAppPurchaseResponse.IapInfo);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, instance2);
						Action<bool, PayInAppPurchaseResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, payInAppPurchaseResponse);
						return;
					}
					else
					{
						if (attemptsLeft > 1)
						{
							NetworkUtils.MiniPurchase.SendPayInAppPurchaseRequestInternal(preOrderID, channelId, attemptsLeft - 1, callback, false);
							return;
						}
						Action<bool, PayInAppPurchaseResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, payInAppPurchaseResponse);
						return;
					}
				}, !ordercheck, ordercheck, string.Empty, !ordercheck);
			}

			public static void SendPayPreOrderRequest(int purchaseId, string productId, int extraType, string extraInfo, long serverTime, Action<bool, PayPreOrderResponse> callback)
			{
				PayPreOrderRequest payPreOrderRequest = new PayPreOrderRequest();
				payPreOrderRequest.CommonParams = NetworkUtils.GetCommonParams();
				payPreOrderRequest.ProductId = purchaseId.ToString();
				payPreOrderRequest.ExtraInfo = extraInfo;
				payPreOrderRequest.PreOrderId = (ulong)serverTime;
				payPreOrderRequest.ExtraType = (uint)extraType;
				DeviceSystem system = GameApp.SDK.WebGameAPI.GetSystem();
				if (system - 2 <= 1 || system == 5)
				{
					payPreOrderRequest.ChannelId = 200U;
				}
				GameApp.NetWork.Send(payPreOrderRequest, delegate(IMessage response)
				{
					PayPreOrderResponse payPreOrderResponse = response as PayPreOrderResponse;
					if (payPreOrderResponse != null && payPreOrderResponse.Code == 0)
					{
						Action<bool, PayPreOrderResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, payPreOrderResponse);
						return;
					}
					else
					{
						if (payPreOrderResponse != null)
						{
							int code = payPreOrderResponse.Code;
						}
						Action<bool, PayPreOrderResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, payPreOrderResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Relic
		{
			public static void DoRelicActiveRequest(int id, Action<bool, RelicActiveResponse> callback = null)
			{
				RelicActiveRequest relicActiveRequest = new RelicActiveRequest();
				relicActiveRequest.CommonParams = NetworkUtils.GetCommonParams();
				relicActiveRequest.RelicId = id;
				GameApp.NetWork.Send(relicActiveRequest, delegate(IMessage response)
				{
					RelicActiveResponse relicActiveResponse = response as RelicActiveResponse;
					if (response != null && relicActiveResponse.Code == 0)
					{
						RedPointController.Instance.ReCalc("Equip.Relic", true);
						Action<bool, RelicActiveResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, relicActiveResponse);
						return;
					}
					else
					{
						Action<bool, RelicActiveResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, relicActiveResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoRelicStrengthRequest(int id, Action<bool, RelicStrengthResponse> callback = null)
			{
				RelicStrengthRequest relicStrengthRequest = new RelicStrengthRequest();
				relicStrengthRequest.CommonParams = NetworkUtils.GetCommonParams();
				relicStrengthRequest.RelicId = id;
				GameApp.NetWork.Send(relicStrengthRequest, delegate(IMessage response)
				{
					RelicStrengthResponse relicStrengthResponse = response as RelicStrengthResponse;
					if (response != null && relicStrengthResponse.Code == 0)
					{
						RedPointController.Instance.ReCalc("Equip.Relic", true);
						Action<bool, RelicStrengthResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, relicStrengthResponse);
						return;
					}
					else
					{
						Action<bool, RelicStrengthResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, relicStrengthResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoRelicStarRequest(int id, Action<bool, RelicStarResponse> callback = null)
			{
				RelicStarRequest relicStarRequest = new RelicStarRequest();
				relicStarRequest.CommonParams = NetworkUtils.GetCommonParams();
				relicStarRequest.RelicId = id;
				GameApp.NetWork.Send(relicStarRequest, delegate(IMessage response)
				{
					RelicStarResponse relicStarResponse = response as RelicStarResponse;
					if (response != null && relicStarResponse.Code == 0)
					{
						RedPointController.Instance.ReCalc("Equip.Relic", true);
						Action<bool, RelicStarResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, relicStarResponse);
						return;
					}
					else
					{
						Action<bool, RelicStarResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, relicStarResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class RogueDungeon
		{
			public static void DoGetPanelInfoRequest(Action<bool, HellGetPanelInfoResponse> callback)
			{
				HellGetPanelInfoRequest hellGetPanelInfoRequest = new HellGetPanelInfoRequest();
				hellGetPanelInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(hellGetPanelInfoRequest, delegate(IMessage response)
				{
					HellGetPanelInfoResponse hellGetPanelInfoResponse = response as HellGetPanelInfoResponse;
					if (hellGetPanelInfoResponse == null || hellGetPanelInfoResponse.Code != 0)
					{
						Action<bool, HellGetPanelInfoResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, hellGetPanelInfoResponse);
						}
						return;
					}
					GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).UpdateBaseInfo(hellGetPanelInfoResponse);
					Action<bool, HellGetPanelInfoResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, hellGetPanelInfoResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoEnterChallengeRequest(Action<bool, HellEnterBattleResponse> callback)
			{
				HellEnterBattleRequest hellEnterBattleRequest = new HellEnterBattleRequest();
				hellEnterBattleRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(hellEnterBattleRequest, delegate(IMessage response)
				{
					HellEnterBattleResponse hellEnterBattleResponse = response as HellEnterBattleResponse;
					if (hellEnterBattleResponse == null || hellEnterBattleResponse.Code != 0)
					{
						Action<bool, HellEnterBattleResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, hellEnterBattleResponse);
						}
						return;
					}
					GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).EnterChallenge(hellEnterBattleResponse);
					Action<bool, HellEnterBattleResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, hellEnterBattleResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoHellEnterSelectSkillRequest(List<int> skills, Action<bool, HellEnterSelectSkillResponse> callback)
			{
				HellEnterSelectSkillRequest hellEnterSelectSkillRequest = new HellEnterSelectSkillRequest();
				hellEnterSelectSkillRequest.CommonParams = NetworkUtils.GetCommonParams();
				for (int i = 0; i < skills.Count; i++)
				{
					hellEnterSelectSkillRequest.SkillList.Add(skills[i]);
				}
				GameApp.NetWork.Send(hellEnterSelectSkillRequest, delegate(IMessage response)
				{
					HellEnterSelectSkillResponse hellEnterSelectSkillResponse = response as HellEnterSelectSkillResponse;
					if (hellEnterSelectSkillResponse == null || hellEnterSelectSkillResponse.Code != 0)
					{
						Action<bool, HellEnterSelectSkillResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, hellEnterSelectSkillResponse);
						}
						return;
					}
					GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).SetRoundEnterSkillSelected();
					Action<bool, HellEnterSelectSkillResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, hellEnterSelectSkillResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoBattleRequest(uint floorId, Action<bool, HellDoChallengeResponse> callback)
			{
				HellDoChallengeRequest hellDoChallengeRequest = new HellDoChallengeRequest();
				hellDoChallengeRequest.CommonParams = NetworkUtils.GetCommonParams();
				hellDoChallengeRequest.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
				hellDoChallengeRequest.StageId = (int)floorId;
				GameApp.NetWork.Send(hellDoChallengeRequest, delegate(IMessage response)
				{
					HellDoChallengeResponse hellDoChallengeResponse = response as HellDoChallengeResponse;
					if (hellDoChallengeResponse == null || hellDoChallengeResponse.Code != 0)
					{
						Action<bool, HellDoChallengeResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, hellDoChallengeResponse);
						}
						return;
					}
					if (hellDoChallengeResponse.Result >= 0)
					{
						GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).UpdateBattleInfo(hellDoChallengeResponse);
					}
					Action<bool, HellDoChallengeResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, hellDoChallengeResponse);
				}, false, false, string.Empty, true);
			}

			public static void DoHellSaveSkillRequest(long currentHp, List<int> skills, Dictionary<string, int> attDic, Action<bool, HellSaveSkillResponse> callback)
			{
				HellSaveSkillRequest hellSaveSkillRequest = new HellSaveSkillRequest();
				hellSaveSkillRequest.CommonParams = NetworkUtils.GetCommonParams();
				hellSaveSkillRequest.Hp = currentHp;
				foreach (int num in skills)
				{
					hellSaveSkillRequest.SkillList.Add(num);
				}
				foreach (string text in attDic.Keys)
				{
					hellSaveSkillRequest.AttrMap.Add(text, attDic[text]);
				}
				GameApp.NetWork.Send(hellSaveSkillRequest, delegate(IMessage response)
				{
					HellSaveSkillResponse hellSaveSkillResponse = response as HellSaveSkillResponse;
					if (hellSaveSkillResponse == null || hellSaveSkillResponse.Code != 0)
					{
						Action<bool, HellSaveSkillResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, hellSaveSkillResponse);
						}
						return;
					}
					RogueDungeonDataModule dataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
					dataModule.SetRoundEnter();
					dataModule.UpdateAttribute(hellSaveSkillResponse.AttrMap);
					Action<bool, HellSaveSkillResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, hellSaveSkillResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoHellExitBattleRequest(Action<bool, HellExitBattleResponse> callback)
			{
				HellExitBattleRequest hellExitBattleRequest = new HellExitBattleRequest();
				hellExitBattleRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(hellExitBattleRequest, delegate(IMessage response)
				{
					HellExitBattleResponse hellExitBattleResponse = response as HellExitBattleResponse;
					if (hellExitBattleResponse == null || hellExitBattleResponse.Code != 0)
					{
						Action<bool, HellExitBattleResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, hellExitBattleResponse);
						}
						return;
					}
					GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).Escape(hellExitBattleResponse.CommonData.Reward);
					Action<bool, HellExitBattleResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, hellExitBattleResponse);
				}, true, false, string.Empty, true);
			}

			public static void DoHellRevertHpRequest(Action<bool, HellRevertHpResponse> callback)
			{
				HellRevertHpRequest hellRevertHpRequest = new HellRevertHpRequest();
				hellRevertHpRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(hellRevertHpRequest, delegate(IMessage response)
				{
					HellRevertHpResponse hellRevertHpResponse = response as HellRevertHpResponse;
					if (hellRevertHpResponse == null || hellRevertHpResponse.Code != 0)
					{
						Action<bool, HellRevertHpResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(false, hellRevertHpResponse);
						}
						return;
					}
					Action<bool, HellRevertHpResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, hellRevertHpResponse);
				}, false, false, string.Empty, true);
			}

			public static void DoHellRankRequest(int page, bool isNextPage, bool isShowMask, Action<int, bool, bool, HellRankResponse> callback)
			{
				HellRankRequest hellRankRequest = new HellRankRequest();
				hellRankRequest.CommonParams = NetworkUtils.GetCommonParams();
				hellRankRequest.Page = page;
				GameApp.NetWork.Send(hellRankRequest, delegate(IMessage response)
				{
					HellRankResponse hellRankResponse = response as HellRankResponse;
					if (hellRankResponse == null || hellRankResponse.Code != 0)
					{
						Action<int, bool, bool, HellRankResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(page, isNextPage, false, hellRankResponse);
						}
						return;
					}
					GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).UpdateRank(page, hellRankResponse);
					Action<int, bool, bool, HellRankResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(page, isNextPage, true, hellRankResponse);
				}, isShowMask, false, string.Empty, true);
			}
		}

		public class SevenDayCarnival
		{
			public static void RequestGetSevenDayInfo(Action<SevenDayTaskGetInfoResponse> onSuccess, Action<int> onError)
			{
				SevenDayTaskGetInfoRequest sevenDayTaskGetInfoRequest = new SevenDayTaskGetInfoRequest();
				sevenDayTaskGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(sevenDayTaskGetInfoRequest, delegate(IMessage response)
				{
					SevenDayTaskGetInfoResponse sevenDayTaskGetInfoResponse = response as SevenDayTaskGetInfoResponse;
					if (sevenDayTaskGetInfoResponse != null && sevenDayTaskGetInfoResponse.Code == 0)
					{
						Action<SevenDayTaskGetInfoResponse> onSuccess2 = onSuccess;
						if (onSuccess2 != null)
						{
							onSuccess2(sevenDayTaskGetInfoResponse);
						}
					}
					else
					{
						Action<int> onError2 = onError;
						if (onError2 != null)
						{
							onError2((sevenDayTaskGetInfoResponse != null) ? sevenDayTaskGetInfoResponse.Code : 0);
						}
					}
					RedPointController.Instance.ReCalc("Main.Carnival", true);
				}, false, false, string.Empty, true);
			}

			public static void RequestGetSevenDayTaskReward(int taskId, Action<SevenDayTaskRewardResponse> onSuccess, Action<int> onError)
			{
				SevenDayTaskRewardRequest sevenDayTaskRewardRequest = new SevenDayTaskRewardRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					TaskId = (uint)taskId
				};
				GameApp.NetWork.Send(sevenDayTaskRewardRequest, delegate(IMessage response)
				{
					SevenDayTaskRewardResponse sevenDayTaskRewardResponse = response as SevenDayTaskRewardResponse;
					if (sevenDayTaskRewardResponse != null && sevenDayTaskRewardResponse.Code == 0)
					{
						Action<SevenDayTaskRewardResponse> onSuccess2 = onSuccess;
						if (onSuccess2 != null)
						{
							onSuccess2(sevenDayTaskRewardResponse);
						}
					}
					else
					{
						Action<int> onError2 = onError;
						if (onError2 != null)
						{
							onError2((sevenDayTaskRewardResponse != null) ? sevenDayTaskRewardResponse.Code : 0);
						}
					}
					RedPointController.Instance.ReCalc("Main.Carnival", true);
				}, true, false, string.Empty, true);
			}

			public static void RequestGetCarnivalActiveReward(int configId, int selectedRewardIdx, Action<SevenDayTaskActiveRewardResponse> onSuccess, Action<int> onError)
			{
				SevenDayTaskActiveRewardRequest sevenDayTaskActiveRewardRequest = new SevenDayTaskActiveRewardRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					ConfigId = (uint)configId
				};
				sevenDayTaskActiveRewardRequest.SelectIdx = (uint)selectedRewardIdx;
				GameApp.NetWork.Send(sevenDayTaskActiveRewardRequest, delegate(IMessage response)
				{
					SevenDayTaskActiveRewardResponse sevenDayTaskActiveRewardResponse = response as SevenDayTaskActiveRewardResponse;
					if (sevenDayTaskActiveRewardResponse != null && sevenDayTaskActiveRewardResponse.Code == 0)
					{
						Action<SevenDayTaskActiveRewardResponse> onSuccess2 = onSuccess;
						if (onSuccess2 != null)
						{
							onSuccess2(sevenDayTaskActiveRewardResponse);
						}
					}
					else
					{
						Action<int> onError2 = onError;
						if (onError2 != null)
						{
							onError2((sevenDayTaskActiveRewardResponse != null) ? sevenDayTaskActiveRewardResponse.Code : 0);
						}
					}
					RedPointController.Instance.ReCalc("Main.Carnival", true);
				}, true, false, string.Empty, true);
			}

			public static void RequestSevenDayFreeReward(int configId, Action<SevenDayFreeRewardResponse> onSuccess, Action<int> onError)
			{
				SevenDayFreeRewardRequest sevenDayFreeRewardRequest = new SevenDayFreeRewardRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					ConfigId = (uint)configId
				};
				GameApp.NetWork.Send(sevenDayFreeRewardRequest, delegate(IMessage response)
				{
					SevenDayFreeRewardResponse sevenDayFreeRewardResponse = response as SevenDayFreeRewardResponse;
					if (sevenDayFreeRewardResponse != null && sevenDayFreeRewardResponse.Code == 0)
					{
						Action<SevenDayFreeRewardResponse> onSuccess2 = onSuccess;
						if (onSuccess2 != null)
						{
							onSuccess2(sevenDayFreeRewardResponse);
						}
					}
					else
					{
						Action<int> onError2 = onError;
						if (onError2 != null)
						{
							onError2((sevenDayFreeRewardResponse != null) ? sevenDayFreeRewardResponse.Code : 0);
						}
					}
					RedPointController.Instance.ReCalc("Main.Carnival", true);
				}, true, false, string.Empty, true);
			}
		}

		public class Shop
		{
			public static void IntegralShopGetInfoRequest(ShopType shopType, Action<bool, IntegralShopGetInfoResponse> callback)
			{
				IntegralShopGetInfoRequest integralShopGetInfoRequest = new IntegralShopGetInfoRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					ShopConfigId = (int)shopType
				};
				GameApp.NetWork.Send(integralShopGetInfoRequest, delegate(IMessage response)
				{
					IntegralShopGetInfoResponse integralShopGetInfoResponse = response as IntegralShopGetInfoResponse;
					bool flag = integralShopGetInfoResponse != null && integralShopGetInfoResponse.Code == 0;
					if (flag)
					{
						if (integralShopGetInfoResponse.Shop != null)
						{
							EventArgsRefreshShopData instance = Singleton<EventArgsRefreshShopData>.Instance;
							instance.ShopType = (ShopType)integralShopGetInfoResponse.Shop.ShopConfigId;
							instance.ShopInfo = integralShopGetInfoResponse.Shop;
							GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, instance);
						}
						RedPointController.Instance.ReCalc("MainShop", true);
					}
					Action<bool, IntegralShopGetInfoResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(flag, integralShopGetInfoResponse);
				}, true, false, string.Empty, true);
			}

			public static void IntegralShopRefreshRequest(ShopType shopType, int refreshCurrencyID, int refreshPrice, Action<bool, IntegralShopRefreshResponse> callback)
			{
				IntegralShopRefreshRequest integralShopRefreshRequest = new IntegralShopRefreshRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					ShopConfigId = (int)shopType
				};
				GameApp.NetWork.Send(integralShopRefreshRequest, delegate(IMessage response)
				{
					IntegralShopRefreshResponse integralShopRefreshResponse = response as IntegralShopRefreshResponse;
					bool flag = integralShopRefreshResponse != null && integralShopRefreshResponse.Code == 0;
					if (flag)
					{
						EventArgsRefreshShopData instance = Singleton<EventArgsRefreshShopData>.Instance;
						instance.ShopType = (ShopType)integralShopRefreshResponse.Shop.ShopConfigId;
						instance.ShopInfo = integralShopRefreshResponse.Shop;
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, instance);
						RedPointController.Instance.ReCalc("MainShop", true);
						RedPointController.Instance.ReCalc("Main.BlackMarket", true);
					}
					Action<bool, IntegralShopRefreshResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(flag, integralShopRefreshResponse);
				}, true, false, string.Empty, true);
			}

			public static void IntegralShopBuyRequest(ShopType shopType, int configId, int buyCurrencyID, int buyPrice, Action<bool, IntegralShopBuyResponse> callback)
			{
				IntegralShopBuyRequest integralShopBuyRequest = new IntegralShopBuyRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					GoodsConfigId = configId,
					ShopConfigId = (int)shopType
				};
				GameApp.NetWork.Send(integralShopBuyRequest, delegate(IMessage response)
				{
					IntegralShopBuyResponse integralShopBuyResponse = response as IntegralShopBuyResponse;
					if (integralShopBuyResponse != null && integralShopBuyResponse.Code == 0)
					{
						EventArgsBuyShopItem instance = Singleton<EventArgsBuyShopItem>.Instance;
						instance.ShopItemId = integralShopBuyResponse.GoodsConfigId;
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_BuyShopItem, instance);
						RedPointController.Instance.ReCalc("MainShop", true);
						RedPointController.Instance.ReCalc("Equip", true);
						RedPointController.Instance.ReCalc("Main.BlackMarket", true);
					}
					Action<bool, IntegralShopBuyResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(integralShopBuyResponse != null && integralShopBuyResponse.Code == 0, integralShopBuyResponse);
				}, true, false, string.Empty, true);
			}

			public static void ShopBuyItemRequest(int configId, int costType, Action<bool, ShopBuyItemResponse> callback)
			{
				ShopBuyItemRequest shopBuyItemRequest = new ShopBuyItemRequest();
				shopBuyItemRequest.CommonParams = NetworkUtils.GetCommonParams();
				shopBuyItemRequest.ConfigId = (uint)configId;
				shopBuyItemRequest.BuyType = (uint)costType;
				GameApp.NetWork.Send(shopBuyItemRequest, delegate(IMessage resp)
				{
					ShopBuyItemResponse shopBuyItemResponse = resp as ShopBuyItemResponse;
					if (shopBuyItemResponse != null && shopBuyItemResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(shopBuyItemResponse.AdData);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
						RedPointController.Instance.ReCalc("MainShop", true);
						RedPointController.Instance.ReCalc("Equip", true);
						RedPointController.Instance.ReCalc("Main.BlackMarket", true);
						DxxTools.UI.OpenRewardCommon(shopBuyItemResponse.CommonData.Reward, null, true);
					}
					Action<bool, ShopBuyItemResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(shopBuyItemResponse != null && shopBuyItemResponse.Code == 0, shopBuyItemResponse);
				}, true, false, string.Empty, true);
			}

			public static void ShopDoDrawRequest(int summonId, int costType, int drawType, Action<bool, ShopDoDrawResponse> callback)
			{
				ShopDoDrawRequest shopDoDrawRequest = new ShopDoDrawRequest();
				shopDoDrawRequest.CommonParams = NetworkUtils.GetCommonParams();
				shopDoDrawRequest.SummonId = (uint)summonId;
				shopDoDrawRequest.CostType = (uint)costType;
				shopDoDrawRequest.DrawType = (uint)drawType;
				GameApp.NetWork.Send(shopDoDrawRequest, delegate(IMessage response)
				{
					ShopDoDrawResponse shopDoDrawResponse = response as ShopDoDrawResponse;
					if (shopDoDrawResponse != null && shopDoDrawResponse.Code.Equals(0))
					{
						IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
						dataModule.UpdateShopDrawCount(shopDoDrawResponse.ShopDrawDto);
						dataModule.UpdateShopSUpPoolDrawCount(shopDoDrawResponse.ShopSupCount);
						GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(shopDoDrawResponse.AdData);
						RedPointController.Instance.ReCalc("MainShop", true);
						Action<bool, ShopDoDrawResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, shopDoDrawResponse);
						return;
					}
					else
					{
						Action<bool, ShopDoDrawResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, null);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void FinishAdvertRequest(int adId, Action<bool, FinishAdvertResponse> callback)
			{
				FinishAdvertRequest finishAdvertRequest = new FinishAdvertRequest();
				finishAdvertRequest.CommonParams = NetworkUtils.GetCommonParams();
				finishAdvertRequest.AdId = adId;
				GameApp.NetWork.Send(finishAdvertRequest, delegate(IMessage response)
				{
					FinishAdvertResponse finishAdvertResponse = response as FinishAdvertResponse;
					if (finishAdvertResponse != null && finishAdvertResponse.Code.Equals(0))
					{
						GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(finishAdvertResponse.AdData);
						Action<bool, FinishAdvertResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, finishAdvertResponse);
						return;
					}
					else
					{
						Action<bool, FinishAdvertResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, null);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoHeadFrameActiveRequest(Action<bool, HeadFrameActiveResponse> callback)
			{
				HeadFrameActiveRequest headFrameActiveRequest = new HeadFrameActiveRequest();
				headFrameActiveRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(headFrameActiveRequest, delegate(IMessage response)
				{
					HeadFrameActiveResponse headFrameActiveResponse = response as HeadFrameActiveResponse;
					if (headFrameActiveResponse != null && headFrameActiveResponse.Code.Equals(0))
					{
						GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.SetHeadState(headFrameActiveResponse.IsHeadFrameActive);
						Action<bool, HeadFrameActiveResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, headFrameActiveResponse);
						return;
					}
					else
					{
						Action<bool, HeadFrameActiveResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, null);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoBuyItemRewardRequest(int configId, int equipId, Action<bool, BuyItemRewardResponse> callback)
			{
				BuyItemRewardRequest buyItemRewardRequest = new BuyItemRewardRequest();
				buyItemRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				buyItemRewardRequest.ConfigId = configId;
				buyItemRewardRequest.EquipId = equipId;
				GameApp.NetWork.Send(buyItemRewardRequest, delegate(IMessage response)
				{
					BuyItemRewardResponse buyItemRewardResponse = response as BuyItemRewardResponse;
					if (buyItemRewardResponse != null && buyItemRewardResponse.Code.Equals(0))
					{
						GameApp.Data.GetDataModule(DataName.IAPDataModule).UpdateShopSUpPoolDrawCount(buyItemRewardResponse.ShopSupCount);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
						RedPointController.Instance.ReCalc("MainShop", true);
						DxxTools.UI.OpenRewardCommon(buyItemRewardResponse.CommonData.Reward, null, true);
						Action<bool, BuyItemRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, buyItemRewardResponse);
						return;
					}
					else
					{
						Action<bool, BuyItemRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, null);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class SiGnIn
		{
			public static void SignInGetInfoRequest(Action<bool, SignInGetInfoResponse> callback)
			{
				SignInGetInfoRequest signInGetInfoRequest = new SignInGetInfoRequest();
				signInGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(signInGetInfoRequest, delegate(IMessage response)
				{
					SignInGetInfoResponse signInGetInfoResponse = response as SignInGetInfoResponse;
					if (signInGetInfoResponse != null && signInGetInfoResponse.Code == 0)
					{
						Action<bool, SignInGetInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, signInGetInfoResponse);
						return;
					}
					else
					{
						Action<bool, SignInGetInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, signInGetInfoResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void SignInDoSignRequest(Action<bool, SignInDoSignResponse> callback)
			{
				SignInDoSignRequest signInDoSignRequest = new SignInDoSignRequest();
				signInDoSignRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(signInDoSignRequest, delegate(IMessage response)
				{
					SignInDoSignResponse signInDoSignResponse = response as SignInDoSignResponse;
					if (signInDoSignResponse != null && signInDoSignResponse.Code == 0)
					{
						Action<bool, SignInDoSignResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, signInDoSignResponse);
						return;
					}
					else
					{
						Action<bool, SignInDoSignResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, signInDoSignResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Sociality
		{
			public static void DoSocialPowerRankRequest(int page, Action<bool, SocialPowerRankResponse> callback)
			{
				SocialPowerRankRequest socialPowerRankRequest = new SocialPowerRankRequest();
				socialPowerRankRequest.CommonParams = NetworkUtils.GetCommonParams();
				socialPowerRankRequest.Page = page;
				GameApp.NetWork.Send(socialPowerRankRequest, delegate(IMessage response)
				{
					SocialPowerRankResponse socialPowerRankResponse = response as SocialPowerRankResponse;
					if (socialPowerRankResponse != null && socialPowerRankResponse.Code == 0)
					{
						Action<bool, SocialPowerRankResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, socialPowerRankResponse);
						return;
					}
					else
					{
						Action<bool, SocialPowerRankResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, socialPowerRankResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void DoInteractListRequest(Action<bool, InteractListResponse> callback)
			{
				InteractListRequest interactListRequest = new InteractListRequest();
				interactListRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(interactListRequest, delegate(IMessage response)
				{
					InteractListResponse interactListResponse = response as InteractListResponse;
					if (interactListResponse != null && interactListResponse.Code == 0)
					{
						Action<bool, InteractListResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, interactListResponse);
						}
						RedPointController.Instance.ReCalc("Main.Sociality", true);
						return;
					}
					Action<bool, InteractListResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, interactListResponse);
				}, false, false, string.Empty, true);
			}

			public static void DoInteractDetailRequest(long rowID, Action<bool, InteractDetailResponse> callback)
			{
				InteractDetailRequest interactDetailRequest = new InteractDetailRequest();
				interactDetailRequest.CommonParams = NetworkUtils.GetCommonParams();
				interactDetailRequest.RowId = rowID;
				GameApp.NetWork.Send(interactDetailRequest, delegate(IMessage response)
				{
					InteractDetailResponse interactDetailResponse = response as InteractDetailResponse;
					if (interactDetailResponse != null && interactDetailResponse.Code == 0)
					{
						Action<bool, InteractDetailResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, interactDetailResponse);
						}
						RedPointController.Instance.ReCalc("Main.Sociality", true);
						return;
					}
					Action<bool, InteractDetailResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, interactDetailResponse);
				}, false, false, string.Empty, true);
			}
		}

		public class Talent
		{
			public static void DoAttributeUpgradeRequest(int step, string attributeKey, Action<bool, TalentsLvUpResponse> callback)
			{
				TalentsLvUpRequest talentsLvUpRequest = new TalentsLvUpRequest();
				talentsLvUpRequest.CommonParams = NetworkUtils.GetCommonParams();
				talentsLvUpRequest.AttributeType = attributeKey;
				talentsLvUpRequest.Step = (uint)step;
				GameApp.NetWork.Send(talentsLvUpRequest, delegate(IMessage response)
				{
					TalentsLvUpResponse talentsLvUpResponse = response as TalentsLvUpResponse;
					if (talentsLvUpResponse == null)
					{
						return;
					}
					if (talentsLvUpResponse.Code == 0)
					{
						if (talentsLvUpResponse.TalentsInfo != null)
						{
							TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
							if ((ulong)talentsLvUpResponse.TalentsInfo.ExpProcess > (ulong)((long)dataModule.TalentExp))
							{
								dataModule.RefreshTalentData(talentsLvUpResponse.TalentsInfo);
								RedPointController.Instance.ReCalc("Talent", true);
								RedPointController.Instance.ReCalc("Main.NewWorld", true);
								GameApp.Event.Dispatch(null, LocalMessageName.CC_UITalent_RefreshData, null);
							}
							Action<bool, TalentsLvUpResponse> callback2 = callback;
							if (callback2 != null)
							{
								callback2(true, talentsLvUpResponse);
							}
							GameApp.SDK.Analyze.Track_TalentUp(attributeKey, talentsLvUpResponse.CommonData.CostDto);
							return;
						}
						Action<bool, TalentsLvUpResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, talentsLvUpResponse);
						return;
					}
					else
					{
						Action<bool, TalentsLvUpResponse> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, talentsLvUpResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}
		}

		public class TalentLegacy
		{
			public static void DoTalentLegacyRankRequest(Action<bool, TalentLegacyLeaderBoardResponse> callBack)
			{
				TalentLegacyLeaderBoardRequest talentLegacyLeaderBoardRequest = new TalentLegacyLeaderBoardRequest();
				talentLegacyLeaderBoardRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(talentLegacyLeaderBoardRequest, delegate(IMessage response)
				{
					TalentLegacyLeaderBoardResponse talentLegacyLeaderBoardResponse = response as TalentLegacyLeaderBoardResponse;
					if (talentLegacyLeaderBoardResponse != null && talentLegacyLeaderBoardResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnSetRankInfo(talentLegacyLeaderBoardResponse.UserList);
						Action<bool, TalentLegacyLeaderBoardResponse> callBack2 = callBack;
						if (callBack2 == null)
						{
							return;
						}
						callBack2(true, talentLegacyLeaderBoardResponse);
						return;
					}
					else
					{
						Action<bool, TalentLegacyLeaderBoardResponse> callBack3 = callBack;
						if (callBack3 == null)
						{
							return;
						}
						callBack3(false, talentLegacyLeaderBoardResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoTalentLegacyInfoRequest(Action<bool, TalentLegacyInfoResponse> callBack, bool isShowMask = true)
			{
				TalentLegacyInfoRequest talentLegacyInfoRequest = new TalentLegacyInfoRequest();
				talentLegacyInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(talentLegacyInfoRequest, delegate(IMessage response)
				{
					TalentLegacyInfoResponse talentLegacyInfoResponse = response as TalentLegacyInfoResponse;
					if (talentLegacyInfoResponse != null && talentLegacyInfoResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnSetTalentLegacyInfo(talentLegacyInfoResponse);
						RedPointController.Instance.ReCalc("Talent.TalentLegacy", true);
						RedPointController.Instance.ReCalc("Talent.TalentLegacy.TalentLegacyNode", true);
						RedPointController.Instance.ReCalc("Equip.TalentLegacySkill", true);
						Action<bool, TalentLegacyInfoResponse> callBack2 = callBack;
						if (callBack2 == null)
						{
							return;
						}
						callBack2(true, talentLegacyInfoResponse);
						return;
					}
					else
					{
						Action<bool, TalentLegacyInfoResponse> callBack3 = callBack;
						if (callBack3 == null)
						{
							return;
						}
						callBack3(false, talentLegacyInfoResponse);
						return;
					}
				}, isShowMask, false, string.Empty, true);
			}

			public static void DoTalentLegacyLevelUpRequest(int careerId, int talentLegacyId, Action<bool, TalentLegacyLevelUpResponse> callBack)
			{
				TalentLegacyLevelUpRequest talentLegacyLevelUpRequest = new TalentLegacyLevelUpRequest();
				talentLegacyLevelUpRequest.CommonParams = NetworkUtils.GetCommonParams();
				talentLegacyLevelUpRequest.CareerId = careerId;
				talentLegacyLevelUpRequest.TalentLegacyId = talentLegacyId;
				GameApp.NetWork.Send(talentLegacyLevelUpRequest, delegate(IMessage response)
				{
					TalentLegacyLevelUpResponse talentLegacyLevelUpResponse = response as TalentLegacyLevelUpResponse;
					if (talentLegacyLevelUpResponse != null && talentLegacyLevelUpResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnTalentLegacyLevelUpBack(talentLegacyLevelUpResponse);
						PlayerPrefsKeys.SetTalentLegacyNode(HLog.StringBuilder(careerId.ToString(), "_", talentLegacyId.ToString()));
						RedPointController.Instance.ReCalc("Talent.TalentLegacy", true);
						RedPointController.Instance.ReCalc("Talent.TalentLegacy.TalentLegacyNode", true);
						GameApp.Event.DispatchNow(null, 465, null);
						GameApp.SDK.Analyze.Track_TalentLegacyStudy(talentLegacyId, talentLegacyLevelUpResponse.CommonData.CostDto);
						Action<bool, TalentLegacyLevelUpResponse> callBack2 = callBack;
						if (callBack2 == null)
						{
							return;
						}
						callBack2(true, talentLegacyLevelUpResponse);
						return;
					}
					else
					{
						Action<bool, TalentLegacyLevelUpResponse> callBack3 = callBack;
						if (callBack3 == null)
						{
							return;
						}
						callBack3(false, talentLegacyLevelUpResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoTalentLegacySkillSwitchRequest(int careerId, int talentLegacyId, int slotIndex, Action<bool, TalentLegacySwitchResponse> callBack)
			{
				TalentLegacySwitchRequest talentLegacySwitchRequest = new TalentLegacySwitchRequest();
				talentLegacySwitchRequest.CommonParams = NetworkUtils.GetCommonParams();
				talentLegacySwitchRequest.FromCareerId = careerId;
				talentLegacySwitchRequest.ToTalentLegacyId = talentLegacyId;
				talentLegacySwitchRequest.Index = slotIndex;
				GameApp.NetWork.Send(talentLegacySwitchRequest, delegate(IMessage response)
				{
					TalentLegacySwitchResponse talentLegacySwitchResponse = response as TalentLegacySwitchResponse;
					if (talentLegacySwitchResponse != null && talentLegacySwitchResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnTalentLegacySkillSwitchBack(talentLegacySwitchResponse);
						RedPointController.Instance.ReCalc("Talent.TalentLegacy", true);
						RedPointController.Instance.ReCalc("Equip.TalentLegacySkill", true);
						GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).MathAddAttributeData();
						GameApp.Event.DispatchNow(null, 145, null);
						GameApp.Event.DispatchNow(null, 469, null);
						Action<bool, TalentLegacySwitchResponse> callBack2 = callBack;
						if (callBack2 == null)
						{
							return;
						}
						callBack2(true, talentLegacySwitchResponse);
						return;
					}
					else
					{
						Action<bool, TalentLegacySwitchResponse> callBack3 = callBack;
						if (callBack3 == null)
						{
							return;
						}
						callBack3(false, talentLegacySwitchResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoTalentLegacyLevelUpCoolDownRequest(int careerId, int talentLegacyId, int useType, int useNum, Action<bool, TalentLegacyLevelUpCoolDownResponse> callBack)
			{
				TalentLegacyLevelUpCoolDownRequest talentLegacyLevelUpCoolDownRequest = new TalentLegacyLevelUpCoolDownRequest();
				talentLegacyLevelUpCoolDownRequest.CommonParams = NetworkUtils.GetCommonParams();
				talentLegacyLevelUpCoolDownRequest.CareerId = careerId;
				talentLegacyLevelUpCoolDownRequest.TalentLegacyId = talentLegacyId;
				talentLegacyLevelUpCoolDownRequest.UseType = useType;
				talentLegacyLevelUpCoolDownRequest.UseNum = useNum;
				GameApp.NetWork.Send(talentLegacyLevelUpCoolDownRequest, delegate(IMessage response)
				{
					TalentLegacyLevelUpCoolDownResponse talentLegacyLevelUpCoolDownResponse = response as TalentLegacyLevelUpCoolDownResponse;
					if (talentLegacyLevelUpCoolDownResponse != null && talentLegacyLevelUpCoolDownResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(talentLegacyLevelUpCoolDownResponse.AdData);
						TalentLegacyDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
						bool flag = false;
						if (dataModule.OnGetTalentLegacySkillInfo(careerId, talentLegacyId).LevelUpTime > 0L)
						{
							flag = true;
						}
						dataModule.OnTalentLegacyLevelUpCoolDownBack(talentLegacyLevelUpCoolDownResponse);
						if (dataModule.OnGetTalentLegacySkillInfo(careerId, talentLegacyId).LevelUpTime <= 0L && flag)
						{
							GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).MathAddAttributeData();
							GameApp.Event.DispatchNow(null, 145, null);
							dataModule.OpenStudyFinishPanel(careerId, talentLegacyId);
							EventArgsNodeTimeEnd eventArgsNodeTimeEnd = new EventArgsNodeTimeEnd(careerId, talentLegacyId);
							GameApp.Event.DispatchNow(null, LocalMessageName.CC_TalentLegacyNodeSpeedEnd, eventArgsNodeTimeEnd);
						}
						RedPointController.Instance.ReCalc("Talent.TalentLegacy", true);
						RedPointController.Instance.ReCalc("Talent.TalentLegacy.TalentLegacyNode", true);
						GameApp.Event.DispatchNow(null, 466, null);
						Action<bool, TalentLegacyLevelUpCoolDownResponse> callBack2 = callBack;
						if (callBack2 == null)
						{
							return;
						}
						callBack2(true, talentLegacyLevelUpCoolDownResponse);
						return;
					}
					else
					{
						Action<bool, TalentLegacyLevelUpCoolDownResponse> callBack3 = callBack;
						if (callBack3 == null)
						{
							return;
						}
						callBack3(false, talentLegacyLevelUpCoolDownResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoTalentLegacySelectCareerRequest(int careerId, Action<bool, TalentLegacySelectCareerResponse> callBack)
			{
				TalentLegacySelectCareerRequest talentLegacySelectCareerRequest = new TalentLegacySelectCareerRequest();
				talentLegacySelectCareerRequest.CommonParams = NetworkUtils.GetCommonParams();
				talentLegacySelectCareerRequest.CareerId = careerId;
				GameApp.NetWork.Send(talentLegacySelectCareerRequest, delegate(IMessage response)
				{
					TalentLegacySelectCareerResponse talentLegacySelectCareerResponse = response as TalentLegacySelectCareerResponse;
					if (talentLegacySelectCareerResponse != null && talentLegacySelectCareerResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnTalentLegacySelectCareerBack(talentLegacySelectCareerResponse);
						RedPointController.Instance.ReCalc("Talent.TalentLegacy", true);
						RedPointController.Instance.ReCalc("Equip.TalentLegacySkill", true);
						GameApp.Event.DispatchNow(null, 470, null);
						GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).MathAddAttributeData();
						GameApp.Event.DispatchNow(null, 145, null);
						Action<bool, TalentLegacySelectCareerResponse> callBack2 = callBack;
						if (callBack2 == null)
						{
							return;
						}
						callBack2(true, talentLegacySelectCareerResponse);
						return;
					}
					else
					{
						Action<bool, TalentLegacySelectCareerResponse> callBack3 = callBack;
						if (callBack3 == null)
						{
							return;
						}
						callBack3(false, talentLegacySelectCareerResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Task
		{
			public static void DoTaskGetInfoRequest(Action<bool, TaskGetInfoResponse> callback)
			{
				TaskGetInfoRequest taskGetInfoRequest = new TaskGetInfoRequest();
				taskGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(taskGetInfoRequest, delegate(IMessage response)
				{
					TaskGetInfoResponse taskGetInfoResponse = response as TaskGetInfoResponse;
					if (taskGetInfoResponse != null && taskGetInfoResponse.Code == 0)
					{
						Action<bool, TaskGetInfoResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, taskGetInfoResponse);
						}
						RedPointController.Instance.ReCalc("Main.Mission", true);
						return;
					}
					Action<bool, TaskGetInfoResponse> callback3 = callback;
					if (callback3 != null)
					{
						callback3(false, taskGetInfoResponse);
					}
					HLog.LogError("发送任务-获取数据 失败:", (taskGetInfoResponse != null) ? taskGetInfoResponse.Code.ToString() : null);
				}, false, false, string.Empty, true);
			}

			public static void DoTaskRewardDailyRequest(int id, Action<bool, TaskRewardDailyResponse> callback = null)
			{
				TaskRewardDailyRequest taskRewardDailyRequest = new TaskRewardDailyRequest();
				taskRewardDailyRequest.CommonParams = NetworkUtils.GetCommonParams();
				taskRewardDailyRequest.Id = (uint)id;
				GameApp.NetWork.Send(taskRewardDailyRequest, delegate(IMessage response)
				{
					TaskRewardDailyResponse taskRewardDailyResponse = response as TaskRewardDailyResponse;
					if (taskRewardDailyResponse != null && taskRewardDailyResponse.Code == 0)
					{
						TaskDataModule dataModule = GameApp.Data.GetDataModule(DataName.TaskDataModule);
						uint activeDaily = taskRewardDailyResponse.ActiveDaily;
						int dailyActive = dataModule.DailyActive;
						EventArgsTaskDataReceiveRewardDailyTask instance = Singleton<EventArgsTaskDataReceiveRewardDailyTask>.Instance;
						instance.SetData(dataModule.DailyActive, dataModule.WeeklyActive, taskRewardDailyResponse);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_TaskDataModule_ReceiveRewardDailyTask, instance);
						Action<bool, TaskRewardDailyResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, taskRewardDailyResponse);
						}
						RedPointController.Instance.ReCalc("Main.Mission", true);
						GameApp.SDK.Analyze.Track_AtiveTask_CollectPoint();
						return;
					}
					Action<bool, TaskRewardDailyResponse> callback3 = callback;
					if (callback3 != null)
					{
						callback3(false, taskRewardDailyResponse);
					}
					HLog.LogError("发送任务-每日领取奖励 失败:", (taskRewardDailyResponse != null) ? taskRewardDailyResponse.Code.ToString() : null);
				}, true, false, string.Empty, true);
			}

			public static void DoTaskRewardAchieveRequest(int id, Action<bool, TaskRewardAchieveResponse> callback)
			{
				TaskRewardAchieveRequest taskRewardAchieveRequest = new TaskRewardAchieveRequest();
				taskRewardAchieveRequest.CommonParams = NetworkUtils.GetCommonParams();
				taskRewardAchieveRequest.Id = (uint)id;
				GameApp.NetWork.Send(taskRewardAchieveRequest, delegate(IMessage response)
				{
					TaskRewardAchieveResponse taskRewardAchieveResponse = response as TaskRewardAchieveResponse;
					if (taskRewardAchieveResponse != null && taskRewardAchieveResponse.Code == 0)
					{
						EventArgsTaskDataReceiveAchievementRewardTask instance = Singleton<EventArgsTaskDataReceiveAchievementRewardTask>.Instance;
						instance.SetData(id, taskRewardAchieveResponse);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_TaskDataModule_ReceiveAchievementRewardTask, instance);
						Action<bool, TaskRewardAchieveResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, taskRewardAchieveResponse);
						}
						RedPointController.Instance.ReCalc("Main.Mission", true);
						return;
					}
					Action<bool, TaskRewardAchieveResponse> callback3 = callback;
					if (callback3 != null)
					{
						callback3(false, taskRewardAchieveResponse);
					}
					HLog.LogError("发送成就-领取奖励 失败:", (taskRewardAchieveResponse != null) ? taskRewardAchieveResponse.Code.ToString() : null);
				}, true, false, string.Empty, true);
			}

			public static void DoTaskActiveRewardRequest(int id, int type, Action<bool, TaskActiveRewardResponse> callback)
			{
				TaskActiveRewardRequest taskActiveRewardRequest = new TaskActiveRewardRequest();
				taskActiveRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				taskActiveRewardRequest.Id = (uint)id;
				taskActiveRewardRequest.Type = (uint)type;
				GameApp.NetWork.Send(taskActiveRewardRequest, delegate(IMessage response)
				{
					TaskActiveRewardResponse taskActiveRewardResponse = response as TaskActiveRewardResponse;
					if (taskActiveRewardResponse != null && taskActiveRewardResponse.Code == 0)
					{
						Action<bool, TaskActiveRewardResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, taskActiveRewardResponse);
						}
						RedPointController.Instance.ReCalc("Main.Mission", true);
						return;
					}
					Action<bool, TaskActiveRewardResponse> callback3 = callback;
					if (callback3 != null)
					{
						callback3(false, taskActiveRewardResponse);
					}
					HLog.LogError("发送任务-领取活跃度奖励 失败:", (taskActiveRewardResponse != null) ? taskActiveRewardResponse.Code.ToString() : null);
				}, true, false, string.Empty, true);
			}

			public static void DoTaskActiveRewardAllRequest(int type, Action<bool, TaskActiveRewardAllResponse> callback)
			{
				TaskDataModule dataModule = GameApp.Data.GetDataModule(DataName.TaskDataModule);
				new List<int>();
				if (type != 1)
				{
					if (type != 2)
					{
						throw new ArgumentOutOfRangeException();
					}
					dataModule.GetCanWeeklyActiveReceiveIDs();
				}
				else
				{
					dataModule.GetCanDailyActiveReceiveIDs();
				}
				TaskActiveRewardAllRequest taskActiveRewardAllRequest = new TaskActiveRewardAllRequest();
				taskActiveRewardAllRequest.CommonParams = NetworkUtils.GetCommonParams();
				taskActiveRewardAllRequest.Type = (uint)type;
				GameApp.NetWork.Send(taskActiveRewardAllRequest, delegate(IMessage response)
				{
					TaskActiveRewardAllResponse taskActiveRewardAllResponse = response as TaskActiveRewardAllResponse;
					if (taskActiveRewardAllResponse != null && taskActiveRewardAllResponse.Code == 0)
					{
						EventArgsTaskDataReceiveActiveRewardAllTask instance = Singleton<EventArgsTaskDataReceiveActiveRewardAllTask>.Instance;
						instance.SetData(taskActiveRewardAllResponse);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_TaskDataModule_ReceiveActiveRewardAllTask, instance);
						Action<bool, TaskActiveRewardAllResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, taskActiveRewardAllResponse);
						}
						RedPointController.Instance.ReCalc("Main.Mission", true);
						GameApp.SDK.Analyze.Track_AtiveTask_CollectItem(taskActiveRewardAllResponse.CommonData.Reward);
						return;
					}
					Action<bool, TaskActiveRewardAllResponse> callback3 = callback;
					if (callback3 != null)
					{
						callback3(false, taskActiveRewardAllResponse);
					}
					HLog.LogError("发送任务-领取全活跃度奖励 失败:", (taskActiveRewardAllResponse != null) ? taskActiveRewardAllResponse.Code.ToString() : null);
				}, true, false, string.Empty, true);
			}
		}

		public class Ticket
		{
			public static void DoShopBuyTicketsRequest(UserTicketKind ticketType, int ticketCount, Action<bool, ShopBuyTicketsResponse> callback)
			{
				ShopBuyTicketsRequest shopBuyTicketsRequest = new ShopBuyTicketsRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					TicketType = (uint)ticketType,
					TicketNum = (uint)ticketCount
				};
				GameApp.NetWork.Send(shopBuyTicketsRequest, delegate(IMessage response)
				{
					ShopBuyTicketsResponse shopBuyTicketsResponse = response as ShopBuyTicketsResponse;
					if (shopBuyTicketsResponse != null && shopBuyTicketsResponse.Code == 0)
					{
						if (shopBuyTicketsResponse.CommonData != null && shopBuyTicketsResponse.CommonData.Reward != null && shopBuyTicketsResponse.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(shopBuyTicketsResponse.CommonData.Reward, null, true);
						}
						Action<bool, ShopBuyTicketsResponse> callback2 = callback;
						if (callback2 != null)
						{
							callback2(true, shopBuyTicketsResponse);
						}
						int num = (int)(1121800 + ticketType);
						GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(num), shopBuyTicketsResponse.CommonData.Reward, null, null, null, null);
						GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(num), shopBuyTicketsResponse.CommonData.CostDto, null);
						return;
					}
					Action<bool, ShopBuyTicketsResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, shopBuyTicketsResponse);
				}, false, false, string.Empty, true);
			}

			public static void DoTicketsGetListRequest(Action<bool, TicketsGetListResponse> callback)
			{
				TicketsGetListRequest ticketsGetListRequest = new TicketsGetListRequest();
				ticketsGetListRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(ticketsGetListRequest, delegate(IMessage response)
				{
					TicketsGetListResponse ticketsGetListResponse = response as TicketsGetListResponse;
					if (ticketsGetListResponse != null && ticketsGetListResponse.Code == 0)
					{
						Action<bool, TicketsGetListResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, ticketsGetListResponse);
						return;
					}
					else
					{
						Action<bool, TicketsGetListResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, ticketsGetListResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}
		}

		public class TicketDailyExchange
		{
			public static void ADGetRewardRequest(int adType, int adID, Action<bool, ADGetRewardResponse> callBack)
			{
				ADGetRewardRequest adgetRewardRequest = new ADGetRewardRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					AdType = adType
				};
				GameApp.NetWork.Send(adgetRewardRequest, delegate(IMessage response)
				{
					ADGetRewardResponse adgetRewardResponse = response as ADGetRewardResponse;
					if (adgetRewardResponse != null && adgetRewardResponse.Code == 0)
					{
						if (adgetRewardResponse.CommonData != null)
						{
							if (adgetRewardResponse.CommonData.AdData != null)
							{
								GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(adgetRewardResponse.CommonData.AdData);
							}
							if (adgetRewardResponse.CommonData != null && adgetRewardResponse.CommonData.Reward != null && adgetRewardResponse.CommonData.Reward.Count > 0)
							{
								DxxTools.UI.OpenRewardCommon(adgetRewardResponse.CommonData.Reward, null, true);
							}
						}
						GameApp.Data.GetDataModule(DataName.TicketDailyExchangeDataModule).SetFreeAdLifeReFreshTime(UserTicketKind.UserLife, adgetRewardResponse.FreeAdLifeRefreshtime);
						Action<bool, ADGetRewardResponse> callBack2 = callBack;
						if (callBack2 != null)
						{
							callBack2(true, adgetRewardResponse);
						}
						if (adType == 1)
						{
							GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(adID), "REWARD ", "", adgetRewardResponse.CommonData.Reward, null);
							return;
						}
					}
					else
					{
						Action<bool, ADGetRewardResponse> callBack3 = callBack;
						if (callBack3 == null)
						{
							return;
						}
						callBack3(false, adgetRewardResponse);
					}
				}, true, false, string.Empty, true);
			}
		}

		public class Tower
		{
			public static void TowerChallengeRequest(int levelConfigId, Action<bool, TowerChallengeResponse> callback)
			{
				TowerDataModule dataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
				int num = dataModule.CalculateShouldChallengeLevelID(dataModule.CompleteTowerLevelId);
				TowerChallenge_Tower towerConfigByLevelId = dataModule.GetTowerConfigByLevelId(num);
				int towerNum = dataModule.GetTowerConfigNum(towerConfigByLevelId);
				int levelNum = dataModule.GetLevelNumByLevelId(num);
				TicketDataModule ticketData = GameApp.Data.GetDataModule(DataName.TicketDataModule);
				GameApp.SDK.Analyze.Track_TowerBattleStart(towerNum, levelNum, (int)ticketData.GetTicket(UserTicketKind.Tower).NewNum);
				TowerChallengeRequest towerChallengeRequest = new TowerChallengeRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					ConfigId = (uint)levelConfigId,
					ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion()
				};
				GameApp.NetWork.Send(towerChallengeRequest, delegate(IMessage response)
				{
					TowerChallengeResponse towerChallengeResponse = response as TowerChallengeResponse;
					if (towerChallengeResponse != null && towerChallengeResponse.Code == 0)
					{
						RedPointController.Instance.ReCalc("Main.NewWorld", true);
						GameApp.SDK.Analyze.Track_TowerBattleEnd(towerNum, levelNum, (int)ticketData.GetTicket(UserTicketKind.Tower).NewNum, (int)towerChallengeResponse.Result, towerChallengeResponse.CommonData.Reward);
					}
					Action<bool, TowerChallengeResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(towerChallengeResponse != null && towerChallengeResponse.Code == 0, towerChallengeResponse);
				}, true, false, string.Empty, true);
			}

			public static void TowerRewardRequest(int towerConfigId, Action<bool, TowerRewardResponse> callback)
			{
				TowerRewardRequest towerRewardRequest = new TowerRewardRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					ConfigId = (uint)towerConfigId
				};
				GameApp.NetWork.Send(towerRewardRequest, delegate(IMessage response)
				{
					TowerRewardResponse towerRewardResponse = response as TowerRewardResponse;
					bool flag = towerRewardResponse != null && towerRewardResponse.Code == 0;
					Action<bool, TowerRewardResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(flag, towerRewardResponse);
				}, true, false, string.Empty, true);
			}

			public static void TowerRankRequest(int page, bool isShowMask, Action<bool, TowerRankResponse> callback)
			{
				TowerRankRequest towerRankRequest = new TowerRankRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					Page = page
				};
				GameApp.NetWork.Send(towerRankRequest, delegate(IMessage response)
				{
					TowerRankResponse towerRankResponse = response as TowerRankResponse;
					Action<bool, TowerRankResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(towerRankResponse != null && towerRankResponse.Code == 0, towerRankResponse);
				}, isShowMask, false, string.Empty, true);
			}

			public static void TowerRankIndexRequest(bool isShowMask, Action<bool, TowerRankIndexResponse> callback)
			{
				TowerRankIndexRequest towerRankIndexRequest = new TowerRankIndexRequest
				{
					CommonParams = NetworkUtils.GetCommonParams()
				};
				GameApp.NetWork.Send(towerRankIndexRequest, delegate(IMessage response)
				{
					TowerRankIndexResponse towerRankIndexResponse = response as TowerRankIndexResponse;
					bool flag = towerRankIndexResponse != null && towerRankIndexResponse.Code == 0;
					if (flag)
					{
						EventArgsSetCurTowerRankData instance = Singleton<EventArgsSetCurTowerRankData>.Instance;
						instance.TowerRank = (int)towerRankIndexResponse.Index;
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_TowerDataMoudule_SetCurTowerRankData, instance);
					}
					Action<bool, TowerRankIndexResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(flag, towerRankIndexResponse);
				}, isShowMask, false, string.Empty, true);
			}
		}

		public class TVReward
		{
			public static void GetTVInfoListRequest(bool isLoginRequest = false, bool isShowMask = true, bool showError = true)
			{
				if (isLoginRequest)
				{
					isShowMask = false;
					showError = false;
				}
				GMVideoListRequest gmvideoListRequest = new GMVideoListRequest
				{
					CommonParams = NetworkUtils.GetCommonParams()
				};
				GameApp.NetWork.Send(gmvideoListRequest, delegate(IMessage response)
				{
					GMVideoListResponse gmvideoListResponse = response as GMVideoListResponse;
					if (gmvideoListResponse != null && gmvideoListResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.TVRewardDataModule).OnNetResponse_TVInfoList(gmvideoListResponse.GmVideoDtos, isLoginRequest);
					}
				}, isShowMask, false, string.Empty, showError);
			}
		}

		public class User
		{
			public static void DoUserGetPlayerInfoRequest(List<long> userIDs, Action<bool, UserGetOtherPlayerInfoResponse> callback)
			{
				UserGetOtherPlayerInfoRequest userGetOtherPlayerInfoRequest = new UserGetOtherPlayerInfoRequest();
				userGetOtherPlayerInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				userGetOtherPlayerInfoRequest.OtherUserIds.AddRange(userIDs);
				userGetOtherPlayerInfoRequest.NeedAttrDetail = true;
				GameApp.NetWork.Send(userGetOtherPlayerInfoRequest, delegate(IMessage response)
				{
					UserGetOtherPlayerInfoResponse userGetOtherPlayerInfoResponse = response as UserGetOtherPlayerInfoResponse;
					if (userGetOtherPlayerInfoResponse != null && userGetOtherPlayerInfoResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.PlayerInformationDataModule).AddOrUpdateCachePlayerInfo(userGetOtherPlayerInfoResponse.PlayerInfos.ToList<PlayerInfoDto>());
						Action<bool, UserGetOtherPlayerInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userGetOtherPlayerInfoResponse);
						return;
					}
					else
					{
						Action<bool, UserGetOtherPlayerInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userGetOtherPlayerInfoResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void DoUserGetCityInfoRequest(long userID, Action<bool, UserGetCityInfoResponse> callback)
			{
				UserGetCityInfoRequest userGetCityInfoRequest = new UserGetCityInfoRequest();
				userGetCityInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
				userGetCityInfoRequest.UserId = userID;
				GameApp.NetWork.Send(userGetCityInfoRequest, delegate(IMessage response)
				{
					UserGetCityInfoResponse userGetCityInfoResponse = response as UserGetCityInfoResponse;
					if (userGetCityInfoResponse != null && userGetCityInfoResponse.Code == 0)
					{
						Action<bool, UserGetCityInfoResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userGetCityInfoResponse);
						return;
					}
					else
					{
						Action<bool, UserGetCityInfoResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userGetCityInfoResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void DoUserHeartbeatSyncRequest(Action<bool, UserHeartbeatSyncResponse> callback)
			{
				UserHeartbeatSyncRequest userHeartbeatSyncRequest = new UserHeartbeatSyncRequest();
				userHeartbeatSyncRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(userHeartbeatSyncRequest, delegate(IMessage response)
				{
					UserHeartbeatSyncResponse userHeartbeatSyncResponse = response as UserHeartbeatSyncResponse;
					if (userHeartbeatSyncResponse != null && userHeartbeatSyncResponse.Code == 0)
					{
						DxxTools.Time.SyncServerTimestamp((long)userHeartbeatSyncResponse.Time);
						if (userHeartbeatSyncResponse.SetCloseFunction)
						{
							FunctionDataModule dataModule = GameApp.Data.GetDataModule(DataName.FunctionDataModule);
							if (dataModule != null && userHeartbeatSyncResponse.CloseFunctionId != null)
							{
								dataModule.CommonUpdateServerCloseFunction(userHeartbeatSyncResponse.CloseFunctionId);
							}
						}
						Action<bool, UserHeartbeatSyncResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userHeartbeatSyncResponse);
						return;
					}
					else
					{
						Action<bool, UserHeartbeatSyncResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userHeartbeatSyncResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void DoUserGetBattleReportRequest(ulong reportid, Action<bool, UserGetBattleReportResponse> callback)
			{
				UserGetBattleReportRequest userGetBattleReportRequest = new UserGetBattleReportRequest();
				userGetBattleReportRequest.CommonParams = NetworkUtils.GetCommonParams();
				userGetBattleReportRequest.ReportId = reportid;
				GameApp.NetWork.Send(userGetBattleReportRequest, delegate(IMessage response)
				{
					UserGetBattleReportResponse userGetBattleReportResponse = response as UserGetBattleReportResponse;
					if (userGetBattleReportResponse != null && userGetBattleReportResponse.Code == 0)
					{
						Action<bool, UserGetBattleReportResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userGetBattleReportResponse);
						return;
					}
					else
					{
						Action<bool, UserGetBattleReportResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userGetBattleReportResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void DoUserUpdateGuideMaskRequest(ulong guideMask, Action<bool, UserUpdateGuideMaskResponse> callback)
			{
				UserUpdateGuideMaskRequest userUpdateGuideMaskRequest = new UserUpdateGuideMaskRequest();
				userUpdateGuideMaskRequest.CommonParams = NetworkUtils.GetCommonParams();
				userUpdateGuideMaskRequest.GuideMask = guideMask;
				GameApp.NetWork.Send(userUpdateGuideMaskRequest, delegate(IMessage response)
				{
					UserUpdateGuideMaskResponse userUpdateGuideMaskResponse = response as UserUpdateGuideMaskResponse;
					if (userUpdateGuideMaskResponse != null && userUpdateGuideMaskResponse.Code == 0)
					{
						Action<bool, UserUpdateGuideMaskResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userUpdateGuideMaskResponse);
						return;
					}
					else
					{
						Action<bool, UserUpdateGuideMaskResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userUpdateGuideMaskResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void bindHabbyId(string email, string authCode, string tgaDeviceId, string tgaDistinctId, string language, Action<bool, UserHabbyMailBindResponse> callback)
			{
				UserHabbyMailBindRequest userHabbyMailBindRequest = new UserHabbyMailBindRequest();
				userHabbyMailBindRequest.CommonParams = NetworkUtils.GetCommonParams();
				userHabbyMailBindRequest.EmailDress = email;
				userHabbyMailBindRequest.BindParams.Add("authCode", authCode);
				userHabbyMailBindRequest.BindParams.Add("tgaDeviceId", tgaDeviceId);
				userHabbyMailBindRequest.BindParams.Add("tgaDistinctId", tgaDistinctId);
				userHabbyMailBindRequest.BindParams.Add("language", language);
				GameApp.NetWork.Send(userHabbyMailBindRequest, delegate(IMessage response)
				{
					UserHabbyMailBindResponse userHabbyMailBindResponse = response as UserHabbyMailBindResponse;
					if (userHabbyMailBindResponse != null && userHabbyMailBindResponse.Code == 0)
					{
						Action<bool, UserHabbyMailBindResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userHabbyMailBindResponse);
						return;
					}
					else
					{
						Action<bool, UserHabbyMailBindResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userHabbyMailBindResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}

			public static void bindHabbyIdReward(Action<bool, UserHabbyMailRewardResponse> callback)
			{
				UserHabbyMailRewardRequest userHabbyMailRewardRequest = new UserHabbyMailRewardRequest();
				userHabbyMailRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(userHabbyMailRewardRequest, delegate(IMessage response)
				{
					UserHabbyMailRewardResponse userHabbyMailRewardResponse = response as UserHabbyMailRewardResponse;
					if (userHabbyMailRewardResponse != null && userHabbyMailRewardResponse.Code == 0)
					{
						Action<bool, UserHabbyMailRewardResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, userHabbyMailRewardResponse);
						return;
					}
					else
					{
						Action<bool, UserHabbyMailRewardResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, userHabbyMailRewardResponse);
						return;
					}
				}, false, false, string.Empty, true);
			}
		}

		public class WorldBoss
		{
			public static void DoGetWorldBossInfo(bool isShowMask = true, Action<bool, int> callback = null)
			{
				WorldBossGetInfoRequest worldBossGetInfoRequest = new WorldBossGetInfoRequest
				{
					CommonParams = NetworkUtils.GetCommonParams()
				};
				GameApp.NetWork.Send(worldBossGetInfoRequest, delegate(IMessage response)
				{
					WorldBossGetInfoResponse worldBossGetInfoResponse = response as WorldBossGetInfoResponse;
					if (worldBossGetInfoResponse != null)
					{
						if (worldBossGetInfoResponse.Code == 0)
						{
							WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
							dataModule.UpdateByServerInfo(worldBossGetInfoResponse.Info);
							dataModule.DebugLog();
							Action<bool, int> callback2 = callback;
							if (callback2 == null)
							{
								return;
							}
							callback2(true, worldBossGetInfoResponse.Code);
							return;
						}
						else
						{
							Action<bool, int> callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3(false, worldBossGetInfoResponse.Code);
							return;
						}
					}
					else
					{
						Action<bool, int> callback4 = callback;
						if (callback4 == null)
						{
							return;
						}
						callback4(false, -106);
						return;
					}
				}, isShowMask, false, string.Empty, true);
			}

			public static void DoGetWorldBossBoxReward(int boxId, Action<bool> callBack = null)
			{
				WorldBossBoxRewardRequest worldBossBoxRewardRequest = new WorldBossBoxRewardRequest
				{
					CommonParams = NetworkUtils.GetCommonParams(),
					BoxRewardId = boxId
				};
				GameApp.NetWork.Send(worldBossBoxRewardRequest, delegate(IMessage response)
				{
					WorldBossBoxRewardResponse worldBossBoxRewardResponse = (WorldBossBoxRewardResponse)response;
					if (worldBossBoxRewardResponse != null && worldBossBoxRewardResponse.Code == 0)
					{
						WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
						dataModule.UpdateRewardMaxClaimed(worldBossBoxRewardResponse);
						dataModule.DebugLog();
						Action<bool> callBack2 = callBack;
						if (callBack2 == null)
						{
							return;
						}
						callBack2(true);
						return;
					}
					else
					{
						Action<bool> callBack3 = callBack;
						if (callBack3 == null)
						{
							return;
						}
						callBack3(false);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoStartWorldBoss(Action<bool, StartWorldBossResponse> callback = null)
			{
				StartWorldBossRequest startWorldBossRequest = new StartWorldBossRequest();
				startWorldBossRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(startWorldBossRequest, delegate(IMessage response)
				{
					StartWorldBossResponse startWorldBossResponse = (StartWorldBossResponse)response;
					if (startWorldBossResponse != null && startWorldBossResponse.Code == 0)
					{
						GameApp.Data.GetDataModule(DataName.WorldBossDataModule).UpdateReadyBattleInfo(startWorldBossResponse);
						Action<bool, StartWorldBossResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, startWorldBossResponse);
						return;
					}
					else
					{
						Action<bool, StartWorldBossResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, startWorldBossResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}

			public static void DoEndWorldBoss(List<int> skills, Action<bool, EndWorldBossResponse> callback = null)
			{
				EndWorldBossRequest endWorldBossRequest = new EndWorldBossRequest();
				endWorldBossRequest.CommonParams = NetworkUtils.GetCommonParams();
				endWorldBossRequest.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
				for (int i = 0; i < skills.Count; i++)
				{
					endWorldBossRequest.RoundSkillList.Add(skills[i]);
				}
				GameApp.NetWork.Send(endWorldBossRequest, delegate(IMessage response)
				{
					EndWorldBossResponse endWorldBossResponse = (EndWorldBossResponse)response;
					if (endWorldBossResponse != null && endWorldBossResponse.Code == 0)
					{
						WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
						dataModule.UpdateChallengeInfo(endWorldBossResponse);
						dataModule.DebugLog();
						Action<bool, EndWorldBossResponse> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(true, endWorldBossResponse);
						return;
					}
					else
					{
						Action<bool, EndWorldBossResponse> callback3 = callback;
						if (callback3 == null)
						{
							return;
						}
						callback3(false, endWorldBossResponse);
						return;
					}
				}, true, false, string.Empty, true);
			}
		}

		public class UserHeartbeat
		{
			public static void DoUserHeartbeatResponse(Action<UserHeartbeatResponse> callback)
			{
				UserHeartbeatRequest userHeartbeatRequest = new UserHeartbeatRequest();
				userHeartbeatRequest.CommonParams = NetworkUtils.GetCommonParams();
				GameApp.NetWork.Send(userHeartbeatRequest, delegate(IMessage response)
				{
					UserHeartbeatResponse userHeartbeatResponse = response as UserHeartbeatResponse;
					if (userHeartbeatResponse != null)
					{
						int code = userHeartbeatResponse.Code;
					}
					Action<UserHeartbeatResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(userHeartbeatResponse);
				}, false, false, string.Empty, true);
			}
		}
	}
}

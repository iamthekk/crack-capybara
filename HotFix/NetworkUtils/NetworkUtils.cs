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
			GameApp.NetWork.Send(chapterBattleLogRequest, delegate (IMessage response)
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
			GameApp.NetWork.Send(leaderBoardRequest, delegate (IMessage response)
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
			GameApp.NetWork.Send(userGetLastLoginRequest, delegate (IMessage response)
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
			GameApp.NetWork.Send(findServerListRequest, delegate (IMessage response)
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
			GameApp.NetWork.Send(gmvideoRequest, delegate (IMessage response)
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

	}
}

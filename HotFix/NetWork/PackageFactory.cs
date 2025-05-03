using System;
using System.Collections.Generic;
using Google.Protobuf;
using Proto.Activity;
using Proto.Actor;
using Proto.ActTime;
using Proto.Artifact;
using Proto.Battle;
using Proto.Chapter;
using Proto.Chat;
using Proto.Collection;
using Proto.Common;
using Proto.Conquer;
using Proto.CrossArena;
using Proto.Develop;
using Proto.Dungeon;
using Proto.Equip;
using Proto.Guild;
using Proto.GuildRace;
using Proto.IntegralShop;
using Proto.Item;
using Proto.LeaderBoard;
using Proto.Mail;
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
using Shop.Arena;
using Socket.Guild;

namespace NetWork
{
	public class PackageFactory
	{
		public static IMessage CreateMessage(ushort id)
		{
			if (id <= 12504)
			{
				if (id <= 11038)
				{
					if (id <= 10202)
					{
						if (id <= 9999)
						{
							switch (id)
							{
							case 9001:
								return new DevelopLoginRequest();
							case 9002:
								return new DevelopLoginResponse();
							case 9003:
								return new DevelopChangeResourceRequest();
							case 9004:
								return new DevelopChangeResourceResponse();
							case 9005:
								return new DevelopToolsRequest();
							case 9006:
								return new DevelopToolsResponse();
							default:
								if (id == 9999)
								{
									return new ErrorMsg();
								}
								break;
							}
						}
						else
						{
							switch (id)
							{
							case 10101:
								return new UserLoginRequest();
							case 10102:
								return new UserLoginResponse();
							case 10103:
								return new UserGetIapInfoRequest();
							case 10104:
								return new UserGetIapInfoResponse();
							case 10105:
								return new UserHeartbeatRequest();
							case 10106:
								return new UserHeartbeatResponse();
							case 10107:
								return new UserUpdateSystemMaskRequest();
							case 10108:
								return new UserUpdateSystemMaskResponse();
							case 10109:
								return new UserUpdateGuideMaskRequest();
							case 10110:
								return new UserUpdateGuideMaskResponse();
							case 10111:
								return new UserCancelAccountRequest();
							case 10112:
								return new UserCancelAccountResponse();
							case 10113:
								return new UserUpdateInfoRequest();
							case 10114:
								return new UserUpdateInfoResponse();
							case 10115:
								return new UserGetPlayerInfoRequest();
							case 10116:
								return new UserGetPlayerInfoResponse();
							case 10117:
								return new UserGetOtherPlayerInfoRequest();
							case 10118:
								return new UserGetOtherPlayerInfoResponse();
							case 10119:
								return new UserGetCityInfoRequest();
							case 10120:
								return new UserGetCityInfoResponse();
							case 10121:
								return new UserHeartbeatSyncRequest();
							case 10122:
								return new UserHeartbeatSyncResponse();
							case 10123:
								return new UserGetBattleReportRequest();
							case 10124:
								return new UserGetBattleReportResponse();
							case 10125:
								return new UserOpenModelRequest();
							case 10126:
								return new UserOpenModelResponse();
							case 10127:
								return new UserRefDataRequest();
							case 10128:
								return new UserRefDataResponse();
							case 10129:
							case 10130:
								break;
							case 10131:
								return new UserSetShenFenRequest();
							case 10132:
								return new UserSetShenFenResponse();
							case 10133:
								return new AccountSignInRequest();
							case 10134:
								return new AccountSignInResponse();
							case 10135:
								return new AccountLoginRequest();
							case 10136:
								return new AccountLoginResponse();
							case 10137:
								return new UnlockAvatarRequest();
							case 10138:
								return new UnlockAvatarResponse();
							case 10139:
								return new UpdateUserAvatarRequest();
							case 10140:
								return new UpdateUserAvatarResponse();
							case 10141:
								return new UnlockUserAvatarRequest();
							case 10142:
								return new UnlockUserAvatarResponse();
							case 10143:
								return new UserGetAllPanelInfoRequest();
							case 10144:
								return new UserGetAllPanelInfoResponse();
							case 10145:
								return new UserHabbyMailBindRequest();
							case 10146:
								return new UserHabbyMailBindResponse();
							case 10147:
								return new UserHabbyMailRewardRequest();
							case 10148:
								return new UserHabbyMailRewardResponse();
							case 10149:
								return new ADGetRewardRequest();
							case 10150:
								return new ADGetRewardResponse();
							default:
								if (id == 10201)
								{
									return new ItemUseRequest();
								}
								if (id == 10202)
								{
									return new ItemUseResponse();
								}
								break;
							}
						}
					}
					else if (id <= 10510)
					{
						switch (id)
						{
						case 10401:
							return new MissionGetInfoRequest();
						case 10402:
							return new MissionGetInfoResponse();
						case 10403:
							return new MissionStartRequest();
						case 10404:
							return new MissionStartResponse();
						case 10405:
							return new MissionCompleteRequest();
						case 10406:
							return new MissionCompleteResponse();
						case 10407:
							return new MissionGetHangUpItemsRequest();
						case 10408:
							return new MissionGetHangUpItemsResponse();
						case 10409:
							return new MissionReceiveHangUpItemsRequest();
						case 10410:
							return new MissionReceiveHangUpItemsResponse();
						case 10411:
							return new MissionQuickHangUpRequest();
						case 10412:
							return new MissionQuickHangUpResponse();
						case 10413:
						case 10414:
							break;
						case 10415:
							return new WorldBossGetInfoRequest();
						case 10416:
							return new WorldBossGetInfoResponse();
						case 10417:
							return new EndWorldBossRequest();
						case 10418:
							return new EndWorldBossResponse();
						case 10419:
							return new StartWorldBossRequest();
						case 10420:
							return new StartWorldBossResponse();
						case 10421:
							return new WorldBossBoxRewardRequest();
						case 10422:
							return new WorldBossBoxRewardResponse();
						default:
							switch (id)
							{
							case 10501:
								return new TaskGetInfoRequest();
							case 10502:
								return new TaskGetInfoResponse();
							case 10503:
								return new TaskRewardDailyRequest();
							case 10504:
								return new TaskRewardDailyResponse();
							case 10505:
								return new TaskRewardAchieveRequest();
							case 10506:
								return new TaskRewardAchieveResponse();
							case 10507:
								return new TaskActiveRewardRequest();
							case 10508:
								return new TaskActiveRewardResponse();
							case 10509:
								return new TaskActiveRewardAllRequest();
							case 10510:
								return new TaskActiveRewardAllResponse();
							}
							break;
						}
					}
					else
					{
						switch (id)
						{
						case 10701:
							return new PayInAppPurchaseRequest();
						case 10702:
							return new PayInAppPurchaseResponse();
						case 10703:
							return new PayPreOrderRequest();
						case 10704:
							return new PayPreOrderResponse();
						case 10705:
							return new MonthCardGetRewardRequest();
						case 10706:
							return new MonthCardGetRewardResponse();
						case 10707:
							return new BattlePassRewardRequest();
						case 10708:
							return new BattlePassRewardResponse();
						case 10709:
							return new BattlePassChangeScoreRequest();
						case 10710:
							return new BattlePassChangeScoreResponse();
						case 10711:
							return new BattlePassFinalRewardRequest();
						case 10712:
							return new BattlePassFinalRewardResponse();
						case 10713:
							return new VIPLevelRewardRequest();
						case 10714:
							return new VIPLevelRewardResponse();
						case 10715:
							return new LevelFundRewardRequest();
						case 10716:
							return new LevelFundRewardResponse();
						case 10717:
							return new FirstRechargeRewardRequest();
						case 10718:
							return new FirstRechargeRewardResponse();
						case 10719:
							return new FirstRechargeRewardV1Request();
						case 10720:
							return new FirstRechargeRewardV1Response();
						default:
							switch (id)
							{
							case 10801:
								return new MailGetListRequest();
							case 10802:
								return new MailGetListResponse();
							case 10803:
								return new MailReceiveAwardsRequest();
							case 10804:
								return new MailReceiveAwardsResponse();
							case 10805:
								return new MailDeleteRequest();
							case 10806:
								return new MailDeleteResponse();
							default:
								switch (id)
								{
								case 11001:
									return new StartChapterRequest();
								case 11002:
									return new StartChapterResponse();
								case 11003:
									return new EndChapterRequest();
								case 11004:
									return new EndChapterResponse();
								case 11005:
									return new GetWaveRewardRequest();
								case 11006:
									return new GetWaveRewardResponse();
								case 11007:
									return new ChapterSyncProcessRequest();
								case 11008:
									return new ChapterSyncProcessResponse();
								case 11009:
									return new ChapterActRewardRequest();
								case 11010:
									return new ChapterActRewardResponse();
								case 11011:
									return new StartChapterSweepRequest();
								case 11012:
									return new StartChapterSweepResponse();
								case 11013:
									return new EndChapterSweepRequest();
								case 11014:
									return new EndChapterSweepResponse();
								case 11015:
									return new ChapterActRankRequest();
								case 11016:
									return new ChapterActRankResponse();
								case 11017:
									return new ChapterRankRewardRequest();
								case 11018:
									return new ChapterRankRewardResponse();
								case 11019:
									return new EndChapterCheckRequest();
								case 11020:
									return new EndChapterCheckResponse();
								case 11021:
									return new ChapterBattleLogRequest();
								case 11022:
									return new ChapterBattleLogResponse();
								case 11023:
									return new GetHangUpInfoRequest();
								case 11024:
									return new GetHangUpInfoResponse();
								case 11025:
									return new GetHangUpRewardRequest();
								case 11026:
									return new GetHangUpRewardResponse();
								case 11027:
									return new ChapterBattlePassScoreRequest();
								case 11028:
									return new ChapterBattlePassScoreResponse();
								case 11029:
									return new GetChapterBattlePassRewardRequest();
								case 11030:
									return new GetChapterBattlePassRewardResponse();
								case 11031:
									return new ChapterBattlePassOpenBoxRequest();
								case 11032:
									return new ChapterBattlePassOpenBoxResponse();
								case 11033:
									return new ChapterWheelScoreRequest();
								case 11034:
									return new ChapterWheelScoreResponse();
								case 11035:
									return new ChapterWheelSpineRequest();
								case 11036:
									return new ChapterWheelSpineResponse();
								case 11037:
									return new ChapterWheelInfoRequest();
								case 11038:
									return new ChapterWheelInfoResponse();
								}
								break;
							}
							break;
						}
					}
				}
				else if (id <= 11558)
				{
					if (id <= 11240)
					{
						switch (id)
						{
						case 11101:
							return new EquipStrengthRequest();
						case 11102:
							return new EquipStrengthResponse();
						case 11103:
							return new EquipComposeRequest();
						case 11104:
							return new EquipComposeResponse();
						case 11105:
							return new EquipUpgradeRequest();
						case 11106:
							return new EquipUpgradeResponse();
						case 11107:
							return new EquipDressRequest();
						case 11108:
							return new EquipDressResponse();
						case 11109:
							return new EquipLevelResetRequest();
						case 11110:
							return new EquipLevelResetResponse();
						case 11111:
							return new EquipEvolutionRequest();
						case 11112:
							return new EquipEvolutionResponse();
						case 11113:
							return new EquipDecomposeRequest();
						case 11114:
							return new EquipDecomposeResponse();
						default:
							switch (id)
							{
							case 11201:
								return new ShopGetInfoRequest();
							case 11202:
								return new ShopGetInfoResponse();
							case 11203:
								return new ShopBuyItemRequest();
							case 11204:
								return new ShopBuyItemResponse();
							case 11205:
								return new ShopBuyIAPItemRequest();
							case 11206:
								return new ShopBuyIAPItemResponse();
							case 11207:
								return new ShopDoDrawRequest();
							case 11208:
								return new ShopDoDrawResponse();
							case 11209:
								return new ShopIntegralGetInfoRequest();
							case 11210:
								return new ShopIntegralGetInfoResponse();
							case 11211:
								return new ShopIntegralRefreshItemRequest();
							case 11212:
								return new ShopIntegralRefreshItemResponse();
							case 11213:
								return new ShopIntegralBuyItemRequest();
							case 11214:
								return new ShopIntegralBuyItemResponse();
							case 11215:
								return new ShopGacheWishRequest();
							case 11216:
								return new ShopGacheWishResponse();
							case 11217:
								return new ShopBuyTicketsRequest();
							case 11218:
								return new ShopBuyTicketsResponse();
							case 11219:
								return new ShopFreeIAPItemRequest();
							case 11220:
								return new ShopFreeIAPItemResponse();
							case 11221:
								return new DungeonAdGetItemRequest();
							case 11222:
								return new DungeonAdGetItemResponse();
							case 11223:
								return new TicketsGetListRequest();
							case 11224:
								return new TicketsGetListResponse();
							case 11225:
								return new FinishAdvertRequest();
							case 11226:
								return new FinishAdvertResponse();
							case 11227:
								return new IapPushRemoveRequest();
							case 11228:
								return new IapPushRemoveResponse();
							case 11229:
								return new GetIapPushDtoRequest();
							case 11230:
								return new GetIapPushDtoResponse();
							case 11231:
								return new HeadFrameActiveRequest();
							case 11232:
								return new HeadFrameActiveResponse();
							case 11233:
								return new GMVideoRequest();
							case 11234:
								return new GMVideoResponse();
							case 11235:
								return new GMVideoListRequest();
							case 11236:
								return new GMVideoListResponse();
							case 11237:
								return new StartAdvertRequest();
							case 11238:
								return new StartAdvertResponse();
							case 11239:
								return new BuyItemRewardRequest();
							case 11240:
								return new BuyItemRewardResponse();
							}
							break;
						}
					}
					else
					{
						switch (id)
						{
						case 11401:
							return new ActivityGetListRequest();
						case 11402:
							return new ActivityGetListResponse();
						case 11403:
							return new getChainPacksTimeRequest();
						case 11404:
							return new getChainPacksTimeResponse();
						case 11405:
							return new takeChainPacksRewardRequest();
						case 11406:
							return new takeChainPacksRewardResponse();
						case 11407:
							return new takePushChainRewardRequest();
						case 11408:
							return new takePushChainRewardResponse();
						default:
							switch (id)
							{
							case 11501:
								return new SignInGetInfoRequest();
							case 11502:
								return new SignInGetInfoResponse();
							case 11503:
								return new SignInDoSignRequest();
							case 11504:
								return new SignInDoSignResponse();
							default:
								switch (id)
								{
								case 11551:
									return new SevenDayTaskGetInfoRequest();
								case 11552:
									return new SevenDayTaskGetInfoResponse();
								case 11553:
									return new SevenDayTaskRewardRequest();
								case 11554:
									return new SevenDayTaskRewardResponse();
								case 11555:
									return new SevenDayTaskActiveRewardRequest();
								case 11556:
									return new SevenDayTaskActiveRewardResponse();
								case 11557:
									return new SevenDayFreeRewardRequest();
								case 11558:
									return new SevenDayFreeRewardResponse();
								}
								break;
							}
							break;
						}
					}
				}
				else if (id <= 11714)
				{
					switch (id)
					{
					case 11601:
						return new ActShopBuyRequest();
					case 11602:
						return new ActShopBuyResponse();
					case 11603:
						return new ActDropBuyRequest();
					case 11604:
						return new ActDropBuyResponse();
					case 11605:
						return new ActTimeInfoRequest();
					case 11606:
						return new ActTimeInfoResponse();
					case 11607:
						return new ActTimeRewardRequest();
					case 11608:
						return new ActTimeRewardResponse();
					case 11609:
						return new ActTimePayFreeRewardRequest();
					case 11610:
						return new ActTimePayFreeRewardResponse();
					case 11611:
						return new ActTimeRankRequest();
					case 11612:
						return new ActTimeRankResponse();
					case 11613:
					case 11614:
					case 11615:
					case 11616:
					case 11617:
					case 11618:
					case 11619:
					case 11620:
						break;
					case 11621:
						return new UserGetLastLoginRequest();
					case 11622:
						return new UserGetLastLoginResponse();
					case 11623:
						return new FindServerListRequest();
					case 11624:
						return new FindServerListResponse();
					default:
						switch (id)
						{
						case 11701:
							return new TurnTableGetInfoRequest();
						case 11702:
							return new TurnTableGetInfoResponse();
						case 11703:
							return new TurnTableExtractRequest();
						case 11704:
							return new TurnTableExtractResponse();
						case 11705:
							return new TurnTableReceiveCumulativeRewardRequest();
						case 11706:
							return new TurnTableReceiveCumulativeRewardResponse();
						case 11709:
							return new TurnTableTaskReceiveRewardRequest();
						case 11710:
							return new TurnTableTaskReceiveRewardResponse();
						case 11711:
							return new TurnTableSelectBigGuaranteeItemRequest();
						case 11712:
							return new TurnTableSelectBigGuaranteeItemResponse();
						case 11713:
							return new TurnPayAdRequest();
						case 11714:
							return new TurnPayAdResponse();
						}
						break;
					}
				}
				else
				{
					switch (id)
					{
					case 12301:
						return new RelicActiveRequest();
					case 12302:
						return new RelicActiveResponse();
					case 12303:
						return new RelicStrengthRequest();
					case 12304:
						return new RelicStrengthResponse();
					case 12305:
						return new RelicStarRequest();
					case 12306:
						return new RelicStarResponse();
					default:
						switch (id)
						{
						case 12415:
							return new TalentsLvUpRequest();
						case 12416:
							return new TalentsLvUpResponse();
						case 12417:
							return new TalentLegacyLeaderBoardRequest();
						case 12418:
							return new TalentLegacyLeaderBoardResponse();
						case 12419:
							return new TalentLegacyInfoRequest();
						case 12420:
							return new TalentLegacyInfoResponse();
						case 12421:
							return new TalentLegacyLevelUpRequest();
						case 12422:
							return new TalentLegacyLevelUpResponse();
						case 12423:
							return new TalentLegacySwitchRequest();
						case 12424:
							return new TalentLegacySwitchResponse();
						case 12425:
							return new TalentLegacyLevelUpCoolDownRequest();
						case 12426:
							return new TalentLegacyLevelUpCoolDownResponse();
						case 12427:
							return new TalentLegacySelectCareerRequest();
						case 12428:
							return new TalentLegacySelectCareerResponse();
						case 12429:
						case 12430:
						case 12431:
						case 12432:
						case 12433:
						case 12434:
						case 12435:
						case 12436:
							break;
						case 12437:
							return new StartDungeonRequest();
						case 12438:
							return new StartDungeonResponse();
						default:
							switch (id)
							{
							case 12501:
								return new ActorLevelUpRequest();
							case 12502:
								return new ActorLevelUpResponse();
							case 12503:
								return new ActorAdvanceUpRequest();
							case 12504:
								return new ActorAdvanceUpResponse();
							}
							break;
						}
						break;
					}
				}
			}
			else if (id <= 20318)
			{
				if (id <= 13008)
				{
					if (id <= 12706)
					{
						switch (id)
						{
						case 12601:
							return new CityGoldmineLevelUpRequest();
						case 12602:
							return new CityGoldmineLevelUpResponse();
						case 12603:
							return new CityGoldmineHangRewardRequest();
						case 12604:
							return new CityGoldmineHangRewardResponse();
						case 12605:
							return new CityGetChestInfoRequest();
						case 12606:
							return new CityGetChestInfoResponse();
						case 12607:
							return new CityOpenChestRequest();
						case 12608:
							return new CityOpenChestResponse();
						case 12609:
							return new CityTakeScoreRewardRequest();
						case 12610:
							return new CityTakeScoreRewardResponse();
						case 12611:
							return new CitySyncPowerRequest();
						case 12612:
							return new CitySyncPowerResponse();
						case 12613:
							return new CityGetInfoRequest();
						case 12614:
							return new CityGetInfoResponse();
						default:
							switch (id)
							{
							case 12701:
								return new InteractListRequest();
							case 12702:
								return new InteractListResponse();
							case 12703:
								return new SocialPowerRankRequest();
							case 12704:
								return new SocialPowerRankResponse();
							case 12705:
								return new InteractDetailRequest();
							case 12706:
								return new InteractDetailResponse();
							}
							break;
						}
					}
					else
					{
						switch (id)
						{
						case 12801:
							return new ConquerListRequest();
						case 12802:
							return new ConquerListResponse();
						case 12803:
							return new ConquerBattleRequest();
						case 12804:
							return new ConquerBattleResponse();
						case 12805:
							return new ConquerRevoltRequest();
						case 12806:
							return new ConquerRevoltResponse();
						case 12807:
							return new ConquerLootRequest();
						case 12808:
							return new ConquerLootResponse();
						case 12809:
							return new ConquerPardonRequest();
						case 12810:
							return new ConquerPardonResponse();
						default:
							switch (id)
							{
							case 12901:
								return new IntegralShopBuyRequest();
							case 12902:
								return new IntegralShopBuyResponse();
							case 12903:
								return new IntegralShopRefreshRequest();
							case 12904:
								return new IntegralShopRefreshResponse();
							case 12905:
								return new IntegralShopGetInfoRequest();
							case 12906:
								return new IntegralShopGetInfoResponse();
							default:
								switch (id)
								{
								case 13001:
									return new TowerChallengeRequest();
								case 13002:
									return new TowerChallengeResponse();
								case 13003:
									return new TowerRewardRequest();
								case 13004:
									return new TowerRewardResponse();
								case 13005:
									return new TowerRankRequest();
								case 13006:
									return new TowerRankResponse();
								case 13007:
									return new TowerRankIndexRequest();
								case 13008:
									return new TowerRankIndexResponse();
								}
								break;
							}
							break;
						}
					}
				}
				else if (id <= 20103)
				{
					switch (id)
					{
					case 13101:
						return new CrossArenaGetInfoRequest();
					case 13102:
						return new CrossArenaGetInfoResponse();
					case 13103:
						return new CrossArenaChallengeListRequest();
					case 13104:
						return new CrossArenaChallengeListResponse();
					case 13105:
						return new CrossArenaChallengeRequest();
					case 13106:
						return new CrossArenaChallengeResponse();
					case 13107:
						return new CrossArenaRankRequest();
					case 13108:
						return new CrossArenaRankResponse();
					case 13109:
						return new CrossArenaRecordRequest();
					case 13110:
						return new CrossArenaRecordResponse();
					case 13111:
						return new CrossArenaEnterRequest();
					case 13112:
						return new CrossArenaEnterResponse();
					case 13113:
						return new LeaderBoardRequest();
					case 13114:
						return new LeaderBoardResponse();
					case 13115:
					case 13116:
					case 13117:
					case 13118:
					case 13119:
					case 13120:
					case 13121:
					case 13122:
					case 13123:
					case 13124:
					case 13125:
					case 13126:
					case 13127:
					case 13128:
					case 13129:
					case 13130:
						break;
					case 13131:
						return new HellGetPanelInfoRequest();
					case 13132:
						return new HellGetPanelInfoResponse();
					case 13133:
						return new HellEnterBattleRequest();
					case 13134:
						return new HellEnterBattleResponse();
					case 13135:
						return new HellEnterSelectSkillRequest();
					case 13136:
						return new HellEnterSelectSkillResponse();
					case 13137:
						return new HellDoChallengeRequest();
					case 13138:
						return new HellDoChallengeResponse();
					case 13139:
						return new HellSaveSkillRequest();
					case 13140:
						return new HellSaveSkillResponse();
					case 13141:
						return new HellExitBattleRequest();
					case 13142:
						return new HellExitBattleResponse();
					case 13143:
						return new HellRankRequest();
					case 13144:
						return new HellRankResponse();
					case 13145:
						return new HellRevertHpRequest();
					case 13146:
						return new HellRevertHpResponse();
					default:
						if (id == 20103)
						{
							return new PowerRequest();
						}
						break;
					}
				}
				else
				{
					if (id == 20104)
					{
						return new PowerResponse();
					}
					switch (id)
					{
					case 20201:
						return new PetStrengthRequest();
					case 20202:
						return new PetStrengthResponse();
					case 20203:
						return new PetStarRequest();
					case 20204:
						return new PetStarResponse();
					case 20205:
						return new PetResetRequest();
					case 20206:
						return new PetResetResponse();
					case 20207:
						return new PetFormationRequest();
					case 20208:
						return new PetFormationResponse();
					case 20209:
						return new PetDrawRequest();
					case 20210:
						return new PetDrawResponse();
					case 20211:
						return new PetFetterActiveRequest();
					case 20212:
						return new PetFetterActiveResponse();
					case 20213:
						return new PetShowRequest();
					case 20214:
						return new PetShowResponse();
					case 20215:
						return new PetComposeRequest();
					case 20216:
						return new PetComposeResponse();
					case 20217:
						return new CollecStrengthRequest();
					case 20218:
						return new CollecStrengthResponse();
					case 20219:
						return new CollecStarRequest();
					case 20220:
						return new CollecStarResponse();
					case 20221:
						return new CollecComposeRequest();
					case 20222:
						return new CollecComposeResponse();
					case 20223:
					case 20224:
					case 20225:
					case 20226:
					case 20227:
					case 20228:
					case 20229:
					case 20230:
						break;
					case 20231:
						return new ChestUseRequest();
					case 20232:
						return new ChestUseResponse();
					case 20233:
						return new ChestRewardRequest();
					case 20234:
						return new ChestRewardResponse();
					default:
						switch (id)
						{
						case 20301:
							return new GetMiningInfoRequest();
						case 20302:
							return new GetMiningInfoResponse();
						case 20303:
							return new DoMiningRequest();
						case 20304:
							return new DoMiningResponse();
						case 20305:
							return new OpenBombRequest();
						case 20306:
							return new OpenBombResponse();
						case 20307:
							return new GetMiningRewardRequest();
						case 20308:
							return new GetMiningRewardResponse();
						case 20309:
							return new SetMiningOptionRequest();
						case 20310:
							return new SetMiningOptionResponse();
						case 20311:
							return new OpenNextDoorRequest();
						case 20312:
							return new OpenNextDoorResponse();
						case 20313:
							return new BounDrawRequest();
						case 20314:
							return new BounDrawResponse();
						case 20315:
							return new MiningBoxUpgradeRewardRequest();
						case 20316:
							return new MiningBoxUpgradeRewardResponse();
						case 20317:
							return new MiningAdGetItemRequest();
						case 20318:
							return new MiningAdGetItemResponse();
						}
						break;
					}
				}
			}
			else if (id <= 30602)
			{
				if (id <= 30318)
				{
					switch (id)
					{
					case 20401:
						return new PetTrainRequest();
					case 20402:
						return new PetTrainResponse();
					case 20403:
						return new PetTrainSureRequest();
					case 20404:
						return new PetTrainSureResponse();
					default:
						switch (id)
						{
						case 30101:
							return new GuildGetInfoRequest();
						case 30102:
							return new GuildGetInfoResponse();
						case 30103:
							return new GuildCreateRequest();
						case 30104:
							return new GuildCreateResponse();
						case 30105:
							return new GuildSearchRequest();
						case 30106:
							return new GuildSearchResponse();
						case 30107:
							return new GuildGetDetailRequest();
						case 30108:
							return new GuildGetDetailResponse();
						case 30109:
							return new GuildGetMemberListRequest();
						case 30110:
							return new GuildGetMemberListResponse();
						case 30111:
							return new GuildModifyRequest();
						case 30112:
							return new GuildModifyResponse();
						case 30113:
							return new GuildDismissRequest();
						case 30114:
							return new GuildDismissResponse();
						case 30115:
							return new GuildApplyJoinRequest();
						case 30116:
							return new GuildApplyJoinResponse();
						case 30117:
							return new GuildCancelApplyRequest();
						case 30118:
							return new GuildCancelApplyResponse();
						case 30119:
							return new GuildAutoJoinRequest();
						case 30120:
							return new GuildAutoJoinResponse();
						case 30121:
							return new GuildGetApplyListRequest();
						case 30122:
							return new GuildGetApplyListResponse();
						case 30123:
							return new GuildAgreeJoinRequest();
						case 30124:
							return new GuildAgreeJoinResponse();
						case 30125:
							return new GuildRefuseJoinRequest();
						case 30126:
							return new GuildRefuseJoinResponse();
						case 30127:
							return new GuildKickOutRequest();
						case 30128:
							return new GuildKickOutResponse();
						case 30129:
							return new GuildLeaveRequest();
						case 30130:
							return new GuildLeaveResponse();
						case 30131:
							return new GuildUpPositionRequest();
						case 30132:
							return new GuildUpPositionResponse();
						case 30133:
							return new GuildTransferPresidentRequest();
						case 30134:
							return new GuildTransferPresidentResponse();
						case 30135:
							return new GuildGetFeaturesInfoRequest();
						case 30136:
							return new GuildGetFeaturesInfoResponse();
						case 30137:
							return new GuildLevelUpRequest();
						case 30138:
							return new GuildLevelUpResponse();
						case 30141:
							return new GuildSignInRequest();
						case 30142:
							return new GuildSignInResponse();
						case 30151:
							return new GuildShopBuyRequest();
						case 30152:
							return new GuildShopBuyResponse();
						case 30153:
							return new GuildShopRefreshRequest();
						case 30154:
							return new GuildShopRefreshResponse();
						case 30161:
							return new GuildTaskRewardRequest();
						case 30162:
							return new GuildTaskRewardResponse();
						case 30163:
							return new GuildTaskRefreshRequest();
						case 30164:
							return new GuildTaskRefreshResponse();
						case 30171:
							return new GuildGetMessageRecordsRequest();
						case 30172:
							return new GuildGetMessageRecordsResponse();
						case 30181:
							return new GuildBossGetInfoRequest();
						case 30182:
							return new GuildBossGetInfoResponse();
						case 30183:
							return new GuildBossStartRequest();
						case 30184:
							return new GuildBossStartResponse();
						case 30185:
							return new GuildBossEndRequest();
						case 30186:
							return new GuildBossEndResponse();
						case 30187:
							return new GuildBossBuyCntRequest();
						case 30188:
							return new GuildBossBuyCntResponse();
						case 30189:
							return new GuildBossTaskRewardRequest();
						case 30190:
							return new GuildBossTaskRewardResponse();
						case 30191:
							return new GuildBossBoxRewardRequest();
						case 30192:
							return new GuildBossBoxRewardResponse();
						case 30193:
							return new GuildBossGetChallengeListRequest();
						case 30194:
							return new GuildBossGetChallengeListResponse();
						case 30201:
							return new GuildDonationReqItemRequest();
						case 30202:
							return new GuildDonationReqItemResponse();
						case 30203:
							return new GuildDonationSendItemRequest();
						case 30204:
							return new GuildDonationSendItemResponse();
						case 30205:
							return new GuildDonationReceiveRequest();
						case 30206:
							return new GuildDonationReceiveResponse();
						case 30207:
							return new GuildDonationGetRecordsRequest();
						case 30208:
							return new GuildDonationGetRecordsResponse();
						case 30209:
							return new GuildDonationGetOperationRecordsRequest();
						case 30210:
							return new GuildDonationGetOperationRecordsResponse();
						case 30211:
							return new GuildBossEndBattleRequest();
						case 30212:
							return new GuildBossEndBattleResponse();
						case 30213:
							return new GuildBossBattleGRankRequest();
						case 30214:
							return new GuildBossBattleGRankResponse();
						case 30241:
							return new ArtifactUpgradeRequest();
						case 30242:
							return new ArtifactUpgradeResponse();
						case 30243:
							return new ArtifactUpgradeAllRequest();
						case 30244:
							return new ArtifactUpgradeAllResponse();
						case 30245:
							return new ArtifactItemStarRequest();
						case 30246:
							return new ArtifactItemStarResponse();
						case 30247:
							return new ArtifactApplySkillRequest();
						case 30248:
							return new ArtifactApplySkillResponse();
						case 30249:
							return new ArtifactUnlockRequest();
						case 30250:
							return new ArtifactUnlockResponse();
						case 30281:
							return new MountUpgradeRequest();
						case 30282:
							return new MountUpgradeResponse();
						case 30283:
							return new MountUpgradeAllRequest();
						case 30284:
							return new MountUpgradeAllResponse();
						case 30285:
							return new MountItemStarRequest();
						case 30286:
							return new MountItemStarResponse();
						case 30287:
							return new MountApplySkillRequest();
						case 30288:
							return new MountApplySkillResponse();
						case 30289:
							return new MountDressRequest();
						case 30290:
							return new MountDressResponse();
						case 30291:
							return new MountUnlockRequest();
						case 30292:
							return new MountUnlockResponse();
						case 30301:
							return new GuildRaceGuildApplyRequest();
						case 30302:
							return new GuildRaceGuildApplyResponse();
						case 30303:
							return new GuildRaceUserApplyRequest();
						case 30304:
							return new GuildRaceUserApplyResponse();
						case 30305:
							return new GuildRaceEditSeqRequest();
						case 30306:
							return new GuildRaceEditSeqResponse();
						case 30307:
							return new GuildRaceInfoRequest();
						case 30308:
							return new GuildRaceInfoResponse();
						case 30309:
							return new GuildRaceOwnerUserApplyListRequest();
						case 30310:
							return new GuildRaceOwnerUserApplyListResponse();
						case 30311:
							return new GuildRaceOppUserApplyListRequest();
						case 30312:
							return new GuildRaceOppUserApplyListResponse();
						case 30313:
							return new GuildRaceUserInfoRequest();
						case 30314:
							return new GuildRaceUserInfoResponse();
						case 30315:
							return new GuildRaceGuildRecordRequest();
						case 30316:
							return new GuildRaceGuildRecordResponse();
						case 30317:
							return new GuildRacePVPRecordRequest();
						case 30318:
							return new GuildRacePVPRecordResponse();
						}
						break;
					}
				}
				else
				{
					switch (id)
					{
					case 30501:
						return new GuildTechUpgradeRequest();
					case 30502:
						return new GuildTechUpgradeResponse();
					case 30503:
						return new GuildLogRequest();
					case 30504:
						return new GuildLogResponse();
					case 30505:
						return new GuildContributeRequest();
					case 30506:
						return new GuildContributeResponse();
					case 30507:
						return new GuildBossKilledRewardRequest();
					case 30508:
						return new GuildBossKilledRewardResponse();
					default:
						if (id == 30601)
						{
							return new GuildBossStartBattleRequest();
						}
						if (id == 30602)
						{
							return new GuildBossStartBattleResponse();
						}
						break;
					}
				}
			}
			else if (id <= 31108)
			{
				switch (id)
				{
				case 30701:
					return new EnterRequest();
				case 30702:
					return new EnterResponse();
				case 30703:
					return new LikeRequest();
				case 30704:
					return new LikeResponse();
				case 30705:
					return new NewWorldInfoRequest();
				case 30706:
					return new NewWorldInfoResponse();
				case 30707:
					return new NewWorldTaskRewardRequest();
				case 30708:
					return new NewWorldTaskRewardResponse();
				default:
					switch (id)
					{
					case 31101:
						return new SocketLoginRequest();
					case 31102:
						return new SocketLoginResponse();
					case 31103:
						return new SocketJoinGroupRequest();
					case 31104:
						return new SocketJoinGroupResponse();
					case 31105:
						return new SocketQuitGroupRequest();
					case 31106:
						return new SocketQuitGroupResponse();
					case 31107:
						return new SocketHeartBeatRequest();
					case 31108:
						return new SocketHeartBeatResponse();
					}
					break;
				}
			}
			else
			{
				switch (id)
				{
				case 31202:
					return new SocketLoginRepeatMessage();
				case 31203:
				case 31205:
					break;
				case 31204:
					return new SocketReconnectMessage();
				case 31206:
					return new SocketPushMessage();
				default:
					if (id == 31299)
					{
						return new SocketErrorMessage();
					}
					switch (id)
					{
					case 32101:
						return new ChatTranslateRequest();
					case 32102:
						return new ChatTranslateResponse();
					case 32103:
						return new ChatGuildRequest();
					case 32104:
						return new ChatGuildResponse();
					case 32105:
						return new ChatGuildShowItemRequest();
					case 32106:
						return new ChatGuildShowItemResponse();
					case 32107:
						return new ChatCommonRequest();
					case 32108:
						return new ChatCommonResponse();
					case 32109:
						return new ChatGetMessageRecordsRequest();
					case 32110:
						return new ChatGetMessageRecordsResponse();
					}
					break;
				}
			}
			return null;
		}

		public static ushort GetMessageId(IMessage message)
		{
			if (message is ActivityGetListRequest)
			{
				return 11401;
			}
			if (message is ActivityGetListResponse)
			{
				return 11402;
			}
			if (message is getChainPacksTimeRequest)
			{
				return 11403;
			}
			if (message is getChainPacksTimeResponse)
			{
				return 11404;
			}
			if (message is takeChainPacksRewardRequest)
			{
				return 11405;
			}
			if (message is takeChainPacksRewardResponse)
			{
				return 11406;
			}
			if (message is takePushChainRewardRequest)
			{
				return 11407;
			}
			if (message is takePushChainRewardResponse)
			{
				return 11408;
			}
			if (message is ActorLevelUpRequest)
			{
				return 12501;
			}
			if (message is ActorLevelUpResponse)
			{
				return 12502;
			}
			if (message is ActorAdvanceUpRequest)
			{
				return 12503;
			}
			if (message is ActorAdvanceUpResponse)
			{
				return 12504;
			}
			if (message is ActShopBuyRequest)
			{
				return 11601;
			}
			if (message is ActShopBuyResponse)
			{
				return 11602;
			}
			if (message is ActDropBuyRequest)
			{
				return 11603;
			}
			if (message is ActDropBuyResponse)
			{
				return 11604;
			}
			if (message is ActTimeInfoRequest)
			{
				return 11605;
			}
			if (message is ActTimeInfoResponse)
			{
				return 11606;
			}
			if (message is ActTimeRewardRequest)
			{
				return 11607;
			}
			if (message is ActTimeRewardResponse)
			{
				return 11608;
			}
			if (message is ActTimePayFreeRewardRequest)
			{
				return 11609;
			}
			if (message is ActTimePayFreeRewardResponse)
			{
				return 11610;
			}
			if (message is ActTimeRankRequest)
			{
				return 11611;
			}
			if (message is ActTimeRankResponse)
			{
				return 11612;
			}
			if (message is ArtifactUpgradeRequest)
			{
				return 30241;
			}
			if (message is ArtifactUpgradeResponse)
			{
				return 30242;
			}
			if (message is ArtifactUpgradeAllRequest)
			{
				return 30243;
			}
			if (message is ArtifactUpgradeAllResponse)
			{
				return 30244;
			}
			if (message is ArtifactItemStarRequest)
			{
				return 30245;
			}
			if (message is ArtifactItemStarResponse)
			{
				return 30246;
			}
			if (message is ArtifactApplySkillRequest)
			{
				return 30247;
			}
			if (message is ArtifactApplySkillResponse)
			{
				return 30248;
			}
			if (message is ArtifactUnlockRequest)
			{
				return 30249;
			}
			if (message is ArtifactUnlockResponse)
			{
				return 30250;
			}
			if (message is PowerRequest)
			{
				return 20103;
			}
			if (message is PowerResponse)
			{
				return 20104;
			}
			if (message is StartChapterRequest)
			{
				return 11001;
			}
			if (message is StartChapterResponse)
			{
				return 11002;
			}
			if (message is EndChapterRequest)
			{
				return 11003;
			}
			if (message is EndChapterResponse)
			{
				return 11004;
			}
			if (message is GetWaveRewardRequest)
			{
				return 11005;
			}
			if (message is GetWaveRewardResponse)
			{
				return 11006;
			}
			if (message is ChapterSyncProcessRequest)
			{
				return 11007;
			}
			if (message is ChapterSyncProcessResponse)
			{
				return 11008;
			}
			if (message is ChapterActRewardRequest)
			{
				return 11009;
			}
			if (message is ChapterActRewardResponse)
			{
				return 11010;
			}
			if (message is StartChapterSweepRequest)
			{
				return 11011;
			}
			if (message is StartChapterSweepResponse)
			{
				return 11012;
			}
			if (message is EndChapterSweepRequest)
			{
				return 11013;
			}
			if (message is EndChapterSweepResponse)
			{
				return 11014;
			}
			if (message is ChapterActRankRequest)
			{
				return 11015;
			}
			if (message is ChapterActRankResponse)
			{
				return 11016;
			}
			if (message is ChapterRankRewardRequest)
			{
				return 11017;
			}
			if (message is ChapterRankRewardResponse)
			{
				return 11018;
			}
			if (message is EndChapterCheckRequest)
			{
				return 11019;
			}
			if (message is EndChapterCheckResponse)
			{
				return 11020;
			}
			if (message is ChapterBattleLogRequest)
			{
				return 11021;
			}
			if (message is ChapterBattleLogResponse)
			{
				return 11022;
			}
			if (message is GetHangUpInfoRequest)
			{
				return 11023;
			}
			if (message is GetHangUpInfoResponse)
			{
				return 11024;
			}
			if (message is GetHangUpRewardRequest)
			{
				return 11025;
			}
			if (message is GetHangUpRewardResponse)
			{
				return 11026;
			}
			if (message is ChapterBattlePassScoreRequest)
			{
				return 11027;
			}
			if (message is ChapterBattlePassScoreResponse)
			{
				return 11028;
			}
			if (message is GetChapterBattlePassRewardRequest)
			{
				return 11029;
			}
			if (message is GetChapterBattlePassRewardResponse)
			{
				return 11030;
			}
			if (message is ChapterBattlePassOpenBoxRequest)
			{
				return 11031;
			}
			if (message is ChapterBattlePassOpenBoxResponse)
			{
				return 11032;
			}
			if (message is ChapterWheelScoreRequest)
			{
				return 11033;
			}
			if (message is ChapterWheelScoreResponse)
			{
				return 11034;
			}
			if (message is ChapterWheelSpineRequest)
			{
				return 11035;
			}
			if (message is ChapterWheelSpineResponse)
			{
				return 11036;
			}
			if (message is ChapterWheelInfoRequest)
			{
				return 11037;
			}
			if (message is ChapterWheelInfoResponse)
			{
				return 11038;
			}
			if (message is ChatTranslateRequest)
			{
				return 32101;
			}
			if (message is ChatTranslateResponse)
			{
				return 32102;
			}
			if (message is ChatGuildRequest)
			{
				return 32103;
			}
			if (message is ChatGuildResponse)
			{
				return 32104;
			}
			if (message is ChatGuildShowItemRequest)
			{
				return 32105;
			}
			if (message is ChatGuildShowItemResponse)
			{
				return 32106;
			}
			if (message is ChatCommonRequest)
			{
				return 32107;
			}
			if (message is ChatCommonResponse)
			{
				return 32108;
			}
			if (message is ChatGetMessageRecordsRequest)
			{
				return 32109;
			}
			if (message is ChatGetMessageRecordsResponse)
			{
				return 32110;
			}
			if (message is ChestUseRequest)
			{
				return 20231;
			}
			if (message is ChestUseResponse)
			{
				return 20232;
			}
			if (message is ChestRewardRequest)
			{
				return 20233;
			}
			if (message is ChestRewardResponse)
			{
				return 20234;
			}
			if (message is CityGoldmineLevelUpRequest)
			{
				return 12601;
			}
			if (message is CityGoldmineLevelUpResponse)
			{
				return 12602;
			}
			if (message is CityGoldmineHangRewardRequest)
			{
				return 12603;
			}
			if (message is CityGoldmineHangRewardResponse)
			{
				return 12604;
			}
			if (message is CityGetChestInfoRequest)
			{
				return 12605;
			}
			if (message is CityGetChestInfoResponse)
			{
				return 12606;
			}
			if (message is CityOpenChestRequest)
			{
				return 12607;
			}
			if (message is CityOpenChestResponse)
			{
				return 12608;
			}
			if (message is CityTakeScoreRewardRequest)
			{
				return 12609;
			}
			if (message is CityTakeScoreRewardResponse)
			{
				return 12610;
			}
			if (message is CitySyncPowerRequest)
			{
				return 12611;
			}
			if (message is CitySyncPowerResponse)
			{
				return 12612;
			}
			if (message is CityGetInfoRequest)
			{
				return 12613;
			}
			if (message is CityGetInfoResponse)
			{
				return 12614;
			}
			if (message is CollecStrengthRequest)
			{
				return 20217;
			}
			if (message is CollecStrengthResponse)
			{
				return 20218;
			}
			if (message is CollecStarRequest)
			{
				return 20219;
			}
			if (message is CollecStarResponse)
			{
				return 20220;
			}
			if (message is CollecComposeRequest)
			{
				return 20221;
			}
			if (message is CollecComposeResponse)
			{
				return 20222;
			}
			if (message is ErrorMsg)
			{
				return 9999;
			}
			if (message is ConquerListRequest)
			{
				return 12801;
			}
			if (message is ConquerListResponse)
			{
				return 12802;
			}
			if (message is ConquerBattleRequest)
			{
				return 12803;
			}
			if (message is ConquerBattleResponse)
			{
				return 12804;
			}
			if (message is ConquerRevoltRequest)
			{
				return 12805;
			}
			if (message is ConquerRevoltResponse)
			{
				return 12806;
			}
			if (message is ConquerLootRequest)
			{
				return 12807;
			}
			if (message is ConquerLootResponse)
			{
				return 12808;
			}
			if (message is ConquerPardonRequest)
			{
				return 12809;
			}
			if (message is ConquerPardonResponse)
			{
				return 12810;
			}
			if (message is CrossArenaGetInfoRequest)
			{
				return 13101;
			}
			if (message is CrossArenaGetInfoResponse)
			{
				return 13102;
			}
			if (message is CrossArenaChallengeListRequest)
			{
				return 13103;
			}
			if (message is CrossArenaChallengeListResponse)
			{
				return 13104;
			}
			if (message is CrossArenaChallengeRequest)
			{
				return 13105;
			}
			if (message is CrossArenaChallengeResponse)
			{
				return 13106;
			}
			if (message is CrossArenaRankRequest)
			{
				return 13107;
			}
			if (message is CrossArenaRankResponse)
			{
				return 13108;
			}
			if (message is CrossArenaRecordRequest)
			{
				return 13109;
			}
			if (message is CrossArenaRecordResponse)
			{
				return 13110;
			}
			if (message is CrossArenaEnterRequest)
			{
				return 13111;
			}
			if (message is CrossArenaEnterResponse)
			{
				return 13112;
			}
			if (message is DevelopLoginRequest)
			{
				return 9001;
			}
			if (message is DevelopLoginResponse)
			{
				return 9002;
			}
			if (message is DevelopChangeResourceRequest)
			{
				return 9003;
			}
			if (message is DevelopChangeResourceResponse)
			{
				return 9004;
			}
			if (message is DevelopToolsRequest)
			{
				return 9005;
			}
			if (message is DevelopToolsResponse)
			{
				return 9006;
			}
			if (message is StartDungeonRequest)
			{
				return 12437;
			}
			if (message is StartDungeonResponse)
			{
				return 12438;
			}
			if (message is EquipStrengthRequest)
			{
				return 11101;
			}
			if (message is EquipStrengthResponse)
			{
				return 11102;
			}
			if (message is EquipComposeRequest)
			{
				return 11103;
			}
			if (message is EquipComposeResponse)
			{
				return 11104;
			}
			if (message is EquipUpgradeRequest)
			{
				return 11105;
			}
			if (message is EquipUpgradeResponse)
			{
				return 11106;
			}
			if (message is EquipDressRequest)
			{
				return 11107;
			}
			if (message is EquipDressResponse)
			{
				return 11108;
			}
			if (message is EquipLevelResetRequest)
			{
				return 11109;
			}
			if (message is EquipLevelResetResponse)
			{
				return 11110;
			}
			if (message is EquipEvolutionRequest)
			{
				return 11111;
			}
			if (message is EquipEvolutionResponse)
			{
				return 11112;
			}
			if (message is EquipDecomposeRequest)
			{
				return 11113;
			}
			if (message is EquipDecomposeResponse)
			{
				return 11114;
			}
			if (message is GuildGetInfoRequest)
			{
				return 30101;
			}
			if (message is GuildGetInfoResponse)
			{
				return 30102;
			}
			if (message is GuildCreateRequest)
			{
				return 30103;
			}
			if (message is GuildCreateResponse)
			{
				return 30104;
			}
			if (message is GuildSearchRequest)
			{
				return 30105;
			}
			if (message is GuildSearchResponse)
			{
				return 30106;
			}
			if (message is GuildGetDetailRequest)
			{
				return 30107;
			}
			if (message is GuildGetDetailResponse)
			{
				return 30108;
			}
			if (message is GuildGetMemberListRequest)
			{
				return 30109;
			}
			if (message is GuildGetMemberListResponse)
			{
				return 30110;
			}
			if (message is GuildModifyRequest)
			{
				return 30111;
			}
			if (message is GuildModifyResponse)
			{
				return 30112;
			}
			if (message is GuildDismissRequest)
			{
				return 30113;
			}
			if (message is GuildDismissResponse)
			{
				return 30114;
			}
			if (message is GuildApplyJoinRequest)
			{
				return 30115;
			}
			if (message is GuildApplyJoinResponse)
			{
				return 30116;
			}
			if (message is GuildCancelApplyRequest)
			{
				return 30117;
			}
			if (message is GuildCancelApplyResponse)
			{
				return 30118;
			}
			if (message is GuildAutoJoinRequest)
			{
				return 30119;
			}
			if (message is GuildAutoJoinResponse)
			{
				return 30120;
			}
			if (message is GuildGetApplyListRequest)
			{
				return 30121;
			}
			if (message is GuildGetApplyListResponse)
			{
				return 30122;
			}
			if (message is GuildAgreeJoinRequest)
			{
				return 30123;
			}
			if (message is GuildAgreeJoinResponse)
			{
				return 30124;
			}
			if (message is GuildRefuseJoinRequest)
			{
				return 30125;
			}
			if (message is GuildRefuseJoinResponse)
			{
				return 30126;
			}
			if (message is GuildKickOutRequest)
			{
				return 30127;
			}
			if (message is GuildKickOutResponse)
			{
				return 30128;
			}
			if (message is GuildLeaveRequest)
			{
				return 30129;
			}
			if (message is GuildLeaveResponse)
			{
				return 30130;
			}
			if (message is GuildUpPositionRequest)
			{
				return 30131;
			}
			if (message is GuildUpPositionResponse)
			{
				return 30132;
			}
			if (message is GuildTransferPresidentRequest)
			{
				return 30133;
			}
			if (message is GuildTransferPresidentResponse)
			{
				return 30134;
			}
			if (message is GuildGetFeaturesInfoRequest)
			{
				return 30135;
			}
			if (message is GuildGetFeaturesInfoResponse)
			{
				return 30136;
			}
			if (message is GuildLevelUpRequest)
			{
				return 30137;
			}
			if (message is GuildLevelUpResponse)
			{
				return 30138;
			}
			if (message is GuildSignInRequest)
			{
				return 30141;
			}
			if (message is GuildSignInResponse)
			{
				return 30142;
			}
			if (message is GuildShopBuyRequest)
			{
				return 30151;
			}
			if (message is GuildShopBuyResponse)
			{
				return 30152;
			}
			if (message is GuildShopRefreshRequest)
			{
				return 30153;
			}
			if (message is GuildShopRefreshResponse)
			{
				return 30154;
			}
			if (message is GuildTaskRewardRequest)
			{
				return 30161;
			}
			if (message is GuildTaskRewardResponse)
			{
				return 30162;
			}
			if (message is GuildTaskRefreshRequest)
			{
				return 30163;
			}
			if (message is GuildTaskRefreshResponse)
			{
				return 30164;
			}
			if (message is GuildGetMessageRecordsRequest)
			{
				return 30171;
			}
			if (message is GuildGetMessageRecordsResponse)
			{
				return 30172;
			}
			if (message is GuildBossGetInfoRequest)
			{
				return 30181;
			}
			if (message is GuildBossGetInfoResponse)
			{
				return 30182;
			}
			if (message is GuildBossStartRequest)
			{
				return 30183;
			}
			if (message is GuildBossStartResponse)
			{
				return 30184;
			}
			if (message is GuildBossEndRequest)
			{
				return 30185;
			}
			if (message is GuildBossEndResponse)
			{
				return 30186;
			}
			if (message is GuildBossBuyCntRequest)
			{
				return 30187;
			}
			if (message is GuildBossBuyCntResponse)
			{
				return 30188;
			}
			if (message is GuildBossTaskRewardRequest)
			{
				return 30189;
			}
			if (message is GuildBossTaskRewardResponse)
			{
				return 30190;
			}
			if (message is GuildBossBoxRewardRequest)
			{
				return 30191;
			}
			if (message is GuildBossBoxRewardResponse)
			{
				return 30192;
			}
			if (message is GuildBossGetChallengeListRequest)
			{
				return 30193;
			}
			if (message is GuildBossGetChallengeListResponse)
			{
				return 30194;
			}
			if (message is GuildBossBattleGRankRequest)
			{
				return 30213;
			}
			if (message is GuildBossBattleGRankResponse)
			{
				return 30214;
			}
			if (message is GuildDonationReqItemRequest)
			{
				return 30201;
			}
			if (message is GuildDonationReqItemResponse)
			{
				return 30202;
			}
			if (message is GuildDonationSendItemRequest)
			{
				return 30203;
			}
			if (message is GuildDonationSendItemResponse)
			{
				return 30204;
			}
			if (message is GuildDonationReceiveRequest)
			{
				return 30205;
			}
			if (message is GuildDonationReceiveResponse)
			{
				return 30206;
			}
			if (message is GuildDonationGetRecordsRequest)
			{
				return 30207;
			}
			if (message is GuildDonationGetRecordsResponse)
			{
				return 30208;
			}
			if (message is GuildDonationGetOperationRecordsRequest)
			{
				return 30209;
			}
			if (message is GuildDonationGetOperationRecordsResponse)
			{
				return 30210;
			}
			if (message is GuildBossEndBattleRequest)
			{
				return 30211;
			}
			if (message is GuildBossEndBattleResponse)
			{
				return 30212;
			}
			if (message is GuildBossStartBattleRequest)
			{
				return 30601;
			}
			if (message is GuildBossStartBattleResponse)
			{
				return 30602;
			}
			if (message is GuildTechUpgradeRequest)
			{
				return 30501;
			}
			if (message is GuildTechUpgradeResponse)
			{
				return 30502;
			}
			if (message is GuildLogRequest)
			{
				return 30503;
			}
			if (message is GuildLogResponse)
			{
				return 30504;
			}
			if (message is GuildContributeRequest)
			{
				return 30505;
			}
			if (message is GuildContributeResponse)
			{
				return 30506;
			}
			if (message is GuildBossKilledRewardRequest)
			{
				return 30507;
			}
			if (message is GuildBossKilledRewardResponse)
			{
				return 30508;
			}
			if (message is GuildRaceGuildApplyRequest)
			{
				return 30301;
			}
			if (message is GuildRaceGuildApplyResponse)
			{
				return 30302;
			}
			if (message is GuildRaceUserApplyRequest)
			{
				return 30303;
			}
			if (message is GuildRaceUserApplyResponse)
			{
				return 30304;
			}
			if (message is GuildRaceEditSeqRequest)
			{
				return 30305;
			}
			if (message is GuildRaceEditSeqResponse)
			{
				return 30306;
			}
			if (message is GuildRaceInfoRequest)
			{
				return 30307;
			}
			if (message is GuildRaceInfoResponse)
			{
				return 30308;
			}
			if (message is GuildRaceOwnerUserApplyListRequest)
			{
				return 30309;
			}
			if (message is GuildRaceOwnerUserApplyListResponse)
			{
				return 30310;
			}
			if (message is GuildRaceOppUserApplyListRequest)
			{
				return 30311;
			}
			if (message is GuildRaceOppUserApplyListResponse)
			{
				return 30312;
			}
			if (message is GuildRaceUserInfoRequest)
			{
				return 30313;
			}
			if (message is GuildRaceUserInfoResponse)
			{
				return 30314;
			}
			if (message is GuildRaceGuildRecordRequest)
			{
				return 30315;
			}
			if (message is GuildRaceGuildRecordResponse)
			{
				return 30316;
			}
			if (message is GuildRacePVPRecordRequest)
			{
				return 30317;
			}
			if (message is GuildRacePVPRecordResponse)
			{
				return 30318;
			}
			if (message is HellGetPanelInfoRequest)
			{
				return 13131;
			}
			if (message is HellGetPanelInfoResponse)
			{
				return 13132;
			}
			if (message is HellEnterBattleRequest)
			{
				return 13133;
			}
			if (message is HellEnterBattleResponse)
			{
				return 13134;
			}
			if (message is HellEnterSelectSkillRequest)
			{
				return 13135;
			}
			if (message is HellEnterSelectSkillResponse)
			{
				return 13136;
			}
			if (message is HellDoChallengeRequest)
			{
				return 13137;
			}
			if (message is HellDoChallengeResponse)
			{
				return 13138;
			}
			if (message is HellSaveSkillRequest)
			{
				return 13139;
			}
			if (message is HellSaveSkillResponse)
			{
				return 13140;
			}
			if (message is HellExitBattleRequest)
			{
				return 13141;
			}
			if (message is HellExitBattleResponse)
			{
				return 13142;
			}
			if (message is HellRankRequest)
			{
				return 13143;
			}
			if (message is HellRankResponse)
			{
				return 13144;
			}
			if (message is HellRevertHpRequest)
			{
				return 13145;
			}
			if (message is HellRevertHpResponse)
			{
				return 13146;
			}
			if (message is IntegralShopBuyRequest)
			{
				return 12901;
			}
			if (message is IntegralShopBuyResponse)
			{
				return 12902;
			}
			if (message is IntegralShopRefreshRequest)
			{
				return 12903;
			}
			if (message is IntegralShopRefreshResponse)
			{
				return 12904;
			}
			if (message is IntegralShopGetInfoRequest)
			{
				return 12905;
			}
			if (message is IntegralShopGetInfoResponse)
			{
				return 12906;
			}
			if (message is ItemUseRequest)
			{
				return 10201;
			}
			if (message is ItemUseResponse)
			{
				return 10202;
			}
			if (message is LeaderBoardRequest)
			{
				return 13113;
			}
			if (message is LeaderBoardResponse)
			{
				return 13114;
			}
			if (message is MailGetListRequest)
			{
				return 10801;
			}
			if (message is MailGetListResponse)
			{
				return 10802;
			}
			if (message is MailReceiveAwardsRequest)
			{
				return 10803;
			}
			if (message is MailReceiveAwardsResponse)
			{
				return 10804;
			}
			if (message is MailDeleteRequest)
			{
				return 10805;
			}
			if (message is MailDeleteResponse)
			{
				return 10806;
			}
			if (message is GetMiningInfoRequest)
			{
				return 20301;
			}
			if (message is GetMiningInfoResponse)
			{
				return 20302;
			}
			if (message is DoMiningRequest)
			{
				return 20303;
			}
			if (message is DoMiningResponse)
			{
				return 20304;
			}
			if (message is OpenBombRequest)
			{
				return 20305;
			}
			if (message is OpenBombResponse)
			{
				return 20306;
			}
			if (message is GetMiningRewardRequest)
			{
				return 20307;
			}
			if (message is GetMiningRewardResponse)
			{
				return 20308;
			}
			if (message is SetMiningOptionRequest)
			{
				return 20309;
			}
			if (message is SetMiningOptionResponse)
			{
				return 20310;
			}
			if (message is OpenNextDoorRequest)
			{
				return 20311;
			}
			if (message is OpenNextDoorResponse)
			{
				return 20312;
			}
			if (message is BounDrawRequest)
			{
				return 20313;
			}
			if (message is BounDrawResponse)
			{
				return 20314;
			}
			if (message is MiningBoxUpgradeRewardRequest)
			{
				return 20315;
			}
			if (message is MiningBoxUpgradeRewardResponse)
			{
				return 20316;
			}
			if (message is MiningAdGetItemRequest)
			{
				return 20317;
			}
			if (message is MiningAdGetItemResponse)
			{
				return 20318;
			}
			if (message is MissionGetInfoRequest)
			{
				return 10401;
			}
			if (message is MissionGetInfoResponse)
			{
				return 10402;
			}
			if (message is MissionStartRequest)
			{
				return 10403;
			}
			if (message is MissionStartResponse)
			{
				return 10404;
			}
			if (message is MissionCompleteRequest)
			{
				return 10405;
			}
			if (message is MissionCompleteResponse)
			{
				return 10406;
			}
			if (message is MissionGetHangUpItemsRequest)
			{
				return 10407;
			}
			if (message is MissionGetHangUpItemsResponse)
			{
				return 10408;
			}
			if (message is MissionReceiveHangUpItemsRequest)
			{
				return 10409;
			}
			if (message is MissionReceiveHangUpItemsResponse)
			{
				return 10410;
			}
			if (message is MissionQuickHangUpRequest)
			{
				return 10411;
			}
			if (message is MissionQuickHangUpResponse)
			{
				return 10412;
			}
			if (message is MountUpgradeRequest)
			{
				return 30281;
			}
			if (message is MountUpgradeResponse)
			{
				return 30282;
			}
			if (message is MountUpgradeAllRequest)
			{
				return 30283;
			}
			if (message is MountUpgradeAllResponse)
			{
				return 30284;
			}
			if (message is MountItemStarRequest)
			{
				return 30285;
			}
			if (message is MountItemStarResponse)
			{
				return 30286;
			}
			if (message is MountApplySkillRequest)
			{
				return 30287;
			}
			if (message is MountApplySkillResponse)
			{
				return 30288;
			}
			if (message is MountDressRequest)
			{
				return 30289;
			}
			if (message is MountDressResponse)
			{
				return 30290;
			}
			if (message is MountUnlockRequest)
			{
				return 30291;
			}
			if (message is MountUnlockResponse)
			{
				return 30292;
			}
			if (message is EnterRequest)
			{
				return 30701;
			}
			if (message is EnterResponse)
			{
				return 30702;
			}
			if (message is LikeRequest)
			{
				return 30703;
			}
			if (message is LikeResponse)
			{
				return 30704;
			}
			if (message is NewWorldInfoRequest)
			{
				return 30705;
			}
			if (message is NewWorldInfoResponse)
			{
				return 30706;
			}
			if (message is NewWorldTaskRewardRequest)
			{
				return 30707;
			}
			if (message is NewWorldTaskRewardResponse)
			{
				return 30708;
			}
			if (message is PayInAppPurchaseRequest)
			{
				return 10701;
			}
			if (message is PayInAppPurchaseResponse)
			{
				return 10702;
			}
			if (message is PayPreOrderRequest)
			{
				return 10703;
			}
			if (message is PayPreOrderResponse)
			{
				return 10704;
			}
			if (message is MonthCardGetRewardRequest)
			{
				return 10705;
			}
			if (message is MonthCardGetRewardResponse)
			{
				return 10706;
			}
			if (message is BattlePassRewardRequest)
			{
				return 10707;
			}
			if (message is BattlePassRewardResponse)
			{
				return 10708;
			}
			if (message is BattlePassChangeScoreRequest)
			{
				return 10709;
			}
			if (message is BattlePassChangeScoreResponse)
			{
				return 10710;
			}
			if (message is BattlePassFinalRewardRequest)
			{
				return 10711;
			}
			if (message is BattlePassFinalRewardResponse)
			{
				return 10712;
			}
			if (message is VIPLevelRewardRequest)
			{
				return 10713;
			}
			if (message is VIPLevelRewardResponse)
			{
				return 10714;
			}
			if (message is LevelFundRewardRequest)
			{
				return 10715;
			}
			if (message is LevelFundRewardResponse)
			{
				return 10716;
			}
			if (message is FirstRechargeRewardRequest)
			{
				return 10717;
			}
			if (message is FirstRechargeRewardResponse)
			{
				return 10718;
			}
			if (message is FirstRechargeRewardV1Request)
			{
				return 10719;
			}
			if (message is FirstRechargeRewardV1Response)
			{
				return 10720;
			}
			if (message is PetStrengthRequest)
			{
				return 20201;
			}
			if (message is PetStrengthResponse)
			{
				return 20202;
			}
			if (message is PetStarRequest)
			{
				return 20203;
			}
			if (message is PetStarResponse)
			{
				return 20204;
			}
			if (message is PetResetRequest)
			{
				return 20205;
			}
			if (message is PetResetResponse)
			{
				return 20206;
			}
			if (message is PetFormationRequest)
			{
				return 20207;
			}
			if (message is PetFormationResponse)
			{
				return 20208;
			}
			if (message is PetDrawRequest)
			{
				return 20209;
			}
			if (message is PetDrawResponse)
			{
				return 20210;
			}
			if (message is PetFetterActiveRequest)
			{
				return 20211;
			}
			if (message is PetFetterActiveResponse)
			{
				return 20212;
			}
			if (message is PetShowRequest)
			{
				return 20213;
			}
			if (message is PetShowResponse)
			{
				return 20214;
			}
			if (message is PetComposeRequest)
			{
				return 20215;
			}
			if (message is PetComposeResponse)
			{
				return 20216;
			}
			if (message is PetTrainRequest)
			{
				return 20401;
			}
			if (message is PetTrainResponse)
			{
				return 20402;
			}
			if (message is PetTrainSureRequest)
			{
				return 20403;
			}
			if (message is PetTrainSureResponse)
			{
				return 20404;
			}
			if (message is RelicActiveRequest)
			{
				return 12301;
			}
			if (message is RelicActiveResponse)
			{
				return 12302;
			}
			if (message is RelicStrengthRequest)
			{
				return 12303;
			}
			if (message is RelicStrengthResponse)
			{
				return 12304;
			}
			if (message is RelicStarRequest)
			{
				return 12305;
			}
			if (message is RelicStarResponse)
			{
				return 12306;
			}
			if (message is UserGetLastLoginRequest)
			{
				return 11621;
			}
			if (message is UserGetLastLoginResponse)
			{
				return 11622;
			}
			if (message is FindServerListRequest)
			{
				return 11623;
			}
			if (message is FindServerListResponse)
			{
				return 11624;
			}
			if (message is SevenDayTaskGetInfoRequest)
			{
				return 11551;
			}
			if (message is SevenDayTaskGetInfoResponse)
			{
				return 11552;
			}
			if (message is SevenDayTaskRewardRequest)
			{
				return 11553;
			}
			if (message is SevenDayTaskRewardResponse)
			{
				return 11554;
			}
			if (message is SevenDayTaskActiveRewardRequest)
			{
				return 11555;
			}
			if (message is SevenDayTaskActiveRewardResponse)
			{
				return 11556;
			}
			if (message is SevenDayFreeRewardRequest)
			{
				return 11557;
			}
			if (message is SevenDayFreeRewardResponse)
			{
				return 11558;
			}
			if (message is ShopGetInfoRequest)
			{
				return 11201;
			}
			if (message is ShopGetInfoResponse)
			{
				return 11202;
			}
			if (message is ShopBuyItemRequest)
			{
				return 11203;
			}
			if (message is ShopBuyItemResponse)
			{
				return 11204;
			}
			if (message is ShopBuyIAPItemRequest)
			{
				return 11205;
			}
			if (message is ShopBuyIAPItemResponse)
			{
				return 11206;
			}
			if (message is ShopDoDrawRequest)
			{
				return 11207;
			}
			if (message is ShopDoDrawResponse)
			{
				return 11208;
			}
			if (message is ShopIntegralGetInfoRequest)
			{
				return 11209;
			}
			if (message is ShopIntegralGetInfoResponse)
			{
				return 11210;
			}
			if (message is ShopIntegralRefreshItemRequest)
			{
				return 11211;
			}
			if (message is ShopIntegralRefreshItemResponse)
			{
				return 11212;
			}
			if (message is ShopIntegralBuyItemRequest)
			{
				return 11213;
			}
			if (message is ShopIntegralBuyItemResponse)
			{
				return 11214;
			}
			if (message is ShopGacheWishRequest)
			{
				return 11215;
			}
			if (message is ShopGacheWishResponse)
			{
				return 11216;
			}
			if (message is ShopBuyTicketsRequest)
			{
				return 11217;
			}
			if (message is ShopBuyTicketsResponse)
			{
				return 11218;
			}
			if (message is ShopFreeIAPItemRequest)
			{
				return 11219;
			}
			if (message is ShopFreeIAPItemResponse)
			{
				return 11220;
			}
			if (message is DungeonAdGetItemRequest)
			{
				return 11221;
			}
			if (message is DungeonAdGetItemResponse)
			{
				return 11222;
			}
			if (message is TicketsGetListRequest)
			{
				return 11223;
			}
			if (message is TicketsGetListResponse)
			{
				return 11224;
			}
			if (message is FinishAdvertRequest)
			{
				return 11225;
			}
			if (message is FinishAdvertResponse)
			{
				return 11226;
			}
			if (message is IapPushRemoveRequest)
			{
				return 11227;
			}
			if (message is IapPushRemoveResponse)
			{
				return 11228;
			}
			if (message is GetIapPushDtoRequest)
			{
				return 11229;
			}
			if (message is GetIapPushDtoResponse)
			{
				return 11230;
			}
			if (message is HeadFrameActiveRequest)
			{
				return 11231;
			}
			if (message is HeadFrameActiveResponse)
			{
				return 11232;
			}
			if (message is GMVideoRequest)
			{
				return 11233;
			}
			if (message is GMVideoResponse)
			{
				return 11234;
			}
			if (message is GMVideoListRequest)
			{
				return 11235;
			}
			if (message is GMVideoListResponse)
			{
				return 11236;
			}
			if (message is StartAdvertRequest)
			{
				return 11237;
			}
			if (message is StartAdvertResponse)
			{
				return 11238;
			}
			if (message is BuyItemRewardRequest)
			{
				return 11239;
			}
			if (message is BuyItemRewardResponse)
			{
				return 11240;
			}
			if (message is SignInGetInfoRequest)
			{
				return 11501;
			}
			if (message is SignInGetInfoResponse)
			{
				return 11502;
			}
			if (message is SignInDoSignRequest)
			{
				return 11503;
			}
			if (message is SignInDoSignResponse)
			{
				return 11504;
			}
			if (message is InteractListRequest)
			{
				return 12701;
			}
			if (message is InteractListResponse)
			{
				return 12702;
			}
			if (message is SocialPowerRankRequest)
			{
				return 12703;
			}
			if (message is SocialPowerRankResponse)
			{
				return 12704;
			}
			if (message is InteractDetailRequest)
			{
				return 12705;
			}
			if (message is InteractDetailResponse)
			{
				return 12706;
			}
			if (message is SocketLoginRequest)
			{
				return 31101;
			}
			if (message is SocketLoginResponse)
			{
				return 31102;
			}
			if (message is SocketJoinGroupRequest)
			{
				return 31103;
			}
			if (message is SocketJoinGroupResponse)
			{
				return 31104;
			}
			if (message is SocketQuitGroupRequest)
			{
				return 31105;
			}
			if (message is SocketQuitGroupResponse)
			{
				return 31106;
			}
			if (message is SocketHeartBeatRequest)
			{
				return 31107;
			}
			if (message is SocketHeartBeatResponse)
			{
				return 31108;
			}
			if (message is SocketLoginRepeatMessage)
			{
				return 31202;
			}
			if (message is SocketReconnectMessage)
			{
				return 31204;
			}
			if (message is SocketPushMessage)
			{
				return 31206;
			}
			if (message is SocketErrorMessage)
			{
				return 31299;
			}
			if (message is TalentsLvUpRequest)
			{
				return 12415;
			}
			if (message is TalentsLvUpResponse)
			{
				return 12416;
			}
			if (message is TalentLegacyLeaderBoardRequest)
			{
				return 12417;
			}
			if (message is TalentLegacyLeaderBoardResponse)
			{
				return 12418;
			}
			if (message is TalentLegacyInfoRequest)
			{
				return 12419;
			}
			if (message is TalentLegacyInfoResponse)
			{
				return 12420;
			}
			if (message is TalentLegacyLevelUpRequest)
			{
				return 12421;
			}
			if (message is TalentLegacyLevelUpResponse)
			{
				return 12422;
			}
			if (message is TalentLegacySwitchRequest)
			{
				return 12423;
			}
			if (message is TalentLegacySwitchResponse)
			{
				return 12424;
			}
			if (message is TalentLegacyLevelUpCoolDownRequest)
			{
				return 12425;
			}
			if (message is TalentLegacyLevelUpCoolDownResponse)
			{
				return 12426;
			}
			if (message is TalentLegacySelectCareerRequest)
			{
				return 12427;
			}
			if (message is TalentLegacySelectCareerResponse)
			{
				return 12428;
			}
			if (message is TaskGetInfoRequest)
			{
				return 10501;
			}
			if (message is TaskGetInfoResponse)
			{
				return 10502;
			}
			if (message is TaskRewardDailyRequest)
			{
				return 10503;
			}
			if (message is TaskRewardDailyResponse)
			{
				return 10504;
			}
			if (message is TaskRewardAchieveRequest)
			{
				return 10505;
			}
			if (message is TaskRewardAchieveResponse)
			{
				return 10506;
			}
			if (message is TaskActiveRewardRequest)
			{
				return 10507;
			}
			if (message is TaskActiveRewardResponse)
			{
				return 10508;
			}
			if (message is TaskActiveRewardAllRequest)
			{
				return 10509;
			}
			if (message is TaskActiveRewardAllResponse)
			{
				return 10510;
			}
			if (message is TowerChallengeRequest)
			{
				return 13001;
			}
			if (message is TowerChallengeResponse)
			{
				return 13002;
			}
			if (message is TowerRewardRequest)
			{
				return 13003;
			}
			if (message is TowerRewardResponse)
			{
				return 13004;
			}
			if (message is TowerRankRequest)
			{
				return 13005;
			}
			if (message is TowerRankResponse)
			{
				return 13006;
			}
			if (message is TowerRankIndexRequest)
			{
				return 13007;
			}
			if (message is TowerRankIndexResponse)
			{
				return 13008;
			}
			if (message is TurnTableGetInfoRequest)
			{
				return 11701;
			}
			if (message is TurnTableGetInfoResponse)
			{
				return 11702;
			}
			if (message is TurnTableExtractRequest)
			{
				return 11703;
			}
			if (message is TurnTableExtractResponse)
			{
				return 11704;
			}
			if (message is TurnTableReceiveCumulativeRewardRequest)
			{
				return 11705;
			}
			if (message is TurnTableReceiveCumulativeRewardResponse)
			{
				return 11706;
			}
			if (message is TurnTableTaskReceiveRewardRequest)
			{
				return 11709;
			}
			if (message is TurnTableTaskReceiveRewardResponse)
			{
				return 11710;
			}
			if (message is TurnTableSelectBigGuaranteeItemRequest)
			{
				return 11711;
			}
			if (message is TurnTableSelectBigGuaranteeItemResponse)
			{
				return 11712;
			}
			if (message is TurnPayAdRequest)
			{
				return 11713;
			}
			if (message is TurnPayAdResponse)
			{
				return 11714;
			}
			if (message is UserLoginRequest)
			{
				return 10101;
			}
			if (message is UserLoginResponse)
			{
				return 10102;
			}
			if (message is UserGetIapInfoRequest)
			{
				return 10103;
			}
			if (message is UserGetIapInfoResponse)
			{
				return 10104;
			}
			if (message is UserHeartbeatRequest)
			{
				return 10105;
			}
			if (message is UserHeartbeatResponse)
			{
				return 10106;
			}
			if (message is UserUpdateSystemMaskRequest)
			{
				return 10107;
			}
			if (message is UserUpdateSystemMaskResponse)
			{
				return 10108;
			}
			if (message is UserUpdateGuideMaskRequest)
			{
				return 10109;
			}
			if (message is UserUpdateGuideMaskResponse)
			{
				return 10110;
			}
			if (message is UserCancelAccountRequest)
			{
				return 10111;
			}
			if (message is UserCancelAccountResponse)
			{
				return 10112;
			}
			if (message is UserUpdateInfoRequest)
			{
				return 10113;
			}
			if (message is UserUpdateInfoResponse)
			{
				return 10114;
			}
			if (message is UserGetPlayerInfoRequest)
			{
				return 10115;
			}
			if (message is UserGetPlayerInfoResponse)
			{
				return 10116;
			}
			if (message is UserGetOtherPlayerInfoRequest)
			{
				return 10117;
			}
			if (message is UserGetOtherPlayerInfoResponse)
			{
				return 10118;
			}
			if (message is UserGetCityInfoRequest)
			{
				return 10119;
			}
			if (message is UserGetCityInfoResponse)
			{
				return 10120;
			}
			if (message is UserHeartbeatSyncRequest)
			{
				return 10121;
			}
			if (message is UserHeartbeatSyncResponse)
			{
				return 10122;
			}
			if (message is UserGetBattleReportRequest)
			{
				return 10123;
			}
			if (message is UserGetBattleReportResponse)
			{
				return 10124;
			}
			if (message is UserOpenModelRequest)
			{
				return 10125;
			}
			if (message is UserOpenModelResponse)
			{
				return 10126;
			}
			if (message is UserRefDataRequest)
			{
				return 10127;
			}
			if (message is UserRefDataResponse)
			{
				return 10128;
			}
			if (message is UserSetShenFenRequest)
			{
				return 10131;
			}
			if (message is UserSetShenFenResponse)
			{
				return 10132;
			}
			if (message is AccountSignInRequest)
			{
				return 10133;
			}
			if (message is AccountSignInResponse)
			{
				return 10134;
			}
			if (message is AccountLoginRequest)
			{
				return 10135;
			}
			if (message is AccountLoginResponse)
			{
				return 10136;
			}
			if (message is UnlockAvatarRequest)
			{
				return 10137;
			}
			if (message is UnlockAvatarResponse)
			{
				return 10138;
			}
			if (message is UpdateUserAvatarRequest)
			{
				return 10139;
			}
			if (message is UpdateUserAvatarResponse)
			{
				return 10140;
			}
			if (message is UnlockUserAvatarRequest)
			{
				return 10141;
			}
			if (message is UnlockUserAvatarResponse)
			{
				return 10142;
			}
			if (message is UserGetAllPanelInfoRequest)
			{
				return 10143;
			}
			if (message is UserGetAllPanelInfoResponse)
			{
				return 10144;
			}
			if (message is UserHabbyMailBindRequest)
			{
				return 10145;
			}
			if (message is UserHabbyMailBindResponse)
			{
				return 10146;
			}
			if (message is UserHabbyMailRewardRequest)
			{
				return 10147;
			}
			if (message is UserHabbyMailRewardResponse)
			{
				return 10148;
			}
			if (message is ADGetRewardRequest)
			{
				return 10149;
			}
			if (message is ADGetRewardResponse)
			{
				return 10150;
			}
			if (message is WorldBossGetInfoRequest)
			{
				return 10415;
			}
			if (message is WorldBossGetInfoResponse)
			{
				return 10416;
			}
			if (message is StartWorldBossRequest)
			{
				return 10419;
			}
			if (message is StartWorldBossResponse)
			{
				return 10420;
			}
			if (message is EndWorldBossRequest)
			{
				return 10417;
			}
			if (message is EndWorldBossResponse)
			{
				return 10418;
			}
			if (message is WorldBossBoxRewardRequest)
			{
				return 10421;
			}
			if (message is WorldBossBoxRewardResponse)
			{
				return 10422;
			}
			return 0;
		}

		public static IMessage GetCacheMessage(ushort id)
		{
			if (PackageFactory.packageCacheDic.ContainsKey(id))
			{
				return PackageFactory.packageCacheDic[id];
			}
			IMessage message = PackageFactory.CreateMessage(id);
			if (message == null)
			{
				return null;
			}
			PackageFactory.packageCacheDic.Add(id, message);
			return message;
		}

		private static Dictionary<ushort, IMessage> packageCacheDic = new Dictionary<ushort, IMessage>();
	}
}

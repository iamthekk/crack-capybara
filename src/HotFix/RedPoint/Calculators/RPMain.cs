using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix.RedPoint.Calculators
{
	public class RPMain
	{
		public class Activity_Week : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				ActivityWeekDataModule dataModule = GameApp.Data.GetDataModule(DataName.ActivityWeekDataModule);
				if (dataModule == null)
				{
					return 0;
				}
				if (!dataModule.ShowAnyRed())
				{
					return 0;
				}
				return 1;
			}
		}

		public class Activity_Slot : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				ActivitySlotTrainDataModule dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
				if (dataModule == null)
				{
					return 0;
				}
				if (!dataModule.ShowAnyRed())
				{
					return 0;
				}
				return 1;
			}
		}

		public class Avatar : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				if (dataModule.IsAvatarOrFrameRedNode())
				{
					return 1;
				}
				if (dataModule.IsClothesRedNode())
				{
					return 1;
				}
				if (dataModule.IsSceneSkinRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Avatar : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (GameApp.Data.GetDataModule(DataName.LoginDataModule).IsAvatarOrFrameRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Avatar_Icon : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (GameApp.Data.GetDataModule(DataName.LoginDataModule).IsAvatarRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Avatar_Frame : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (GameApp.Data.GetDataModule(DataName.LoginDataModule).IsAvatarFrameRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Avatar_Title : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (GameApp.Data.GetDataModule(DataName.LoginDataModule).IsAvatarTitleRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Clotes : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (GameApp.Data.GetDataModule(DataName.LoginDataModule).IsClothesRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Clotes_Body : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (GameApp.Data.GetDataModule(DataName.LoginDataModule).IsClothesBodyRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Clotes_Head : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (GameApp.Data.GetDataModule(DataName.LoginDataModule).IsClothesHeadRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Clotes_Accessory : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (GameApp.Data.GetDataModule(DataName.LoginDataModule).IsClothesAccessoryRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Bind_HabbyId : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!GameApp.Data.GetDataModule(DataName.LoginDataModule).habbyMailBind)
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Scene : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (GameApp.Data.GetDataModule(DataName.LoginDataModule).IsSceneSkinRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Avatar_Notice : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return Singleton<NoticeManager>.Instance.OnRefreshRedCount();
			}
		}

		public class Bag : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class BlackMarket : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.BlackMarket, false))
				{
					return 0;
				}
				ShopDataModule dataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
				GameApp.Data.GetDataModule(DataName.IAPDataModule);
				List<IntegralShop_goods> shopItemsConfig = dataModule.GetShopItemsConfig(ShopType.BlackMarket, GoodsRefreshType.None);
				AdDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.AdDataModule);
				if (shopItemsConfig != null)
				{
					for (int i = 0; i < shopItemsConfig.Count; i++)
					{
						IntegralShop_goods integralShop_goods = shopItemsConfig[i];
						if (integralShop_goods.Price == 0 && !dataModule.GetIsSellOut(integralShop_goods))
						{
							return 1;
						}
					}
				}
				IList<Shop_ShopSell> allElements = GameApp.Table.GetManager().GetShop_ShopSellModelInstance().GetAllElements();
				for (int j = 0; j < allElements.Count; j++)
				{
					if (allElements[j].type == 1)
					{
						Shop_ShopSell shop_ShopSell = allElements[j];
						if (shop_ShopSell != null)
						{
							Shop_Shop elementById = GameApp.Table.GetManager().GetShop_ShopModelInstance().GetElementById(shop_ShopSell.id);
							if (elementById != null && elementById.adId > 0)
							{
								Shop_Ad elementById2 = GameApp.Table.GetManager().GetShop_AdModelInstance().GetElementById(elementById.adId);
								if (elementById2 != null)
								{
									int adTimes = elementById2.adTimes;
									if (dataModule2.GetWatchTimes(elementById.adId) < adTimes)
									{
										return 1;
									}
								}
							}
						}
					}
				}
				return 0;
			}
		}

		public class Box : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class Carnival : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_Carnival, false))
				{
					return 0;
				}
				if (GameApp.Data.GetDataModule(DataName.SevenDayCarnivalDataModule).IfCanShowRedDot())
				{
					return 1;
				}
				return 0;
			}
		}

		public class ChainPacks : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!GameApp.Data.GetDataModule(DataName.ChainPacksDataModule).ShowAnyRed(ChainPacksDataModule.ChainPacksType.All))
				{
					return 0;
				}
				return 1;
			}
		}

		public class ChapterBattlePass : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity, false))
				{
					return 0;
				}
				if (!GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule).IsRedPoint())
				{
					return 0;
				}
				return 1;
			}
		}

		public class ChapterReward : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (GameApp.Data.GetDataModule(DataName.ChapterDataModule).GetShowReward().state == ChapterRewardData.ChapterRewardState.CanGet)
				{
					return 1;
				}
				return 0;
			}
		}

		public class ChapterWheel : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule).IsRedPoint())
				{
					return 0;
				}
				return 1;
			}
		}

		public class Energy : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				TicketDailyExchangeDataModule dataModule = GameApp.Data.GetDataModule(DataName.TicketDailyExchangeDataModule);
				if (dataModule != null && dataModule.ShowAnyRed(UserTicketKind.UserLife))
				{
					return 1;
				}
				return 0;
			}
		}

		public class Gold : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class Guild : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class HangUpReward : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.HangUp, false))
				{
					return 0;
				}
				if (GameApp.Data.GetDataModule(DataName.HangUpDataModule).IsMaxReward())
				{
					return 1;
				}
				return 0;
			}
		}

		public class IAPPrivilegeCard
		{
			public class Card : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsHaveRedPointForMonth())
					{
						return 0;
					}
					return 1;
				}
			}

			public class CardFree : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsShowRedPoint(IAPMonthCardData.CardType.Free))
					{
						return 0;
					}
					return 1;
				}
			}

			public class CardNoAd : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsShowRedPoint(IAPMonthCardData.CardType.NoAd))
					{
						return 0;
					}
					return 1;
				}
			}

			public class CardMonth : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsShowRedPoint(IAPMonthCardData.CardType.Month))
					{
						return 0;
					}
					return 1;
				}
			}

			public class CardLifetime : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsShowRedPoint(IAPMonthCardData.CardType.Lifetime))
					{
						return 0;
					}
					return 1;
				}
			}

			public class CardAutoMining : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsShowRedPoint(IAPMonthCardData.CardType.AutoMining))
					{
						return 0;
					}
					return 1;
				}
			}
		}

		public class IAPRechargeGift
		{
			public class Fund : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
					if (dataModule.BattlePass != null && dataModule.BattlePass.IsHaveRedPoint())
					{
						return 1;
					}
					if (dataModule.LevelFund != null && dataModule.LevelFund.IsHaveRedPoint())
					{
						return 1;
					}
					return 0;
				}
			}

			public class BattlePass : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
					if (dataModule.BattlePass == null)
					{
						return 0;
					}
					if (!dataModule.BattlePass.IsHaveRedPoint())
					{
						return 0;
					}
					return 1;
				}
			}

			public class TalentFund : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					IAPLevelFundGroup currentFundGroup = GameApp.Data.GetDataModule(DataName.IAPDataModule).LevelFund.GetCurrentFundGroup(IAPLevelFundGroupKind.TalentLevel);
					if (currentFundGroup == null)
					{
						return 0;
					}
					if (currentFundGroup.GetCanCollectList().Count <= 0)
					{
						return 0;
					}
					return 1;
				}
			}

			public class TowerFund : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					IAPLevelFundGroup currentFundGroup = GameApp.Data.GetDataModule(DataName.IAPDataModule).LevelFund.GetCurrentFundGroup(IAPLevelFundGroupKind.TowerLevel);
					if (currentFundGroup == null)
					{
						return 0;
					}
					if (currentFundGroup.GetCanCollectList().Count <= 0)
					{
						return 0;
					}
					return 1;
				}
			}

			public class RogueDungeonFund : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					IAPLevelFundGroup currentFundGroup = GameApp.Data.GetDataModule(DataName.IAPDataModule).LevelFund.GetCurrentFundGroup(IAPLevelFundGroupKind.RogueDungeonFloor);
					if (currentFundGroup == null)
					{
						return 0;
					}
					if (currentFundGroup.GetCanCollectList().Count <= 0)
					{
						return 0;
					}
					return 1;
				}
			}
		}

		public class Mail : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Mail, false))
				{
					return 0;
				}
				record.UseClickToHideRedPoint = true;
				if (!GameApp.Data.GetDataModule(DataName.MailDataModule).GetIsCanShowRed())
				{
					return 0;
				}
				return 1;
			}
		}

		public class Mission : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Task, false))
				{
					return 0;
				}
				if (GameApp.Data.GetDataModule(DataName.TaskDataModule).GetIsCanReceiveForTaskTask())
				{
					return 1;
				}
				return 0;
			}
		}

		public class NewWorld : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity, false))
				{
					return 0;
				}
				if (!GameApp.Data.GetDataModule(DataName.NewWorldDataModule).IsRedPoint())
				{
					return 0;
				}
				return 1;
			}
		}

		public class Ranking : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class Setting : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class Shop : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class Sign : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_Sign, false))
				{
					return 0;
				}
				SignDataModule dataModule = GameApp.Data.GetDataModule(DataName.SignDataModule);
				if (dataModule.IsShouldRefresh() || dataModule.IsCanSignIn)
				{
					return 1;
				}
				return 0;
			}
		}

		public class Sociality : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class TVReward : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TVReward, false))
				{
					return 0;
				}
				if (GameApp.Data.GetDataModule(DataName.TVRewardDataModule).ShowAnyRed())
				{
					return 1;
				}
				return 0;
			}
		}

		public class MainShop
		{
			public class TabEquip : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Shop, false))
					{
						return 0;
					}
					IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
					AdDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.AdDataModule);
					PropDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.PropDataModule);
					List<int> list = new List<int>();
					list.Add(200001);
					list.Add(200002);
					List<IAPShopActivityData> shopActivities = dataModule.GetShopActivities(1);
					if (shopActivities == null || shopActivities.Count == 0)
					{
						list.Add(200003);
					}
					else
					{
						for (int i = 0; i < shopActivities.Count; i++)
						{
							list.Add(shopActivities[i].linkId);
						}
					}
					IAPShopActivityData shopSUpActivityData = dataModule.GetShopSUpActivityData();
					if (shopSUpActivityData != null)
					{
						if (dataModule.GetEquipSUpPoolLeftSelectTime() > 0)
						{
							return 1;
						}
						list.Add(shopSUpActivityData.linkId);
					}
					for (int j = 0; j < list.Count; j++)
					{
						Shop_EquipActivity elementById = GameApp.Table.GetManager().GetShop_EquipActivityModelInstance().GetElementById(list[j]);
						if (elementById != null)
						{
							Shop_Summon elementById2 = GameApp.Table.GetManager().GetShop_SummonModelInstance().GetElementById(elementById.id);
							if (elementById2 != null)
							{
								int freeTimes = elementById2.freeTimes;
								int freeCostTimes = dataModule.GetFreeCostTimes(elementById2.id);
								if (freeTimes > 0 && freeCostTimes < freeTimes)
								{
									return 1;
								}
								if (elementById2.adId > 0)
								{
									Shop_Ad elementById3 = GameApp.Table.GetManager().GetShop_AdModelInstance().GetElementById(elementById2.adId);
									if (elementById3 != null)
									{
										int adTimes = elementById3.adTimes;
										int watchTimes = dataModule2.GetWatchTimes(elementById2.adId);
										long watchCD = dataModule2.GetWatchCD(elementById2.adId);
										bool flag = dataModule2.CheckCloudDataAdOpen();
										if (watchTimes < adTimes && watchCD == 0L && flag)
										{
											return 1;
										}
									}
								}
								if (elementById2.priceId > 0)
								{
									int priceId = elementById2.priceId;
									long itemDataCountByid = dataModule3.GetItemDataCountByid((ulong)((long)priceId));
									if (itemDataCountByid >= (long)elementById2.tenPrice || itemDataCountByid >= (long)elementById2.singlePrice)
									{
										return 1;
									}
								}
							}
						}
					}
					return 0;
				}
			}

			public class TabBlackMarket : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					return 0;
				}
			}

			public class TabSuperValue : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Shop, false))
					{
						return 0;
					}
					IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
					List<PurchaseCommonData.PurchaseData> showPurchaseData = dataModule.SuperValuePack.GetShowPurchaseData();
					for (int i = 0; i < showPurchaseData.Count; i++)
					{
						if (dataModule.SuperValuePack.IsShowRedPoint(showPurchaseData[i].m_id))
						{
							return 1;
						}
					}
					return 0;
				}
			}

			public class TabGiftShop : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Shop, false))
					{
						return 0;
					}
					IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
					if (dataModule.TimePackData.IsHaveRedPointForDaily())
					{
						return 1;
					}
					if (dataModule.TimePackData.IsHaveRedPointForWeekly())
					{
						return 1;
					}
					if (dataModule.TimePackData.IsHaveRedPointForMonth())
					{
						return 1;
					}
					return 0;
				}
			}

			public class TabDiamondShop : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Shop, false))
					{
						return 0;
					}
					IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
					List<PurchaseCommonData.PurchaseData> showPurchaseData = dataModule.DiamondsPackData.GetShowPurchaseData();
					if (showPurchaseData != null)
					{
						for (int i = 0; i < showPurchaseData.Count; i++)
						{
							if (dataModule.DiamondsPackData.IsShowRedPoint(showPurchaseData[i].m_id))
							{
								return 1;
							}
						}
					}
					return 0;
				}
			}

			public class Diamonds_Diamonds : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Shop, false))
					{
						return 0;
					}
					if (!GameApp.Data.GetDataModule(DataName.IAPDataModule).DiamondsPackData.IsHaveRedPoint())
					{
						return 0;
					}
					return 1;
				}
			}

			public class Diamonds_Diamonds_VIP : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					return 0;
				}
			}

			public class Diamonds_Daily : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					return 0;
				}
			}

			public class Diamonds_Weekly : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					return 0;
				}
			}

			public class Diamonds_Month : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					return 0;
				}
			}

			public class Gift_First : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					return 0;
				}
			}

			public class Gift_OpenServer : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					return 0;
				}
			}

			public class Gift_Limited : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					return 0;
				}
			}

			public class Gift_Chatper_Node : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					return 0;
				}

				public int m_id;
			}

			public class Gift_Meeting : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
					if (dataModule.MeetingGift == null)
					{
						return 0;
					}
					if (!dataModule.MeetingGift.IsRedPoint())
					{
						return 0;
					}
					return 1;
				}
			}
		}
	}
}

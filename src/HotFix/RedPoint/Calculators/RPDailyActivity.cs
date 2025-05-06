using System;
using Framework;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix.RedPoint.Calculators
{
	public class RPDailyActivity
	{
		public class DailyActivity_Tower : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_ChallengeTower, false))
				{
					return 0;
				}
				if (GameApp.Data.GetDataModule(DataName.TowerDataModule).IsAllFinish)
				{
					return 0;
				}
				UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.Tower);
				if (ticket != null && ticket.NewNum > 0U)
				{
					return 1;
				}
				return 0;
			}
		}

		public class DailyActivity_WorldBoss : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
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

		public class DailyActivity_CrossArena : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity_CrossArena, false))
				{
					return 0;
				}
				UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.CrossArena);
				if (ticket != null && ticket.NewNum > 0U)
				{
					return 1;
				}
				return 0;
			}
		}

		public class DailyActivity_RogueDungeon : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.RogueDungeon, false))
				{
					return 0;
				}
				UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.RogueDungeon);
				if (ticket != null && ticket.NewNum > 0U)
				{
					return 1;
				}
				return 0;
			}
		}

		public class DailyActivity_DragonLair : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity_DragonsLair, false))
				{
					return 0;
				}
				UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.DragonLair);
				if (ticket != null && ticket.NewNum > 0U)
				{
					return 1;
				}
				Dungeon_DungeonBase elementById = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetElementById(1);
				if (elementById != null)
				{
					AdDataModule dataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
					int watchTimes = dataModule.GetWatchTimes(elementById.adId);
					int maxTimes = dataModule.GetMaxTimes(elementById.adId);
					if (watchTimes < maxTimes)
					{
						return 1;
					}
				}
				return 0;
			}
		}

		public class DailyActivity_AstralTree : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity_AstralTree, false))
				{
					return 0;
				}
				UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.AstralTree);
				if (ticket != null && ticket.NewNum > 0U)
				{
					return 1;
				}
				Dungeon_DungeonBase elementById = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetElementById(2);
				if (elementById != null)
				{
					AdDataModule dataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
					int watchTimes = dataModule.GetWatchTimes(elementById.adId);
					int maxTimes = dataModule.GetMaxTimes(elementById.adId);
					if (watchTimes < maxTimes)
					{
						return 1;
					}
				}
				return 0;
			}
		}

		public class DailyActivity_SwordIsland : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity_SwordIsland, false))
				{
					return 0;
				}
				UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.SwordIsland);
				if (ticket != null && ticket.NewNum > 0U)
				{
					return 1;
				}
				Dungeon_DungeonBase elementById = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetElementById(3);
				if (elementById != null)
				{
					AdDataModule dataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
					int watchTimes = dataModule.GetWatchTimes(elementById.adId);
					int maxTimes = dataModule.GetMaxTimes(elementById.adId);
					if (watchTimes < maxTimes)
					{
						return 1;
					}
				}
				return 0;
			}
		}

		public class DailyActivity_DeepSeaRuins : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!LoginDataModule.IsTestB())
				{
					return 0;
				}
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity_DeepSeaRuins, false))
				{
					return 0;
				}
				UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.DeepSeaRuins);
				if (ticket != null && ticket.NewNum > 0U)
				{
					return 1;
				}
				Dungeon_DungeonBase elementById = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetElementById(4);
				if (elementById != null)
				{
					AdDataModule dataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
					int watchTimes = dataModule.GetWatchTimes(elementById.adId);
					int maxTimes = dataModule.GetMaxTimes(elementById.adId);
					if (watchTimes < maxTimes)
					{
						return 1;
					}
				}
				return 0;
			}
		}

		public class Mining
		{
			public class Ore : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Mining, false))
					{
						return 0;
					}
					UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.Mining);
					if (ticket != null && ticket.NewNum > 0U)
					{
						return 1;
					}
					AdDataModule dataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
					int maxTimes = dataModule.GetMaxTimes(12);
					if (dataModule.GetWatchTimes(12) < maxTimes)
					{
						return 1;
					}
					MiningDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.MiningDataModule);
					if (dataModule2.IsNotGetReward())
					{
						return 1;
					}
					if (dataModule2.IsTreasureCanGet)
					{
						return 1;
					}
					return 0;
				}
			}

			public class Draw : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Mining, false))
					{
						return 0;
					}
					PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
					MiningDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.MiningDataModule);
					long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)GameConfig.Mining_Draw_ItemId));
					int mining_Draw_Cost = GameConfig.Mining_Draw_Cost;
					if ((dataModule2.MiningDraw != null && dataModule2.MiningDraw.FreeTimes > 0) || itemDataCountByid >= (long)mining_Draw_Cost)
					{
						return 1;
					}
					return 0;
				}
			}
		}
	}
}

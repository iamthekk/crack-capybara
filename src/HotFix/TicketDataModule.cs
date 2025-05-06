using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;
using Proto.User;
using Server;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public class TicketDataModule : IDataModule
	{
		public int GetName()
		{
			return 145;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(432, new HandlerEvent(this.OnEventDayChanged));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(432, new HandlerEvent(this.OnEventDayChanged));
		}

		public void Reset()
		{
		}

		private void InitRecoverTime()
		{
			this.ticketRecoverDataDic.Clear();
			this.ticketRecoverDataDic.Add(UserTicketKind.UserLife, new TicketDataModule.TicketRecoverData
			{
				ticketKind = UserTicketKind.UserLife,
				initRecoverTime = Singleton<GameConfig>.Instance.APRecoverInterval
			});
			this.ticketRecoverDataDic.Add(UserTicketKind.Tower, new TicketDataModule.TicketRecoverData
			{
				ticketKind = UserTicketKind.Tower,
				initRecoverTime = Singleton<GameConfig>.Instance.Tower_RecoverInterval
			});
			this.ticketRecoverDataDic.Add(UserTicketKind.Mining, new TicketDataModule.TicketRecoverData
			{
				ticketKind = UserTicketKind.Mining,
				initRecoverTime = GameConfig.Mining_Ticket_RecoverInterval
			});
		}

		public void CalcRealRecoverTime()
		{
			bool flag = false;
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			foreach (TicketDataModule.TicketRecoverData ticketRecoverData in this.ticketRecoverDataDic.Values)
			{
				UserTicketKind ticketKind = ticketRecoverData.ticketKind;
				if (ticketKind != UserTicketKind.UserLife)
				{
					if (ticketKind != UserTicketKind.Mining)
					{
						ticketRecoverData.realRecoverTime = ticketRecoverData.initRecoverTime;
					}
					else
					{
						float num = dataModule.MemberAttributeData.AxeRecoverRate.AsFloat();
						ticketRecoverData.realRecoverTime = (int)((float)ticketRecoverData.initRecoverTime / (1f + num)).GetValue();
					}
				}
				else
				{
					float num2 = dataModule.MemberAttributeData.StaminaRecoveryRate.AsFloat();
					ticketRecoverData.realRecoverTime = (int)((float)ticketRecoverData.initRecoverTime / (1f + num2)).GetValue();
				}
				UserTicket ticket = this.GetTicket(ticketRecoverData.ticketKind);
				if (ticket != null && ticket.NewNum < ticket.RevertLimit && ticket.TicketTimestamp > 0UL && ticket.TicketTimestamp + (ulong)((long)ticketRecoverData.realRecoverTime) <= (ulong)serverTimestamp)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.DoNormalTicketsGetListRequest();
				return;
			}
			this.RegisterRecoverTicket();
		}

		private void RefreshTableData()
		{
			this.ticketTableDataDic.Clear();
			foreach (IGrouping<int, TicketExchange_Exchange> grouping in from item in GameApp.Table.GetManager().GetTicketExchange_ExchangeModelInstance().GetAllElements()
				group item by item.type)
			{
				int num = 0;
				int num2 = 0;
				Dictionary<int, TicketExchange_Exchange> dictionary = new Dictionary<int, TicketExchange_Exchange>();
				foreach (TicketExchange_Exchange ticketExchange_Exchange in grouping)
				{
					if (ticketExchange_Exchange.count > num)
					{
						num = ticketExchange_Exchange.count;
					}
					dictionary[ticketExchange_Exchange.count] = ticketExchange_Exchange;
					num2 += ticketExchange_Exchange.unit;
				}
				TicketDataModule.TicketTableData ticketTableData = new TicketDataModule.TicketTableData((UserTicketKind)grouping.Key, num, num2, dictionary);
				this.ticketTableDataDic[ticketTableData.TicketType] = ticketTableData;
			}
		}

		public void SetLoginData(UserLoginResponse loginResponse)
		{
			this.RefreshTableData();
			this.InitRecoverTime();
			if (loginResponse == null)
			{
				return;
			}
			this.userTickets.Clear();
			this.userTickets.AddRange(loginResponse.UserTickets);
			this.CalcRealRecoverTime();
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Ticket);
		}

		public void UpdateUserTicket(UserTicket ticket)
		{
			if (ticket == null)
			{
				return;
			}
			UserTicketKind ticketType = (UserTicketKind)ticket.TicketType;
			UserTicket userTicket = null;
			UserTicket ticket2 = ticket;
			int num = this.userTickets.FindIndex((UserTicket item) => item.TicketType == ticket.TicketType);
			if (num < 0)
			{
				this.userTickets.Add(ticket);
			}
			else
			{
				userTicket = this.userTickets[num];
				this.userTickets[num] = ticket;
			}
			EventArgsTicketUpdate instance = Singleton<EventArgsTicketUpdate>.Instance;
			instance.SetData(ticketType, userTicket, ticket2);
			GameApp.Event.DispatchNow(null, 123, instance);
			if (ticketType == UserTicketKind.UserLife)
			{
				GameApp.Event.DispatchNow(null, 127, null);
			}
			RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.Tower", true);
			RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.WorldBoss", true);
			RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.CrossArena", true);
			RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.RogueDungeon", true);
			RedPointController.Instance.ReCalc("DailyActivity.DungeonTag.DragonLair", true);
			RedPointController.Instance.ReCalc("DailyActivity.DungeonTag.AstralTree", true);
			RedPointController.Instance.ReCalc("DailyActivity.DungeonTag.SwordIsland", true);
			RedPointController.Instance.ReCalc("DailyActivity.DungeonTag.DeepSeaRuins", true);
			RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.Mining", true);
			this.RegisterRecoverTicket();
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Ticket);
		}

		public UserTicket GetTicket(UserTicketKind kind)
		{
			foreach (UserTicket userTicket in this.userTickets)
			{
				if (userTicket.TicketType == (uint)kind)
				{
					return userTicket;
				}
			}
			return null;
		}

		public TicketDataModule.TicketTableData GetTicketTableData(UserTicketKind kind)
		{
			TicketDataModule.TicketTableData ticketTableData;
			this.ticketTableDataDic.TryGetValue(kind, out ticketTableData);
			return ticketTableData;
		}

		public int GetTicketCount(UserTicketKind kind)
		{
			UserTicket ticket = this.GetTicket(kind);
			if (ticket != null)
			{
				return (int)ticket.NewNum;
			}
			return 0;
		}

		public int GetCanBuyCount(UserTicketKind ticketType)
		{
			TicketDataModule.TicketTableData ticketTableData;
			if (!this.ticketTableDataDic.TryGetValue(ticketType, out ticketTableData))
			{
				return 0;
			}
			UserTicket ticket = this.GetTicket(ticketType);
			if (ticket == null)
			{
				return 0;
			}
			int num = ticketTableData.MaxBuyCount - (int)ticket.BuyTimes;
			return Mathf.Max(0, num);
		}

		public void GetTicketDataByBuyCount(UserTicketKind ticketType, int buyCount, bool ignoreCurBuyTimes, out int ticketPrice, out int ticketCount)
		{
			ticketCount = 0;
			ticketPrice = 0;
			TicketDataModule.TicketTableData ticketTableData;
			if (!this.ticketTableDataDic.TryGetValue(ticketType, out ticketTableData))
			{
				return;
			}
			int num = 0;
			if (!ignoreCurBuyTimes)
			{
				UserTicket ticket = this.GetTicket(ticketType);
				if (ticket == null)
				{
					return;
				}
				num = (int)ticket.BuyTimes;
			}
			for (int i = 0; i < buyCount; i++)
			{
				num++;
				if (num > ticketTableData.MaxBuyCount)
				{
					break;
				}
				TicketExchange_Exchange ticketExchange_Exchange = ticketTableData.TableDataDic[num];
				ticketCount += ticketExchange_Exchange.unit;
				ticketPrice += ticketExchange_Exchange.diamondCost;
			}
		}

		private void RegisterRecoverTicket()
		{
			DxxTools.UI.RemoveServerTimeClockCallback("TicketDataRefreshKey");
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			long num = 0L;
			foreach (TicketDataModule.TicketRecoverData ticketRecoverData in this.ticketRecoverDataDic.Values)
			{
				UserTicket ticket = this.GetTicket(ticketRecoverData.ticketKind);
				if (ticket != null && ticket.NewNum < ticket.RevertLimit)
				{
					long num2 = (long)(ticket.TicketTimestamp + (ulong)((long)ticketRecoverData.realRecoverTime));
					if (num == 0L)
					{
						num = num2;
					}
					if (num2 != 0L && num2 < num)
					{
						num = num2;
					}
				}
			}
			if (num > 0L && num > serverTimestamp)
			{
				DxxTools.UI.AddServerTimeCallback("TicketDataRefreshKey", new Action(this.DoNormalTicketsGetListRequest), num, 0);
			}
		}

		public void DoNormalTicketsGetListRequest()
		{
			this.DoTicketsGetListRequest(null);
		}

		public void DoDayChangedTicketsGetListRequest()
		{
			this.DoTicketsGetListRequest(delegate(bool isOk)
			{
				if (isOk)
				{
					GameApp.Data.GetDataModule(DataName.TicketDailyExchangeDataModule).OnDayChangeUpdated();
				}
			});
		}

		private void DoTicketsGetListRequest(Action<bool> callback = null)
		{
			NetworkUtils.Ticket.DoTicketsGetListRequest(delegate(bool isOk, TicketsGetListResponse resp)
			{
				if (isOk)
				{
					this.RegisterRecoverTicket();
				}
				Action<bool> callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2(isOk);
			});
		}

		public List<UserTicket> GetTickets()
		{
			return this.userTickets;
		}

		public TicketDataModule.TicketRecoverData GetTicketRecoverData(UserTicketKind kind)
		{
			TicketDataModule.TicketRecoverData ticketRecoverData;
			if (this.ticketRecoverDataDic.TryGetValue(kind, out ticketRecoverData))
			{
				return ticketRecoverData;
			}
			return null;
		}

		private void OnEventDayChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			this.DoDayChangedTicketsGetListRequest();
		}

		public long GetRecoverTimestamp(UserTicketKind kind)
		{
			UserTicket ticket = this.GetTicket(kind);
			TicketDataModule.TicketRecoverData ticketRecoverData;
			if (this.ticketRecoverDataDic.TryGetValue(kind, out ticketRecoverData) && ticket != null && ticket.NewNum < ticket.RevertLimit)
			{
				return (long)(ticket.TicketTimestamp + (ulong)((long)ticketRecoverData.realRecoverTime));
			}
			return 0L;
		}

		private readonly Dictionary<UserTicketKind, TicketDataModule.TicketTableData> ticketTableDataDic = new Dictionary<UserTicketKind, TicketDataModule.TicketTableData>();

		private Dictionary<UserTicketKind, TicketDataModule.TicketRecoverData> ticketRecoverDataDic = new Dictionary<UserTicketKind, TicketDataModule.TicketRecoverData>();

		private readonly List<UserTicket> userTickets = new List<UserTicket>();

		private const string TicketDataRefreshKey = "TicketDataRefreshKey";

		public struct TicketTableData
		{
			public readonly UserTicketKind TicketType { get; }

			public readonly int MaxBuyCount { get; }

			public readonly int MaxTicketCount { get; }

			public readonly Dictionary<int, TicketExchange_Exchange> TableDataDic { get; }

			public TicketTableData(UserTicketKind ticketType, int maxBuyCount, int maxTicketCount, Dictionary<int, TicketExchange_Exchange> tableDataDic)
			{
				this.TicketType = ticketType;
				this.MaxBuyCount = maxBuyCount;
				this.MaxTicketCount = maxTicketCount;
				this.TableDataDic = tableDataDic ?? new Dictionary<int, TicketExchange_Exchange>();
			}
		}

		public class TicketRecoverData
		{
			public UserTicketKind ticketKind;

			public int initRecoverTime;

			public int realRecoverTime;
		}
	}
}

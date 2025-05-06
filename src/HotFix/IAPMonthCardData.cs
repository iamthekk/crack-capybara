using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class IAPMonthCardData
	{
		public int HeadActiveState
		{
			get
			{
				return this._headActiveState;
			}
		}

		public IAPMonthCardData(PurchaseCommonData commonDataVal)
		{
			this.commonData = commonDataVal;
			this.data = new MapField<uint, IAPMonthCardDto>();
		}

		public void SetData(MapField<uint, IAPMonthCardDto> dataVal, bool isClear, int headActiveState)
		{
			this.data = dataVal;
			this._headActiveState = headActiveState;
			if (this.data == null)
			{
				HLog.LogError(string.Format("{0} {1} {2} is null", this, "SetData", "data"));
				this.data = new MapField<uint, IAPMonthCardDto>();
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Refresh_PrivilegeCard, null);
		}

		public void SetHeadState(int headActiveState)
		{
			this._headActiveState = headActiveState;
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.IAP);
		}

		public string GetNextTimeString(IAPMonthCardData.CardType cardType)
		{
			long nextTime = this.GetNextTime(cardType);
			if (nextTime < 0L)
			{
				return string.Empty;
			}
			return Utility.Math.GetTime3String(nextTime);
		}

		public long GetNextTime(IAPMonthCardData.CardType cardType)
		{
			long num = -1L;
			int tableID = this.GetTableID(cardType);
			IAPMonthCardDto iapmonthCardDto;
			this.data.TryGetValue((uint)tableID, ref iapmonthCardDto);
			if (iapmonthCardDto != null)
			{
				num = (long)(iapmonthCardDto.NextRewardTime - (ulong)DxxTools.Time.ServerTimestamp);
			}
			return num;
		}

		public int GetTableID(IAPMonthCardData.CardType cardType)
		{
			int num = 0;
			switch (cardType)
			{
			case IAPMonthCardData.CardType.NoAd:
				num = 301;
				break;
			case IAPMonthCardData.CardType.Month:
				num = 302;
				break;
			case IAPMonthCardData.CardType.Free:
				num = 303;
				break;
			case IAPMonthCardData.CardType.Lifetime:
				num = 304;
				break;
			case IAPMonthCardData.CardType.AutoMining:
				num = 305;
				break;
			}
			return num;
		}

		public bool IsActivation(IAPMonthCardData.CardType cardType)
		{
			int tableID = this.GetTableID(cardType);
			IAPMonthCardDto iapmonthCardDto;
			this.data.TryGetValue((uint)tableID, ref iapmonthCardDto);
			return iapmonthCardDto != null;
		}

		public bool IsCanReward(IAPMonthCardData.CardType cardType)
		{
			int tableID = this.GetTableID(cardType);
			IAPMonthCardDto iapmonthCardDto;
			this.data.TryGetValue((uint)tableID, ref iapmonthCardDto);
			return iapmonthCardDto != null && iapmonthCardDto.CanReward > 0U;
		}

		public int GetLastDay(IAPMonthCardData.CardType cardType)
		{
			int tableID = this.GetTableID(cardType);
			IAPMonthCardDto iapmonthCardDto;
			this.data.TryGetValue((uint)tableID, ref iapmonthCardDto);
			if (iapmonthCardDto == null)
			{
				return 0;
			}
			ulong num = iapmonthCardDto.EndTime - (ulong)DxxTools.Time.ServerTimestamp;
			if (num > 0UL)
			{
				return (int)(num / 86400UL);
			}
			return 0;
		}

		public bool IsShowRedPoint(IAPMonthCardData.CardType cardType)
		{
			if (!this.IsActivation(cardType))
			{
				return false;
			}
			bool flag = this.IsCanReward(cardType);
			return this.GetNextTime(cardType) <= 0L || flag;
		}

		public bool IsHaveRedPointForMonth()
		{
			return this.IsShowRedPoint(IAPMonthCardData.CardType.NoAd) || this.IsShowRedPoint(IAPMonthCardData.CardType.Month) || this.IsShowRedPoint(IAPMonthCardData.CardType.Lifetime) || this.IsShowRedPoint(IAPMonthCardData.CardType.Free) || this.IsShowRedPoint(IAPMonthCardData.CardType.AutoMining);
		}

		public bool IsAlwaysActive(IAPMonthCardData.CardType cardType)
		{
			int tableID = this.GetTableID(cardType);
			return GameApp.Table.GetManager().GetIAP_MonthCardModelInstance().GetElementById(tableID)
				.duration <= 0;
		}

		public bool IsActivePrivilege(CardPrivilegeKind kind)
		{
			if (this.data == null)
			{
				return false;
			}
			foreach (IAPMonthCardDto iapmonthCardDto in this.data.Values)
			{
				IAP_MonthCard elementById = GameApp.Table.GetManager().GetIAP_MonthCardModelInstance().GetElementById((int)iapmonthCardDto.ConfigId);
				if (elementById != null)
				{
					for (int i = 0; i < elementById.privilege.Length; i++)
					{
						List<string> listString = elementById.privilege[i].GetListString(',');
						int num;
						if (listString.Count > 0 && int.TryParse(listString[0], out num) && num == (int)kind)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private PurchaseCommonData commonData;

		private MapField<uint, IAPMonthCardDto> data;

		private int _headActiveState;

		public enum CardType
		{
			NoAd,
			Month,
			Free,
			Lifetime,
			AutoMining
		}
	}
}

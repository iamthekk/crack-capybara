using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Chapter;
using Proto.Common;

namespace HotFix
{
	public class HangUpDataModule : IDataModule
	{
		public HungUpInfoDto HungUpInfoDto { get; private set; }

		private ChapterDataModule chapterDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			}
		}

		public int GetName()
		{
			return 158;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_DayChange_HangUp_DataPull, new HandlerEvent(this.OnEventChangeDay));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_DayChange_HangUp_DataPull, new HandlerEvent(this.OnEventChangeDay));
		}

		public void Reset()
		{
		}

		public void SetLoginData(HungUpInfoDto hangUpInfoDto)
		{
			this.UpdateHangUpInfo(hangUpInfoDto);
		}

		public void UpdateHangUpInfo(HungUpInfoDto hangUpInfoDto)
		{
			if (hangUpInfoDto == null)
			{
				return;
			}
			this.HungUpInfoDto = hangUpInfoDto;
		}

		public long GetDropCurrency(CurrencyType type)
		{
			if (this.HungUpInfoDto == null)
			{
				return 0L;
			}
			long num = 0L;
			if (type != CurrencyType.Gold)
			{
				if (type != CurrencyType.Diamond)
				{
					return num;
				}
			}
			else
			{
				using (IEnumerator<HungUpDetailDto> enumerator = this.HungUpInfoDto.HungUpRewards.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						HungUpDetailDto hungUpDetailDto = enumerator.Current;
						num += (long)hungUpDetailDto.GoldDrop;
					}
					return num;
				}
			}
			foreach (HungUpDetailDto hungUpDetailDto2 in this.HungUpInfoDto.HungUpRewards.Values)
			{
				num += (long)hungUpDetailDto2.GemDrop;
			}
			return num;
		}

		public int GetTimeDropCurrency(CurrencyType type)
		{
			int num = 0;
			if (type != CurrencyType.Gold)
			{
				if (type == CurrencyType.Diamond)
				{
					num = this.chapterDataModule.CurrentChapter.Config.gemDrop;
				}
			}
			else
			{
				num = this.chapterDataModule.CurrentChapter.Config.goldDrop;
			}
			return num;
		}

		public List<PropData> GetDropRewards()
		{
			if (this.HungUpInfoDto == null)
			{
				return new List<PropData>();
			}
			List<ItemData> list = new List<ItemData>();
			foreach (HungUpDetailDto hungUpDetailDto in this.HungUpInfoDto.HungUpRewards.Values)
			{
				list.AddRange(hungUpDetailDto.Drops.ToItemDataList());
			}
			Dictionary<int, PropData> dictionary = new Dictionary<int, PropData>();
			for (int i = 0; i < list.Count; i++)
			{
				ItemData itemData = list[i];
				PropData propData;
				if (dictionary.TryGetValue(itemData.ID, out propData))
				{
					propData.count += (ulong)itemData.TotalCount;
				}
				else
				{
					dictionary.Add(itemData.ID, itemData.ToPropData());
				}
			}
			return dictionary.Values.ToList<PropData>();
		}

		public bool IsHaveReward()
		{
			return this.GetDropCurrency(CurrencyType.Gold) > 0L || this.GetDropCurrency(CurrencyType.Diamond) > 0L || this.GetDropRewards().Count > 0;
		}

		public bool IsMaxReward()
		{
			if (this.HungUpInfoDto != null)
			{
				long num = DxxTools.Time.ServerTimestamp - this.HungUpInfoDto.LastSettlementTime + (long)this.HungUpInfoDto.TotalSettleTime;
				int num2 = this.GetHangUpMaxTime() * 60;
				return num >= (long)num2;
			}
			return false;
		}

		private void OnEventChangeDay(object sender, int type, BaseEventArgs eventArgs)
		{
			NetworkUtils.Chapter.DoGetHangUpInfoRequest(delegate(bool result, GetHangUpInfoResponse response)
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Refresh_HangUp, null);
			});
		}

		public int GetHangUpMaxTime()
		{
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			return GameConfig.HangUp_Max_Time + dataModule.MemberAttributeData.HangTimeMax.AsInt();
		}
	}
}

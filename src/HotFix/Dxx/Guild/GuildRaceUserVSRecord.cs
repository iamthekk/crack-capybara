using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildRaceUserVSRecord
	{
		public bool IsSuperPosition
		{
			get
			{
				return this.Position > GuildRaceBattlePosition.Warrior;
			}
		}

		public bool IsEmptyRecord
		{
			get
			{
				return this.User1 == null || this.User1.IsEmptyUser || this.User2 == null || this.User2.IsEmptyUser || this.ResultList == null || this.ResultList.Count <= 0;
			}
		}

		public GuildRaceMember GetWinRaceUser()
		{
			int num = 0;
			long num2 = 0L;
			if (this.User1 != null && !this.User1.IsEmptyUser)
			{
				num2 = this.User1.UserData.UserID;
			}
			for (int i = 0; i < this.ResultList.Count; i++)
			{
				GuildRaceUserVSRecordResult guildRaceUserVSRecordResult = this.ResultList[i];
				if (guildRaceUserVSRecordResult != null && guildRaceUserVSRecordResult.WinUserID == num2)
				{
					num++;
				}
			}
			if (num * 2 >= this.ResultList.Count)
			{
				return this.User1;
			}
			return this.User2;
		}

		public static int SortByIndex(GuildRaceUserVSRecord x, GuildRaceUserVSRecord y)
		{
			return x.Index.CompareTo(y.Index);
		}

		public GuildRaceUserVSRecordResult GetResult(int index)
		{
			if (index < 0 || index >= this.ResultList.Count)
			{
				return null;
			}
			return this.ResultList[index];
		}

		public void CorrectResultList()
		{
			long num = ((this.User1 != null) ? this.User1.UserData.UserID : 0L);
			if (num == 0L)
			{
				return;
			}
			GuildRaceUserVSRecordResult guildRaceUserVSRecordResult = null;
			int i = 0;
			while (i < this.ResultList.Count)
			{
				if (this.ResultList[i].HomeUserID == num)
				{
					if (i != 0)
					{
						guildRaceUserVSRecordResult = this.ResultList[i];
						this.ResultList.RemoveAt(i);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (guildRaceUserVSRecordResult != null)
			{
				this.ResultList.Insert(0, guildRaceUserVSRecordResult);
			}
		}

		public GuildRaceMember User1;

		public GuildRaceMember User2;

		public GuildRaceBattlePosition Position;

		public int Index;

		public List<GuildRaceUserVSRecordResult> ResultList = new List<GuildRaceUserVSRecordResult>();
	}
}

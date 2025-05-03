using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildListGroup
	{
		public int CurrentCount
		{
			get
			{
				return this.mGuildList.Count;
			}
		}

		public int HistoryCount
		{
			get
			{
				return this.mHistoryGuildList.Count;
			}
		}

		private int GetIndexInHistory(GuildShareData data)
		{
			if (data == null)
			{
				return -1;
			}
			for (int i = 0; i < this.mHistoryGuildList.Count; i++)
			{
				if (this.mHistoryGuildList[i] != null && this.mHistoryGuildList[i].GuildID == data.GuildID)
				{
					return i;
				}
			}
			return -1;
		}

		public List<GuildShareData> GetGuildList()
		{
			return this.mGuildList;
		}

		public List<GuildShareData> GetHistoryGuildList()
		{
			return this.mHistoryGuildList;
		}

		public void AddRange(List<GuildShareData> newlist)
		{
			if (newlist == null)
			{
				this.IsAllServerData = true;
				return;
			}
			bool flag = false;
			this.mGuildList.Clear();
			for (int i = 0; i < newlist.Count; i++)
			{
				GuildShareData guildShareData = newlist[i];
				if (guildShareData != null)
				{
					GuildShareData guildShareData2;
					if (this.mGuildDic.TryGetValue(guildShareData.GuildID, out guildShareData2))
					{
						guildShareData2.CloneFrom(guildShareData);
						int indexInHistory = this.GetIndexInHistory(guildShareData);
						if (indexInHistory >= 0)
						{
							this.mHistoryGuildList.RemoveAt(indexInHistory);
						}
					}
					else
					{
						this.mGuildDic[guildShareData.GuildID] = guildShareData;
						flag = true;
					}
					this.mHistoryGuildList.Add(guildShareData);
					this.mGuildList.Add(guildShareData);
				}
			}
			if (!flag)
			{
				this.IsAllServerData = true;
			}
		}

		public List<string> GetGuildIDList()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.mHistoryGuildList.Count; i++)
			{
				list.Add(this.mHistoryGuildList[i].GuildID);
			}
			return list;
		}

		public void KeepHistoryCountInRange()
		{
			while (this.mHistoryGuildList.Count > this.HistoryIDListMaxCount)
			{
				GuildShareData guildShareData = this.mHistoryGuildList[0];
				this.mHistoryGuildList.RemoveAt(0);
				if (guildShareData != null)
				{
					this.mGuildDic.Remove(guildShareData.GuildID);
				}
			}
		}

		public List<ulong> GetGuildIDULongList(bool isExcludeApply)
		{
			List<ulong> list = new List<ulong>();
			for (int i = 0; i < this.mHistoryGuildList.Count; i++)
			{
				if (!isExcludeApply || !this.mHistoryGuildList[i].IsApply)
				{
					list.Add(this.mHistoryGuildList[i].GuildID_ULong);
				}
			}
			return list;
		}

		public void CheckMakeRandomListFromHistory(int count, bool clear, bool isExcludeApply)
		{
			if (clear)
			{
				this.mGuildList.Clear();
			}
			Random random = new Random((int)DateTime.UtcNow.Ticks);
			Dictionary<string, GuildShareData> dictionary = new Dictionary<string, GuildShareData>();
			for (int i = 0; i < this.mGuildList.Count; i++)
			{
				GuildShareData guildShareData = this.mGuildList[i];
				if (guildShareData != null)
				{
					dictionary[guildShareData.GuildID] = guildShareData;
				}
			}
			List<GuildShareData> list = new List<GuildShareData>();
			list.AddRange(this.mHistoryGuildList);
			int num = 0;
			while (num < count && list.Count != 0)
			{
				int num2 = random.Next(list.Count);
				GuildShareData guildShareData2 = list[num2];
				list.RemoveAt(num2);
				if (!dictionary.ContainsKey(guildShareData2.GuildID) && (!guildShareData2.IsApply || !isExcludeApply))
				{
					dictionary[guildShareData2.GuildID] = guildShareData2;
					this.mGuildList.Add(guildShareData2);
					if (this.mGuildList.Count >= count)
					{
						break;
					}
				}
				num++;
			}
		}

		public void Clear()
		{
			this.HistoryIDListMaxCount = 80;
			this.IsAllServerData = false;
			this.mGuildList.Clear();
			this.mGuildDic.Clear();
			this.mHistoryGuildList.Clear();
		}

		public int HistoryIDListMaxCount = 80;

		public bool IsAllServerData;

		private List<GuildShareData> mGuildList = new List<GuildShareData>();

		private List<GuildShareData> mHistoryGuildList = new List<GuildShareData>();

		private Dictionary<string, GuildShareData> mGuildDic = new Dictionary<string, GuildShareData>();
	}
}

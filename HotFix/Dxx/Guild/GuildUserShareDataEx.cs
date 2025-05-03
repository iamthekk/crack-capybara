using System;
using System.Collections.Generic;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildUserShareDataEx
	{
		public static string GetPositionLanguage(this GuildUserShareData data)
		{
			return GuildUserShareDataEx.GetPositionLanguageByPos((int)data.GuildPosition);
		}

		public static string GetPositionLanguageByPos(int position)
		{
			return GuildProxy.Language.GetInfoByID((position + 400010).ToString());
		}

		public static string GetNick(this GuildUserShareData data)
		{
			if (string.IsNullOrEmpty(data.ServerSetNick))
			{
				return GuildProxy.GameUser.GetPlayerDefaultNick(data.UserID);
			}
			return data.ServerSetNick;
		}

		public static GuildMemberInfoDto GetMemberInfo(this GuildUserShareData data)
		{
			GuildMemberInfoDto guildMemberInfoDto = data.OriginalData as GuildMemberInfoDto;
			if (guildMemberInfoDto != null)
			{
				return guildMemberInfoDto;
			}
			return null;
		}

		public static void CustomSort(this List<GuildUserShareData> list)
		{
			ulong num = (ulong)GuildProxy.Net.ServerTime();
			List<GuildUserShareData> list2 = new List<GuildUserShareData>();
			List<GuildUserShareData> list3 = new List<GuildUserShareData>();
			ulong num2 = (ulong)((long)GuildProxy.UI.OfflineSec());
			for (int i = 0; i < list.Count; i++)
			{
				if (num - list[i].LastOnlineTime > num2)
				{
					list3.Add(list[i]);
				}
				else
				{
					list2.Add(list[i]);
				}
			}
			list2.Sort(new Comparison<GuildUserShareData>(GuildUserShareDataEx.SortByPosition));
			list3.Sort(new Comparison<GuildUserShareData>(GuildUserShareDataEx.SortByPosition));
			list.Clear();
			list.AddRange(list2);
			list.AddRange(list3);
		}

		public static int SortByPosition(GuildUserShareData x, GuildUserShareData y)
		{
			int num = x.GuildPosition.CompareTo(y.GuildPosition);
			if (num == 0)
			{
				num = y.LastOnlineTime.CompareTo(x.LastOnlineTime);
			}
			if (num == 0)
			{
				num = y.UserID.CompareTo(x.UserID);
			}
			return num;
		}

		public static GuildUserShareData MakeEmptyUser()
		{
			return new GuildUserShareData
			{
				UserID = 0L
			};
		}
	}
}

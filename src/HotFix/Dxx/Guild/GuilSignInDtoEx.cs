using System;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuilSignInDtoEx
	{
		public static GuildSignData ToGuildSignData(this GuilSignInDto sign)
		{
			return new GuildSignData
			{
				SignCount = (int)sign.Count,
				MaxSignCount = (int)sign.Limit,
				SignCost = new GuildItemData
				{
					rowId = 0L,
					id = (int)sign.NeedItemId,
					count = (int)sign.NeedItemCount
				},
				Rewards = sign.Rewards.ToGuildItemList()
			};
		}
	}
}

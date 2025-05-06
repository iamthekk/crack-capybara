using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class GameEventDropData
	{
		public GameEventDropData(int id, int count)
		{
			this.id = id;
			this.count = count;
		}

		public static GameEventDropData ToItemData(RewardDto reward)
		{
			return new GameEventDropData((int)reward.ConfigId, (int)reward.Count);
		}

		public static List<GameEventDropData> ToItemDatas(RepeatedField<RewardDto> rewards)
		{
			List<GameEventDropData> list = new List<GameEventDropData>(rewards.Count);
			for (int i = 0; i < rewards.Count; i++)
			{
				RewardDto rewardDto = rewards[i];
				list.Add(GameEventDropData.ToItemData(rewardDto));
			}
			return list;
		}

		public int id;

		public int count;
	}
}

using System;
using System.Collections.Generic;
using Proto.Common;

namespace HotFix
{
	public class BattleChapterDropData
	{
		public int type { get; private set; }

		public int id { get; private set; }

		public long baseCount { get; private set; }

		public int rate { get; private set; }

		public ChapterDropSource source { get; private set; }

		public long finalCount
		{
			get
			{
				float addDropBase = Singleton<GameEventController>.Instance.GetAddDropBase(this.id, this.source);
				long num = ChapterDataModule.CalcDynamicDrop(this.id, this.baseCount, addDropBase, true);
				if (this.rate > 1)
				{
					num *= (long)this.rate;
				}
				return num;
			}
		}

		public BattleChapterDropData(int type, int id, long baseCount, int rate, ChapterDropSource source)
		{
			this.type = type;
			this.id = id;
			this.baseCount = baseCount;
			this.rate = rate;
			this.source = source;
		}

		public static List<RewardDto> ToServerData(List<BattleChapterDropData> list)
		{
			List<RewardDto> list2 = new List<RewardDto>();
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(new RewardDto
				{
					Type = (uint)list[i].type,
					ConfigId = (uint)((list[i].id == 4) ? 1 : list[i].id),
					Count = (ulong)list[i].finalCount
				});
			}
			return list2;
		}
	}
}

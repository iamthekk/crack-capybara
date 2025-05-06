using System;
using System.Collections.Generic;
using Server;

namespace HotFix
{
	public class SweepRecordData
	{
		public void DropDataToJson(List<BattleChapterDropData> list)
		{
			this.items = new string[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				this.items[i] = JsonManager.SerializeObject(list[i]);
			}
		}

		public List<BattleChapterDropData> GetDropList()
		{
			List<BattleChapterDropData> list = new List<BattleChapterDropData>();
			if (this.items != null)
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					BattleChapterDropData battleChapterDropData = JsonManager.ToObject<BattleChapterDropData>(this.items[i]);
					list.Add(battleChapterDropData);
				}
			}
			return list;
		}

		public int chapterId;

		public int rate;

		public int queueIndex;

		public int seed;

		public string[] items;

		public long SaveServerTime;
	}
}

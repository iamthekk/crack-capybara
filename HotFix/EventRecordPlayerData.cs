using System;
using System.Collections.Generic;
using System.Text;
using Server;

namespace HotFix
{
	public class EventRecordPlayerData
	{
		public List<int> GetSkillBuildIdList()
		{
			if (string.IsNullOrEmpty(this.skillIds))
			{
				return new List<int>();
			}
			return this.skillIds.GetListInt('|');
		}

		public List<GameEventItemData> GetEventItemList()
		{
			List<GameEventItemData> list = new List<GameEventItemData>();
			if (this.eventItems != null)
			{
				for (int i = 0; i < this.eventItems.Length; i++)
				{
					GameEventItemData gameEventItemData = JsonManager.ToObject<GameEventItemData>(this.eventItems[i]);
					if (gameEventItemData != null)
					{
						list.Add(gameEventItemData);
					}
				}
			}
			return list;
		}

		public List<BattleChapterDropData> GetBattleChapterDropList()
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

		public void SkillBuildToString(List<int> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.Append(list[i]);
				if (i != list.Count - 1)
				{
					stringBuilder.Append("|");
				}
			}
			this.skillIds = stringBuilder.ToString();
		}

		public void EventItemsToJson(List<GameEventItemData> list)
		{
			this.eventItems = new string[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				string text = JsonManager.SerializeObject(list[i]);
				this.eventItems[i] = text;
			}
		}

		public void DropDataToJson(List<BattleChapterDropData> list)
		{
			this.items = new string[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				this.items[i] = JsonManager.SerializeObject(list[i]);
			}
		}

		public int chapterId;

		public int globalSeed;

		public string battleKey;

		public int expLv;

		public int currentExp;

		public double currentHp;

		public double maxHpPercent;

		public double atkPercent;

		public double defPercent;

		public int queueIndex;

		public long playerCoin;

		public int playerDiamond;

		public int playerChips;

		public string skillIds;

		public string[] eventItems;

		public string[] items;

		public int BattleEndEffectActiveNum;

		public int ReviveCount;

		public int RefreshSkillCount;

		public int PlayBigBonusCount;

		public int PlayMinorBonusCount;

		public long SaveServerTime;
	}
}

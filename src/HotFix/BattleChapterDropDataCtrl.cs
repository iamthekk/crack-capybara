using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class BattleChapterDropDataCtrl
	{
		public void AddDropList(List<NodeItemParam> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				NodeItemParam nodeItemParam = list[i];
				this.AddDrop(nodeItemParam);
			}
		}

		private void AddDrop(NodeItemParam item)
		{
			if (item == null)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(item.itemId);
			int num = 3;
			if (elementById != null)
			{
				num = elementById.itemType;
			}
			BattleChapterDropData battleChapterDropData = new BattleChapterDropData(num, item.itemId, item.itemNum, item.rate, item.source);
			List<BattleChapterDropData> list;
			if (this.dropDic.TryGetValue(item.source, out list))
			{
				list.Add(battleChapterDropData);
				return;
			}
			this.dropDic.Add(item.source, new List<BattleChapterDropData> { battleChapterDropData });
		}

		public void AddDrop(BattleChapterDropData data)
		{
			if (data == null)
			{
				return;
			}
			List<BattleChapterDropData> list;
			if (this.dropDic.TryGetValue(data.source, out list))
			{
				list.Add(data);
				return;
			}
			this.dropDic.Add(data.source, new List<BattleChapterDropData> { data });
		}

		public List<BattleChapterDropData> GetDropDataList(ChapterDropSource dropSource)
		{
			List<BattleChapterDropData> list;
			if (this.dropDic.TryGetValue(dropSource, out list))
			{
				return list;
			}
			return new List<BattleChapterDropData>();
		}

		public List<BattleChapterDropData> GetDropDataListExclude(ChapterDropSource dropSource)
		{
			List<BattleChapterDropData> list = new List<BattleChapterDropData>();
			foreach (ChapterDropSource chapterDropSource in this.dropDic.Keys)
			{
				if (chapterDropSource != dropSource)
				{
					list.AddRange(this.dropDic[chapterDropSource]);
				}
			}
			return list;
		}

		public List<BattleChapterDropData> GetDropDataList()
		{
			List<BattleChapterDropData> list = new List<BattleChapterDropData>();
			foreach (List<BattleChapterDropData> list2 in this.dropDic.Values)
			{
				list.AddRange(list2);
			}
			return list;
		}

		public List<PropData> GetDropDataShowList()
		{
			Dictionary<int, PropData> dictionary = new Dictionary<int, PropData>();
			List<BattleChapterDropData> dropDataList = this.GetDropDataList();
			for (int i = 0; i < dropDataList.Count; i++)
			{
				BattleChapterDropData battleChapterDropData = dropDataList[i];
				int num = battleChapterDropData.id;
				ulong finalCount = (ulong)battleChapterDropData.finalCount;
				if (battleChapterDropData.id == 4)
				{
					num = 1;
				}
				PropData propData;
				if (dictionary.TryGetValue(num, out propData))
				{
					propData.count += finalCount;
				}
				else
				{
					dictionary.Add(num, new PropData
					{
						id = (uint)num,
						count = finalCount
					});
				}
			}
			return dictionary.Values.ToList<PropData>();
		}

		public void Clear()
		{
			this.dropDic.Clear();
		}

		private Dictionary<ChapterDropSource, List<BattleChapterDropData>> dropDic = new Dictionary<ChapterDropSource, List<BattleChapterDropData>>();
	}
}

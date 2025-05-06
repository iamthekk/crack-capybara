using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class NodeItemParam : NodeParamBase
	{
		public NodeItemType type { get; private set; }

		public int itemId { get; private set; }

		public long itemNum { get; private set; }

		public ChapterDropSource source { get; private set; }

		public int rate { get; private set; }

		public ItemType itemType { get; private set; }

		public override NodeKind GetNodeKind()
		{
			return NodeKind.Item;
		}

		public override double FinalCount
		{
			get
			{
				float addDropBase = Singleton<GameEventController>.Instance.GetAddDropBase(this.itemId, this.source);
				long num = ChapterDataModule.CalcDynamicDrop(this.itemId, this.itemNum, addDropBase, true);
				if (this.rate > 1)
				{
					num *= (long)this.rate;
				}
				return (double)num;
			}
		}

		public NodeItemParam(NodeItemType type, int itemId, long itemNum, ChapterDropSource source, int rate = 1)
		{
			this.type = type;
			this.itemId = itemId;
			this.itemNum = itemNum;
			this.source = source;
			this.rate = rate;
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemId);
			if (elementById != null)
			{
				this.itemType = (ItemType)elementById.itemType;
			}
		}

		public void SetRate(int rate)
		{
			this.rate = rate;
		}

		public void SetNum(int num)
		{
			this.itemNum = (long)num;
		}

		public NodeItemParam Clone()
		{
			return new NodeItemParam(this.type, this.itemId, this.itemNum, this.source, 1);
		}

		public ItemData ToItemData()
		{
			int num = this.itemId;
			if (this.itemId == 4)
			{
				num = 1;
			}
			return new ItemData(num, (long)this.FinalCount);
		}

		public static List<ItemTypeData> GetItemParamList(List<NodeItemParam> items)
		{
			List<ItemTypeData> list = new List<ItemTypeData>();
			foreach (NodeItemParam.MergerItem mergerItem in NodeItemParam.MergerSameItem(items).Values)
			{
				int id = mergerItem.id;
				if (mergerItem.count != 0L)
				{
					ItemTypeData itemTypeData = new ItemTypeData();
					int num = 0;
					string text = "";
					if (mergerItem.type == NodeItemType.ChapterEvent)
					{
						Chapter_eventItem elementById = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(id);
						if (elementById == null)
						{
							continue;
						}
						num = elementById.atlas;
						text = elementById.icon;
					}
					else if (mergerItem.type == NodeItemType.Item)
					{
						Item_Item elementById2 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(id);
						if (elementById2 == null)
						{
							continue;
						}
						num = elementById2.atlasID;
						text = elementById2.icon;
					}
					itemTypeData.atlas = num;
					itemTypeData.icon = text;
					itemTypeData.m_value = NodeItemParam.GetNodeItemRewardText(mergerItem.id, mergerItem.count, mergerItem.type);
					itemTypeData.m_tgaValue = NodeItemParam.GetNodeItemRewardText_TGA(mergerItem.id, mergerItem.count, mergerItem.type);
					list.Add(itemTypeData);
				}
			}
			return list;
		}

		public static string GetNodeItemRewardText(int itemId, long itemNum, NodeItemType type)
		{
			string text = "";
			string text2 = "";
			if (type == NodeItemType.ChapterEvent)
			{
				Chapter_eventItem elementById = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(itemId);
				text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.languageId);
			}
			else if (type == NodeItemType.Item)
			{
				Item_Item elementById2 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemId);
				text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.nameID);
				if (elementById2.itemType == 0)
				{
					text2 = string.Format("+{0}", itemNum);
				}
			}
			return Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_153", new object[] { "<color=#91EEF6>" + text + text2 + "</color>" });
		}

		public static string GetNodeItemRewardText(NodeItemParam nodeItemParam)
		{
			return NodeItemParam.GetNodeItemRewardText(nodeItemParam.itemId, (long)((int)nodeItemParam.FinalCount), nodeItemParam.type);
		}

		public static string GetNodeItemRewardText_TGA(int itemId, long itemNum, NodeItemType type)
		{
			string text = "";
			string text2 = "";
			if (type == NodeItemType.ChapterEvent)
			{
				Chapter_eventItem elementById = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(itemId);
				text = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById.languageId);
			}
			else if (type == NodeItemType.Item)
			{
				Item_Item elementById2 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemId);
				text = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById2.nameID);
				if (elementById2.itemType == 0)
				{
					text2 = string.Format("+{0}", itemNum);
				}
			}
			return Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_153", new object[] { text + text2 });
		}

		private static Dictionary<int, NodeItemParam.MergerItem> MergerSameItem(List<NodeItemParam> list)
		{
			Dictionary<int, NodeItemParam.MergerItem> dictionary = new Dictionary<int, NodeItemParam.MergerItem>();
			for (int i = 0; i < list.Count; i++)
			{
				int itemId = list[i].itemId;
				long num = (long)list[i].FinalCount;
				NodeItemType type = list[i].type;
				NodeItemParam.MergerItem mergerItem;
				if (dictionary.TryGetValue(itemId, out mergerItem))
				{
					mergerItem.count += num;
				}
				else
				{
					dictionary.Add(itemId, new NodeItemParam.MergerItem
					{
						id = itemId,
						count = num,
						type = type
					});
				}
			}
			return dictionary;
		}

		private class MergerItem
		{
			public int id;

			public long count;

			public NodeItemType type;
		}
	}
}

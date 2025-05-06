using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class GameEventItemFactory
	{
		public void Init()
		{
			this.eventItemDic.Clear();
		}

		public void AddEventItem(int id, int num, int stage)
		{
			GameEventItemData gameEventItemData;
			if (this.eventItemDic.TryGetValue(id, out gameEventItemData))
			{
				gameEventItemData.ResetStage(stage);
				if (gameEventItemData.isOverlay)
				{
					gameEventItemData.AddNum(num);
				}
			}
			else
			{
				Chapter_eventItem elementById = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(id);
				if (elementById == null)
				{
					HLog.LogError(string.Format("Table [chapter_eventItem] not found id={0}", id));
					return;
				}
				EventItemType function = (EventItemType)elementById.function;
				int num2 = 1;
				if (elementById.isOverlay > 0)
				{
					num2 = num;
				}
				if (function == EventItemType.Sick || function == EventItemType.Weather)
				{
					num2 = 1;
					List<int> list = new List<int>();
					foreach (int num3 in this.eventItemDic.Keys)
					{
						if (this.eventItemDic[num3].itemType == function)
						{
							list.Add(num3);
						}
					}
					for (int i = 0; i < list.Count; i++)
					{
						this.eventItemDic.Remove(list[i]);
					}
				}
				gameEventItemData = new GameEventItemData(id, stage, num2);
				this.eventItemDic.Add(id, gameEventItemData);
				if (gameEventItemData.isShowUI)
				{
					EventArgGetEventItemFly eventArgGetEventItemFly = new EventArgGetEventItemFly();
					eventArgGetEventItemFly.SetData(gameEventItemData);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_GetEventItem, eventArgGetEventItemFly);
				}
				else
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshEventTaskItems, null);
				}
			}
			this.DoGetFunction(gameEventItemData);
		}

		public void CheckEventItemRemove(int currentStage)
		{
			List<int> list = new List<int>();
			foreach (GameEventItemData gameEventItemData in this.eventItemDic.Values)
			{
				if (currentStage >= gameEventItemData.endStage)
				{
					this.DoRemoveFunction(gameEventItemData);
					list.Add(gameEventItemData.id);
				}
			}
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				int num = list[i];
				if (this.eventItemDic.ContainsKey(num))
				{
					flag = true;
					this.eventItemDic.Remove(num);
				}
			}
			if (flag)
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshEventTaskItems, null);
			}
		}

		private void RemoveItem(int id)
		{
			if (this.eventItemDic.ContainsKey(id))
			{
				this.eventItemDic.Remove(id);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshEventTaskItems, null);
			}
		}

		public bool IsItemsActiveEvent(int[] items)
		{
			foreach (int num in items)
			{
				if (!this.eventItemDic.ContainsKey(num))
				{
					return false;
				}
			}
			return true;
		}

		private void DoGetFunction(GameEventItemData item)
		{
			if (item == null)
			{
				return;
			}
			switch (item.itemType)
			{
			case EventItemType.Weather:
			{
				EventArgWeather instance = Singleton<EventArgWeather>.Instance;
				instance.SetData(item);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_Weather, instance);
				return;
			}
			case EventItemType.Sick:
			{
				EventArgSick instance2 = Singleton<EventArgSick>.Instance;
				instance2.SetData(item);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_Sick, instance2);
				return;
			}
			case EventItemType.StoryItem:
			case EventItemType.FishRod:
				break;
			case EventItemType.Bait:
			{
				GameEventFishingFactory fishingFactory = Singleton<GameEventController>.Instance.GetFishingFactory();
				if (fishingFactory != null)
				{
					fishingFactory.AddBait(item.itemNum);
				}
				this.RemoveItem(item.id);
				break;
			}
			default:
				return;
			}
		}

		private void DoRemoveFunction(GameEventItemData item)
		{
			if (item == null)
			{
				return;
			}
			EventItemType itemType = item.itemType;
			int num = itemType - EventItemType.Weather;
		}

		public List<GameEventItemData> GetShowItemDatas()
		{
			List<GameEventItemData> list = new List<GameEventItemData>();
			foreach (GameEventItemData gameEventItemData in this.eventItemDic.Values)
			{
				if (gameEventItemData.isShowUI)
				{
					list.Add(gameEventItemData);
				}
			}
			return list;
		}

		public bool IsEventItemBuyEnabled(int itemId, int num)
		{
			GameEventItemData gameEventItemData;
			return this.eventItemDic.TryGetValue(itemId, out gameEventItemData) && gameEventItemData.itemNum >= num;
		}

		public void EventItemBuy(int itemId, int num)
		{
			GameEventItemData gameEventItemData;
			if (this.eventItemDic.TryGetValue(itemId, out gameEventItemData) && gameEventItemData.itemNum >= num)
			{
				gameEventItemData.AddNum(-num);
				if (gameEventItemData.itemNum <= 0)
				{
					this.RemoveItem(itemId);
				}
			}
		}

		public bool IsHaveEventItem(int itemId)
		{
			return this.eventItemDic.ContainsKey(itemId);
		}

		public int GetLostFood()
		{
			int num = 0;
			foreach (GameEventItemData gameEventItemData in this.eventItemDic.Values)
			{
				num += gameEventItemData.costFood;
			}
			return num;
		}

		public List<GameEventItemData> GetItemsByType(EventItemType type)
		{
			List<GameEventItemData> list = new List<GameEventItemData>();
			foreach (GameEventItemData gameEventItemData in this.eventItemDic.Values)
			{
				if (gameEventItemData.itemType == type)
				{
					list.Add(gameEventItemData);
				}
			}
			return list;
		}

		public List<GameEventItemData> GetItems()
		{
			List<GameEventItemData> list = new List<GameEventItemData>();
			foreach (GameEventItemData gameEventItemData in this.eventItemDic.Values)
			{
				list.Add(gameEventItemData);
			}
			return list;
		}

		public void AddRecordItems(List<GameEventItemData> record)
		{
			if (record == null)
			{
				return;
			}
			for (int i = 0; i < record.Count; i++)
			{
				this.eventItemDic.Add(record[i].id, record[i]);
				this.DoGetFunction(record[i]);
			}
		}

		private Dictionary<int, GameEventItemData> eventItemDic = new Dictionary<int, GameEventItemData>();
	}
}

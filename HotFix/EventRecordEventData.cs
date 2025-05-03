using System;
using System.Collections.Generic;
using Server;

namespace HotFix
{
	public class EventRecordEventData
	{
		public void ToRecordData(GameEventRandomData data)
		{
			this.randomDataJson = JsonManager.SerializeObject(data);
			this.dropJsonArr = new string[data.serverDrops.Count];
			for (int i = 0; i < data.serverDrops.Count; i++)
			{
				this.dropJsonArr[i] = JsonManager.SerializeObject(data.serverDrops[i]);
			}
			this.monsterDropJsonArr = new string[data.monsterDrops.Count];
			for (int j = 0; j < data.monsterDrops.Count; j++)
			{
				this.monsterDropJsonArr[j] = JsonManager.SerializeObject(data.monsterDrops[j]);
			}
			this.battleDropJsonArr = new string[data.battleDrops.Count];
			for (int k = 0; k < data.battleDrops.Count; k++)
			{
				this.battleDropJsonArr[k] = JsonManager.SerializeObject(data.battleDrops[k]);
			}
		}

		public GameEventRandomData ToEventData()
		{
			GameEventRandomData gameEventRandomData = JsonManager.ToObject<GameEventRandomData>(this.randomDataJson);
			List<GameEventDropData> list = new List<GameEventDropData>();
			if (this.dropJsonArr != null)
			{
				for (int i = 0; i < this.dropJsonArr.Length; i++)
				{
					GameEventDropData gameEventDropData = JsonManager.ToObject<GameEventDropData>(this.dropJsonArr[i]);
					list.Add(gameEventDropData);
				}
			}
			gameEventRandomData.SetDrop(list);
			List<GameEventDropData> list2 = new List<GameEventDropData>();
			if (this.monsterDropJsonArr != null)
			{
				for (int j = 0; j < this.monsterDropJsonArr.Length; j++)
				{
					GameEventDropData gameEventDropData2 = JsonManager.ToObject<GameEventDropData>(this.monsterDropJsonArr[j]);
					list2.Add(gameEventDropData2);
				}
				gameEventRandomData.SetMonsterDrop(list2);
			}
			List<GameEventDropData> list3 = new List<GameEventDropData>();
			if (this.battleDropJsonArr != null)
			{
				for (int k = 0; k < this.battleDropJsonArr.Length; k++)
				{
					GameEventDropData gameEventDropData3 = JsonManager.ToObject<GameEventDropData>(this.battleDropJsonArr[k]);
					list3.Add(gameEventDropData3);
				}
				gameEventRandomData.SetBattleDrop(list3);
			}
			return gameEventRandomData;
		}

		public string randomDataJson;

		public string[] dropJsonArr;

		public string[] monsterDropJsonArr;

		public string[] battleDropJsonArr;
	}
}

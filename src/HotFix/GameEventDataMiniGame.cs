using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventDataMiniGame : GameEventData
	{
		public GameEventDataMiniGame(GameEventPoolData poolData)
		{
			this.poolData = poolData;
			Chapter_eventRes elementById = GameApp.Table.GetManager().GetChapter_eventResModelInstance().GetElementById(poolData.tableId);
			if (elementById != null && elementById.drop != null && elementById.drop.Length >= 2)
			{
				int num;
				if (int.TryParse(elementById.drop[0], out num))
				{
					switch (num)
					{
					case 2:
						this.miniGameType = MiniGameType.MiniSlot;
						this.param = this.RandomMiniGameId(elementById.drop[1]);
						return;
					case 3:
						this.miniGameType = MiniGameType.CardFlipping;
						this.param = this.RandomMiniGameId(elementById.drop[1]);
						return;
					case 4:
						if (int.TryParse(elementById.drop[1], out this.param))
						{
							this.miniGameType = MiniGameType.Turntable;
							return;
						}
						break;
					case 5:
						if (int.TryParse(elementById.drop[1], out this.param))
						{
							this.miniGameType = MiniGameType.PaySlot;
							return;
						}
						break;
					default:
						return;
					}
				}
			}
			else
			{
				HLog.LogError(string.Format("Chapter_eventRes表未找到ID{0}，或者drop字段配置错误", poolData.tableId));
			}
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.MiniGame;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Normal;
		}

		public override string GetInfo()
		{
			return "MiniGame";
		}

		public override GameEventData GetNext(int index)
		{
			if (this.children.Count > 0 && index < this.children.Count)
			{
				GameEventData gameEventData = this.children[index];
				while (gameEventData != null && gameEventData.GetNodeOptionType() == GameEventNodeOptionType.Option)
				{
					gameEventData = gameEventData.GetNext(0);
				}
				return gameEventData;
			}
			return null;
		}

		public int RandomMiniGameId(string stringParam)
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<string> listString = stringParam.GetListString(',');
			for (int i = 0; i < listString.Count; i++)
			{
				string[] array = listString[i].Split('&', StringSplitOptions.None);
				int num;
				int num2;
				if (array.Length >= 2 && int.TryParse(array[0], out num) && int.TryParse(array[1], out num2))
				{
					list.Add(num);
					list2.Add(num2);
				}
			}
			int num3 = 0;
			if (list2.Count > 0)
			{
				XRandom xrandom = new XRandom(this.poolData.randomSeed);
				int weightedRandomSelection = RandUtils.GetWeightedRandomSelection(list2, xrandom);
				if (weightedRandomSelection < list.Count)
				{
					num3 = list[weightedRandomSelection];
				}
			}
			else
			{
				num3 = int.Parse(stringParam);
			}
			return num3;
		}

		public MiniGameType miniGameType;

		public int param;
	}
}

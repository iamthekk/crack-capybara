using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventMonsterFactory
	{
		public void Init(int randomSeed)
		{
			this.xRandom = new XRandom(randomSeed);
			List<int> monsterGroupList = GameApp.Data.GetDataModule(DataName.ChapterDataModule).CurrentChapter.GetMonsterGroupList();
			this.CreateMonsterPool(monsterGroupList);
		}

		private void CreateMonsterPool(List<int> groupList)
		{
			this.bossPoolIdList.Clear();
			this.initBossPoolIdList.Clear();
			this.eventDic.Clear();
			IList<MonsterCfg_monsterCfg> allElements = GameApp.Table.GetManager().GetMonsterCfg_monsterCfgModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				MonsterCfg_monsterCfg monsterCfg_monsterCfg = allElements[i];
				if (groupList.Contains(monsterCfg_monsterCfg.group))
				{
					GameEventBattleType battleType = (GameEventBattleType)monsterCfg_monsterCfg.battleType;
					if (battleType == GameEventBattleType.Boss)
					{
						this.initBossPoolIdList.Add(monsterCfg_monsterCfg.id);
					}
					else
					{
						int difficult = monsterCfg_monsterCfg.difficult;
						Dictionary<GameEventBattleType, DifferentRandom> dictionary;
						if (!this.eventDic.TryGetValue(difficult, out dictionary))
						{
							dictionary = new Dictionary<GameEventBattleType, DifferentRandom>();
							this.eventDic.Add(difficult, dictionary);
						}
						DifferentRandom differentRandom = null;
						if (!dictionary.TryGetValue(battleType, out differentRandom))
						{
							differentRandom = new DifferentRandom(this.xRandom.NextInt());
							dictionary.Add(battleType, differentRandom);
						}
						differentRandom.Add(monsterCfg_monsterCfg.id);
					}
				}
			}
			for (int j = 0; j < this.initBossPoolIdList.Count - 1; j++)
			{
				this.bossPoolIdList.Add(this.initBossPoolIdList[j]);
			}
			this.bossPoolIdList.RandomSort<int>();
			if (this.initBossPoolIdList.Count > 0)
			{
				this.bossPoolIdList.Add(this.initBossPoolIdList[this.initBossPoolIdList.Count - 1]);
				return;
			}
			HLog.LogError("未配置boss数据!");
		}

		public int GetBossPoolId()
		{
			if (this.bossPoolIdList.Count == 0)
			{
				this.bossPoolIdList.AddRange(this.initBossPoolIdList);
				this.bossPoolIdList.RandomSort<int>();
			}
			int num = this.bossPoolIdList[0];
			this.bossPoolIdList.RemoveAt(0);
			return num;
		}

		public int GetBattlePoolId(int difficult, GameEventBattleType battleType)
		{
			if (!this.eventDic.ContainsKey(difficult))
			{
				HLog.LogError(string.Format("GameEventMonsterFactory.GetBattlePoolId: difficult={0} not found", difficult));
				return 0;
			}
			Dictionary<GameEventBattleType, DifferentRandom> dictionary = this.eventDic[difficult];
			if (!dictionary.ContainsKey(battleType))
			{
				HLog.LogError(string.Format("GameEventMonsterFactory.GetBattlePoolId: difficult={0} battleType={1} not found", difficult, battleType));
				return 0;
			}
			return dictionary[battleType].GetRandom();
		}

		public void ReCreateMonsterPool(List<int> groupList)
		{
			this.CreateMonsterPool(groupList);
		}

		public int RandomPoolIdByGroup(int group, int difficult, int seed)
		{
			List<MonsterCfg_monsterCfg> list = new List<MonsterCfg_monsterCfg>();
			IList<MonsterCfg_monsterCfg> allElements = GameApp.Table.GetManager().GetMonsterCfg_monsterCfgModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				MonsterCfg_monsterCfg monsterCfg_monsterCfg = allElements[i];
				if (monsterCfg_monsterCfg.group == group && monsterCfg_monsterCfg.difficult == difficult)
				{
					list.Add(monsterCfg_monsterCfg);
				}
			}
			if (list.Count > 0)
			{
				int num = new XRandom(seed).Range(0, list.Count);
				return list[num].id;
			}
			return 0;
		}

		private List<int> bossPoolIdList = new List<int>();

		private List<int> initBossPoolIdList = new List<int>();

		private Dictionary<int, Dictionary<GameEventBattleType, DifferentRandom>> eventDic = new Dictionary<int, Dictionary<GameEventBattleType, DifferentRandom>>();

		private XRandom xRandom;
	}
}

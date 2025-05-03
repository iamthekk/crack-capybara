using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class GameEventRandomData
	{
		public int id { get; private set; }

		public GameEventType eventType { get; private set; }

		public int stage { get; private set; }

		public int poolId { get; private set; }

		public int randomSeed { get; private set; }

		public int serverRate { get; private set; }

		public ulong[] actRowIdArr { get; private set; }

		public List<GameEventDropData> serverDrops { get; private set; }

		public List<GameEventDropData> monsterDrops { get; private set; }

		public List<GameEventDropData> battleDrops { get; private set; }

		public int queueIndex { get; private set; }

		public bool isAddEvent { get; private set; }

		public GameEventRandomData(int id, int stage, int poolId, int randomSeed, int serverRate, ulong[] actRowIdArr, List<GameEventDropData> drops, List<GameEventDropData> monsterDrops, List<GameEventDropData> battleDrops)
		{
			this.id = id;
			this.stage = stage;
			this.poolId = poolId;
			this.randomSeed = randomSeed;
			this.serverRate = serverRate;
			this.actRowIdArr = actRowIdArr;
			this.serverDrops = drops;
			this.monsterDrops = monsterDrops;
			this.battleDrops = battleDrops;
			Chapter_eventType elementById = GameApp.Table.GetManager().GetChapter_eventTypeModelInstance().GetElementById(id);
			if (elementById != null)
			{
				this.eventType = (GameEventType)elementById.gameEventType;
			}
		}

		public void SetPoolId(int newPoolId)
		{
			this.poolId = newPoolId;
		}

		public void SetId(int newId)
		{
			this.id = newId;
		}

		public void SetDrop(List<GameEventDropData> drops)
		{
			this.serverDrops = drops;
		}

		public void SetAddActivity()
		{
			this.isAddEvent = true;
		}

		public void SetIndex(int realIndex)
		{
			this.queueIndex = realIndex;
		}

		public void SetMonsterDrop(List<GameEventDropData> drops)
		{
			this.monsterDrops = drops;
		}

		public void SetBattleDrop(List<GameEventDropData> drops)
		{
			this.battleDrops = drops;
		}
	}
}

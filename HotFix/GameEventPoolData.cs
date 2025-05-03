using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class GameEventPoolData
	{
		public int tableId { get; private set; }

		public string path { get; private set; }

		public int stage { get; private set; }

		public GameEventType eventType { get; private set; }

		public EventSizeType eventSizeType { get; private set; }

		public int randomSeed { get; private set; }

		public int serverRate { get; private set; }

		public int atkUpgrade { get; private set; }

		public int hpUpgrade { get; private set; }

		public int difficult { get; private set; }

		public int monsterCfgId { get; private set; }

		public bool IsActivity
		{
			get
			{
				return this.actRowIdArr.Length != 0;
			}
		}

		public ulong[] actRowIdArr { get; private set; }

		public List<GameEventDropData> serverDrops { get; private set; }

		public List<GameEventDropData> monsterDrops { get; private set; }

		public List<GameEventDropData> battleDrops { get; private set; }

		public int queueRealIndex { get; private set; }

		private ChapterDataModule chapterDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			}
		}

		public GameEventPoolData(int tableId, int stage, GameEventType type, int difficult, int seed, int rate, List<GameEventDropData> drops, List<GameEventDropData> monsterDrops, List<GameEventDropData> battleDrops)
		{
			this.tableId = tableId;
			this.stage = stage;
			this.eventType = type;
			this.difficult = difficult;
			this.randomSeed = seed;
			this.serverRate = ((rate <= 1) ? 1 : rate);
			this.serverDrops = drops;
			this.monsterDrops = monsterDrops;
			this.battleDrops = battleDrops;
			this.Init();
		}

		private void Init()
		{
			Chapter_eventRes elementById = GameApp.Table.GetManager().GetChapter_eventResModelInstance().GetElementById(this.tableId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("GameEventPoolData.Init: not found eventRes id={0}", this.tableId));
				return;
			}
			this.path = elementById.path;
			this.eventSizeType = (EventSizeType)elementById.type;
			int num;
			int num2;
			this.chapterDataModule.CalcAttributesUpgrade(this.stage, this.eventType, out num, out num2);
			this.atkUpgrade = num;
			this.hpUpgrade = num2;
			if (this.difficult > 0)
			{
				this.monsterCfgId = Singleton<GameEventController>.Instance.GetMonsterPoolId(this.difficult, this.eventType);
			}
		}

		public void SetData(int realIndex, ulong[] actIdArr)
		{
			this.queueRealIndex = realIndex;
			this.actRowIdArr = actIdArr;
		}
	}
}

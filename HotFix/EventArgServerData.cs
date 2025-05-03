using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Chapter;

namespace HotFix
{
	public class EventArgServerData : BaseEventArgs
	{
		public int Seed { get; private set; }

		public MapField<uint, EventDetail> EventMap { get; private set; }

		public MapField<uint, EventDetail> AddActivityMap { get; private set; }

		public string BattleKey { get; private set; }

		public uint BattleTimes { get; private set; }

		public void SetData(int seed, MapField<uint, EventDetail> eventMap, MapField<uint, EventDetail> addActivityMap, string battleKey, uint battleTimes)
		{
			this.Seed = seed;
			this.EventMap = eventMap;
			this.AddActivityMap = addActivityMap;
			this.BattleKey = battleKey;
			this.BattleTimes = battleTimes;
		}

		public override void Clear()
		{
		}
	}
}

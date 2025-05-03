using System;
using LocalModels.Bean;

namespace HotFix
{
	public class SingleSlotData
	{
		public int id { get; private set; }

		public int weight { get; private set; }

		public SingleSlotRewardType rewardType { get; private set; }

		public int rewardNum { get; private set; }

		public int atlasId { get; private set; }

		public string icon { get; private set; }

		public string nameId { get; private set; }

		public SingleSlotData(ChapterMiniGame_singleSlot table)
		{
			this.id = table.id;
			this.weight = table.weight;
			this.rewardType = (SingleSlotRewardType)((table.param.Length != 0) ? table.param[0] : 0);
			this.rewardNum = ((table.param.Length > 1) ? table.param[1] : 0);
			this.atlasId = table.atlas;
			this.icon = table.icon;
			this.nameId = table.nameId;
		}
	}
}

using System;

namespace LocalModels.Bean
{
	public class MainLevelReward_MainLevelChest : BaseLocalBean
	{
		public int id { get; set; }

		public int level { get; set; }

		public int[] Reward { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.level = base.readInt();
			this.Reward = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new MainLevelReward_MainLevelChest();
		}
	}
}

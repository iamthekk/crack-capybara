using System;

namespace LocalModels.Bean
{
	public class TowerChallenge_Tower : BaseLocalBean
	{
		public int id { get; set; }

		public string name { get; set; }

		public int[] level { get; set; }

		public int[] ChestReward { get; set; }

		public int[] ChestRewardB { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.name = base.readLocalString();
			this.level = base.readArrayint();
			this.ChestReward = base.readArrayint();
			this.ChestRewardB = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TowerChallenge_Tower();
		}
	}
}

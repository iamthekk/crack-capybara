using System;

namespace LocalModels.Bean
{
	public class IAP_BattlePassReward : BaseLocalBean
	{
		public int id { get; set; }

		public int level { get; set; }

		public int groupId { get; set; }

		public int type { get; set; }

		public string[] freeReward { get; set; }

		public string[] battlePassReward { get; set; }

		public int score { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.level = base.readInt();
			this.groupId = base.readInt();
			this.type = base.readInt();
			this.freeReward = base.readArraystring();
			this.battlePassReward = base.readArraystring();
			this.score = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_BattlePassReward();
		}
	}
}

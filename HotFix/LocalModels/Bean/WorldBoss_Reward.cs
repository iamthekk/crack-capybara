using System;

namespace LocalModels.Bean
{
	public class WorldBoss_Reward : BaseLocalBean
	{
		public int ID { get; set; }

		public int Tag { get; set; }

		public int Times { get; set; }

		public string[] Reward { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Tag = base.readInt();
			this.Times = base.readInt();
			this.Reward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new WorldBoss_Reward();
		}
	}
}

using System;

namespace LocalModels.Bean
{
	public class WorldBoss_WorldBossBox : BaseLocalBean
	{
		public int ID { get; set; }

		public long Damage { get; set; }

		public int BossId { get; set; }

		public string[] Reward { get; set; }

		public string[] OtherReward { get; set; }

		public int ShowBoxAtlas { get; set; }

		public string ShowBoxIconIdle { get; set; }

		public string ShowBoxIconOpen { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Damage = base.readLong();
			this.BossId = base.readInt();
			this.Reward = base.readArraystring();
			this.OtherReward = base.readArraystring();
			this.ShowBoxAtlas = base.readInt();
			this.ShowBoxIconIdle = base.readLocalString();
			this.ShowBoxIconOpen = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new WorldBoss_WorldBossBox();
		}
	}
}

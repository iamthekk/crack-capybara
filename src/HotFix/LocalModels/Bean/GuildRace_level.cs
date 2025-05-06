using System;

namespace LocalModels.Bean
{
	public class GuildRace_level : BaseLocalBean
	{
		public int ID { get; set; }

		public int Level { get; set; }

		public int name { get; set; }

		public string Notes { get; set; }

		public int serverNum { get; set; }

		public int upNum { get; set; }

		public int downNum { get; set; }

		public int generalNum { get; set; }

		public int eliteNum { get; set; }

		public int warriorNum { get; set; }

		public string[] rewards { get; set; }

		public string[] winRewards { get; set; }

		public int atlasID { get; set; }

		public string icon { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Level = base.readInt();
			this.name = base.readInt();
			this.Notes = base.readLocalString();
			this.serverNum = base.readInt();
			this.upNum = base.readInt();
			this.downNum = base.readInt();
			this.generalNum = base.readInt();
			this.eliteNum = base.readInt();
			this.warriorNum = base.readInt();
			this.rewards = base.readArraystring();
			this.winRewards = base.readArraystring();
			this.atlasID = base.readInt();
			this.icon = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildRace_level();
		}
	}
}

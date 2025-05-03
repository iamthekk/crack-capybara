using System;

namespace LocalModels.Bean
{
	public class GuildRace_baseRace : BaseLocalBean
	{
		public int ID { get; set; }

		public string Notes { get; set; }

		public int TypeInt { get; set; }

		public int[] TypeIntArray { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Notes = base.readLocalString();
			this.TypeInt = base.readInt();
			this.TypeIntArray = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildRace_baseRace();
		}
	}
}

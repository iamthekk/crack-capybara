using System;

namespace LocalModels.Bean
{
	public class Guild_guildPower : BaseLocalBean
	{
		public int ID { get; set; }

		public string Notes { get; set; }

		public int[] Power { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Notes = base.readLocalString();
			this.Power = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildPower();
		}
	}
}

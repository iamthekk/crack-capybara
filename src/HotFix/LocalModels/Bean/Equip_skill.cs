using System;

namespace LocalModels.Bean
{
	public class Equip_skill : BaseLocalBean
	{
		public int id { get; set; }

		public string descId { get; set; }

		public string action { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.descId = base.readLocalString();
			this.action = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Equip_skill();
		}
	}
}

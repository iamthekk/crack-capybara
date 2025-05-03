using System;

namespace LocalModels.Bean
{
	public class Relic_data : BaseLocalBean
	{
		public int id { get; set; }

		public int ParamType { get; set; }

		public string LangugaeID { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.ParamType = base.readInt();
			this.LangugaeID = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Relic_data();
		}
	}
}

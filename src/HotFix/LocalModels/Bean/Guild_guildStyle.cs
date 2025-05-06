using System;

namespace LocalModels.Bean
{
	public class Guild_guildStyle : BaseLocalBean
	{
		public int ID { get; set; }

		public int Style { get; set; }

		public int AtlasID { get; set; }

		public string Icon { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Style = base.readInt();
			this.AtlasID = base.readInt();
			this.Icon = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildStyle();
		}
	}
}

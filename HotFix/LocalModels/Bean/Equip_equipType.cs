using System;

namespace LocalModels.Bean
{
	public class Equip_equipType : BaseLocalBean
	{
		public int id { get; set; }

		public int atlasID { get; set; }

		public string iconName { get; set; }

		public string typeName { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.atlasID = base.readInt();
			this.iconName = base.readLocalString();
			this.typeName = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Equip_equipType();
		}
	}
}

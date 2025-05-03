using System;

namespace LocalModels.Bean
{
	public class ArtSkin_equipSkin : BaseLocalBean
	{
		public int id { get; set; }

		public int equipType { get; set; }

		public int subType { get; set; }

		public string skinName { get; set; }

		public string appearName { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.equipType = base.readInt();
			this.subType = base.readInt();
			this.skinName = base.readLocalString();
			this.appearName = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ArtSkin_equipSkin();
		}
	}
}

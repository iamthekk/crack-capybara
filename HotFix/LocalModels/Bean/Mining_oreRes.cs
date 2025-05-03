using System;

namespace LocalModels.Bean
{
	public class Mining_oreRes : BaseLocalBean
	{
		public int id { get; set; }

		public int atlas { get; set; }

		public string icon { get; set; }

		public string languageId { get; set; }

		public string desId { get; set; }

		public int model { get; set; }

		public int uptimes { get; set; }

		public int knock { get; set; }

		public int isShowknock { get; set; }

		public string[] qualityRandom { get; set; }

		public float[] iconOffset { get; set; }

		public float iconScale { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.atlas = base.readInt();
			this.icon = base.readLocalString();
			this.languageId = base.readLocalString();
			this.desId = base.readLocalString();
			this.model = base.readInt();
			this.uptimes = base.readInt();
			this.knock = base.readInt();
			this.isShowknock = base.readInt();
			this.qualityRandom = base.readArraystring();
			this.iconOffset = base.readArrayfloat();
			this.iconScale = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Mining_oreRes();
		}
	}
}

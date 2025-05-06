using System;

namespace LocalModels.Bean
{
	public class Avatar_Skin : BaseLocalBean
	{
		public int id { get; set; }

		public string name { get; set; }

		public int skinType { get; set; }

		public int atlasId { get; set; }

		public string iconId { get; set; }

		public int quality_atlasId { get; set; }

		public string quality_iconId { get; set; }

		public int part { get; set; }

		public string[] unlockItemId { get; set; }

		public int[] mutualParts { get; set; }

		public int skinPrefab { get; set; }

		public int isHide { get; set; }

		public string getLanguageId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.name = base.readLocalString();
			this.skinType = base.readInt();
			this.atlasId = base.readInt();
			this.iconId = base.readLocalString();
			this.quality_atlasId = base.readInt();
			this.quality_iconId = base.readLocalString();
			this.part = base.readInt();
			this.unlockItemId = base.readArraystring();
			this.mutualParts = base.readArrayint();
			this.skinPrefab = base.readInt();
			this.isHide = base.readInt();
			this.getLanguageId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Avatar_Skin();
		}
	}
}

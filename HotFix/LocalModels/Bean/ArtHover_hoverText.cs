using System;

namespace LocalModels.Bean
{
	public class ArtHover_hoverText : BaseLocalBean
	{
		public int id { get; set; }

		public string textId { get; set; }

		public int atlasId { get; set; }

		public string iconName { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.textId = base.readLocalString();
			this.atlasId = base.readInt();
			this.iconName = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ArtHover_hoverText();
		}
	}
}

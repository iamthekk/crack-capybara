using System;

namespace LocalModels.Bean
{
	public class Quality_collectionQuality : BaseLocalBean
	{
		public int id { get; set; }

		public int atlasId { get; set; }

		public string bgSpriteName { get; set; }

		public string itemBg { get; set; }

		public string cardBg { get; set; }

		public string colorName { get; set; }

		public string[] shareFragmentIcon { get; set; }

		public string imgFragment { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.atlasId = base.readInt();
			this.bgSpriteName = base.readLocalString();
			this.itemBg = base.readLocalString();
			this.cardBg = base.readLocalString();
			this.colorName = base.readLocalString();
			this.shareFragmentIcon = base.readArraystring();
			this.imgFragment = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Quality_collectionQuality();
		}
	}
}

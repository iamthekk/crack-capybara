using System;

namespace LocalModels.Bean
{
	public class Quality_itemQuality : BaseLocalBean
	{
		public int id { get; set; }

		public int atlasId { get; set; }

		public string bgSpriteName { get; set; }

		public string drawCardBgEffect { get; set; }

		public string drawCardFireEffect { get; set; }

		public string drawCardFireEffect2 { get; set; }

		public string drawCardFireBurstEffect { get; set; }

		public string drawCardFireBurstEffect2 { get; set; }

		public string drawCardBurstLightEffect { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.atlasId = base.readInt();
			this.bgSpriteName = base.readLocalString();
			this.drawCardBgEffect = base.readLocalString();
			this.drawCardFireEffect = base.readLocalString();
			this.drawCardFireEffect2 = base.readLocalString();
			this.drawCardFireBurstEffect = base.readLocalString();
			this.drawCardFireBurstEffect2 = base.readLocalString();
			this.drawCardBurstLightEffect = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Quality_itemQuality();
		}
	}
}

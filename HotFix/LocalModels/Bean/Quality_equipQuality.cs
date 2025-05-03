using System;

namespace LocalModels.Bean
{
	public class Quality_equipQuality : BaseLocalBean
	{
		public int id { get; set; }

		public int atlasId { get; set; }

		public string bgSpriteName { get; set; }

		public string equipTypeBgSpriteName { get; set; }

		public string pointSpriteName { get; set; }

		public string lockSpriteName { get; set; }

		public string composePlusSpriteName { get; set; }

		public string colorNumLight { get; set; }

		public string colorNumDark { get; set; }

		public string colorImgLightBlend { get; set; }

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
			this.equipTypeBgSpriteName = base.readLocalString();
			this.pointSpriteName = base.readLocalString();
			this.lockSpriteName = base.readLocalString();
			this.composePlusSpriteName = base.readLocalString();
			this.colorNumLight = base.readLocalString();
			this.colorNumDark = base.readLocalString();
			this.colorImgLightBlend = base.readLocalString();
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
			return new Quality_equipQuality();
		}
	}
}

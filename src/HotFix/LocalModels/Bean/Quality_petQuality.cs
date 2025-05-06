using System;

namespace LocalModels.Bean
{
	public class Quality_petQuality : BaseLocalBean
	{
		public int id { get; set; }

		public string nameID { get; set; }

		public int atlasId { get; set; }

		public string bgSpriteName { get; set; }

		public string typeBgSpriteName { get; set; }

		public string typeTxtBg { get; set; }

		public string imgBottomCircle { get; set; }

		public string imgFragment { get; set; }

		public string[] passiveTextColor { get; set; }

		public string[] passiveTextColor2 { get; set; }

		public string colorNum { get; set; }

		public string colorNumDark { get; set; }

		public string drawCardBgEffect { get; set; }

		public string drawCardFireEffect { get; set; }

		public string drawCardFireEffect2 { get; set; }

		public string drawCardFireBurstEffect { get; set; }

		public string drawCardFireBurstEffect2 { get; set; }

		public string drawCardBurstLightEffect { get; set; }

		public string passiveSweepEffect { get; set; }

		public string passiveStreamerEffect { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.nameID = base.readLocalString();
			this.atlasId = base.readInt();
			this.bgSpriteName = base.readLocalString();
			this.typeBgSpriteName = base.readLocalString();
			this.typeTxtBg = base.readLocalString();
			this.imgBottomCircle = base.readLocalString();
			this.imgFragment = base.readLocalString();
			this.passiveTextColor = base.readArraystring();
			this.passiveTextColor2 = base.readArraystring();
			this.colorNum = base.readLocalString();
			this.colorNumDark = base.readLocalString();
			this.drawCardBgEffect = base.readLocalString();
			this.drawCardFireEffect = base.readLocalString();
			this.drawCardFireEffect2 = base.readLocalString();
			this.drawCardFireBurstEffect = base.readLocalString();
			this.drawCardFireBurstEffect2 = base.readLocalString();
			this.drawCardBurstLightEffect = base.readLocalString();
			this.passiveSweepEffect = base.readLocalString();
			this.passiveStreamerEffect = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Quality_petQuality();
		}
	}
}

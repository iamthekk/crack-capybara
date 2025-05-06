using System;

namespace LocalModels.Bean
{
	public class ChapterMiniGame_turntableReward : BaseLocalBean
	{
		public int id { get; set; }

		public int weight { get; set; }

		public int showWeight { get; set; }

		public string[] param { get; set; }

		public int atlas { get; set; }

		public string icon { get; set; }

		public int planeColor { get; set; }

		public int textStyle { get; set; }

		public string textColor { get; set; }

		public string textId { get; set; }

		public string resultTextId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.weight = base.readInt();
			this.showWeight = base.readInt();
			this.param = base.readArraystring();
			this.atlas = base.readInt();
			this.icon = base.readLocalString();
			this.planeColor = base.readInt();
			this.textStyle = base.readInt();
			this.textColor = base.readLocalString();
			this.textId = base.readLocalString();
			this.resultTextId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterMiniGame_turntableReward();
		}
	}
}

using System;

namespace LocalModels.Bean
{
	public class ChapterActivity_ActvTurntableDetail : BaseLocalBean
	{
		public int id { get; set; }

		public int weight { get; set; }

		public int weightAdd { get; set; }

		public int showWeight { get; set; }

		public int reward { get; set; }

		public int again { get; set; }

		public int planeColor { get; set; }

		public string textColor { get; set; }

		public string xNumtId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.weight = base.readInt();
			this.weightAdd = base.readInt();
			this.showWeight = base.readInt();
			this.reward = base.readInt();
			this.again = base.readInt();
			this.planeColor = base.readInt();
			this.textColor = base.readLocalString();
			this.xNumtId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterActivity_ActvTurntableDetail();
		}
	}
}

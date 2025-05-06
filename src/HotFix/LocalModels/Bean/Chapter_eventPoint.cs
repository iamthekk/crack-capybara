using System;

namespace LocalModels.Bean
{
	public class Chapter_eventPoint : BaseLocalBean
	{
		public int id { get; set; }

		public float[] createOffsetSea { get; set; }

		public float[] createOffsetLand { get; set; }

		public string[] action { get; set; }

		public string[] defaultAction { get; set; }

		public string playerArriveAction { get; set; }

		public int bottomId { get; set; }

		public string path { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.createOffsetSea = base.readArrayfloat();
			this.createOffsetLand = base.readArrayfloat();
			this.action = base.readArraystring();
			this.defaultAction = base.readArraystring();
			this.playerArriveAction = base.readLocalString();
			this.bottomId = base.readInt();
			this.path = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_eventPoint();
		}
	}
}

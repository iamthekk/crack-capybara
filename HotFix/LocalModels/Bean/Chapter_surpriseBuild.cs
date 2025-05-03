using System;

namespace LocalModels.Bean
{
	public class Chapter_surpriseBuild : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int buildId { get; set; }

		public int eventPointId { get; set; }

		public int memberId { get; set; }

		public int modelId { get; set; }

		public int weight { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.buildId = base.readInt();
			this.eventPointId = base.readInt();
			this.memberId = base.readInt();
			this.modelId = base.readInt();
			this.weight = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_surpriseBuild();
		}
	}
}

using System;

namespace LocalModels.Bean
{
	public class Chapter_boxBuild : BaseLocalBean
	{
		public int id { get; set; }

		public int eventPointId { get; set; }

		public int memberId { get; set; }

		public string[] skillNumWeight { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.eventPointId = base.readInt();
			this.memberId = base.readInt();
			this.skillNumWeight = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_boxBuild();
		}
	}
}

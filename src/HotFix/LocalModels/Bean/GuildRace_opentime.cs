using System;

namespace LocalModels.Bean
{
	public class GuildRace_opentime : BaseLocalBean
	{
		public int ID { get; set; }

		public string openTime { get; set; }

		public int stage1 { get; set; }

		public int stage2 { get; set; }

		public int stage3 { get; set; }

		public int stage4 { get; set; }

		public int stage5 { get; set; }

		public int stage6 { get; set; }

		public int stage7 { get; set; }

		public int stage8 { get; set; }

		public int stage9 { get; set; }

		public int stage10 { get; set; }

		public int stage11 { get; set; }

		public int stage12 { get; set; }

		public int stage13 { get; set; }

		public int stage14 { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.openTime = base.readLocalString();
			this.stage1 = base.readInt();
			this.stage2 = base.readInt();
			this.stage3 = base.readInt();
			this.stage4 = base.readInt();
			this.stage5 = base.readInt();
			this.stage6 = base.readInt();
			this.stage7 = base.readInt();
			this.stage8 = base.readInt();
			this.stage9 = base.readInt();
			this.stage10 = base.readInt();
			this.stage11 = base.readInt();
			this.stage12 = base.readInt();
			this.stage13 = base.readInt();
			this.stage14 = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildRace_opentime();
		}
	}
}

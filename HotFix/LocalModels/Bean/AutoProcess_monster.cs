using System;

namespace LocalModels.Bean
{
	public class AutoProcess_monster : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int MemberId1 { get; set; }

		public int MemberId2 { get; set; }

		public int MemberId3 { get; set; }

		public string Attributes { get; set; }

		public int skill { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.MemberId1 = base.readInt();
			this.MemberId2 = base.readInt();
			this.MemberId3 = base.readInt();
			this.Attributes = base.readLocalString();
			this.skill = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new AutoProcess_monster();
		}
	}
}

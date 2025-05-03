using System;

namespace LocalModels.Bean
{
	public class Mount_mountStage : BaseLocalBean
	{
		public int id { get; set; }

		public int stage { get; set; }

		public int memberId { get; set; }

		public string attribute { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.stage = base.readInt();
			this.memberId = base.readInt();
			this.attribute = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Mount_mountStage();
		}
	}
}

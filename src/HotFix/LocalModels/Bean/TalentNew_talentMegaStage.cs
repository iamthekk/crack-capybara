using System;

namespace LocalModels.Bean
{
	public class TalentNew_talentMegaStage : BaseLocalBean
	{
		public int id { get; set; }

		public string languageId { get; set; }

		public int minStep { get; set; }

		public int maxStep { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.languageId = base.readLocalString();
			this.minStep = base.readInt();
			this.maxStep = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TalentNew_talentMegaStage();
		}
	}
}

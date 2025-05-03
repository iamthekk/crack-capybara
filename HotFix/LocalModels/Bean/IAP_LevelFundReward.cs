using System;

namespace LocalModels.Bean
{
	public class IAP_LevelFundReward : BaseLocalBean
	{
		public int id { get; set; }

		public int groupId { get; set; }

		public string param { get; set; }

		public string[] freeReward { get; set; }

		public string[] fundReward { get; set; }

		public string prodReparationMail { get; set; }

		public string testReparationMail { get; set; }

		public string[] reparationReward { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.groupId = base.readInt();
			this.param = base.readLocalString();
			this.freeReward = base.readArraystring();
			this.fundReward = base.readArraystring();
			this.prodReparationMail = base.readLocalString();
			this.testReparationMail = base.readLocalString();
			this.reparationReward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_LevelFundReward();
		}
	}
}

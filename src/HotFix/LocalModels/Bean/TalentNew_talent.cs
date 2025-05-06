using System;

namespace LocalModels.Bean
{
	public class TalentNew_talent : BaseLocalBean
	{
		public int id { get; set; }

		public int talentLevel { get; set; }

		public int evolution { get; set; }

		public int rewardType { get; set; }

		public string reward { get; set; }

		public int iconAtlasID { get; set; }

		public string iconID { get; set; }

		public string talentName { get; set; }

		public string talentDesc { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.talentLevel = base.readInt();
			this.evolution = base.readInt();
			this.rewardType = base.readInt();
			this.reward = base.readLocalString();
			this.iconAtlasID = base.readInt();
			this.iconID = base.readLocalString();
			this.talentName = base.readLocalString();
			this.talentDesc = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TalentNew_talent();
		}
	}
}

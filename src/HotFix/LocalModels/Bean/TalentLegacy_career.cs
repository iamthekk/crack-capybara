using System;

namespace LocalModels.Bean
{
	public class TalentLegacy_career : BaseLocalBean
	{
		public int id { get; set; }

		public string nameID { get; set; }

		public int isOpen { get; set; }

		public string condition { get; set; }

		public int previewIconId { get; set; }

		public string previewIcon { get; set; }

		public string unLockTips { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.nameID = base.readLocalString();
			this.isOpen = base.readInt();
			this.condition = base.readLocalString();
			this.previewIconId = base.readInt();
			this.previewIcon = base.readLocalString();
			this.unLockTips = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TalentLegacy_career();
		}
	}
}

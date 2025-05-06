using System;

namespace LocalModels.Bean
{
	public class ChapterMiniGame_paySlotReward : BaseLocalBean
	{
		public int id { get; set; }

		public int groupId { get; set; }

		public int weight { get; set; }

		public string[] param { get; set; }

		public int atlas { get; set; }

		public string icon { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.groupId = base.readInt();
			this.weight = base.readInt();
			this.param = base.readArraystring();
			this.atlas = base.readInt();
			this.icon = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterMiniGame_paySlotReward();
		}
	}
}

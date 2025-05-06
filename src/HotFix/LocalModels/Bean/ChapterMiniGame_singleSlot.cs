using System;

namespace LocalModels.Bean
{
	public class ChapterMiniGame_singleSlot : BaseLocalBean
	{
		public int id { get; set; }

		public int weight { get; set; }

		public int[] param { get; set; }

		public int atlas { get; set; }

		public string icon { get; set; }

		public string nameId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.weight = base.readInt();
			this.param = base.readArrayint();
			this.atlas = base.readInt();
			this.icon = base.readLocalString();
			this.nameId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterMiniGame_singleSlot();
		}
	}
}

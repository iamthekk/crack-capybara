using System;

namespace LocalModels.Bean
{
	public class NewWorld_newWorldRank : BaseLocalBean
	{
		public int id { get; set; }

		public int rank { get; set; }

		public string[] reward { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.rank = base.readInt();
			this.reward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new NewWorld_newWorldRank();
		}
	}
}

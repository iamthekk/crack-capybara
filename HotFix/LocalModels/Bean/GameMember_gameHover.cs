using System;

namespace LocalModels.Bean
{
	public class GameMember_gameHover : BaseLocalBean
	{
		public int id { get; set; }

		public int bodyPosID { get; set; }

		public int prefabID { get; set; }

		public int isDeathClose { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.bodyPosID = base.readInt();
			this.prefabID = base.readInt();
			this.isDeathClose = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameMember_gameHover();
		}
	}
}

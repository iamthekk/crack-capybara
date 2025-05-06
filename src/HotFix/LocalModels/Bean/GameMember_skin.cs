using System;

namespace LocalModels.Bean
{
	public class GameMember_skin : BaseLocalBean
	{
		public int id { get; set; }

		public string skin { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.skin = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameMember_skin();
		}
	}
}

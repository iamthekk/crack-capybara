using System;

namespace LocalModels.Bean
{
	public class GameBullet_bulletType : BaseLocalBean
	{
		public int id { get; set; }

		public string sClassName { get; set; }

		public string cClassName { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.sClassName = base.readLocalString();
			this.cClassName = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameBullet_bulletType();
		}
	}
}

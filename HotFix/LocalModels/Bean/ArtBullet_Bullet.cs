using System;

namespace LocalModels.Bean
{
	public class ArtBullet_Bullet : BaseLocalBean
	{
		public int id { get; set; }

		public string path { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.path = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ArtBullet_Bullet();
		}
	}
}

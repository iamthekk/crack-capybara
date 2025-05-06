using System;

namespace LocalModels.Bean
{
	public class GameBullet_bullet : BaseLocalBean
	{
		public int id { get; set; }

		public int prefabID { get; set; }

		public int bulletType { get; set; }

		public string parameters { get; set; }

		public int frame { get; set; }

		public int shakeID { get; set; }

		public string hitAddBuffs { get; set; }

		public float destroyDuation { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.prefabID = base.readInt();
			this.bulletType = base.readInt();
			this.parameters = base.readLocalString();
			this.frame = base.readInt();
			this.shakeID = base.readInt();
			this.hitAddBuffs = base.readLocalString();
			this.destroyDuation = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameBullet_bullet();
		}
	}
}

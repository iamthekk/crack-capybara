using System;

namespace LocalModels.Bean
{
	public class ArtAnimation_animation : BaseLocalBean
	{
		public string id { get; set; }

		public int hideWeapon { get; set; }

		public override bool readImpl()
		{
			this.id = base.readLocalString();
			this.hideWeapon = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ArtAnimation_animation();
		}
	}
}

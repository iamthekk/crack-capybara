using System;

namespace LocalModels.Bean
{
	public class ArtMember_member : BaseLocalBean
	{
		public int id { get; set; }

		public string path { get; set; }

		public float uiScale { get; set; }

		public float miningScale { get; set; }

		public float hotSpringScale { get; set; }

		public float hotSpringModelScale { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.path = base.readLocalString();
			this.uiScale = base.readFloat();
			this.miningScale = base.readFloat();
			this.hotSpringScale = base.readFloat();
			this.hotSpringModelScale = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ArtMember_member();
		}
	}
}

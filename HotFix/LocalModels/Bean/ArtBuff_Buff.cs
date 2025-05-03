using System;

namespace LocalModels.Bean
{
	public class ArtBuff_Buff : BaseLocalBean
	{
		public int id { get; set; }

		public string path { get; set; }

		public float effectDuration { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.path = base.readLocalString();
			this.effectDuration = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ArtBuff_Buff();
		}
	}
}

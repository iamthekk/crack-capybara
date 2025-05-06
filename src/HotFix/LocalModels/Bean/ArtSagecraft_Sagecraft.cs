using System;

namespace LocalModels.Bean
{
	public class ArtSagecraft_Sagecraft : BaseLocalBean
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
			return new ArtSagecraft_Sagecraft();
		}
	}
}

using System;

namespace LocalModels.Bean
{
	public class ArtMember_morph : BaseLocalBean
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
			return new ArtMember_morph();
		}
	}
}

using System;

namespace LocalModels.Bean
{
	public class ArtEffect_Effect : BaseLocalBean
	{
		public int id { get; set; }

		public string path { get; set; }

		public string pointName { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.path = base.readLocalString();
			this.pointName = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ArtEffect_Effect();
		}
	}
}

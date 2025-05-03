using System;

namespace LocalModels.Bean
{
	public class TGASource_Get : BaseLocalBean
	{
		public int id { get; set; }

		public string source { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.source = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TGASource_Get();
		}
	}
}

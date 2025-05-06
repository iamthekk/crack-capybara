using System;

namespace LocalModels.Bean
{
	public class TGASource_Page : BaseLocalBean
	{
		public string id { get; set; }

		public string source { get; set; }

		public int track { get; set; }

		public override bool readImpl()
		{
			this.id = base.readLocalString();
			this.source = base.readLocalString();
			this.track = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TGASource_Page();
		}
	}
}

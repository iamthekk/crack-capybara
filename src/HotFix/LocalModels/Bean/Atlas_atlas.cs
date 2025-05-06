using System;

namespace LocalModels.Bean
{
	public class Atlas_atlas : BaseLocalBean
	{
		public int id { get; set; }

		public string notes { get; set; }

		public string path { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.notes = base.readLocalString();
			this.path = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Atlas_atlas();
		}
	}
}

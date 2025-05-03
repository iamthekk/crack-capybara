using System;

namespace LocalModels.Bean
{
	public class Relic_group : BaseLocalBean
	{
		public int id { get; set; }

		public string NameId { get; set; }

		public string DescId { get; set; }

		public int type { get; set; }

		public int[] Content { get; set; }

		public string[] GroupAttributes { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.NameId = base.readLocalString();
			this.DescId = base.readLocalString();
			this.type = base.readInt();
			this.Content = base.readArrayint();
			this.GroupAttributes = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Relic_group();
		}
	}
}

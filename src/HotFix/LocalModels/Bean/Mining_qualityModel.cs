using System;

namespace LocalModels.Bean
{
	public class Mining_qualityModel : BaseLocalBean
	{
		public int id { get; set; }

		public int skinId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.skinId = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Mining_qualityModel();
		}
	}
}

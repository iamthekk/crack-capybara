using System;

namespace LocalModels.Bean
{
	public class Box_output : BaseLocalBean
	{
		public int Id { get; set; }

		public int[] num { get; set; }

		public string[] dropQuality { get; set; }

		public int definiteDropQuality { get; set; }

		public override bool readImpl()
		{
			this.Id = base.readInt();
			this.num = base.readArrayint();
			this.dropQuality = base.readArraystring();
			this.definiteDropQuality = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Box_output();
		}
	}
}

using System;

namespace LocalModels.Bean
{
	public class ChapterMiniGame_turntableBase : BaseLocalBean
	{
		public int id { get; set; }

		public int[] rewards { get; set; }

		public int count { get; set; }

		public int[] cost { get; set; }

		public float offsetAngle { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.rewards = base.readArrayint();
			this.count = base.readInt();
			this.cost = base.readArrayint();
			this.offsetAngle = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterMiniGame_turntableBase();
		}
	}
}

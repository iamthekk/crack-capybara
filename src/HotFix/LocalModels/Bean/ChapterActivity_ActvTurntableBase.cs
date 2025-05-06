using System;

namespace LocalModels.Bean
{
	public class ChapterActivity_ActvTurntableBase : BaseLocalBean
	{
		public int id { get; set; }

		public string openTime { get; set; }

		public string endTime { get; set; }

		public int group { get; set; }

		public int[] rewards { get; set; }

		public int cost { get; set; }

		public int[] guaranteePool { get; set; }

		public int[] guaranteeTimes { get; set; }

		public float offsetAngle { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.openTime = base.readLocalString();
			this.endTime = base.readLocalString();
			this.group = base.readInt();
			this.rewards = base.readArrayint();
			this.cost = base.readInt();
			this.guaranteePool = base.readArrayint();
			this.guaranteeTimes = base.readArrayint();
			this.offsetAngle = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterActivity_ActvTurntableBase();
		}
	}
}

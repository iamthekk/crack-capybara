using System;

namespace LocalModels.Bean
{
	public class ChestList_ChestList : BaseLocalBean
	{
		public int id { get; set; }

		public int point { get; set; }

		public string[] reward { get; set; }

		public int pointCircle { get; set; }

		public string[] rewardCircle { get; set; }

		public int next { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.point = base.readInt();
			this.reward = base.readArraystring();
			this.pointCircle = base.readInt();
			this.rewardCircle = base.readArraystring();
			this.next = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChestList_ChestList();
		}
	}
}

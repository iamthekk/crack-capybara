using System;

namespace LocalModels.Bean
{
	public class CrossArena_CrossArenaRobot : BaseLocalBean
	{
		public int ID { get; set; }

		public int[] Score { get; set; }

		public int Count { get; set; }

		public int[] Power { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Score = base.readArrayint();
			this.Count = base.readInt();
			this.Power = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new CrossArena_CrossArenaRobot();
		}
	}
}

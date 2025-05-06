using System;

namespace LocalModels.Bean
{
	public class Task_WeeklyActive : BaseLocalBean
	{
		public int ID { get; set; }

		public int Requirements { get; set; }

		public string[] FixReward { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Requirements = base.readInt();
			this.FixReward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Task_WeeklyActive();
		}
	}
}

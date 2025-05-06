using System;

namespace LocalModels.Bean
{
	public class SevenDay_SevenDayTask : BaseLocalBean
	{
		public int ID { get; set; }

		public int Day { get; set; }

		public int TaskType { get; set; }

		public int StatisticsType { get; set; }

		public int Need { get; set; }

		public string Describe { get; set; }

		public int ProgressType { get; set; }

		public int Active { get; set; }

		public string[] Reward { get; set; }

		public int Jump { get; set; }

		public int UnlockNeed { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Day = base.readInt();
			this.TaskType = base.readInt();
			this.StatisticsType = base.readInt();
			this.Need = base.readInt();
			this.Describe = base.readLocalString();
			this.ProgressType = base.readInt();
			this.Active = base.readInt();
			this.Reward = base.readArraystring();
			this.Jump = base.readInt();
			this.UnlockNeed = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new SevenDay_SevenDayTask();
		}
	}
}

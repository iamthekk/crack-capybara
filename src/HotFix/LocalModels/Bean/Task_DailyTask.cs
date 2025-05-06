using System;

namespace LocalModels.Bean
{
	public class Task_DailyTask : BaseLocalBean
	{
		public int ID { get; set; }

		public int DailyType { get; set; }

		public int AccumulationType { get; set; }

		public int DailyNeed { get; set; }

		public int DailyNeedParam { get; set; }

		public string DailyDescribe { get; set; }

		public int DailyActiveReward { get; set; }

		public int Jump { get; set; }

		public int UnlockNeed { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.DailyType = base.readInt();
			this.AccumulationType = base.readInt();
			this.DailyNeed = base.readInt();
			this.DailyNeedParam = base.readInt();
			this.DailyDescribe = base.readLocalString();
			this.DailyActiveReward = base.readInt();
			this.Jump = base.readInt();
			this.UnlockNeed = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Task_DailyTask();
		}
	}
}

using System;

namespace LocalModels.Bean
{
	public class ActivityTurntable_TurntableQuest : BaseLocalBean
	{
		public int ID { get; set; }

		public int TaskType { get; set; }

		public int StatisticsType { get; set; }

		public int Need { get; set; }

		public int NeedOld { get; set; }

		public string Describe { get; set; }

		public string[] Reward { get; set; }

		public int Jump { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.TaskType = base.readInt();
			this.StatisticsType = base.readInt();
			this.Need = base.readInt();
			this.NeedOld = base.readInt();
			this.Describe = base.readLocalString();
			this.Reward = base.readArraystring();
			this.Jump = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ActivityTurntable_TurntableQuest();
		}
	}
}

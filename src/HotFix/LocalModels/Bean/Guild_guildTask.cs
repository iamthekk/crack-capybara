using System;

namespace LocalModels.Bean
{
	public class Guild_guildTask : BaseLocalBean
	{
		public int ID { get; set; }

		public int Type { get; set; }

		public int IsInit { get; set; }

		public int Child { get; set; }

		public int Weight { get; set; }

		public int AccumulationType { get; set; }

		public int Need { get; set; }

		public string[] Reward { get; set; }

		public string[] OtherReward { get; set; }

		public int[] Condition { get; set; }

		public string languageId { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Type = base.readInt();
			this.IsInit = base.readInt();
			this.Child = base.readInt();
			this.Weight = base.readInt();
			this.AccumulationType = base.readInt();
			this.Need = base.readInt();
			this.Reward = base.readArraystring();
			this.OtherReward = base.readArraystring();
			this.Condition = base.readArrayint();
			this.languageId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildTask();
		}
	}
}

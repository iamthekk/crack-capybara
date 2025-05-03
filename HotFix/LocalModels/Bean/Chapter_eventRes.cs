using System;

namespace LocalModels.Bean
{
	public class Chapter_eventRes : BaseLocalBean
	{
		public int id { get; set; }

		public string path { get; set; }

		public int type { get; set; }

		public string randomId { get; set; }

		public int weight { get; set; }

		public int[] items { get; set; }

		public int itemWeight { get; set; }

		public string[] drop { get; set; }

		public string activityPath { get; set; }

		public int activityReward { get; set; }

		public int wheelReward { get; set; }

		public string[] EventAttributesArena { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.path = base.readLocalString();
			this.type = base.readInt();
			this.randomId = base.readLocalString();
			this.weight = base.readInt();
			this.items = base.readArrayint();
			this.itemWeight = base.readInt();
			this.drop = base.readArraystring();
			this.activityPath = base.readLocalString();
			this.activityReward = base.readInt();
			this.wheelReward = base.readInt();
			this.EventAttributesArena = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_eventRes();
		}
	}
}

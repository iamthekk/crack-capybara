using System;

namespace LocalModels.Bean
{
	public class NewWorld_newWorldTask : BaseLocalBean
	{
		public int id { get; set; }

		public int group { get; set; }

		public string nameId { get; set; }

		public int num { get; set; }

		public string[] reward { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.group = base.readInt();
			this.nameId = base.readLocalString();
			this.num = base.readInt();
			this.reward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new NewWorld_newWorldTask();
		}
	}
}

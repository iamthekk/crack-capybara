using System;

namespace LocalModels.Bean
{
	public class Box_boxBase : BaseLocalBean
	{
		public int id { get; set; }

		public int[] dropId { get; set; }

		public int score { get; set; }

		public int name { get; set; }

		public string uiObjName { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.dropId = base.readArrayint();
			this.score = base.readInt();
			this.name = base.readInt();
			this.uiObjName = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Box_boxBase();
		}
	}
}

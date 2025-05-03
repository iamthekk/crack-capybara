using System;

namespace LocalModels.Bean
{
	public class Pet_petCollection : BaseLocalBean
	{
		public int id { get; set; }

		public int group { get; set; }

		public string groupNameId { get; set; }

		public int condition { get; set; }

		public int[] petIDGroup { get; set; }

		public string attributes { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.group = base.readInt();
			this.groupNameId = base.readLocalString();
			this.condition = base.readInt();
			this.petIDGroup = base.readArrayint();
			this.attributes = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Pet_petCollection();
		}
	}
}

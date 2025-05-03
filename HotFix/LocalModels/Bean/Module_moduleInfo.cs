using System;

namespace LocalModels.Bean
{
	public class Module_moduleInfo : BaseLocalBean
	{
		public int id { get; set; }

		public string nameId { get; set; }

		public string infoId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.nameId = base.readLocalString();
			this.infoId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Module_moduleInfo();
		}
	}
}

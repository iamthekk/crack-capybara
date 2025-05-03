using System;

namespace LocalModels.Bean
{
	public class Const_Const : BaseLocalBean
	{
		public string id { get; set; }

		public string note { get; set; }

		public string keyName { get; set; }

		public string valueType { get; set; }

		public string value { get; set; }

		public override bool readImpl()
		{
			this.id = base.readLocalString();
			this.note = base.readLocalString();
			this.keyName = base.readLocalString();
			this.valueType = base.readLocalString();
			this.value = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Const_Const();
		}
	}
}

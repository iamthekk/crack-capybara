using System;

namespace LocalModels.Bean
{
	public class Guild_guilddairy : BaseLocalBean
	{
		public int dairy_id { get; set; }

		public string language { get; set; }

		public override bool readImpl()
		{
			this.dairy_id = base.readInt();
			this.language = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guilddairy();
		}
	}
}

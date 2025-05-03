using System;

namespace LocalModels.Bean
{
	public class GameConfig_Config : BaseLocalBean
	{
		public int ID { get; set; }

		public string Value { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Value = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameConfig_Config();
		}
	}
}

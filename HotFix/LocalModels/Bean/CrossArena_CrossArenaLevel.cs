using System;

namespace LocalModels.Bean
{
	public class CrossArena_CrossArenaLevel : BaseLocalBean
	{
		public int Level { get; set; }

		public string name { get; set; }

		public string Notes { get; set; }

		public int serverNum { get; set; }

		public int playerNum { get; set; }

		public int upPro { get; set; }

		public int downPro { get; set; }

		public override bool readImpl()
		{
			this.Level = base.readInt();
			this.name = base.readLocalString();
			this.Notes = base.readLocalString();
			this.serverNum = base.readInt();
			this.playerNum = base.readInt();
			this.upPro = base.readInt();
			this.downPro = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new CrossArena_CrossArenaLevel();
		}
	}
}

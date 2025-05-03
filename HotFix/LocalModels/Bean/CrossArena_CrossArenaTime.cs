using System;

namespace LocalModels.Bean
{
	public class CrossArena_CrossArenaTime : BaseLocalBean
	{
		public int ID { get; set; }

		public string OpenTime { get; set; }

		public string CloseTime { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.OpenTime = base.readLocalString();
			this.CloseTime = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new CrossArena_CrossArenaTime();
		}
	}
}

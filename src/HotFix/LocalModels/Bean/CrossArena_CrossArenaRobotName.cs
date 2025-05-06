using System;

namespace LocalModels.Bean
{
	public class CrossArena_CrossArenaRobotName : BaseLocalBean
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public override bool readImpl()
		{
			this.Id = base.readInt();
			this.Name = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new CrossArena_CrossArenaRobotName();
		}
	}
}

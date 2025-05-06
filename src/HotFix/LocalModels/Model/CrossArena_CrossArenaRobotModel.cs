using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class CrossArena_CrossArenaRobotModel : BaseLocalModel
	{
		public CrossArena_CrossArenaRobot GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<CrossArena_CrossArenaRobot> GetAllElements()
		{
			return this.modelImpl.GetAllElement();
		}

		public override void Initialise(string name, byte[] assetBytes)
		{
			base.Initialise(name, assetBytes);
			if (assetBytes == null)
			{
				return;
			}
			this.modelImpl.Initialise(name, assetBytes);
		}

		public override void DeInitialise()
		{
			this.modelImpl.DeInitialise();
			base.DeInitialise();
		}

		public static readonly string fileName = "CrossArena_CrossArenaRobot";

		private CrossArena_CrossArenaRobotModelImpl modelImpl = new CrossArena_CrossArenaRobotModelImpl();
	}
}

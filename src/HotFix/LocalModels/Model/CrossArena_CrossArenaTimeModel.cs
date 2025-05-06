using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class CrossArena_CrossArenaTimeModel : BaseLocalModel
	{
		public CrossArena_CrossArenaTime GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<CrossArena_CrossArenaTime> GetAllElements()
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

		public static readonly string fileName = "CrossArena_CrossArenaTime";

		private CrossArena_CrossArenaTimeModelImpl modelImpl = new CrossArena_CrossArenaTimeModelImpl();
	}
}

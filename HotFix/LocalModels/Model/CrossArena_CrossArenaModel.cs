﻿using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class CrossArena_CrossArenaModel : BaseLocalModel
	{
		public CrossArena_CrossArena GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<CrossArena_CrossArena> GetAllElements()
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

		public static readonly string fileName = "CrossArena_CrossArena";

		private CrossArena_CrossArenaModelImpl modelImpl = new CrossArena_CrossArenaModelImpl();
	}
}

﻿using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class NewWorld_newWorldRankModel : BaseLocalModel
	{
		public NewWorld_newWorldRank GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<NewWorld_newWorldRank> GetAllElements()
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

		public static readonly string fileName = "NewWorld_newWorldRank";

		private NewWorld_newWorldRankModelImpl modelImpl = new NewWorld_newWorldRankModelImpl();
	}
}

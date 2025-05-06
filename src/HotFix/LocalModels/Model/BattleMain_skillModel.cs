﻿using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class BattleMain_skillModel : BaseLocalModel
	{
		public BattleMain_skill GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<BattleMain_skill> GetAllElements()
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

		public static readonly string fileName = "BattleMain_skill";

		private BattleMain_skillModelImpl modelImpl = new BattleMain_skillModelImpl();
	}
}

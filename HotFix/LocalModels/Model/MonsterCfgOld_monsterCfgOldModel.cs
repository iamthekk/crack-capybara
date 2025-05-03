﻿using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class MonsterCfgOld_monsterCfgOldModel : BaseLocalModel
	{
		public MonsterCfgOld_monsterCfgOld GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<MonsterCfgOld_monsterCfgOld> GetAllElements()
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

		public static readonly string fileName = "MonsterCfgOld_monsterCfgOld";

		private MonsterCfgOld_monsterCfgOldModelImpl modelImpl = new MonsterCfgOld_monsterCfgOldModelImpl();
	}
}

﻿using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameSkill_skillAnimationModel : BaseLocalModel
	{
		public GameSkill_skillAnimation GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameSkill_skillAnimation> GetAllElements()
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

		public static readonly string fileName = "GameSkill_skillAnimation";

		private GameSkill_skillAnimationModelImpl modelImpl = new GameSkill_skillAnimationModelImpl();
	}
}

﻿using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameMember_skinModel : BaseLocalModel
	{
		public GameMember_skin GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameMember_skin> GetAllElements()
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

		public static readonly string fileName = "GameMember_skin";

		private GameMember_skinModelImpl modelImpl = new GameMember_skinModelImpl();
	}
}

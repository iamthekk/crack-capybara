using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameSkill_skillModel : BaseLocalModel
	{
		public GameSkill_skill GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameSkill_skill> GetAllElements()
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

		public static readonly string fileName = "GameSkill_skill";

		private GameSkill_skillModelImpl modelImpl = new GameSkill_skillModelImpl();
	}
}

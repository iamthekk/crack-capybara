using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameSkillBuild_firstModel : BaseLocalModel
	{
		public GameSkillBuild_first GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameSkillBuild_first> GetAllElements()
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

		public static readonly string fileName = "GameSkillBuild_first";

		private GameSkillBuild_firstModelImpl modelImpl = new GameSkillBuild_firstModelImpl();
	}
}

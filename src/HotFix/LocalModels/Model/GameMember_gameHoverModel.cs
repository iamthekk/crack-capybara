using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameMember_gameHoverModel : BaseLocalModel
	{
		public GameMember_gameHover GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameMember_gameHover> GetAllElements()
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

		public static readonly string fileName = "GameMember_gameHover";

		private GameMember_gameHoverModelImpl modelImpl = new GameMember_gameHoverModelImpl();
	}
}

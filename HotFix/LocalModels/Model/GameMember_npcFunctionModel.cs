using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameMember_npcFunctionModel : BaseLocalModel
	{
		public GameMember_npcFunction GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameMember_npcFunction> GetAllElements()
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

		public static readonly string fileName = "GameMember_npcFunction";

		private GameMember_npcFunctionModelImpl modelImpl = new GameMember_npcFunctionModelImpl();
	}
}

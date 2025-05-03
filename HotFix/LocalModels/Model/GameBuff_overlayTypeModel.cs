using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameBuff_overlayTypeModel : BaseLocalModel
	{
		public GameBuff_overlayType GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameBuff_overlayType> GetAllElements()
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

		public static readonly string fileName = "GameBuff_overlayType";

		private GameBuff_overlayTypeModelImpl modelImpl = new GameBuff_overlayTypeModelImpl();
	}
}

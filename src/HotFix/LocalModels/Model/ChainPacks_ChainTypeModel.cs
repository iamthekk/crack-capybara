using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ChainPacks_ChainTypeModel : BaseLocalModel
	{
		public ChainPacks_ChainType GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ChainPacks_ChainType> GetAllElements()
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

		public static readonly string fileName = "ChainPacks_ChainType";

		private ChainPacks_ChainTypeModelImpl modelImpl = new ChainPacks_ChainTypeModelImpl();
	}
}

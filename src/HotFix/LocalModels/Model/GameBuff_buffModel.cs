using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameBuff_buffModel : BaseLocalModel
	{
		public GameBuff_buff GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameBuff_buff> GetAllElements()
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

		public static readonly string fileName = "GameBuff_buff";

		private GameBuff_buffModelImpl modelImpl = new GameBuff_buffModelImpl();
	}
}

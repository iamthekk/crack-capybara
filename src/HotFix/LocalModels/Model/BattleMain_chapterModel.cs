using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class BattleMain_chapterModel : BaseLocalModel
	{
		public BattleMain_chapter GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<BattleMain_chapter> GetAllElements()
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

		public static readonly string fileName = "BattleMain_chapter";

		private BattleMain_chapterModelImpl modelImpl = new BattleMain_chapterModelImpl();
	}
}

using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ChapterMiniGame_turntableBaseModel : BaseLocalModel
	{
		public ChapterMiniGame_turntableBase GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ChapterMiniGame_turntableBase> GetAllElements()
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

		public static readonly string fileName = "ChapterMiniGame_turntableBase";

		private ChapterMiniGame_turntableBaseModelImpl modelImpl = new ChapterMiniGame_turntableBaseModelImpl();
	}
}

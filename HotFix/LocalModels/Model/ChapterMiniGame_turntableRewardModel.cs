using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ChapterMiniGame_turntableRewardModel : BaseLocalModel
	{
		public ChapterMiniGame_turntableReward GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ChapterMiniGame_turntableReward> GetAllElements()
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

		public static readonly string fileName = "ChapterMiniGame_turntableReward";

		private ChapterMiniGame_turntableRewardModelImpl modelImpl = new ChapterMiniGame_turntableRewardModelImpl();
	}
}

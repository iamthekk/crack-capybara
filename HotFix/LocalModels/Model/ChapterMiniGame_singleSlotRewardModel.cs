using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ChapterMiniGame_singleSlotRewardModel : BaseLocalModel
	{
		public ChapterMiniGame_singleSlotReward GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ChapterMiniGame_singleSlotReward> GetAllElements()
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

		public static readonly string fileName = "ChapterMiniGame_singleSlotReward";

		private ChapterMiniGame_singleSlotRewardModelImpl modelImpl = new ChapterMiniGame_singleSlotRewardModelImpl();
	}
}

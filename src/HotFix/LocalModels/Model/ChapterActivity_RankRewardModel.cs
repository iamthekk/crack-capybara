using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ChapterActivity_RankRewardModel : BaseLocalModel
	{
		public ChapterActivity_RankReward GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ChapterActivity_RankReward> GetAllElements()
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

		public static readonly string fileName = "ChapterActivity_RankReward";

		private ChapterActivity_RankRewardModelImpl modelImpl = new ChapterActivity_RankRewardModelImpl();
	}
}

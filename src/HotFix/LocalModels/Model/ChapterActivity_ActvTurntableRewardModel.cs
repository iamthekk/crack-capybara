using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ChapterActivity_ActvTurntableRewardModel : BaseLocalModel
	{
		public ChapterActivity_ActvTurntableReward GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ChapterActivity_ActvTurntableReward> GetAllElements()
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

		public static readonly string fileName = "ChapterActivity_ActvTurntableReward";

		private ChapterActivity_ActvTurntableRewardModelImpl modelImpl = new ChapterActivity_ActvTurntableRewardModelImpl();
	}
}

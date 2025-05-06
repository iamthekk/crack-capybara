using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ChapterActivity_RankActivityModel : BaseLocalModel
	{
		public ChapterActivity_RankActivity GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ChapterActivity_RankActivity> GetAllElements()
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

		public static readonly string fileName = "ChapterActivity_RankActivity";

		private ChapterActivity_RankActivityModelImpl modelImpl = new ChapterActivity_RankActivityModelImpl();
	}
}

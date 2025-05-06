using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ChapterActivity_ModelModel : BaseLocalModel
	{
		public ChapterActivity_Model GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ChapterActivity_Model> GetAllElements()
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

		public static readonly string fileName = "ChapterActivity_Model";

		private ChapterActivity_ModelModelImpl modelImpl = new ChapterActivity_ModelModelImpl();
	}
}

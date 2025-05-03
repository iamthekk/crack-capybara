using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class AutoProcess_chapterModel : BaseLocalModel
	{
		public AutoProcess_chapter GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<AutoProcess_chapter> GetAllElements()
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

		public static readonly string fileName = "AutoProcess_chapter";

		private AutoProcess_chapterModelImpl modelImpl = new AutoProcess_chapterModelImpl();
	}
}

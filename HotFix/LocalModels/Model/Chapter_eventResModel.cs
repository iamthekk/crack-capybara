using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Chapter_eventResModel : BaseLocalModel
	{
		public Chapter_eventRes GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Chapter_eventRes> GetAllElements()
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

		public static readonly string fileName = "Chapter_eventRes";

		private Chapter_eventResModelImpl modelImpl = new Chapter_eventResModelImpl();
	}
}

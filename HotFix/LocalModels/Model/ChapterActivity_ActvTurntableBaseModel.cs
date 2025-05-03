using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ChapterActivity_ActvTurntableBaseModel : BaseLocalModel
	{
		public ChapterActivity_ActvTurntableBase GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ChapterActivity_ActvTurntableBase> GetAllElements()
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

		public static readonly string fileName = "ChapterActivity_ActvTurntableBase";

		private ChapterActivity_ActvTurntableBaseModelImpl modelImpl = new ChapterActivity_ActvTurntableBaseModelImpl();
	}
}

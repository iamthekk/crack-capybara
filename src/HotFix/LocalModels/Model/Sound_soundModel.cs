using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Sound_soundModel : BaseLocalModel
	{
		public Sound_sound GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Sound_sound> GetAllElements()
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

		public static readonly string fileName = "Sound_sound";

		private Sound_soundModelImpl modelImpl = new Sound_soundModelImpl();
	}
}

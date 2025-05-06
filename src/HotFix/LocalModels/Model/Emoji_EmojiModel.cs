using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Emoji_EmojiModel : BaseLocalModel
	{
		public Emoji_Emoji GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Emoji_Emoji> GetAllElements()
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

		public static readonly string fileName = "Emoji_Emoji";

		private Emoji_EmojiModelImpl modelImpl = new Emoji_EmojiModelImpl();
	}
}

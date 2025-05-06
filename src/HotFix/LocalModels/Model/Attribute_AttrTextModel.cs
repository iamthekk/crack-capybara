using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Attribute_AttrTextModel : BaseLocalModel
	{
		public Attribute_AttrText GetElementById(string id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Attribute_AttrText> GetAllElements()
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

		public static readonly string fileName = "Attribute_AttrText";

		private Attribute_AttrTextModelImpl modelImpl = new Attribute_AttrTextModelImpl();
	}
}

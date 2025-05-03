using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Const_ConstModel : BaseLocalModel
	{
		public Const_Const GetElementById(string id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Const_Const> GetAllElements()
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

		public static readonly string fileName = "Const_Const";

		private Const_ConstModelImpl modelImpl = new Const_ConstModelImpl();
	}
}

using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtMember_memberModel : BaseLocalModel
	{
		public ArtMember_member GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtMember_member> GetAllElements()
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

		public static readonly string fileName = "ArtMember_member";

		private ArtMember_memberModelImpl modelImpl = new ArtMember_memberModelImpl();
	}
}

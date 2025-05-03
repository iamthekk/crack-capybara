using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class SignIn_SignInModel : BaseLocalModel
	{
		public SignIn_SignIn GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<SignIn_SignIn> GetAllElements()
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

		public static readonly string fileName = "SignIn_SignIn";

		private SignIn_SignInModelImpl modelImpl = new SignIn_SignInModelImpl();
	}
}

using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class AutoProcess_skillModel : BaseLocalModel
	{
		public AutoProcess_skill GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<AutoProcess_skill> GetAllElements()
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

		public static readonly string fileName = "AutoProcess_skill";

		private AutoProcess_skillModelImpl modelImpl = new AutoProcess_skillModelImpl();
	}
}

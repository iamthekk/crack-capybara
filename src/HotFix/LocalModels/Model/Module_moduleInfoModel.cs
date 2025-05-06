using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Module_moduleInfoModel : BaseLocalModel
	{
		public Module_moduleInfo GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Module_moduleInfo> GetAllElements()
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

		public static readonly string fileName = "Module_moduleInfo";

		private Module_moduleInfoModelImpl modelImpl = new Module_moduleInfoModelImpl();
	}
}

using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Function_FunctionModel : BaseLocalModel
	{
		public Function_Function GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Function_Function> GetAllElements()
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

		public static readonly string fileName = "Function_Function";

		private Function_FunctionModelImpl modelImpl = new Function_FunctionModelImpl();
	}
}

using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class CommonActivity_PayObjModel : BaseLocalModel
	{
		public CommonActivity_PayObj GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<CommonActivity_PayObj> GetAllElements()
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

		public static readonly string fileName = "CommonActivity_PayObj";

		private CommonActivity_PayObjModelImpl modelImpl = new CommonActivity_PayObjModelImpl();
	}
}

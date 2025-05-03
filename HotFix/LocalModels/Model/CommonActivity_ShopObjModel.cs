using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class CommonActivity_ShopObjModel : BaseLocalModel
	{
		public CommonActivity_ShopObj GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<CommonActivity_ShopObj> GetAllElements()
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

		public static readonly string fileName = "CommonActivity_ShopObj";

		private CommonActivity_ShopObjModelImpl modelImpl = new CommonActivity_ShopObjModelImpl();
	}
}

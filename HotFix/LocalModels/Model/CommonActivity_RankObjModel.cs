using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class CommonActivity_RankObjModel : BaseLocalModel
	{
		public CommonActivity_RankObj GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<CommonActivity_RankObj> GetAllElements()
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

		public static readonly string fileName = "CommonActivity_RankObj";

		private CommonActivity_RankObjModelImpl modelImpl = new CommonActivity_RankObjModelImpl();
	}
}

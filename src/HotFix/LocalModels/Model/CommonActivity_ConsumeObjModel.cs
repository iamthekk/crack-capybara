using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class CommonActivity_ConsumeObjModel : BaseLocalModel
	{
		public CommonActivity_ConsumeObj GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<CommonActivity_ConsumeObj> GetAllElements()
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

		public static readonly string fileName = "CommonActivity_ConsumeObj";

		private CommonActivity_ConsumeObjModelImpl modelImpl = new CommonActivity_ConsumeObjModelImpl();
	}
}

using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class SevenDay_SevenDayTaskModel : BaseLocalModel
	{
		public SevenDay_SevenDayTask GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<SevenDay_SevenDayTask> GetAllElements()
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

		public static readonly string fileName = "SevenDay_SevenDayTask";

		private SevenDay_SevenDayTaskModelImpl modelImpl = new SevenDay_SevenDayTaskModelImpl();
	}
}

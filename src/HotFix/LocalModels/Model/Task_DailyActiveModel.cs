using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Task_DailyActiveModel : BaseLocalModel
	{
		public Task_DailyActive GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Task_DailyActive> GetAllElements()
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

		public static readonly string fileName = "Task_DailyActive";

		private Task_DailyActiveModelImpl modelImpl = new Task_DailyActiveModelImpl();
	}
}

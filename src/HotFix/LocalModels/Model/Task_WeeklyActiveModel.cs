using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Task_WeeklyActiveModel : BaseLocalModel
	{
		public Task_WeeklyActive GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Task_WeeklyActive> GetAllElements()
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

		public static readonly string fileName = "Task_WeeklyActive";

		private Task_WeeklyActiveModelImpl modelImpl = new Task_WeeklyActiveModelImpl();
	}
}

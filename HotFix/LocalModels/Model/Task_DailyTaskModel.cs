using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Task_DailyTaskModel : BaseLocalModel
	{
		public Task_DailyTask GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Task_DailyTask> GetAllElements()
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

		public static readonly string fileName = "Task_DailyTask";

		private Task_DailyTaskModelImpl modelImpl = new Task_DailyTaskModelImpl();
	}
}

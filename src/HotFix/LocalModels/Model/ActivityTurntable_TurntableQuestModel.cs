using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ActivityTurntable_TurntableQuestModel : BaseLocalModel
	{
		public ActivityTurntable_TurntableQuest GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ActivityTurntable_TurntableQuest> GetAllElements()
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

		public static readonly string fileName = "ActivityTurntable_TurntableQuest";

		private ActivityTurntable_TurntableQuestModelImpl modelImpl = new ActivityTurntable_TurntableQuestModelImpl();
	}
}

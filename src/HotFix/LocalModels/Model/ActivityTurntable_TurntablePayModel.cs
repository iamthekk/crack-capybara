using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ActivityTurntable_TurntablePayModel : BaseLocalModel
	{
		public ActivityTurntable_TurntablePay GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ActivityTurntable_TurntablePay> GetAllElements()
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

		public static readonly string fileName = "ActivityTurntable_TurntablePay";

		private ActivityTurntable_TurntablePayModelImpl modelImpl = new ActivityTurntable_TurntablePayModelImpl();
	}
}

using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Ride_RideModel : BaseLocalModel
	{
		public Ride_Ride GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Ride_Ride> GetAllElements()
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

		public static readonly string fileName = "Ride_Ride";

		private Ride_RideModelImpl modelImpl = new Ride_RideModelImpl();
	}
}

using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtRide_RideModel : BaseLocalModel
	{
		public ArtRide_Ride GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtRide_Ride> GetAllElements()
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

		public static readonly string fileName = "ArtRide_Ride";

		private ArtRide_RideModelImpl modelImpl = new ArtRide_RideModelImpl();
	}
}

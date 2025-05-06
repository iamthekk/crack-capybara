using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtRide_RideModelImpl : BaseLocalModelImpl<ArtRide_Ride, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtRide_Ride();
		}

		protected override int GetBeanKey(ArtRide_Ride bean)
		{
			return bean.id;
		}
	}
}

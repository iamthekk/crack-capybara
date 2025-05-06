using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Ride_RideModelImpl : BaseLocalModelImpl<Ride_Ride, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Ride_Ride();
		}

		protected override int GetBeanKey(Ride_Ride bean)
		{
			return bean.id;
		}
	}
}

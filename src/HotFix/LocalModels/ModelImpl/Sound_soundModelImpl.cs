using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Sound_soundModelImpl : BaseLocalModelImpl<Sound_sound, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Sound_sound();
		}

		protected override int GetBeanKey(Sound_sound bean)
		{
			return bean.ID;
		}
	}
}

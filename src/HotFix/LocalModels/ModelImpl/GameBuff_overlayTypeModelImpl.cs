using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameBuff_overlayTypeModelImpl : BaseLocalModelImpl<GameBuff_overlayType, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameBuff_overlayType();
		}

		protected override int GetBeanKey(GameBuff_overlayType bean)
		{
			return bean.id;
		}
	}
}

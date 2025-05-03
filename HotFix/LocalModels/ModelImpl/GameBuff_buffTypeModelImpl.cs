using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameBuff_buffTypeModelImpl : BaseLocalModelImpl<GameBuff_buffType, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameBuff_buffType();
		}

		protected override int GetBeanKey(GameBuff_buffType bean)
		{
			return bean.id;
		}
	}
}

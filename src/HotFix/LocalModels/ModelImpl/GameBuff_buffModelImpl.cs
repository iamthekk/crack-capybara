using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameBuff_buffModelImpl : BaseLocalModelImpl<GameBuff_buff, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameBuff_buff();
		}

		protected override int GetBeanKey(GameBuff_buff bean)
		{
			return bean.id;
		}
	}
}

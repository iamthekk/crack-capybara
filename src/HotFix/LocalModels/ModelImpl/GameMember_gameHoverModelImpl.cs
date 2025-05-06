using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameMember_gameHoverModelImpl : BaseLocalModelImpl<GameMember_gameHover, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameMember_gameHover();
		}

		protected override int GetBeanKey(GameMember_gameHover bean)
		{
			return bean.id;
		}
	}
}

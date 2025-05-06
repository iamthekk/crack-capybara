using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameMember_aiTypeModelImpl : BaseLocalModelImpl<GameMember_aiType, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameMember_aiType();
		}

		protected override int GetBeanKey(GameMember_aiType bean)
		{
			return bean.id;
		}
	}
}

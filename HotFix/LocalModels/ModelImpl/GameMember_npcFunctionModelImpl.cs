using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameMember_npcFunctionModelImpl : BaseLocalModelImpl<GameMember_npcFunction, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameMember_npcFunction();
		}

		protected override int GetBeanKey(GameMember_npcFunction bean)
		{
			return bean.id;
		}
	}
}

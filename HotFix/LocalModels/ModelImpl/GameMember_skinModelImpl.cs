using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameMember_skinModelImpl : BaseLocalModelImpl<GameMember_skin, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameMember_skin();
		}

		protected override int GetBeanKey(GameMember_skin bean)
		{
			return bean.id;
		}
	}
}

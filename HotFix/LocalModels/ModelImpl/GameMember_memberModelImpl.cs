using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameMember_memberModelImpl : BaseLocalModelImpl<GameMember_member, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameMember_member();
		}

		protected override int GetBeanKey(GameMember_member bean)
		{
			return bean.id;
		}
	}
}

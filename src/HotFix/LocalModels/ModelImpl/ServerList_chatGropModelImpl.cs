using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ServerList_chatGropModelImpl : BaseLocalModelImpl<ServerList_chatGrop, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ServerList_chatGrop();
		}

		protected override int GetBeanKey(ServerList_chatGrop bean)
		{
			return bean.id;
		}
	}
}

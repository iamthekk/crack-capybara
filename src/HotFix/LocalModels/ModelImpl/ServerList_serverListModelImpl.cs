using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ServerList_serverListModelImpl : BaseLocalModelImpl<ServerList_serverList, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ServerList_serverList();
		}

		protected override int GetBeanKey(ServerList_serverList bean)
		{
			return bean.id;
		}
	}
}

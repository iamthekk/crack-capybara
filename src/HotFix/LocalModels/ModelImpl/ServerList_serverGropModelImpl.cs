using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ServerList_serverGropModelImpl : BaseLocalModelImpl<ServerList_serverGrop, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ServerList_serverGrop();
		}

		protected override int GetBeanKey(ServerList_serverGrop bean)
		{
			return bean.id;
		}
	}
}

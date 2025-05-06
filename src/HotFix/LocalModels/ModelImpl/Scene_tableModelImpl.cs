using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Scene_tableModelImpl : BaseLocalModelImpl<Scene_table, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Scene_table();
		}

		protected override int GetBeanKey(Scene_table bean)
		{
			return bean.id;
		}
	}
}

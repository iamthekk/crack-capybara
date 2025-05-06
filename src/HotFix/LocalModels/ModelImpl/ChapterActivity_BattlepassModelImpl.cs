using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_BattlepassModelImpl : BaseLocalModelImpl<ChapterActivity_Battlepass, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_Battlepass();
		}

		protected override int GetBeanKey(ChapterActivity_Battlepass bean)
		{
			return bean.id;
		}
	}
}

using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Emoji_EmojiModelImpl : BaseLocalModelImpl<Emoji_Emoji, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Emoji_Emoji();
		}

		protected override int GetBeanKey(Emoji_Emoji bean)
		{
			return bean.id;
		}
	}
}

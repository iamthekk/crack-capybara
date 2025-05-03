using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Attribute_AttrTextModelImpl : BaseLocalModelImpl<Attribute_AttrText, string>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Attribute_AttrText();
		}

		protected override string GetBeanKey(Attribute_AttrText bean)
		{
			return bean.ID;
		}
	}
}

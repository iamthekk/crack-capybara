using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public abstract class BaseFlyItem : CustomBehaviour
	{
		public virtual void SetData(object param)
		{
		}

		public CustomImage m_icon;
	}
}

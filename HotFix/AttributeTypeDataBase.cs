using System;
using Framework.Logic.UI;

namespace HotFix
{
	public abstract class AttributeTypeDataBase
	{
		public abstract void SetImage(CustomImage image);

		public string m_value = string.Empty;

		public string m_tgaValue = string.Empty;
	}
}

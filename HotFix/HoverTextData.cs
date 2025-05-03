using System;

namespace HotFix
{
	public class HoverTextData
	{
		public HoverTextData(EHoverTextType hoverTxetType, string textId)
		{
			this.hoverTxetType = hoverTxetType;
			this.textId = textId;
		}

		public EHoverTextType hoverTxetType;

		public string textId;
	}
}

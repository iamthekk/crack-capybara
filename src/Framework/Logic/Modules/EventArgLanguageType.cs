using System;
using Framework.EventSystem;

namespace Framework.Logic.Modules
{
	public class EventArgLanguageType : BaseEventArgs
	{
		public LanguageType LanguageType
		{
			get
			{
				return this.m_languageType;
			}
		}

		public void SetData(LanguageType languageType)
		{
			this.m_languageType = languageType;
		}

		public override void Clear()
		{
			this.m_languageType = LanguageType.English;
		}

		private LanguageType m_languageType;
	}
}

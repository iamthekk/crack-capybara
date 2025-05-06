using System;

namespace HotFix
{
	public class UIMainPageData
	{
		public UIMainPageData(int pageName, UIBaseMainPageNode pagePrefab, string tabName, UIBaseMainPageTabNode tabNode, string redPointName, string languageID, int functionID)
		{
			this.m_pageName = pageName;
			this.m_pagePrefab = pagePrefab;
			this.m_tabName = tabName;
			this.m_tabNode = tabNode;
			this.m_redPointName = redPointName;
			this.m_languageID = languageID;
			this.m_functionID = functionID;
		}

		public int m_pageName;

		public UIBaseMainPageNode m_pagePrefab;

		public string m_tabName;

		public UIBaseMainPageTabNode m_tabNode;

		public string m_redPointName;

		public string m_languageID;

		public int m_functionID;
	}
}

using System;
using System.Collections.Generic;
using Framework;

namespace HotFix
{
	[RuntimeDefaultSerializedProperty]
	public class MainOpenData
	{
		public UIMainPageName m_pageName;

		public List<MainOpenViewData> m_openViewDatas = new List<MainOpenViewData>();
	}
}

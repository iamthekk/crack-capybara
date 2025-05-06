using System;
using Framework;
using Framework.ViewModule;

namespace HotFix
{
	[RuntimeDefaultSerializedProperty]
	public class MainOpenViewData
	{
		public int m_viewID;

		public UILayers m_layer;

		public object m_openData;
	}
}

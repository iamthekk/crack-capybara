using System;
using Framework.ViewModule;

namespace HotFix
{
	public class WindowQueueInfo
	{
		public WindowQueueInfo(ViewName viewName, object openData = null, UILayers layers = 1)
		{
			this.ViewName = viewName;
			this.OpenData = openData;
			this.Layers = layers;
		}

		public ViewName ViewName;

		public object OpenData;

		public UILayers Layers;
	}
}

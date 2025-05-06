using System;
using Framework.DataModule;
using Framework.EventSystem;

namespace HotFix
{
	public class ActivityCommonDataModule : IDataModule
	{
		public int GetName()
		{
			return 136;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}
	}
}

using System;
using Framework.EventSystem;

namespace Framework.DataModule
{
	public interface IDataModule
	{
		int GetName();

		void RegisterEvents(EventSystemManager manager);

		void UnRegisterEvents(EventSystemManager manager);

		void Reset();
	}
}

using System;
using System.Threading.Tasks;

namespace Framework.ViewModule
{
	public abstract class BaseViewModuleLoader
	{
		public abstract Task OnLoad(object data);

		public abstract void OnUnLoad();
	}
}

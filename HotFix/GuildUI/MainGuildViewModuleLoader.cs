using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.ViewModule;

namespace HotFix.GuildUI
{
	public class MainGuildViewModuleLoader : BaseViewModuleLoader
	{
		public GuildProxy.GuildPreLoad mPreload { get; private set; }

		public override async Task OnLoad(object data)
		{
			this.mPreload = new GuildProxy.GuildPreLoad();
			await Task.WhenAll(new List<Task>
			{
				this.mPreload.LoadGameObject("UI_MainGuildInfo"),
				this.mPreload.LoadGameObject("UI_GuildBuilding")
			});
		}

		public override void OnUnLoad()
		{
			if (this.mPreload != null)
			{
				this.mPreload.ReleaseAll();
				this.mPreload = null;
			}
		}
	}
}

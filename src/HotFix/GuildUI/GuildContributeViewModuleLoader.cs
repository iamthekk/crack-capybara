using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using UnityEngine.U2D;

namespace HotFix.GuildUI
{
	public class GuildContributeViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadPageObjects = new LoadPool<SpriteAtlas>();
			list.Add(this.m_loadPageObjects.Load(GameApp.Table.GetAtlasPath(8)));
			await Task.WhenAll(list);
		}

		public override void OnUnLoad()
		{
			if (this.m_loadPageObjects != null)
			{
				this.m_loadPageObjects.UnLoadAll();
				this.m_loadPageObjects = null;
			}
		}

		public LoadPool<SpriteAtlas> m_loadPageObjects;
	}
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using UnityEngine.U2D;

namespace HotFix.GuildUI
{
	public class GuildCheckSimplePopViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadSpriteAtlas = new LoadPool<SpriteAtlas>();
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(48)));
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(49)));
			await Task.WhenAll(list);
		}

		public override void OnUnLoad()
		{
			if (this.m_loadSpriteAtlas != null)
			{
				this.m_loadSpriteAtlas.UnLoadAll();
				this.m_loadSpriteAtlas = null;
			}
		}

		public LoadPool<SpriteAtlas> m_loadSpriteAtlas;
	}
}

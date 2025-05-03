using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.ViewModule;
using UnityEngine.U2D;

namespace HotFix
{
	public class MainCityBoxViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			IEnumerable<Task> enumerable = new List<Task>();
			this.m_loadSpriteAtlas = new LoadPool<SpriteAtlas>();
			await Task.WhenAll(enumerable);
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

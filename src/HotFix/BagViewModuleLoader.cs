using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using UnityEngine.U2D;

namespace HotFix
{
	public class BagViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_atlasList = new LoadPool<SpriteAtlas>();
			list.Add(this.m_atlasList.Load(GameApp.Table.GetAtlasPath(105)));
			list.Add(this.m_atlasList.Load(GameApp.Table.GetAtlasPath(107)));
			await Task.WhenAll(list);
		}

		public override void OnUnLoad()
		{
			if (this.m_atlasList != null)
			{
				this.m_atlasList.UnLoadAll();
				this.m_atlasList = null;
			}
		}

		public LoadPool<SpriteAtlas> m_atlasList;
	}
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using UnityEngine.U2D;

namespace HotFix
{
	public class BattleRogueDungeonViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadSpriteAtlas = new LoadPool<SpriteAtlas>();
			List<string> list2 = new List<string>();
			list2.Add(GameApp.Table.GetAtlasPath(101));
			list2.Add(GameApp.Table.GetAtlasPath(115));
			list2.Add(GameApp.Table.GetAtlasPath(102));
			for (int i = 0; i < list2.Count; i++)
			{
				list.Add(this.m_loadSpriteAtlas.Load(list2[i]));
			}
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

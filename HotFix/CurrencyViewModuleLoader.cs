using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine.U2D;

namespace HotFix
{
	public class CurrencyViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadSpriteAtlas = new LoadPool<SpriteAtlas>();
			List<int> list2 = new List<int>();
			foreach (Avatar_Avatar avatar_Avatar in GameApp.Table.GetManager().GetAvatar_AvatarElements())
			{
				if (avatar_Avatar.atlasId > 0 && !list2.Contains(avatar_Avatar.atlasId))
				{
					list2.Add(avatar_Avatar.atlasId);
					list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(avatar_Avatar.atlasId)));
				}
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

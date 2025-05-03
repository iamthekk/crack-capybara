using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine.U2D;

namespace HotFix
{
	public class PlayerAvatarClothesViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadSpriteAtlas = new LoadPool<SpriteAtlas>();
			List<int> list2 = new List<int>();
			foreach (Avatar_Avatar avatar_Avatar in GameApp.Table.GetManager().GetAvatar_AvatarElements())
			{
				if (!list2.Contains(avatar_Avatar.atlasId))
				{
					list2.Add(avatar_Avatar.atlasId);
					list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(avatar_Avatar.atlasId)));
				}
			}
			foreach (Avatar_Skin avatar_Skin in GameApp.Table.GetManager().GetAvatar_SkinElements())
			{
				if (!list2.Contains(avatar_Skin.atlasId))
				{
					list2.Add(avatar_Skin.atlasId);
					list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(avatar_Skin.atlasId)));
				}
				if (!list2.Contains(avatar_Skin.quality_atlasId))
				{
					list2.Add(avatar_Skin.quality_atlasId);
					list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(avatar_Skin.quality_atlasId)));
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

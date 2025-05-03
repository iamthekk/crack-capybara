using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine.U2D;

namespace HotFix
{
	public class ActivityViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadSpriteAtlas = new LoadPool<SpriteAtlas>();
			List<int> list2 = new List<int>();
			foreach (CommonActivity_CommonActivity commonActivity_CommonActivity in GameApp.Table.GetManager().GetCommonActivity_CommonActivityModelInstance().GetAllElements())
			{
				if (!list2.Contains(commonActivity_CommonActivity.atlasID))
				{
					list2.Add(commonActivity_CommonActivity.atlasID);
					list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(commonActivity_CommonActivity.atlasID)));
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

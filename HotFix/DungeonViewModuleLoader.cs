using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class DungeonViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadSprite = new LoadPool<GameObject>();
			IList<Dungeon_DungeonBase> allElements = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				string bgPath = DungeonViewModule.GetBgPath((DungeonID)allElements[i].id);
				if (!string.IsNullOrEmpty(bgPath))
				{
					list.Add(this.m_loadSprite.Load(bgPath));
				}
			}
			await Task.WhenAll(list);
		}

		public override void OnUnLoad()
		{
			if (this.m_loadSprite != null)
			{
				this.m_loadSprite.UnLoadAll();
				this.m_loadSprite = null;
			}
		}

		private LoadPool<GameObject> m_loadSprite;
	}
}

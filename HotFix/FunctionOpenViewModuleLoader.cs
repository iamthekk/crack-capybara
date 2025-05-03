using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine.U2D;

namespace HotFix
{
	public class FunctionOpenViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadSpriteAtlas = new LoadPool<SpriteAtlas>();
			IEnumerable<Function_Function> allElements = GameApp.Table.GetManager().GetFunction_FunctionModelInstance().GetAllElements();
			List<int> list2 = new List<int>();
			foreach (Function_Function function_Function in allElements)
			{
				if (function_Function.iconAtlasID != 0 && !list2.Contains(function_Function.iconAtlasID))
				{
					list2.Add(function_Function.iconAtlasID);
					string atlasPath = GameApp.Table.GetAtlasPath(function_Function.iconAtlasID);
					if (!string.IsNullOrEmpty(atlasPath))
					{
						list.Add(this.m_loadSpriteAtlas.Load(atlasPath));
					}
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

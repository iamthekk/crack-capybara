using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildShopViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadPageObjects = new LoadPool<GameObject>();
			list.Add(this.m_loadPageObjects.Load(MainShopConst.MainShopPanelPathDic[MainShopType.GuildShop]));
			list.Add(this.m_loadPageObjects.Load(MainShopConst.MainShopPanelPathDic[MainShopType.ManaCrystalShop]));
			list.Add(this.m_loadPageObjects.Load(MainShopConst.MainShopPanelPathDic[MainShopType.BlackMarket]));
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

		public LoadPool<GameObject> m_loadPageObjects;
	}
}

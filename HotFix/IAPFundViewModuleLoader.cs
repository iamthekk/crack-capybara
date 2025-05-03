using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class IAPFundViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadSprite = new LoadPool<GameObject>();
			list.Add(this.m_loadSprite.Load("Assets/_Resources/Prefab/UI/IAPFund/IAPFundBattlePassNode.prefab"));
			list.Add(this.m_loadSprite.Load("Assets/_Resources/Prefab/UI/IAPFund/IAPFundTalentNode.prefab"));
			list.Add(this.m_loadSprite.Load("Assets/_Resources/Prefab/UI/IAPFund/IAPFundTowerNode.prefab"));
			list.Add(this.m_loadSprite.Load("Assets/_Resources/Prefab/UI/IAPFund/IAPFundRogueDungeonNode.prefab"));
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

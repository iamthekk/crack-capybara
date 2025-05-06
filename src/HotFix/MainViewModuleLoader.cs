using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.U2D;

namespace HotFix
{
	public class MainViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadGameObjects = new LoadPool<GameObject>();
			HeroDataModule dataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			if (dataModule.MainCardData != null)
			{
				GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(dataModule.MainCardData.m_memberID);
				ArtMember_member elementById2 = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(elementById.modelID);
				list.Add(this.m_loadGameObjects.Load(elementById2.path));
			}
			this.m_loadPageObjects = new LoadPool<GameObject>();
			list.Add(this.m_loadPageObjects.Load("Assets/_Resources/Prefab/UI/Main/UIMainShop.prefab"));
			list.Add(this.m_loadPageObjects.Load("Assets/_Resources/Prefab/UI/Main/UIMainEquip.prefab"));
			list.Add(this.m_loadPageObjects.Load("Assets/_Resources/Prefab/UI/Main/UIMainCity.prefab"));
			list.Add(this.m_loadPageObjects.Load("Assets/_Resources/Prefab/UI/Main/UIMainTalent.prefab"));
			list.Add(this.m_loadPageObjects.Load("Assets/_Resources/Prefab/UI/Main/UIMainChest.prefab"));
			list.Add(this.m_loadPageObjects.Load("Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacyRank.prefab"));
			list.Add(this.m_loadPageObjects.Load("Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacyMain.prefab"));
			list.Add(this.m_loadPageObjects.Load("Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacyTree.prefab"));
			list.Add(this.m_loadPageObjects.Load(MainShopConst.MainShopPanelPathDic[MainShopType.EquipShop]));
			list.Add(this.m_loadPageObjects.Load(MainShopConst.MainShopPanelPathDic[MainShopType.BlackMarket]));
			list.Add(this.m_loadPageObjects.Load(MainShopConst.MainShopPanelPathDic[MainShopType.SuperValue]));
			list.Add(this.m_loadPageObjects.Load(MainShopConst.MainShopPanelPathDic[MainShopType.GiftShop]));
			list.Add(this.m_loadPageObjects.Load(MainShopConst.MainShopPanelPathDic[MainShopType.DiamondShop]));
			this.m_loadSpriteAtlas = new LoadPool<SpriteAtlas>();
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(106)));
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(125)));
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(131)));
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(160)));
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(162)));
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(161)));
			await Task.WhenAll(list);
		}

		public override void OnUnLoad()
		{
			if (this.m_loadGameObjects != null)
			{
				this.m_loadGameObjects.UnLoadAll();
				this.m_loadGameObjects = null;
			}
			if (this.m_loadPageObjects != null)
			{
				this.m_loadPageObjects.UnLoadAll();
				this.m_loadPageObjects = null;
			}
			if (this.m_loadSpriteAtlas != null)
			{
				this.m_loadSpriteAtlas.UnLoadAll();
				this.m_loadSpriteAtlas = null;
			}
		}

		public LoadPool<GameObject> m_loadGameObjects;

		public LoadPool<GameObject> m_loadPageObjects;

		public LoadPool<SpriteAtlas> m_loadSpriteAtlas;
	}
}

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
	public class PetViewModuleLoader : BaseViewModuleLoader
	{
		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_loadSpriteAtlas = new LoadPool<SpriteAtlas>();
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(117)));
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(105)));
			list.Add(this.m_loadSpriteAtlas.Load(GameApp.Table.GetAtlasPath(107)));
			this.m_loadGameObjects = new LoadPool<GameObject>();
			List<ulong> petShowRowIds = GameApp.Data.GetDataModule(DataName.PetDataModule).PetShowRowIds;
			for (int i = 0; i < petShowRowIds.Count; i++)
			{
				PetData petData = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetData(petShowRowIds[i]);
				if (petData != null)
				{
					ArtMember_member elementById = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(petData.CfgPetData.memberId);
					if (elementById != null)
					{
						list.Add(this.m_loadGameObjects.Load(elementById.path));
					}
				}
			}
			List<ulong> formationRowIds = GameApp.Data.GetDataModule(DataName.PetDataModule).FormationRowIds;
			for (int j = 0; j < formationRowIds.Count; j++)
			{
				if (formationRowIds[j] > 0UL)
				{
					PetData petData2 = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetData(formationRowIds[j]);
					if (petData2 != null)
					{
						ArtMember_member elementById2 = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(petData2.CfgPetData.memberId);
						if (elementById2 != null)
						{
							list.Add(this.m_loadGameObjects.Load(elementById2.path));
						}
					}
				}
			}
			await Task.WhenAll(list);
			this.m_loadGameObjects.UnLoadAll();
			this.m_loadGameObjects = null;
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

		public LoadPool<GameObject> m_loadGameObjects;
	}
}

using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class PetTrainingViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.petTrainingPage.Init();
			this.petTrainingSelectPage.Init();
			this.petTrainingSelectPage.Hide();
			if (data != null && data is PetTrainingViewModule.OpenData)
			{
				this.openData = data as PetTrainingViewModule.OpenData;
			}
			if (this.openData != null)
			{
				this.curSelectConfigId = this.openData.petId;
			}
			this.RefreshPetListData();
			this.RefreshSelectData();
			this.petTrainingPage.Show();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.petTrainingPage.DeInit();
			this.petTrainingSelectPage.DeInit();
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnBgClose.m_onClick = new Action(this.OnBgCloseClick);
			manager.RegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.OnEventItemUpdate));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnBgClose.m_onClick = null;
			manager.UnRegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.OnEventItemUpdate));
		}

		private void OnBgCloseClick()
		{
			GameApp.View.CloseView(ViewName.PetTrainingViewModule, null);
		}

		public void UpdateCurIndex(int index)
		{
			this.curIndex = index;
			bool flag = this.curSelectConfigId != this.petListData[this.curIndex].petId;
			this.curSelectConfigId = this.petListData[this.curIndex].petId;
			this.curPetData = this.petListData[this.curIndex];
			if (flag)
			{
				this.petTrainingPage.UpdateView();
			}
			this.UpdatePetAvatar();
		}

		public void UpdateCurSelectConfigId(int petId)
		{
			bool flag = this.curSelectConfigId != petId;
			this.curSelectConfigId = petId;
			this.RefreshSelectData();
			if (flag)
			{
				this.petTrainingPage.UpdateView();
			}
			this.UpdatePetAvatar();
		}

		private async void ShowPlayerModel(int memberId)
		{
			if (!this.cacheMemberId.Equals(memberId))
			{
				this.cacheMemberId = memberId;
				ArtMember_member elementById = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(memberId);
				this.uiSpineModelItem.SetScale(elementById.uiScale);
				await this.uiSpineModelItem.ShowModel(memberId, 0, "Idle", true);
			}
		}

		private void RefreshPetListData()
		{
			this.petListData.Clear();
			PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			List<PetData> list = dataModule.GetPetList(EPetFilterType.Fight).SortByType(this.m_sortType);
			List<PetData> list2 = dataModule.GetPetList(EPetFilterType.Assist).SortByType(this.m_sortType);
			List<PetData> list3 = dataModule.GetPetList(EPetFilterType.Idle).SortByType(this.m_sortType);
			this.petListData.AddRange(list);
			this.petListData.AddRange(list2);
			this.petListData.AddRange(list3);
		}

		private void RefreshSelectData()
		{
			bool flag = false;
			for (int i = 0; i < this.petListData.Count; i++)
			{
				PetData petData = this.petListData[i];
				if (this.curSelectConfigId > 0 && petData.petId.Equals(this.curSelectConfigId))
				{
					this.curIndex = i;
					this.curPetData = petData;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.curSelectConfigId = ((this.petListData.Count > 0) ? this.petListData[0].petId : 0);
				this.curPetData = ((this.petListData.Count > 0) ? this.petListData[0] : null);
				this.curIndex = 0;
			}
			this.UpdatePetAvatar();
		}

		private void UpdatePetAvatar()
		{
			if (this.curPetData != null)
			{
				Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this.curPetData.petId);
				this.ShowPlayerModel(elementById.memberId);
			}
		}

		private void OnEventItemUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.petTrainingPage != null && this.petTrainingPage.gameObject.activeSelf)
			{
				this.petTrainingPage.UpdateTrainingCost();
			}
		}

		public CustomButton btnBgClose;

		public PetTrainingPage petTrainingPage;

		public PetTrainingSelectPage petTrainingSelectPage;

		[Header("宠物模型")]
		public UISpineModelItem uiSpineModelItem;

		[NonSerialized]
		public List<PetData> petListData = new List<PetData>();

		[NonSerialized]
		public EPetSortType m_sortType;

		[NonSerialized]
		public int curIndex;

		[NonSerialized]
		public int curSelectConfigId;

		[NonSerialized]
		public PetData curPetData;

		private PetTrainingViewModule.OpenData openData;

		private int cacheMemberId = -1;

		public class OpenData
		{
			public int petId;
		}
	}
}

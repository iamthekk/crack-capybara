using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using Proto.Common;
using UnityEngine;

namespace HotFix
{
	public class PetOpenEggViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.pageOpenEggTemplate.gameObject.SetActive(false);
			this.rewardShowViewTemplate.gameObject.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_PetOpenEggViewModule_RefreshData, new HandlerEvent(this.OnEventRefreshDrawResultData));
			this.openData = data as PetOpenEggViewModule.OpenData;
			GameApp.Sound.PlayClip(671, 1f);
			this.ShowUI();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this._pageRewardShow != null && this._pageRewardShow.gameObject.activeSelf)
			{
				this._pageRewardShow.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_PetOpenEggViewModule_RefreshData, new HandlerEvent(this.OnEventRefreshDrawResultData));
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnEventRefreshDrawResultData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsDrawPetResultData eventArgsDrawPetResultData = eventargs as EventArgsDrawPetResultData;
			this.openData = eventArgsDrawPetResultData.openData;
			this.ShowUI();
		}

		private void ShowUI()
		{
			if (this.openData == null)
			{
				HLog.LogError("PetOpenEggViewModule OnOpen data is null");
				return;
			}
			if (this._pageOpenEgg != null)
			{
				Object.Destroy(this._pageOpenEgg.gameObject);
			}
			if (this._pageRewardShow != null)
			{
				Object.Destroy(this._pageRewardShow.gameObject);
			}
			this._pageOpenEgg = Object.Instantiate<PetOpenEggViewModule_PageOpenEgg>(this.pageOpenEggTemplate, this.pageOpenEggTemplate.transform.parent, false);
			this._pageRewardShow = Object.Instantiate<PetOpenEggViewModule_PagePetDrawRewardShow>(this.rewardShowViewTemplate, this.rewardShowViewTemplate.transform.parent, false);
			this._pageOpenEgg.OnInit(this.openData.petBoxType, this.openData.petList, this.openData.newPetRowIds);
			this._pageRewardShow.Init();
			if (GameApp.Data.GetDataModule(DataName.PetDataModule).IsSkipAni)
			{
				this._pageOpenEgg.gameObject.SetActive(false);
				this._pageRewardShow.gameObject.SetActive(true);
				base.Invoke("PlayRewardAnimation", 0.1f);
				return;
			}
			if (this.openData.petList != null && this.openData.petList.Count > 0)
			{
				this._pageOpenEgg.gameObject.SetActive(true);
				this._pageRewardShow.gameObject.SetActive(false);
				base.Invoke("PlayOpenEggAnim", 0.1f);
				return;
			}
			this._pageOpenEgg.gameObject.SetActive(false);
			this._pageRewardShow.gameObject.SetActive(true);
			base.Invoke("PlayRewardAnimation", 0.1f);
		}

		private void PlayOpenEggAnim()
		{
			this._pageOpenEgg.gameObject.SetActive(true);
			this._pageRewardShow.gameObject.SetActive(false);
			this._pageOpenEgg.PlayOpenEggsAnimation(new Action(this.PlayRewardAnimation));
		}

		private void PlayRewardAnimation()
		{
			GameApp.Sound.PlayClip(663, 1f);
			this._pageOpenEgg.gameObject.SetActive(false);
			this._pageRewardShow.gameObject.SetActive(true);
			this._pageRewardShow.ShowRewards((EPetBoxType)this.openData.petBoxType, this.openData.rewardList, new Action(this.OnBtnCloseClick));
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.PetOpenEggViewModule, null);
		}

		public PetOpenEggViewModule_PageOpenEgg pageOpenEggTemplate;

		public PetOpenEggViewModule_PagePetDrawRewardShow rewardShowViewTemplate;

		private PetOpenEggViewModule.OpenData openData;

		private PetOpenEggViewModule_PageOpenEgg _pageOpenEgg;

		private PetOpenEggViewModule_PagePetDrawRewardShow _pageRewardShow;

		public class OpenData
		{
			public int petBoxType;

			public List<PetDto> petList;

			public List<ulong> newPetRowIds;

			public List<ItemData> rewardList;
		}
	}
}

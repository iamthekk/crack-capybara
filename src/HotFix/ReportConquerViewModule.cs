using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.Common;
using Proto.Social;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class ReportConquerViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.m_reportDataModule = GameApp.Data.GetDataModule(DataName.ReportConquerDataModule);
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as ReportConquerViewModule.OpenData;
			this.m_titleGroup.Init();
			this.m_contentGroup.Init();
			this.m_playerGroup.Init();
			this.m_equipGroup.Init();
			this.m_content.gameObject.SetActive(false);
			this.m_buttons.gameObject.SetActive(false);
			this.m_netLoading.SetActive(true);
			if (this.m_openData != null)
			{
				SocialityInteractiveData interactiveData = this.m_openData.m_socialityInteractiveData;
				NetworkUtils.Sociality.DoInteractDetailRequest(interactiveData.m_rowID, delegate(bool isOk, InteractDetailResponse response)
				{
					if (!isOk)
					{
						return;
					}
					EventArgsSetReportConquerData eventArgsSetReportConquerData = new EventArgsSetReportConquerData();
					eventArgsSetReportConquerData.SetData(interactiveData, response);
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_ReportConquerData_SetData, eventArgsSetReportConquerData);
					this.RefreshUI();
				});
			}
			else
			{
				this.RefreshUI();
			}
			this.m_maskBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			this.m_closeBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			this.m_playBackBt.onClick.AddListener(new UnityAction(this.OnClickPlayBackBt));
			this.m_gotoCityBt.onClick.AddListener(new UnityAction(this.OnClickGotoCityBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_maskBt.onClick.RemoveAllListeners();
			this.m_closeBt.onClick.RemoveAllListeners();
			this.m_playBackBt.onClick.RemoveAllListeners();
			this.m_gotoCityBt.onClick.RemoveAllListeners();
			this.m_titleGroup.DeInit();
			this.m_contentGroup.DeInit();
			this.m_playerGroup.DeInit();
			this.m_equipGroup.DeInit();
			this.m_openData = null;
			this.m_targetUserDto = null;
		}

		public override void OnDelete()
		{
			this.m_loginDataModule = null;
			this.m_reportDataModule = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.ReportConquerViewModule, null);
		}

		private void OnClickPlayBackBt()
		{
			if (this.m_reportDataModule == null || this.m_reportDataModule.m_interactDetailResponse == null)
			{
				return;
			}
			EventArgsBattleConquerEnter instance = Singleton<EventArgsBattleConquerEnter>.Instance;
			instance.SetData(this.m_reportDataModule.m_interactDetailResponse.Record, true);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleConquer_BattleConquerEnter, instance);
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					EventArgsRefreshMainOpenData instance2 = Singleton<EventArgsRefreshMainOpenData>.Instance;
					instance2.SetData(DxxTools.UI.GetConquerForMainOpenData());
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance2);
					GameApp.View.CloseView(ViewName.ReportConquerViewModule, null);
					EventArgsGameDataEnter instance3 = Singleton<EventArgsGameDataEnter>.Instance;
					instance3.SetData(GameModel.Conquer, null);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance3);
					GameApp.State.ActiveState(StateName.BattleConquerState);
				});
			});
		}

		private void OnClickGotoCityBt()
		{
			OtherMainCityViewModule.OpenData openData = new OtherMainCityViewModule.OpenData();
			openData.m_userID = this.m_targetUserDto.UserId;
			if (GameApp.View.IsOpened(ViewName.OtherMainCityViewModule))
			{
				GameApp.View.CloseView(ViewName.OtherMainCityViewModule, null);
			}
			GameApp.View.OpenView(ViewName.OtherMainCityViewModule, openData, 1, null, null);
			GameApp.View.CloseView(ViewName.ReportConquerViewModule, null);
		}

		public void RefreshUI()
		{
			if (this.m_reportDataModule == null || this.m_reportDataModule.m_interactDetailResponse == null || this.m_reportDataModule.m_socialityInteractiveData == null)
			{
				return;
			}
			if (this.m_netLoading != null)
			{
				this.m_netLoading.SetActive(false);
			}
			if (this.m_content != null)
			{
				this.m_content.gameObject.SetActive(true);
			}
			this.m_targetUserDto = null;
			long userId = this.m_loginDataModule.userId;
			this.m_targetUserDto = ((userId == this.m_reportDataModule.m_interactDetailResponse.Record.OtherUser.UserId) ? null : this.m_reportDataModule.m_interactDetailResponse.Record.OtherUser);
			if (this.m_targetUserDto == null)
			{
				this.m_targetUserDto = ((userId == this.m_reportDataModule.m_interactDetailResponse.Record.OwnerUser.UserId) ? null : this.m_reportDataModule.m_interactDetailResponse.Record.OwnerUser);
			}
			if (this.m_titleGroup != null)
			{
				this.m_titleGroup.RefreshUI(this.m_reportDataModule.m_socialityInteractiveData);
			}
			if (this.m_contentGroup != null)
			{
				this.m_contentGroup.RefreshUI(this.m_reportDataModule.m_socialityInteractiveData);
			}
			if (this.m_playerGroup != null)
			{
				this.m_playerGroup.RefreshUI(this.m_targetUserDto);
			}
			if (this.m_equipGroup != null)
			{
				this.m_equipGroup.RefreshUI(this.m_targetUserDto);
			}
			bool flag = GameApp.Table.GetManager().GetSociality_ReportModelInstance().GetElementById(this.m_reportDataModule.m_socialityInteractiveData.m_id)
				.playBack != 0;
			if (this.m_playBackBt != null)
			{
				this.m_playBackBt.gameObject.SetActive(flag);
			}
			if (this.m_buttons != null)
			{
				this.m_buttons.gameObject.SetActive(true);
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_buttons);
		}

		public CustomButton m_maskBt;

		public CustomButton m_closeBt;

		public RectTransform m_content;

		public GameObject m_netLoading;

		public ReportConquerTitleGroup m_titleGroup;

		public ReportConquerConentGroup m_contentGroup;

		public ReportConquerPlayerGroup m_playerGroup;

		public ReportConquerEquipGroup m_equipGroup;

		public RectTransform m_buttons;

		public CustomButton m_playBackBt;

		public CustomButton m_gotoCityBt;

		public ReportConquerViewModule.OpenData m_openData;

		public BattleUserDto m_targetUserDto;

		private LoginDataModule m_loginDataModule;

		public ReportConquerDataModule m_reportDataModule;

		public class OpenData
		{
			public OpenData(SocialityInteractiveData socialityInteractiveData)
			{
				this.m_socialityInteractiveData = socialityInteractiveData;
			}

			public SocialityInteractiveData m_socialityInteractiveData;
		}
	}
}

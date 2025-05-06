using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.User;
using Server;
using UnityEngine;

namespace HotFix
{
	public class PlayerInformationViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_buttons.SetActive(false);
			this.m_content.SetActive(false);
			this.m_netLoading.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			this.m_playerInformationDataModule = GameApp.Data.GetDataModule(DataName.PlayerInformationDataModule);
			this.m_openData = data as PlayerInformationViewModule.OpenData;
			if (this.m_openData == null)
			{
				return;
			}
			this.m_content.gameObject.SetActive(false);
			this.m_netLoading.gameObject.SetActive(true);
			this.m_popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.m_playerGroup.Init();
			this.m_equipGroup.Init();
			this.m_petsNode.Init();
			this.m_mountsNode.Init();
			this.m_artifactsNode.Init();
			if (this.m_openData != null)
			{
				long userId = this.m_openData.userId;
				PlayerInfoDto playerInfoDto;
				if (this.m_playerInformationDataModule.TryGetCachePlayerInfo(userId, out playerInfoDto))
				{
					this.m_playerInfoDto = playerInfoDto;
					this.UpdateMemberAttributeData();
					this.RefreshUI();
					return;
				}
				if (this.m_netLoading != null)
				{
					this.m_netLoading.gameObject.SetActive(true);
					return;
				}
			}
			else
			{
				this.OnClickCloseBt();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_openData = null;
			this.m_playerGroup.DeInit();
			this.m_equipGroup.DeInit();
			this.m_petsNode.DeInit();
			this.m_mountsNode.DeInit();
			this.m_artifactsNode.DeInit();
			this.m_playerInformationDataModule = null;
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.m_btnDetail.m_onClick = new Action(this.OnBtnDetailClick);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_PlayerInformationData_DataUpdate, new HandlerEvent(this.OnEventDataUpdate));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.m_btnDetail.m_onClick = null;
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_PlayerInformationData_DataUpdate, new HandlerEvent(this.OnEventDataUpdate));
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseBt();
			}
		}

		protected virtual void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
		}

		private void OnBtnDetailClick()
		{
			GameApp.View.OpenView(ViewName.AttributeShowViewModule, this.m_memberAttributeData, 1, null, null);
		}

		protected virtual bool IsSelfOpened()
		{
			return GameApp.View.IsOpened(ViewName.PlayerInformationViewModule);
		}

		private void RefreshUI()
		{
			if (this.m_playerInformationDataModule == null)
			{
				return;
			}
			this.m_content.SetActive(true);
			this.m_netLoading.SetActive(false);
			if (this.m_playerGroup != null)
			{
				this.m_playerGroup.RefreshUI(this.m_playerInfoDto);
			}
			if (this.m_equipGroup != null)
			{
				this.m_equipGroup.RefreshUI(this.m_playerInfoDto, this.GetCameraKey());
			}
			if (this.m_petsNode != null)
			{
				this.m_petsNode.RefreshUI(this.m_playerInfoDto);
			}
			if (this.m_mountsNode != null)
			{
				this.m_mountsNode.RefreshUI(this.m_playerInfoDto);
			}
			if (this.m_artifactsNode != null)
			{
				this.m_artifactsNode.RefreshUI(this.m_playerInfoDto);
			}
		}

		private void OnEventDataUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			if (eventArgs != null)
			{
				EventArgsSetPlayerInformationData eventArgsSetPlayerInformationData = eventArgs as EventArgsSetPlayerInformationData;
				if (eventArgsSetPlayerInformationData != null)
				{
					if (eventArgsSetPlayerInformationData.m_response == null)
					{
						return;
					}
					if (eventArgsSetPlayerInformationData.m_response.UserId != this.m_openData.userId)
					{
						return;
					}
					this.m_playerInfoDto = eventArgsSetPlayerInformationData.m_response;
					this.UpdateMemberAttributeData();
					this.RefreshUI();
				}
			}
		}

		private void UpdateMemberAttributeData()
		{
			if (this.m_playerInfoDto == null)
			{
				return;
			}
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			foreach (KeyValuePair<int, double> keyValuePair in this.m_playerInfoDto.Attributes)
			{
				string attributeKeyByLinkID = Singleton<GameConfig>.Instance.GetAttributeKeyByLinkID(keyValuePair.Key);
				list.Add(new MergeAttributeData(string.Format("{0}={1}", attributeKeyByLinkID, keyValuePair.Value), null, null));
			}
			this.m_memberAttributeData = new MemberAttributeData();
			this.m_memberAttributeData.MergeAttributes(list, false);
			this.m_txtHpValue.text = DxxTools.FormatNumber(this.m_memberAttributeData.GetHpMax4UI()) ?? "";
			this.m_txtAtkValue.text = DxxTools.FormatNumber(this.m_memberAttributeData.GetAttack4UI()) ?? "";
			this.m_txtDefValue.text = DxxTools.FormatNumber(this.m_memberAttributeData.GetDefence4UI()) ?? "";
		}

		public virtual string GetCameraKey()
		{
			return "PlayerInformationViewModule";
		}

		public UIPopCommon m_popCommon;

		public PlayerInformationPlayerGroup m_playerGroup;

		public PlayerInformationEquipGroup m_equipGroup;

		public PlayerInformationPetsNode m_petsNode;

		public PlayerInformationMountsNode m_mountsNode;

		public PlayerInformationArtifactsNode m_artifactsNode;

		public GameObject m_buttons;

		public GameObject m_content;

		public GameObject m_netLoading;

		public CustomText m_txtHpValue;

		public CustomText m_txtAtkValue;

		public CustomText m_txtDefValue;

		public CustomButton m_btnDetail;

		private PlayerInformationDataModule m_playerInformationDataModule;

		protected PlayerInformationViewModule.OpenData m_openData;

		private PlayerInfoDto m_playerInfoDto;

		private MemberAttributeData m_memberAttributeData = new MemberAttributeData();

		public class OpenData
		{
			public OpenData(long userId)
			{
				this.userId = userId;
			}

			public long userId;

			public PlayerInformationViewModuleShowType showType;
		}
	}
}

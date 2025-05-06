using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class UIPlayerInfoNode : CustomBehaviour
	{
		private LoginDataModule loginDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.LoginDataModule);
			}
		}

		protected override void OnInit()
		{
			if (this.m_avatar != null)
			{
				this.m_avatar.Init();
				this.m_avatar.SetEnableButton(true);
				this.m_avatar.OnClick = new Action<UIAvatarCtrl>(this.OnClickAvatarBt);
			}
			this.OnRefreshUI();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLoginData_UserInfoChange, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLoginData_SetCombat, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.Event_CombatUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TipViewModule_AddAddCombatTipNode, new HandlerEvent(this.Event_CombatUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UITalent_RefreshData, new HandlerEvent(this.Event_TalentUpdate));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar));
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLoginData_UserInfoChange, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLoginData_SetCombat, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.Event_CombatUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TipViewModule_AddAddCombatTipNode, new HandlerEvent(this.Event_CombatUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UITalent_RefreshData, new HandlerEvent(this.Event_TalentUpdate));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar));
			if (this.m_avatar != null)
			{
				this.m_avatar.DeInit();
			}
		}

		private void OnClickAvatarBt(UIAvatarCtrl avatarCtrl)
		{
			GameApp.View.OpenView(ViewName.SelfInformationViewModule, null, 1, null, null);
		}

		private void SetAvatar(int avatarID, int avatarFrameID)
		{
			if (this.m_avatar == null)
			{
				return;
			}
			this.m_avatar.RefreshData(avatarID, avatarFrameID);
		}

		private void SetName(string name)
		{
			if (this.m_nameTxt == null)
			{
				return;
			}
			this.m_nameTxt.text = name;
		}

		private void SetCombat()
		{
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			this.m_combatTxt.text = DxxTools.FormatNumber((long)dataModule.Combat);
		}

		private void SetTalent()
		{
			int talentStage = GameApp.Data.GetDataModule(DataName.TalentDataModule).talentData.TalentStage;
			TalentNew_talentEvolution elementById = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(talentStage);
			if (this.m_talentTxt)
			{
				this.m_talentTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.stepLanguageId);
			}
		}

		public void OnRefreshUI()
		{
			this.SetName(this.loginDataModule.NickName);
			this.SetAvatar(this.loginDataModule.Avatar, this.loginDataModule.AvatarFrame);
			this.SetCombat();
			this.SetTalent();
		}

		private void OnEventRefreshUI(object sender, int typeID, BaseEventArgs args)
		{
			this.OnRefreshUI();
		}

		private void Event_CombatUpdate(object sender, int eventid, BaseEventArgs eventArgs)
		{
			this.SetCombat();
		}

		private void Event_TalentUpdate(object sender, int eventid, BaseEventArgs eventArgs)
		{
			this.SetTalent();
		}

		private void OnRedPointChange_Avatar(RedNodeListenData redData)
		{
			if (this.redNodeAvatar != null)
			{
				this.redNodeAvatar.Value = redData.m_count;
			}
		}

		public UIAvatarCtrl m_avatar;

		public CustomText m_nameTxt;

		public CustomText m_combatTxt;

		public CustomText m_talentTxt;

		public RedNodeOneCtrl redNodeAvatar;
	}
}

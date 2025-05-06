using System;
using System.Collections;
using Framework;
using Framework.EventSystem;
using Framework.HabbyWebview;
using Framework.Logic.Modules;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.User;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class SelfInformationViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			if (GameApp.SDK.GetCloudDataValue<bool>("SwitchPreServer", false))
			{
				HabbyIdIntegration.Instance.Init(2, new GameHabbyIdImpl());
			}
			else
			{
				HabbyIdServer habbyIdServer = (GameApp.Config.GetBool("IsReleaseServer") ? 3 : 1);
				HabbyIdIntegration.Instance.Init(habbyIdServer, new GameHabbyIdImpl());
			}
			LanguageDataModule dataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			HabbyIdIntegration.Instance.SetLanguage(dataModule.GetLanguageAbbr(dataModule.GetCurrentLanguageType));
			this.Button_Close.m_onClick = new Action(this.OnClickClose);
			this.Button_Mask.m_onClick = new Action(this.OnClickClose);
			this.Button_ChangeSkin.m_onClick = new Action(this.OnClickChangeSkin);
			this.Avatar_Ctrl.Init();
			this.Avatar_Ctrl.OnClick = new Action<UIAvatarCtrl>(this.OnClickAvatar);
			this.Button_Name_Change.m_onClick = new Action(this.OnClickNameChange);
			this.Button_UID_Copy.m_onClick = new Action(this.OnClickUIDCopy);
			this.Button_ChangeServer.m_onClick = new Action(this.OnClickChangeServer);
			this.Button_Feedback.m_onClick = new Action(this.OnClickFeedback);
			this.Button_Bound.m_onClick = new Action(this.OnClickBound);
			this.Button_Bound_Finish.m_onClick = new Action(this.OnClickBound);
			this.Button_UserAgreement.onClick.AddListener(new UnityAction(this.OnClickUserAgreement));
			this.Button_PrivacyPolicy.onClick.AddListener(new UnityAction(this.OnClickPrivacyPolicy));
			this.Button_Announcement.m_onClick = new Action(this.OnClickAnnouncement);
		}

		public override void OnDelete()
		{
			this.Button_Close.m_onClick = null;
			this.Button_Mask.m_onClick = null;
			this.Button_ChangeSkin.m_onClick = null;
			this.Avatar_Ctrl.DeInit();
			this.Avatar_Ctrl.OnClick = null;
			this.Button_Name_Change.m_onClick = null;
			this.Button_UID_Copy.m_onClick = null;
			this.Button_ChangeServer.m_onClick = null;
			this.Button_Feedback.m_onClick = null;
			this.Button_UserAgreement.onClick.RemoveAllListeners();
			this.Button_PrivacyPolicy.onClick.RemoveAllListeners();
			this.Button_Announcement.m_onClick = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLoginData_UserInfoChange, new HandlerEvent(this.OnEvent_UserInfoChange));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ClothesData_SelfClothesChanged, new HandlerEvent(this.OnEvent_SelfClothesChanged));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.OnEvent_CombatUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLoginData_SetCombat, new HandlerEvent(this.OnEvent_CombatUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLoginData_HabbyIdData, new HandlerEvent(this.OnEvent_HabbyIdChanged));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Notice", new Action<RedNodeListenData>(this.OnRedPointChange_Notice));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.HabbyId", new Action<RedNodeListenData>(this.OnRedPointChange_Bound));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLoginData_UserInfoChange, new HandlerEvent(this.OnEvent_UserInfoChange));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ClothesData_SelfClothesChanged, new HandlerEvent(this.OnEvent_SelfClothesChanged));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.OnEvent_CombatUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLoginData_SetCombat, new HandlerEvent(this.OnEvent_CombatUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLoginData_HabbyIdData, new HandlerEvent(this.OnEvent_HabbyIdChanged));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Notice", new Action<RedNodeListenData>(this.OnRedPointChange_Notice));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.HabbyId", new Action<RedNodeListenData>(this.OnRedPointChange_Bound));
		}

		public override void OnOpen(object data)
		{
			this.Data_Login = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.Data_Clothes = GameApp.Data.GetDataModule(DataName.ClothesDataModule);
			this.Skin_ModelItem.Init();
			this.Data_Clothes.PushUIModelItem(this.Skin_ModelItem, new Action(this.FreshSkin));
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.Init();
			}
			this.FreshTitle();
			this.RefreshAllUI();
			if (this.Data_Login.habbyMailBind)
			{
				this.Button_Bound_Finish.gameObject.SetActive(true);
				this.Button_Bound.gameObject.SetActive(false);
			}
			else
			{
				HabbyIdIntegration.Instance.clearCache();
				this.Button_Bound_Finish.gameObject.SetActive(false);
				this.Button_Bound.gameObject.SetActive(true);
			}
			this.Button_GameClub.gameObject.SetActive(false);
			GameApp.SDK.Analyze.Track_HabbyIDShow();
		}

		public override void OnClose()
		{
			this.Data_Clothes.PopUIModelItem(this.Skin_ModelItem);
			this.Skin_ModelItem.OnHide(false);
			this.Skin_ModelItem.DeInit();
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.DeInit();
			}
			GameApp.SDK.HideGameClubButton();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private IEnumerator ShowGameClubRect()
		{
			yield return new WaitForSeconds(0.5f);
			GameApp.SDK.ShowGameClubButton(this.Button_GameClub.GetComponent<RectTransform>());
			yield break;
		}

		private void RefreshAllUI()
		{
			this.Text_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_title");
			this.FreshAvatar();
			this.FreshCombat();
			this.FreshSelfInfo();
			this.FreshServer();
			this.FreshOthers();
		}

		private void FreshAvatar()
		{
			if (this.Avatar_Ctrl != null)
			{
				this.Avatar_Ctrl.RefreshData(this.Data_Login.Avatar, this.Data_Login.AvatarFrame);
			}
		}

		private void FreshSelfInfo()
		{
			this.Text_Name_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_name");
			this.Text_Name_Value.text = this.Data_Login.NickName;
			this.Text_Gender_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_gender");
			this.sprite_Gender_Value.sprite = this.sprite_male;
			this.Text_Guild_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_guild");
			this.Text_Guild_Value.text = this.Data_Login.GetGuildName(true);
			this.Text_UID_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_uid");
			this.Text_UID_Value.text = this.Data_Login.userId.ToString();
		}

		private void FreshCombat()
		{
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			this.Image_Avatar_Combat.text = DxxTools.FormatNumber((long)dataModule.Combat);
		}

		[ContextMenu("FreshSkin")]
		private void FreshSkin()
		{
			if (!this.Skin_ModelItem.IsCameraShow)
			{
				return;
			}
			this.Text_Button_ChangeSkin.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_changeskin");
			int mountMemberId = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountMemberId(null);
			this.Skin_ModelItem.rectTransform.anchoredPosition = ((mountMemberId > 0) ? this.Skin_ModelItemPos_Mount : this.Skin_ModelItemPos_Normal);
			this.Skin_ModelItem.rectTransform.localScale = ((mountMemberId > 0) ? this.Skin_ModelItemScale_Mount : this.Skin_ModelItemScale_Normal) * Vector3.one;
			this.Skin_ModelItem.OnShow();
			if (!this.Skin_ModelItem.RefreshPlayerSkins(null))
			{
				this.Skin_ModelItem.ShowSelfPlayerModel("UISelfInformation_ModelNodeCtrl", true);
			}
		}

		private void FreshTitle()
		{
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.SetAndFreshToMy();
			}
		}

		private void FreshServer()
		{
			uint serverZoneIdByServerId = GameApp.Data.GetDataModule(DataName.SelectServerDataModule).GetServerZoneIdByServerId(GameApp.NetWork.m_serverID);
			ServerList_serverList serverList_serverList = GameApp.Table.GetManager().GetServerList_serverList((int)serverZoneIdByServerId);
			if (serverList_serverList != null)
			{
				uint num = GameApp.NetWork.m_serverID - (uint)serverList_serverList.range[0] + 1U;
				this.Text_Server_Value.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_server") + Singleton<LanguageManager>.Instance.GetInfoByID(serverList_serverList.serverPrefix, new object[] { num });
			}
			else
			{
				this.Text_Server_Value.text = "";
			}
			this.Text_Button_ChangeServer.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_btn_choose");
		}

		private void FreshOthers()
		{
			this.Text_Button_Announcement.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_btn_announcement");
			this.Text_Button_Support.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_btn_customer");
			this.Text_Button_Code.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_btn_code");
			this.Text_Button_Cache.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_btn_cache");
			this.Text_Button_GameClub.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_btn_gameclub");
			this.Text_Button_Feedback.text = Singleton<LanguageManager>.Instance.GetInfoByID("opinion_feedback");
			this.Text_Button_Bound.text = Singleton<LanguageManager>.Instance.GetInfoByID("ID_bound");
			this.Text_Button_Bound_Finish.text = Singleton<LanguageManager>.Instance.GetInfoByID("ID_bound_finish");
			this.Text_Button_UserAgreement.text = "<u>" + Singleton<LanguageManager>.Instance.GetInfoByID("user_agreement") + "</u>";
			this.Text_Button_PrivacyPolicy.text = "<u>" + Singleton<LanguageManager>.Instance.GetInfoByID("Privacy_Policy") + "</u>";
		}

		private void OpenPlayerAvatarSkinViewModule(int viewType)
		{
			GameApp.View.OpenView(ViewName.PlayerAvatarClothesViewModule, viewType, 1, null, null);
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.SelfInformationViewModule, null);
		}

		private void OnClickAvatar(UIAvatarCtrl obj)
		{
			this.OpenPlayerAvatarSkinViewModule(1);
		}

		private void OnClickChangeSkin()
		{
			this.OpenPlayerAvatarSkinViewModule(2);
		}

		private void OnClickNameChange()
		{
			GameApp.View.OpenView(ViewName.PlayerNameViewModule, null, 1, null, null);
		}

		private void OnClickUIDCopy()
		{
			GUIUtility.systemCopyBuffer = this.Data_Login.userId.ToString();
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("30017"));
		}

		private void OnClickChangeServer()
		{
			SelectServerViewModule.OpenUI();
		}

		private void OnClickFeedback()
		{
			string text = (GameApp.Config.GetBool("IsReleaseServer") ? "https://feedback.advrpg.com" : "https://test-feedback.advrpg.com");
			GameApp.SDK.WebView.OpenFeedbackWebview(text, delegate(bool result)
			{
				if (!result)
				{
					DxxTools.UI.OpenPopCommon(Singleton<LanguageManager>.Instance.GetInfoByID("157"), delegate(int id)
					{
					});
				}
			});
		}

		private void OnClickUserAgreement()
		{
			if (!GameApp.NetWork.IsNetConnect)
			{
				DxxTools.UI.OpenPopCommon(Singleton<LanguageManager>.Instance.GetInfoByID("157"), delegate(int id)
				{
				});
				return;
			}
			Application.OpenURL("https://www.habby.com/termsOfService.html");
		}

		private void OnClickPrivacyPolicy()
		{
			if (!GameApp.NetWork.IsNetConnect)
			{
				DxxTools.UI.OpenPopCommon(Singleton<LanguageManager>.Instance.GetInfoByID("157"), delegate(int id)
				{
				});
				return;
			}
			Application.OpenURL("https://www.habby.com/privacyPolicy.html");
		}

		private void OnClickAnnouncement()
		{
			GameApp.View.OpenView(ViewName.NoticeViewModule, null, 1, null, null);
		}

		private void OnClickBound()
		{
			HabbyIdIntegration.Instance.ClickLoginFromSetting();
		}

		private void OnEvent_UserInfoChange(object sender, int type, BaseEventArgs eventArgs)
		{
			this.FreshAvatar();
			this.FreshTitle();
			this.FreshSelfInfo();
		}

		private void OnEvent_SelfClothesChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			this.FreshSkin();
		}

		private void OnEvent_CombatUpdate(object sender, int eventid, BaseEventArgs eventArgs)
		{
			this.FreshCombat();
		}

		private void OnEvent_HabbyIdChanged(object sender, int eventid, BaseEventArgs eventArgs)
		{
			if (this.Data_Login.habbyMailBind)
			{
				this.Button_Bound_Finish.gameObject.SetActive(true);
				this.Button_Bound.gameObject.SetActive(false);
				if (!this.Data_Login.habbyMailReward)
				{
					NetworkUtils.User.bindHabbyIdReward(delegate(bool b, UserHabbyMailRewardResponse resp)
					{
						if (resp != null && resp.Code == 0)
						{
							if (resp.CommonData.Reward.Count > 0)
							{
								DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
							}
							this.Data_Login.habbyMailReward = true;
						}
					});
					return;
				}
			}
			else
			{
				this.Button_Bound_Finish.gameObject.SetActive(false);
				this.Button_Bound.gameObject.SetActive(true);
			}
		}

		private void OnRedPointChange_Avatar(RedNodeListenData redData)
		{
			if (this.Avatar_RedNode != null)
			{
				this.Avatar_RedNode.Value = redData.m_count;
			}
			if (this.Skin_RedNode != null)
			{
				this.Skin_RedNode.Value = redData.m_count;
			}
		}

		private void OnRedPointChange_Notice(RedNodeListenData redData)
		{
			if (this.Announcement_RedCtrl != null)
			{
				this.Announcement_RedCtrl.Value = redData.m_count;
			}
		}

		private void OnRedPointChange_Bound(RedNodeListenData redData)
		{
			if (this.Bound_RedNode != null)
			{
				this.Bound_RedNode.Value = redData.m_count;
			}
		}

		[Header("界面基础信息")]
		public CustomText Text_Title;

		public CustomButton Button_Close;

		public CustomButton Button_Mask;

		[Header("Avatar")]
		public UIAvatarCtrl Avatar_Ctrl;

		public RedNodeOneCtrl Avatar_RedNode;

		public CustomText Image_Avatar_Combat;

		[Header("自身基本信息")]
		public CustomText Text_Name_Title;

		public CustomText Text_Name_Value;

		public CustomButton Button_Name_Change;

		public CustomText Text_Gender_Title;

		public CustomImage sprite_Gender_Value;

		public CustomButton Button_Gender_Change;

		public Sprite sprite_male;

		public Sprite Sprite_female;

		public CustomText Text_Guild_Title;

		public CustomText Text_Guild_Value;

		public CustomText Text_UID_Title;

		public CustomText Text_UID_Value;

		public CustomButton Button_UID_Copy;

		[Header("称号")]
		public UITitleCtrl TitleCtrl;

		[Header("皮肤")]
		public CustomButton Button_ChangeSkin;

		public CustomText Text_Button_ChangeSkin;

		public RedNodeOneCtrl Skin_RedNode;

		public UIModelItem Skin_ModelItem;

		public Vector2 Skin_ModelItemPos_Normal = new Vector2(0f, -160f);

		public Vector2 Skin_ModelItemPos_Mount = new Vector2(-20f, -200f);

		public float Skin_ModelItemScale_Normal = 2f;

		public float Skin_ModelItemScale_Mount = 1.5f;

		[Header("服务器")]
		public CustomText Text_Server_Value;

		public CustomButton Button_ChangeServer;

		public CustomText Text_Button_ChangeServer;

		[Header("其他按钮")]
		public CustomButton Button_Announcement;

		public CustomText Text_Button_Announcement;

		public RedNodeOneCtrl Announcement_RedCtrl;

		public CustomButton Button_Support;

		public CustomText Text_Button_Support;

		public CustomButton Button_Code;

		public CustomText Text_Button_Code;

		public CustomButton Button_Cache;

		public CustomText Text_Button_Cache;

		public CustomButton Button_GameClub;

		public CustomText Text_Button_GameClub;

		public CustomButton Button_Feedback;

		public CustomText Text_Button_Feedback;

		public Button Button_UserAgreement;

		public TextMeshProUGUI Text_Button_UserAgreement;

		public Button Button_PrivacyPolicy;

		public TextMeshProUGUI Text_Button_PrivacyPolicy;

		public CustomButton Button_Bound;

		public CustomText Text_Button_Bound;

		public CustomButton Button_Bound_Finish;

		public CustomText Text_Button_Bound_Finish;

		public RedNodeOneCtrl Bound_RedNode;

		private LoginDataModule Data_Login;

		private ClothesDataModule Data_Clothes;

		private static GameFeedbackWebview _gameFeedbackWebview;
	}
}

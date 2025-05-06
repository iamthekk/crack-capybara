using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.User;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class PlayerAvatarClothesViewModule : BaseViewModule
	{
		private void SetViewType(int viewType)
		{
			this.m_viewType = viewType;
			foreach (PlayerAvatarClothesViewModule.ViewTypeSetting viewTypeSetting in this.viewTypeSettings)
			{
				if (viewTypeSetting.viewType == viewType)
				{
					this.node_Top.anchoredPosition = new Vector2(this.node_Top.anchoredPosition.x, viewTypeSetting.posY_Node_Top);
					this.node_Middle.anchoredPosition = new Vector2(this.node_Middle.anchoredPosition.x, viewTypeSetting.posY_Node_Middle);
					this.node_Name.anchoredPosition = new Vector2(this.node_Name.anchoredPosition.x, viewTypeSetting.posY_Node_Name);
					this.m_ScrollView.gameObject.SetActiveSafe(viewType != 3);
					this.m_TitleScrollView.gameObject.SetActiveSafe(false);
					this.m_SceneScrollView.gameObject.SetActiveSafe(viewType == 3);
					this.obj_AvatarViewTab.SetActive(viewType == 1);
					this.obj_ClothesViewTab.SetActive(viewType == 2);
					this.obj_SceneViewTab.SetActiveSafe(viewType == 3);
					break;
				}
			}
			this.select_Avatar.SetSelect(viewType == 1, false);
			this.select_Clothes.SetSelect(viewType == 2, false);
			this.select_Scene.SetSelect(viewType == 3, false);
			if (viewType == 1)
			{
				this.SetAvatarTabType(1);
				return;
			}
			if (viewType == 2)
			{
				this.SetClothesTabType(2);
				return;
			}
			if (viewType == 3)
			{
				this.SetScenesTabType(1);
			}
		}

		private void SetAvatarTabType(int tabType)
		{
			this.m_AvatarTabType = tabType;
			if (tabType == 1)
			{
				this.select_Avatar_Icon.SetSelect(true, false);
				this.select_Avatar_Frame.SetSelect(false, false);
				this.select_Avatar_Title.SetSelect(false, false);
				this.m_SelectedId = this.m_loginDataModule.Avatar;
			}
			else if (tabType == 2)
			{
				this.select_Avatar_Icon.SetSelect(false, false);
				this.select_Avatar_Frame.SetSelect(true, false);
				this.select_Avatar_Title.SetSelect(false, false);
				this.m_SelectedId = this.m_loginDataModule.AvatarFrame;
			}
			else if (tabType == 7)
			{
				this.select_Avatar_Icon.SetSelect(false, false);
				this.select_Avatar_Frame.SetSelect(false, false);
				this.select_Avatar_Title.SetSelect(true, false);
				this.m_SelectedId = this.m_loginDataModule.AvatarTitle;
			}
			this.FreshCurrentAvatar();
			if (this.m_AvatarTabType != 7)
			{
				this.m_ScrollView.gameObject.SetActive(true);
				this.m_TitleScrollView.gameObject.SetActive(false);
				this.m_ScrollView.SwitchUIType(this.m_viewType, this.m_AvatarTabType);
				this.m_ScrollView.SelectNode(this.m_SelectedId);
				return;
			}
			this.m_ScrollView.gameObject.SetActive(false);
			this.m_TitleScrollView.gameObject.SetActive(true);
			this.m_TitleScrollView.SwitchUIType(this.m_viewType, this.m_AvatarTabType);
			this.m_TitleScrollView.SelectNode(this.m_SelectedId);
		}

		private void SetClothesTabType(int tabType)
		{
			this.m_ClothesTabType = tabType;
			if (tabType == 2)
			{
				this.m_SelectedId = this.m_ClosthesDataModule.SelfClothesData.HeadId;
				this.select_Clothes_Head.SetSelect(true, false);
				this.select_Clothes_Body.SetSelect(false, false);
				this.select_Clothes_Accessories.SetSelect(false, false);
			}
			else if (tabType == 1)
			{
				this.m_SelectedId = this.m_ClosthesDataModule.SelfClothesData.BodyId;
				this.select_Clothes_Head.SetSelect(false, false);
				this.select_Clothes_Body.SetSelect(true, false);
				this.select_Clothes_Accessories.SetSelect(false, false);
			}
			else if (tabType == 3)
			{
				this.m_SelectedId = this.m_ClosthesDataModule.SelfClothesData.AccessoryId;
				this.select_Clothes_Head.SetSelect(false, false);
				this.select_Clothes_Body.SetSelect(false, false);
				this.select_Clothes_Accessories.SetSelect(true, false);
			}
			this.m_ScrollView.SwitchUIType(this.m_viewType, tabType);
			this.m_ScrollView.SelectNode(this.m_SelectedId);
		}

		private void SetScenesTabType(int tabType)
		{
			this.m_SceneTabType = tabType;
			if (tabType == 1)
			{
				this.select_Scene_SceneSkin.SetSelect(true, false);
				this.m_SelectedId = this.m_ClosthesDataModule.SelfSceneSkinData.CurSkinId;
			}
			this.m_SceneScrollView.SwitchUIType(this.m_viewType, tabType);
			this.m_SceneScrollView.SelectNode(this.m_SelectedId);
		}

		[ContextMenu("FreshCurrentAvatar")]
		private void FreshCurrentAvatar()
		{
			string text = string.Empty;
			long num = 0L;
			this.img_Avatar.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			switch (this.m_viewType)
			{
			case 1:
			{
				int avatarTabType = this.m_AvatarTabType;
				if (avatarTabType != 1)
				{
					if (avatarTabType != 2)
					{
						if (avatarTabType == 7)
						{
							num = this.m_loginDataModule.GetAvatarTitleEndTime(this.m_SelectedId);
						}
					}
					else
					{
						num = this.m_loginDataModule.GetAvatarFrameEndTime(this.m_SelectedId);
					}
				}
				else
				{
					num = this.m_loginDataModule.GetAvatarIconEndTime(this.m_SelectedId);
				}
				Avatar_Avatar avatar_Avatar = GameApp.Table.GetManager().GetAvatar_Avatar(this.m_SelectedId);
				text = avatar_Avatar.name;
				if (this.m_AvatarTabType != 7)
				{
					this.img_Avatar.gameObject.SetActiveSafe(true);
					this.img_Avatar.SetImage(GameApp.Table.GetAtlasPath(avatar_Avatar.atlasId), avatar_Avatar.iconId);
					if (this.titleCtrl != null)
					{
						this.titleCtrl.Hide();
					}
				}
				else
				{
					this.img_Avatar.gameObject.SetActiveSafe(false);
					if (this.titleCtrl != null)
					{
						this.titleCtrl.SetAndFresh(avatar_Avatar);
					}
				}
				this.skin_ModelItem.OnHide(true);
				break;
			}
			case 2:
			{
				this.img_Avatar.gameObject.SetActiveSafe(false);
				if (this.titleCtrl != null)
				{
					this.titleCtrl.Hide();
				}
				text = GameApp.Table.GetManager().GetAvatar_Skin(this.m_SelectedId).name;
				bool flag = false;
				switch (this.m_ClothesTabType)
				{
				case 1:
					flag = this.m_loginDataModule.IsClotheBodyEquipped(this.m_SelectedId);
					num = this.m_loginDataModule.GetClothesBodyEndTime(this.m_SelectedId);
					break;
				case 2:
					flag = this.m_loginDataModule.IsClotheHeadEquipped(this.m_SelectedId);
					num = this.m_loginDataModule.GetClothesHeadEndTime(this.m_SelectedId);
					break;
				case 3:
					flag = this.m_loginDataModule.IsClotheAccessoryEquipped(this.m_SelectedId);
					num = this.m_loginDataModule.GetClothesAccessoryEndTime(this.m_SelectedId);
					break;
				}
				Dictionary<SkinType, SkinData> dictionary = this.m_ClosthesDataModule.SelfClothesData.GetSkinDatas();
				if (!flag)
				{
					ClothesData selfClothesData = this.m_ClosthesDataModule.SelfClothesData;
					AvatarClothesData avatarClothesData = new AvatarClothesData();
					avatarClothesData.ClothesData = new ClothesData(selfClothesData.HeadId, selfClothesData.BodyId, selfClothesData.AccessoryId);
					switch (this.m_ClothesTabType)
					{
					case 1:
						avatarClothesData.ClothesData.DressPart(SkinType.Body, this.m_SelectedId);
						break;
					case 2:
						avatarClothesData.ClothesData.DressPart(SkinType.Head, this.m_SelectedId);
						break;
					case 3:
						avatarClothesData.ClothesData.DressPart(SkinType.Back, this.m_SelectedId);
						break;
					}
					dictionary = avatarClothesData.ClothesData.GetSkinDatas();
				}
				int memberID = GameApp.Data.GetDataModule(DataName.HeroDataModule).MainCardData.m_memberID;
				int weaponId = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetWeaponId();
				int mountMemberId = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountMemberId(null);
				this.skin_ModelItem.rectTransform.anchoredPosition = ((mountMemberId > 0) ? this.skin_ModelItemPos_Mount : this.skin_ModelItemPos_Normal);
				this.skin_ModelItem.rectTransform.localScale = ((mountMemberId > 0) ? this.skin_ModelItemScale_Mount : this.skin_ModelItemScale_Normal) * Vector3.one;
				this.skin_ModelItem.OnShow();
				if (!this.skin_ModelItem.RefreshPlayerSkins(dictionary))
				{
					this.skin_ModelItem.ShowPlayerModel("PlayerAvatarClothes_ModelNodeCtrl", memberID, weaponId, dictionary, mountMemberId);
				}
				break;
			}
			case 3:
			{
				num = this.m_loginDataModule.GetSceneSkinEndTime(this.m_SelectedId);
				this.img_Avatar.transform.localScale = Vector3.one;
				this.img_Avatar.gameObject.SetActiveSafe(true);
				if (this.titleCtrl != null)
				{
					this.titleCtrl.Hide();
				}
				Avatar_SceneSkin avatar_SceneSkin = GameApp.Table.GetManager().GetAvatar_SceneSkin(this.m_SelectedId);
				if (avatar_SceneSkin != null)
				{
					text = avatar_SceneSkin.name;
					this.img_Avatar.SetSprite(this.Register.GetSprite(this.m_SelectedId.ToString()));
				}
				this.skin_ModelItem.OnHide(true);
				break;
			}
			}
			this.text_Name.text = Singleton<LanguageManager>.Instance.GetInfoByID(text);
			if (num > 0L)
			{
				this.m_LeftTime = (double)(num - DxxTools.Time.ServerTimestamp);
				this.FreshLeftTime();
				return;
			}
			this.m_LeftTime = 0.0;
			this.obj_LeftTime.SetActiveSafe(false);
		}

		private void FreshLeftTime()
		{
			if (this.m_LeftTime > 0.0)
			{
				this.obj_LeftTime.SetActiveSafe(true);
				this.text_LeftTime.text = Singleton<LanguageManager>.Instance.GetTime((long)Mathf.CeilToInt((float)this.m_LeftTime));
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutGroup_LeftTime);
				return;
			}
			this.obj_LeftTime.SetActiveSafe(false);
			this.m_LeftTime = 0.0;
		}

		public override void OnCreate(object data)
		{
			this.btn_Close.m_onClick = new Action(this.OnClick_Close);
			this.btn_UnLock.m_onClick = new Action(this.OnClickUnlock);
			this.btn_Save.m_onClick = new Action(this.OnClickSave);
			this.select_Avatar.InitAndSetData(false, new Action<CustomSelectButton>(this.OnClick_Select_Avatar), null);
			this.select_Avatar_Icon.InitAndSetData(false, new Action<CustomSelectButton>(this.OnClick_Select_Avatar_Icon), null);
			this.select_Avatar_Frame.InitAndSetData(false, new Action<CustomSelectButton>(this.OnClick_Select_Avatar_Frame), null);
			this.select_Avatar_Title.InitAndSetData(false, new Action<CustomSelectButton>(this.OnClick_Select_Avatar_Title), null);
			this.select_Clothes.InitAndSetData(false, new Action<CustomSelectButton>(this.OnClick_Select_Clothes), null);
			this.select_Clothes_Head.InitAndSetData(false, new Action<CustomSelectButton>(this.OnClick_Select_Clothes_Head), null);
			this.select_Clothes_Body.InitAndSetData(false, new Action<CustomSelectButton>(this.OnClick_Select_Clothes_Body), null);
			this.select_Clothes_Accessories.InitAndSetData(false, new Action<CustomSelectButton>(this.OnClick_Select_Clothes_Accessories), null);
			this.select_Scene.InitAndSetData(false, new Action<CustomSelectButton>(this.OnClick_Select_Scene), null);
			this.select_Scene_SceneSkin.InitAndSetData(false, new Action<CustomSelectButton>(this.OnClick_Select_Scene_SceneSkin), null);
			this.m_ScrollView.Init();
			this.m_ScrollView.Register = this.Register;
			this.m_ScrollView.OnSelectIcon = new Action<int>(this.OnSwitchAvatarIcon);
			this.m_TitleScrollView.Init();
			this.m_TitleScrollView.Register = this.Register;
			this.m_TitleScrollView.OnSelectIcon = new Action<int>(this.OnSwitchAvatarIcon);
			this.m_SceneScrollView.Init();
			this.m_SceneScrollView.Register = this.Register;
			this.m_SceneScrollView.OnSelectIcon = new Action<int>(this.OnSwitchAvatarIcon);
		}

		public override void OnDelete()
		{
			this.btn_Close.m_onClick = null;
			this.btn_UnLock.m_onClick = null;
			this.btn_Save.m_onClick = null;
			this.select_Avatar.DeInit();
			this.select_Avatar_Icon.DeInit();
			this.select_Avatar_Frame.DeInit();
			this.select_Clothes.DeInit();
			this.select_Clothes_Head.DeInit();
			this.select_Clothes_Body.DeInit();
			this.select_Clothes_Accessories.DeInit();
			this.select_Scene.DeInit();
			this.m_ScrollView.DeInit();
			this.m_ScrollView.OnSelectIcon = null;
			this.m_TitleScrollView.DeInit();
			this.m_TitleScrollView.OnSelectIcon = null;
			this.m_SceneScrollView.DeInit();
			this.m_SceneScrollView.OnSelectIcon = null;
		}

		public override void OnOpen(object data)
		{
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.m_ClosthesDataModule = GameApp.Data.GetDataModule(DataName.ClothesDataModule);
			this.skin_ModelItem.Init();
			this.m_ClosthesDataModule.PushUIModelItem(this.skin_ModelItem, null);
			if (this.titleCtrl != null)
			{
				this.titleCtrl.Init();
			}
			this.text_LeftTimePrefix.text = Singleton<LanguageManager>.Instance.GetInfoByID("avatarclothes_clothes_lefttime");
			this.SetViewType((data != null) ? ((int)data) : 2);
		}

		public override void OnClose()
		{
			this.m_ClosthesDataModule.PopUIModelItem(this.skin_ModelItem);
			this.skin_ModelItem.OnHide(false);
			this.skin_ModelItem.DeInit();
			if (this.titleCtrl != null)
			{
				this.titleCtrl.DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_LeftTime > 0.0)
			{
				this.m_LeftTime -= (double)unscaledDeltaTime;
				this.FreshLeftTime();
				if (this.m_LeftTime <= 0.0)
				{
					this.SetViewType(this.m_viewType);
				}
			}
		}

		private void OnClick_Close()
		{
			GameApp.View.CloseView(ViewName.PlayerAvatarClothesViewModule, null);
		}

		private void OnClick_Select_Avatar(CustomSelectButton selectButton)
		{
			if (this.m_viewType == 1)
			{
				return;
			}
			this.SetViewType(1);
		}

		private void OnClick_Select_Avatar_Icon(CustomSelectButton selectButton)
		{
			if (this.m_AvatarTabType == 1)
			{
				return;
			}
			this.SetAvatarTabType(1);
		}

		private void OnClick_Select_Avatar_Frame(CustomSelectButton selectButton)
		{
			if (this.m_AvatarTabType == 2)
			{
				return;
			}
			this.SetAvatarTabType(2);
		}

		private void OnClick_Select_Avatar_Title(CustomSelectButton selectButton)
		{
			if (this.m_AvatarTabType == 7)
			{
				return;
			}
			this.SetAvatarTabType(7);
		}

		private void OnClick_Select_Clothes(CustomSelectButton selectButton)
		{
			if (this.m_viewType == 2)
			{
				return;
			}
			this.SetViewType(2);
		}

		private void OnClick_Select_Clothes_Head(CustomSelectButton selectButton)
		{
			if (this.m_ClothesTabType == 2)
			{
				return;
			}
			this.SetClothesTabType(2);
		}

		private void OnClick_Select_Clothes_Body(CustomSelectButton selectButton)
		{
			if (this.m_ClothesTabType == 1)
			{
				return;
			}
			this.SetClothesTabType(1);
		}

		private void OnClick_Select_Clothes_Accessories(CustomSelectButton selectButton)
		{
			if (this.m_ClothesTabType == 3)
			{
				return;
			}
			this.SetClothesTabType(3);
		}

		private void OnClick_Select_Scene(CustomSelectButton selectButton)
		{
			if (this.m_viewType == 3)
			{
				return;
			}
			this.SetViewType(3);
		}

		private void OnClick_Select_Scene_SceneSkin(CustomSelectButton selectButton)
		{
			if (this.m_SceneTabType == 1)
			{
				return;
			}
			this.SetScenesTabType(1);
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_GameLoginData_TimeOutUserAvatarClothesScene, new HandlerEvent(this.OnEvent_TimeOutUserAvatarClothesScene));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar.Avatar", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar.Avatar.Icon", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar_Icon));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar.Avatar.Frame", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar_Frame));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar.Avatar.Title", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar_Title));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar.Clothes", new Action<RedNodeListenData>(this.OnRedPointChange_Clothes));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar.Clothes.Head", new Action<RedNodeListenData>(this.OnRedPointChange_Clothes_Head));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar.Clothes.Body", new Action<RedNodeListenData>(this.OnRedPointChange_Clothes_Body));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar.Clothes.Accessory", new Action<RedNodeListenData>(this.OnRedPointChange_Clothes_Accessory));
			RedPointController.Instance.RegRecordChange("Main.SelfInfo.Avatar.Scene", new Action<RedNodeListenData>(this.OnRedPointChange_Scene));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_GameLoginData_TimeOutUserAvatarClothesScene, new HandlerEvent(this.OnEvent_TimeOutUserAvatarClothesScene));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar.Avatar", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar.Avatar.Icon", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar_Icon));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar.Avatar.Frame", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar_Frame));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar.Avatar.Title", new Action<RedNodeListenData>(this.OnRedPointChange_Avatar_Title));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar.Clothes", new Action<RedNodeListenData>(this.OnRedPointChange_Clothes));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar.Clothes.Head", new Action<RedNodeListenData>(this.OnRedPointChange_Clothes_Head));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar.Clothes.Body", new Action<RedNodeListenData>(this.OnRedPointChange_Clothes_Body));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar.Clothes.Accessory", new Action<RedNodeListenData>(this.OnRedPointChange_Clothes_Accessory));
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo.Avatar.Scene", new Action<RedNodeListenData>(this.OnRedPointChange_Scene));
		}

		public int GetTabPartType()
		{
			switch (this.m_viewType)
			{
			case 1:
				if (this.m_AvatarTabType == 1)
				{
					return 1;
				}
				if (this.m_AvatarTabType == 2)
				{
					return 2;
				}
				if (this.m_AvatarTabType == 7)
				{
					return 7;
				}
				break;
			case 2:
				if (this.m_ClothesTabType == 1)
				{
					return 3;
				}
				if (this.m_ClothesTabType == 2)
				{
					return 4;
				}
				if (this.m_ClothesTabType == 3)
				{
					return 5;
				}
				break;
			case 3:
				if (this.m_SceneTabType == 1)
				{
					return 6;
				}
				break;
			}
			return 0;
		}

		private void OnClickSave()
		{
			if (this.m_SelectedId <= 0)
			{
				return;
			}
			int tabPartType = this.GetTabPartType();
			if (tabPartType > 0)
			{
				NetworkUtils.PlayerData.RequestUpdateUserAvatar(tabPartType, this.m_SelectedId, delegate(bool result, UpdateUserAvatarResponse response)
				{
					if (result)
					{
						this.NetChangeFresh();
					}
				}, true);
			}
		}

		private void OnClickUnlock()
		{
			if (this.m_SelectedId <= 0)
			{
				return;
			}
			int tabPartType = this.GetTabPartType();
			if (tabPartType > 0)
			{
				NetworkUtils.PlayerData.RequestUnlockUserAvatar(tabPartType, this.m_SelectedId, delegate(bool result, UnlockUserAvatarResponse response)
				{
					if (result)
					{
						GameApp.Sound.PlayClip(649, 1f);
						this.NetChangeFresh();
					}
				}, true);
			}
		}

		private void NetChangeFresh()
		{
			switch (this.m_viewType)
			{
			case 1:
				if (this.m_AvatarTabType == 7)
				{
					this.m_TitleScrollView.RefreshAllShownItem();
				}
				else
				{
					this.m_ScrollView.RefreshAllShownItem();
				}
				break;
			case 2:
				this.m_ScrollView.RefreshAllShownItem();
				break;
			case 3:
				this.m_SceneScrollView.RefreshAllShownItem();
				break;
			}
			this.FreshSelectedItem();
		}

		private void OnEvent_TimeOutUserAvatarClothesScene(object sender, int type, BaseEventArgs eventArgs = null)
		{
			EventArgClothesTimeOut eventArgClothesTimeOut = eventArgs as EventArgClothesTimeOut;
			if (eventArgClothesTimeOut != null)
			{
				int tabPartType = this.GetTabPartType();
				List<int> list;
				if (eventArgClothesTimeOut.PartTimeOuts.TryGetValue(tabPartType, out list))
				{
					int i = 0;
					while (i < list.Count)
					{
						if (this.m_SelectedId == list[i] || this.m_loginDataModule.IsEquipped(tabPartType, list[i]))
						{
							if (this.m_viewType == 1)
							{
								this.SetAvatarTabType(this.m_AvatarTabType);
								return;
							}
							if (this.m_viewType == 2)
							{
								this.SetClothesTabType(this.m_ClothesTabType);
								return;
							}
							if (this.m_viewType == 3)
							{
								this.SetScenesTabType(this.m_SceneTabType);
								return;
							}
							break;
						}
						else
						{
							i++;
						}
					}
				}
			}
		}

		private void OnSwitchAvatarIcon(int id)
		{
			this.m_SelectedId = id;
			this.FreshSelectedItem();
		}

		private void FreshSelectedItem()
		{
			this.FreshCurrentAvatar();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			string text = string.Empty;
			string text2 = string.Empty;
			switch (this.m_viewType)
			{
			case 1:
				if (this.m_AvatarTabType == 1)
				{
					flag = this.m_loginDataModule.IsAvatarIconEquipped(this.m_SelectedId);
					flag2 = this.m_loginDataModule.IsAvatarIconUnlock(this.m_SelectedId);
					flag3 = this.m_loginDataModule.IsAvatarIconCanUnlock(this.m_SelectedId);
				}
				else if (this.m_AvatarTabType == 2)
				{
					flag = this.m_loginDataModule.IsAvatarFrameEquipped(this.m_SelectedId);
					flag2 = this.m_loginDataModule.IsAvatarFrameUnlock(this.m_SelectedId);
					flag3 = this.m_loginDataModule.IsAvatarFrameCanUnlock(this.m_SelectedId);
				}
				else if (this.m_AvatarTabType == 7)
				{
					flag = this.m_loginDataModule.IsAvatarTitleEquipped(this.m_SelectedId);
					flag2 = this.m_loginDataModule.IsAvatarTitleUnlock(this.m_SelectedId);
					flag3 = this.m_loginDataModule.IsAvatarTitleCanUnlock(this.m_SelectedId);
				}
				if (flag)
				{
					text = "avatarclothes_avatar_equiped";
				}
				else
				{
					Avatar_Avatar avatar_Avatar = GameApp.Table.GetManager().GetAvatar_Avatar(this.m_SelectedId);
					if (avatar_Avatar != null)
					{
						if (flag3)
						{
							text2 = avatar_Avatar.unlockItemId[0];
						}
						else if (!flag2)
						{
							text = avatar_Avatar.getLanguageId;
						}
					}
				}
				break;
			case 2:
				if (this.m_ClothesTabType == 2)
				{
					flag = this.m_loginDataModule.IsClotheHeadEquipped(this.m_SelectedId);
					flag2 = this.m_loginDataModule.IsClothesHeadUnlock(this.m_SelectedId);
					flag3 = this.m_loginDataModule.IsClothesHeadCanUnlock(this.m_SelectedId);
				}
				else if (this.m_ClothesTabType == 1)
				{
					flag = this.m_loginDataModule.IsClotheBodyEquipped(this.m_SelectedId);
					flag2 = this.m_loginDataModule.IsClothesBodyUnlock(this.m_SelectedId);
					flag3 = this.m_loginDataModule.IsClothesBodyCanUnlock(this.m_SelectedId);
				}
				else if (this.m_ClothesTabType == 3)
				{
					flag = this.m_loginDataModule.IsClotheAccessoryEquipped(this.m_SelectedId);
					flag2 = this.m_loginDataModule.IsClothesAccessoriesUnlock(this.m_SelectedId);
					flag3 = this.m_loginDataModule.IsClothesAccessoriesCanUnlock(this.m_SelectedId);
				}
				if (flag)
				{
					text = "avatarclothes_clothes_equiped";
				}
				else
				{
					Avatar_Skin avatar_Skin = GameApp.Table.GetManager().GetAvatar_Skin(this.m_SelectedId);
					if (avatar_Skin != null)
					{
						if (flag3)
						{
							if (avatar_Skin.unlockItemId != null && avatar_Skin.unlockItemId.Length != 0)
							{
								text2 = avatar_Skin.unlockItemId[0];
							}
						}
						else if (!flag2)
						{
							text = avatar_Skin.getLanguageId;
						}
					}
				}
				break;
			case 3:
				if (this.m_SceneTabType == 1)
				{
					flag = this.m_loginDataModule.IsSceneSkinEquipped(this.m_SelectedId);
					flag2 = this.m_loginDataModule.IsSceneSkinUnlock(this.m_SelectedId);
					flag3 = this.m_loginDataModule.IsSceneSkinCanUnlock(this.m_SelectedId);
				}
				if (flag)
				{
					text = "avatarclothes_avatar_equiped";
				}
				else
				{
					Avatar_SceneSkin avatar_SceneSkin = GameApp.Table.GetManager().GetAvatar_SceneSkin(this.m_SelectedId);
					if (avatar_SceneSkin != null)
					{
						if (flag3)
						{
							text2 = avatar_SceneSkin.unlockItemId[0];
						}
						else if (!flag2)
						{
							text = avatar_SceneSkin.getLanguageId;
						}
					}
				}
				break;
			}
			this.btn_Save.gameObject.SetActiveSafe(flag2 && !flag);
			if (string.IsNullOrEmpty(text))
			{
				this.text_SelectTip.text = "";
			}
			else
			{
				this.text_SelectTip.text = Singleton<LanguageManager>.Instance.GetInfoByID(text);
			}
			this.btn_UnLock.gameObject.SetActiveSafe(flag3);
			if (flag3)
			{
				string[] array = text2.Split(',', StringSplitOptions.None);
				int num = int.Parse(array[0]);
				int num2 = int.Parse(array[1]);
				Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(num);
				this.img_UnLock_Item_Icon.SetImage(GameApp.Table.GetAtlasPath(item_Item.atlasID), item_Item.icon);
				long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)num));
				this.text_UnLock_Item_Num.text = num2.ToString();
				if (itemDataCountByid < (long)num2)
				{
					this.text_UnLock_Item_Num.color = Color.red;
					return;
				}
				this.text_UnLock_Item_Num.color = Color.white;
			}
		}

		private void OnRedPointChange_Avatar(RedNodeListenData redData)
		{
			if (this.redNode_Select_Avatar != null)
			{
				this.redNode_Select_Avatar.Value = redData.m_count;
			}
		}

		private void OnRedPointChange_Avatar_Icon(RedNodeListenData redData)
		{
			if (this.redNode_Select_Avatar_Icon != null)
			{
				this.redNode_Select_Avatar_Icon.Value = redData.m_count;
			}
		}

		private void OnRedPointChange_Avatar_Frame(RedNodeListenData redData)
		{
			if (this.redNode_Select_Avatar_Frame != null)
			{
				this.redNode_Select_Avatar_Frame.Value = redData.m_count;
			}
		}

		private void OnRedPointChange_Avatar_Title(RedNodeListenData redData)
		{
			if (this.redNode_Select_Avatar_Title != null)
			{
				this.redNode_Select_Avatar_Title.Value = redData.m_count;
			}
		}

		private void OnRedPointChange_Clothes(RedNodeListenData redData)
		{
			if (this.redNode_Select_Clothes != null)
			{
				this.redNode_Select_Clothes.Value = redData.m_count;
			}
		}

		private void OnRedPointChange_Clothes_Head(RedNodeListenData redData)
		{
			if (this.redNode_Select_Clothes_Head != null)
			{
				this.redNode_Select_Clothes_Head.Value = redData.m_count;
			}
		}

		private void OnRedPointChange_Clothes_Body(RedNodeListenData redData)
		{
			if (this.redNode_Select_Clothes_Body != null)
			{
				this.redNode_Select_Clothes_Body.Value = redData.m_count;
			}
		}

		private void OnRedPointChange_Clothes_Accessory(RedNodeListenData redData)
		{
			if (this.redNode_Select_Clothes_Accessories != null)
			{
				this.redNode_Select_Clothes_Accessories.Value = redData.m_count;
			}
		}

		private void OnRedPointChange_Scene(RedNodeListenData redData)
		{
			if (this.redNode_Scene_SceneSkin != null)
			{
				this.redNode_Scene_SceneSkin.Value = redData.m_count;
			}
			if (this.redNode_Scene_HotSpring != null)
			{
				this.redNode_Scene_HotSpring.Value = redData.m_count;
			}
		}

		[Header("通用")]
		public CustomButton btn_Close;

		public CustomText text_Name;

		public CustomText text_SelectTip;

		public CustomButton btn_Save;

		public CustomButton btn_UnLock;

		public CustomImage img_UnLock_Item_Icon;

		public CustomText text_UnLock_Item_Num;

		public CustomImage img_Avatar;

		public UITitleCtrl titleCtrl;

		public UIModelItem skin_ModelItem;

		public Vector2 skin_ModelItemPos_Normal = new Vector2(0f, -160f);

		public Vector2 skin_ModelItemPos_Mount = new Vector2(-20f, -200f);

		public float skin_ModelItemScale_Normal = 2f;

		public float skin_ModelItemScale_Mount = 1.5f;

		public GameObject obj_LeftTime;

		public RectTransform layoutGroup_LeftTime;

		public CustomText text_LeftTimePrefix;

		public CustomText text_LeftTime;

		[Header("布局设置")]
		public List<PlayerAvatarClothesViewModule.ViewTypeSetting> viewTypeSettings = new List<PlayerAvatarClothesViewModule.ViewTypeSetting>();

		[Header("布局节点")]
		public RectTransform node_Top;

		public RectTransform node_Middle;

		public RectTransform node_Name;

		public GameObject obj_AvatarViewTab;

		public GameObject obj_ClothesViewTab;

		public GameObject obj_SceneViewTab;

		[Header("Avatar")]
		public CustomSelectButton select_Avatar;

		public CustomSelectButton select_Avatar_Icon;

		public CustomSelectButton select_Avatar_Frame;

		public CustomSelectButton select_Avatar_Title;

		public RedNodeOneCtrl redNode_Select_Avatar;

		public RedNodeOneCtrl redNode_Select_Avatar_Icon;

		public RedNodeOneCtrl redNode_Select_Avatar_Frame;

		public RedNodeOneCtrl redNode_Select_Avatar_Title;

		[Header("Clothes")]
		public CustomSelectButton select_Clothes;

		public CustomSelectButton select_Clothes_Head;

		public CustomSelectButton select_Clothes_Body;

		public CustomSelectButton select_Clothes_Accessories;

		public RedNodeOneCtrl redNode_Select_Clothes;

		public RedNodeOneCtrl redNode_Select_Clothes_Head;

		public RedNodeOneCtrl redNode_Select_Clothes_Body;

		public RedNodeOneCtrl redNode_Select_Clothes_Accessories;

		[Header("Scene")]
		public CustomSelectButton select_Scene;

		public CustomSelectButton select_Scene_SceneSkin;

		public RedNodeOneCtrl redNode_Scene_SceneSkin;

		public RedNodeOneCtrl redNode_Scene_HotSpring;

		[Header("Items")]
		public UIPlayerAvatarClothesScrollView m_ScrollView;

		public UIPlayerAvatarClothesScrollView m_SceneScrollView;

		public UIPlayerAvatarClothesScrollView m_TitleScrollView;

		public SpriteRegister Register;

		private LoginDataModule m_loginDataModule;

		private ClothesDataModule m_ClosthesDataModule;

		private int m_viewType;

		private int m_AvatarTabType;

		private int m_ClothesTabType;

		private int m_SceneTabType;

		private int m_SelectedId;

		private double m_LeftTime;

		[Serializable]
		public class ViewTypeSetting
		{
			public int viewType;

			public float posY_Node_Top;

			public float posY_Node_Middle;

			public float posY_Node_Name;
		}
	}
}

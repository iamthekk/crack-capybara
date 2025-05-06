using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIPlayerAvatarClothesCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.m_button != null)
			{
				this.m_button.m_onClick = new Action(this.OnClickIcon);
			}
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.clothesDataModule = GameApp.Data.GetDataModule(DataName.ClothesDataModule);
			this.m_selObj.SetActiveSafe(false);
			this.m_equipedObj.SetActiveSafe(false);
			this.m_lockObj.SetActiveSafe(false);
			this.m_redNode.SetActiveSafe(false);
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.Init();
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_button != null)
			{
				this.m_button.m_onClick = null;
			}
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.DeInit();
			}
			this.loginDataModule = null;
			this.clothesDataModule = null;
		}

		private void OnClickIcon()
		{
			Action<UIPlayerAvatarClothesCtrl> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public void SetSelected(bool selected)
		{
			this.m_selObj.SetActiveSafe(selected);
		}

		public virtual async void RefreshData(int id, SpriteRegister register)
		{
			this.m_spriteRegister = register;
			this.Id = id;
			if (this.viewType == 1)
			{
				this.m_bgObj.SetActiveSafe(true);
				this.m_quality.gameObject.SetActiveSafe(false);
				Avatar_Avatar avatar_Avatar = GameApp.Table.GetManager().GetAvatar_Avatar(id);
				this.m_icon.SetImage(GameApp.Table.GetAtlasPath(avatar_Avatar.atlasId), avatar_Avatar.iconId);
				bool flag = true;
				bool flag2 = false;
				bool flag3 = false;
				if (this.tabType == 1)
				{
					this.m_icon.enabled = true;
					if (this.TitleCtrl != null)
					{
						this.TitleCtrl.Hide();
					}
					flag = this.loginDataModule.IsAvatarIconUnlock(id);
					flag2 = this.loginDataModule.IsAvatarIconCanUnlock(id);
					flag3 = this.loginDataModule.Avatar == id;
				}
				else if (this.tabType == 2)
				{
					this.m_icon.enabled = true;
					if (this.TitleCtrl != null)
					{
						this.TitleCtrl.Hide();
					}
					flag = this.loginDataModule.IsAvatarFrameUnlock(id);
					flag2 = this.loginDataModule.IsAvatarFrameCanUnlock(id);
					flag3 = this.loginDataModule.AvatarFrame == id;
				}
				else if (this.tabType == 7)
				{
					flag = this.loginDataModule.IsAvatarTitleUnlock(id);
					flag2 = this.loginDataModule.IsAvatarTitleCanUnlock(id);
					flag3 = this.loginDataModule.AvatarTitle == id;
					if (id == Singleton<GameConfig>.Instance.AvatarDefaultTitleId)
					{
						this.m_icon.enabled = true;
						if (this.TitleCtrl != null)
						{
							this.TitleCtrl.Hide();
						}
					}
					else
					{
						this.m_icon.enabled = false;
						if (this.TitleCtrl != null)
						{
							this.TitleCtrl.SetAndFresh(avatar_Avatar);
						}
					}
				}
				this.m_lockObj.SetActiveSafe(!flag);
				this.m_redNode.SetActiveSafe(flag2);
				this.SetEquipObjActive(flag3);
			}
			else if (this.viewType == 2)
			{
				Avatar_Skin avatar_Skin = GameApp.Table.GetManager().GetAvatar_Skin(id);
				this.m_bgObj.SetActiveSafe(false);
				this.m_quality.gameObject.SetActiveSafe(true);
				this.m_quality.SetImage(GameApp.Table.GetAtlasPath(avatar_Skin.quality_atlasId), avatar_Skin.quality_iconId);
				this.m_icon.SetImage(GameApp.Table.GetAtlasPath(avatar_Skin.atlasId), avatar_Skin.iconId);
				bool flag4 = true;
				bool flag5 = false;
				bool flag6 = false;
				if (this.tabType == 2)
				{
					flag4 = this.loginDataModule.IsClothesHeadUnlock(id);
					flag5 = this.loginDataModule.IsClothesHeadCanUnlock(id);
					flag6 = this.clothesDataModule.SelfClothesData.HeadId == id;
				}
				else if (this.tabType == 1)
				{
					flag4 = this.loginDataModule.IsClothesBodyUnlock(id);
					flag5 = this.loginDataModule.IsClothesBodyCanUnlock(id);
					flag6 = this.clothesDataModule.SelfClothesData.BodyId == id;
				}
				else if (this.tabType == 3)
				{
					flag4 = this.loginDataModule.IsClothesAccessoriesUnlock(id);
					flag5 = this.loginDataModule.IsClothesAccessoriesCanUnlock(id);
					flag6 = this.clothesDataModule.SelfClothesData.AccessoryId == id;
				}
				this.m_lockObj.SetActiveSafe(!flag4);
				this.m_redNode.SetActiveSafe(flag5);
				this.SetEquipObjActive(flag6);
			}
			else if (this.viewType == 3 && GameApp.Table.GetManager().GetAvatar_SceneSkin(id) != null)
			{
				this.m_bgObj.SetActiveSafe(false);
				this.m_quality.gameObject.SetActiveSafe(false);
				this.SetEquipObjActive(false);
				if (this.m_spriteRegister != null)
				{
					this.m_icon.SetSprite(this.m_spriteRegister.GetSprite(id.ToString()));
				}
				if (this.tabType == 1)
				{
					bool flag7 = this.loginDataModule.IsSceneSkinUnlock(id);
					bool flag8 = this.loginDataModule.IsSceneSkinCanUnlock(id);
					bool flag9 = this.clothesDataModule.SelfSceneSkinData.CurSkinId == id;
					this.m_lockObj.SetActiveSafe(!flag7);
					this.m_redNode.SetActiveSafe(flag8);
					this.SetEquipObjActive(flag9);
				}
			}
		}

		private void SetEquipObjActive(bool active)
		{
			this.m_equipedObj.SetActiveSafe(active);
		}

		public CustomButton m_button;

		public GameObject m_bgObj;

		public CustomImage m_quality;

		public CustomImage m_icon;

		public UITitleCtrl TitleCtrl;

		public GameObject m_selObj;

		public GameObject m_equipedObj;

		public GameObject m_lockObj;

		public GameObject m_redNode;

		[HideInInspector]
		public int viewType;

		[HideInInspector]
		public int tabType;

		private LoginDataModule loginDataModule;

		private ClothesDataModule clothesDataModule;

		public Action<UIPlayerAvatarClothesCtrl> OnClick;

		public int Id;

		private SpriteRegister m_spriteRegister;
	}
}

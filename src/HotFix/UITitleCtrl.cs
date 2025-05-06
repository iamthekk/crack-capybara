using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UITitleCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Hide();
		}

		protected override void OnDeInit()
		{
			this.Hide();
		}

		public void Show()
		{
			if (this.alwaysHide)
			{
				return;
			}
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
		}

		public void Hide()
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
		}

		[ContextMenu("SetAndFreshToMy")]
		public void SetAndFreshToMy()
		{
			this.SetAndFresh(GameApp.Data.GetDataModule(DataName.LoginDataModule).AvatarTitle);
		}

		public void SetAndFresh(int titleId)
		{
			DxxTools.Game.TryVersionMatchTitle(ref titleId);
			Avatar_Avatar avatar_Avatar = GameApp.Table.GetManager().GetAvatar_Avatar(titleId);
			this.SetAndFresh(avatar_Avatar);
		}

		public void SetAndFresh(Avatar_Avatar avatarCfg)
		{
			if (this.alwaysHide)
			{
				this.Hide();
				return;
			}
			if (avatarCfg == null || avatarCfg.id == Singleton<GameConfig>.Instance.AvatarDefaultTitleId)
			{
				this.Hide();
				return;
			}
			this.Show();
			this.imageBg.SetImage(GameApp.Table.GetAtlasPath(avatarCfg.atlasId), avatarCfg.titlebg);
			this.imageIcon.SetImage(GameApp.Table.GetAtlasPath(avatarCfg.atlasId), avatarCfg.iconId);
			this.textName.text = Singleton<LanguageManager>.Instance.GetInfoByID(avatarCfg.titleText);
		}

		public CustomImage imageBg;

		public CustomImage imageIcon;

		public CustomText textName;

		public bool alwaysHide;
	}
}

using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIAvatarRelation : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.m_button != null)
			{
				this.m_button.onClick.AddListener(new UnityAction(this.OnClickBtn));
			}
			if (this.m_levelTxt != null)
			{
				this.m_levelTxt.gameObject.SetActiveSafe(false);
			}
			if (this.m_hostParent != null)
			{
				this.m_hostParent.SetActive(false);
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_button != null)
			{
				this.m_button.onClick.RemoveListener(new UnityAction(this.OnClickBtn));
			}
		}

		private void OnClickBtn()
		{
			if (this.OnClick != null)
			{
				this.OnClick(this);
			}
		}

		public void RefreshData(int iconID, int boxID, int level, int hostIconID, int hostBoxID)
		{
			if (this.m_userParent != null)
			{
				DxxTools.Game.TryVersionMatchAvatar(ref iconID, ref boxID);
				iconID = ((iconID == 0) ? Singleton<GameConfig>.Instance.AvatarDefaultId : iconID);
				boxID = ((boxID == 0) ? Singleton<GameConfig>.Instance.AvatarDefaultFrameId : boxID);
				bool flag = iconID > 0 && boxID > 0;
				this.m_userParent.SetActive(flag);
				if (flag)
				{
					Avatar_Avatar elementById = GameApp.Table.GetManager().GetAvatar_AvatarModelInstance().GetElementById(iconID);
					if (this.m_userIcon != null)
					{
						this.m_userIcon.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasId), elementById.iconId);
					}
					Avatar_Avatar elementById2 = GameApp.Table.GetManager().GetAvatar_AvatarModelInstance().GetElementById(boxID);
					if (this.m_userBox != null)
					{
						this.m_userBox.SetImage(GameApp.Table.GetAtlasPath(elementById2.atlasId), elementById2.iconId);
					}
				}
			}
			if (this.m_levelTxt != null && level > 0)
			{
				this.m_levelTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("9206", new object[] { level });
			}
		}

		public CustomButton m_button;

		public GameObject m_userParent;

		public CustomImage m_userIcon;

		public CustomImage m_userBox;

		public GameObject m_hostParent;

		public CustomImage m_hostIcon;

		public CustomImage m_hostBox;

		public CustomText m_levelTxt;

		public Action<UIAvatarRelation> OnClick;
	}
}

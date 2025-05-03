using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIAvatarCtrl : CustomBehaviour
	{
		public int IconID
		{
			get
			{
				if (this.Data == null)
				{
					return Singleton<GameConfig>.Instance.AvatarDefaultId;
				}
				return this.Data.IconID;
			}
		}

		public int BoxID
		{
			get
			{
				if (this.Data == null)
				{
					return Singleton<GameConfig>.Instance.AvatarDefaultFrameId;
				}
				return this.Data.BoxID;
			}
		}

		protected override void OnInit()
		{
			if (this.m_button != null)
			{
				this.m_button.m_onClick = new Action(this.OnClickIcon);
			}
		}

		protected override void OnDeInit()
		{
			this.OnClick = null;
		}

		private void OnClickIcon()
		{
			Action<UIAvatarCtrl> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public virtual void RefreshData(int iconid, int boxId)
		{
			if (this.Data == null)
			{
				this.Data = new UIAvatarData();
			}
			this.Data.SetIcon(iconid, boxId);
			this.RefreshUI();
		}

		public virtual void RefreshData(UIAvatarData data)
		{
			this.Data = data;
			if (this.Data == null)
			{
				this.Data = new UIAvatarData();
			}
			this.RefreshUI();
		}

		public void SetEnableButton(bool enable)
		{
			if (this.m_button == null)
			{
				return;
			}
			this.m_button.enabled = enable;
		}

		protected virtual void RefreshUI()
		{
			if (this.Data != null)
			{
				DxxTools.Game.TryVersionMatchAvatar(ref this.Data.IconID, ref this.Data.BoxID);
				this.Data.FreshCfg();
			}
			if (this.Data != null && this.Data.IconData != null)
			{
				this.m_icon.gameObject.SetActive(true);
				this.m_icon.SetImage(GameApp.Table.GetAtlasPath(this.Data.IconData.atlasId), this.Data.IconData.iconId);
			}
			else
			{
				this.m_icon.gameObject.SetActive(false);
			}
			if (this.Data != null && this.Data.BoxData != null && this.Data.BoxData.atlasId > 0 && !string.IsNullOrEmpty(this.Data.BoxData.iconId))
			{
				this.m_box.gameObject.SetActive(true);
				this.m_box.SetImage(GameApp.Table.GetAtlasPath(this.Data.BoxData.atlasId), this.Data.BoxData.iconId);
				return;
			}
			this.m_box.gameObject.SetActive(false);
		}

		public CustomButton m_button;

		public CustomImage m_icon;

		public CustomImage m_box;

		public UIAvatarData Data;

		public Action<UIAvatarCtrl> OnClick;
	}
}

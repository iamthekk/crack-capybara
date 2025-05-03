using System;
using Framework;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class OtherMainCity_MainCityNode : BaseOtherMainCityNode
	{
		protected override void OnInit()
		{
			base.OnInit();
			this.m_lordAvatar.OnClick = new Action<UIAvatarCtrl>(this.OnClickLordAvatar);
			this.m_lordAvatar.Init();
		}

		public override void OnShow()
		{
			base.OnShow();
			this.OnRefreshUI();
		}

		public override void OnHide()
		{
			base.OnHide();
			this.m_conquerGroup = null;
			if (this.m_lordAvatar != null)
			{
				this.m_lordAvatar.DeInit();
			}
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
		}

		protected override void OnClickUnlockBt()
		{
			ConquerViewModule.OpenData openData = new ConquerViewModule.OpenData();
			openData.m_targetUserID = this.m_responseData.UserId;
			openData.m_targetNick = this.m_responseData.NickName;
			if (GameApp.View.IsOpened(ViewName.ConquerViewModule))
			{
				GameApp.View.CloseView(ViewName.ConquerViewModule, null);
			}
			GameApp.View.OpenView(ViewName.ConquerViewModule, openData, 1, null, null);
		}

		protected override void OnClickLockBt()
		{
		}

		private void OnClickLordAvatar(UIAvatarCtrl obj)
		{
			if (this.m_responseData == null || this.m_responseData.Extra == null)
			{
				return;
			}
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.m_responseData.Extra.LordUid);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		public void OnRefreshUI()
		{
			if (this.m_responseData.Extra != null && this.m_responseData.Extra.LordUid > 0L)
			{
				this.m_conquerGroup.gameObject.SetActive(true);
				this.m_lordAvatar.RefreshData(this.m_responseData.Extra.LordAvatar, this.m_responseData.Extra.LordAvatarFrame);
				if (this.m_conquerTxt != null)
				{
					this.m_conquerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6451);
					return;
				}
			}
			else
			{
				this.m_conquerGroup.gameObject.SetActive(false);
			}
		}

		public RectTransform m_conquerGroup;

		public UIAvatarCtrl m_lordAvatar;

		public CustomText m_conquerTxt;
	}
}

using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Common;

namespace HotFix
{
	public class ReportConquerPlayerGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_heroAvatar.Init();
		}

		protected override void OnDeInit()
		{
			this.m_heroAvatar.DeInit();
			this.m_heroAvatar = null;
			this.m_nameTxt = null;
			this.m_powerTxt = null;
		}

		public void RefreshUI(BattleUserDto userDto)
		{
			int num = -1;
			int num2 = -1;
			if (userDto.Extra != null && userDto.Extra.LordUid > 0L)
			{
				num = userDto.Extra.LordAvatar;
				num2 = userDto.Extra.LordAvatarFrame;
			}
			if (this.m_heroAvatar != null)
			{
				this.m_heroAvatar.RefreshData(userDto.Avatar, userDto.AvatarFrame, userDto.Level, num, num2);
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = (string.IsNullOrEmpty(userDto.NickName) ? DxxTools.GetDefaultNick(userDto.UserId) : userDto.NickName);
			}
			if (this.m_powerTxt != null)
			{
				this.m_powerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("9205", new object[] { DxxTools.FormatNumber(userDto.Power) });
			}
		}

		public UIAvatarRelation m_heroAvatar;

		public CustomText m_nameTxt;

		public CustomText m_powerTxt;
	}
}

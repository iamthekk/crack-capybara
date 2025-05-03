using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class UIAvatarData
	{
		public void SetIcon(int iconid, int iconbgid)
		{
			this.IconID = iconid;
			this.BoxID = iconbgid;
		}

		public void FreshCfg()
		{
			if (this.IconID == 0 && this.UseDefault)
			{
				this.IconID = Singleton<GameConfig>.Instance.AvatarDefaultId;
			}
			this.IconData = GameApp.Table.GetManager().GetAvatar_AvatarModelInstance().GetElementById(this.IconID);
			if (this.BoxID == 0 && this.UseDefault)
			{
				this.BoxID = Singleton<GameConfig>.Instance.AvatarDefaultFrameId;
			}
			this.BoxData = GameApp.Table.GetManager().GetAvatar_AvatarModelInstance().GetElementById(this.BoxID);
		}

		public static string GetDefaultIcon()
		{
			return GameApp.Table.GetManager().GetAvatar_AvatarModelInstance().GetElementById(Singleton<GameConfig>.Instance.AvatarDefaultId)
				.iconId;
		}

		public static string GetDefaultIconBG()
		{
			return GameApp.Table.GetManager().GetAvatar_AvatarModelInstance().GetElementById(Singleton<GameConfig>.Instance.AvatarDefaultFrameId)
				.iconId;
		}

		public int IconID;

		public Avatar_Avatar IconData;

		public int BoxID;

		public Avatar_Avatar BoxData;

		public bool UseDefault = true;
	}
}

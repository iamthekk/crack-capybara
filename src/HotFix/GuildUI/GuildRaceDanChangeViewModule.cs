using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildRaceDanChangeViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.ButtonMask.m_onClick = null;
			this.TapToClose.OnClose = null;
		}

		protected override void OnViewOpen(object data)
		{
			GuildRaceDanChangeViewModule.OpenData openData = data as GuildRaceDanChangeViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
			}
			if (this.mOpenData == null)
			{
				this.mOpenData = new GuildRaceDanChangeViewModule.OpenData();
			}
			this.ButtonMask.m_onClick = null;
			this.TapToClose.OnClose = null;
			this.RefreshUI();
			base.SDK.GetModule<GuildUIDataModule>().RaceDanChange.SaveCurrentSeason();
			this.Anim.Play("open");
		}

		private void RefreshUI()
		{
			string raceDanName = GuildProxy.Language.GetRaceDanName(base.SDK.GuildActivity.GuildRace.RaceDan);
			this.TextDan.text = GuildProxy.Language.GetInfoByID1_LogError(400402, raceDanName);
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.mDelayAllowClose > 0.0)
			{
				this.mDelayAllowClose -= (double)deltaTime;
				if (this.mDelayAllowClose <= 0.0)
				{
					this.ButtonMask.m_onClick = new Action(this.OnClickCloseThis);
					this.TapToClose.OnClose = new Action(this.OnClickCloseThis);
				}
			}
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
			this.ButtonMask.m_onClick = null;
			this.TapToClose.OnClose = null;
		}

		private void OnClickCloseThis()
		{
			GuildProxy.UI.CloseGuildRaceDanChange();
			if (this.mOpenData != null)
			{
				Action onViewClose = this.mOpenData.OnViewClose;
				if (onViewClose == null)
				{
					return;
				}
				onViewClose();
			}
		}

		public CustomButton ButtonMask;

		public TapToCloseCtrl TapToClose;

		public CustomText TextDan;

		public Animator Anim;

		private double mDelayAllowClose = 0.800000011920929;

		private GuildRaceDanChangeViewModule.OpenData mOpenData;

		public class OpenData
		{
			public Action OnViewClose;
		}
	}
}

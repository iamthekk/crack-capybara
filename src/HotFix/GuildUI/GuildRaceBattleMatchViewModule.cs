using System;
using Dxx.Guild;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildRaceBattleMatchViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
		}

		protected override void OnViewOpen(object data)
		{
			GuildRaceBattleMatchViewModule.OpenData openData = data as GuildRaceBattleMatchViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
			}
			if (this.mOpenData == null)
			{
				this.mOpenData = new GuildRaceBattleMatchViewModule.OpenData();
			}
			this.mDelayAutoCloseView = 5.0;
			this.RefreshUI();
			base.SDK.GetModule<GuildUIDataModule>().RaceBattleMatch.SaveMatchDay(this.mOpenData.Day);
			this.AnimListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimationPlayOver));
			this.Anim.Play("open");
		}

		private void RefreshUI()
		{
			if (this.mOpenData == null)
			{
				return;
			}
			if (this.mOpenData.MyGuild != null && this.mOpenData.MyGuild.ShareData != null)
			{
				GuildShareData shareData = this.mOpenData.MyGuild.ShareData;
				this.TextMyGuildName.text = shareData.GuildShowName;
				this.IconMy.SetIcon(shareData.GuildIcon);
			}
			if (this.mOpenData.OtherGuild != null && this.mOpenData.OtherGuild.ShareData != null)
			{
				GuildShareData shareData2 = this.mOpenData.OtherGuild.ShareData;
				this.TextOtherGuildName.text = shareData2.GuildShowName;
				this.IconOther.SetIcon(shareData2.GuildIcon);
			}
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.mDelayAutoCloseView -= (double)unscaledDeltaTime;
			if (this.mDelayAutoCloseView <= 0.0)
			{
				this.mDelayAutoCloseView = 5.0;
				this.OnClickCloseThis();
			}
		}

		protected override void OnViewClose()
		{
			if (this.AnimListen != null)
			{
				this.AnimListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimationPlayOver));
			}
		}

		private void OnAnimationPlayOver(GameObject arg0, string arg1)
		{
			this.OnClickCloseThis();
		}

		private void OnClickCloseThis()
		{
			GuildProxy.UI.CloseGuildRaceBattleMatchViewModule();
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

		[GameTestMethod("界面", "排位赛匹配动画", "", 0)]
		private static void OnTest()
		{
			GuildRaceBattleMatchViewModule.OpenData openData = new GuildRaceBattleMatchViewModule.OpenData();
			openData.MyGuild = new GuildRaceGuild();
			openData.MyGuild.ShareData = new GuildShareData();
			openData.MyGuild.ShareData.GuildShowName = "公会1";
			openData.MyGuild.ShareData.GuildIcon = 2005;
			openData.MyGuild.ShareData.GuildIconBg = 1002;
			openData.OtherGuild = new GuildRaceGuild();
			openData.OtherGuild.ShareData = new GuildShareData();
			openData.OtherGuild.ShareData.GuildShowName = "公会2";
			openData.OtherGuild.ShareData.GuildIcon = 2002;
			openData.OtherGuild.ShareData.GuildIconBg = 1004;
			GameApp.View.OpenView(ViewName.GuildRaceBattleMatchViewModule, openData, 1, null, null);
		}

		[Header("自身公会")]
		public CustomText TextMyGuildName;

		public UIGuildIcon IconMy;

		[Header("对手公会")]
		public CustomText TextOtherGuildName;

		public UIGuildIcon IconOther;

		[Header("其他")]
		public Animator Anim;

		public AnimatorListen AnimListen;

		private GuildRaceBattleMatchViewModule.OpenData mOpenData;

		private const double DELAY_CLOSE_SECOND = 5.0;

		private double mDelayAutoCloseView = 5.0;

		public class OpenData
		{
			public int Day;

			public GuildRaceGuild MyGuild;

			public GuildRaceGuild OtherGuild;

			public Action OnViewClose;
		}
	}
}

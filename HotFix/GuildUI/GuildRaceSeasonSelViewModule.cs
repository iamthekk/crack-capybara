using System;
using Framework.Logic.UI;
using Proto.GuildRace;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildRaceSeasonSelViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.Button_Mask.onClick.AddListener(new UnityAction(this.ClickCloseThis));
			this.Button_Sure.onClick.AddListener(new UnityAction(this.OnApplyRace));
			this.m_closeBt.onClick.AddListener(new UnityAction(this.ClickCloseThis));
			this.Season1.Index = 1;
			this.Season1.Init();
			this.Season1.OnClick = new Action<GuildRaceSeasonSelButton>(this.OnSelectSeason);
			this.Season2.Init();
			this.Season2.OnClick = new Action<GuildRaceSeasonSelButton>(this.OnSelectSeason);
		}

		protected override void OnViewOpen(object data)
		{
			this.OnSelectSeason(this.Season1);
		}

		protected override void OnViewClose()
		{
		}

		protected override void OnViewDelete()
		{
			CustomButton button_Mask = this.Button_Mask;
			if (button_Mask != null)
			{
				button_Mask.onClick.RemoveListener(new UnityAction(this.ClickCloseThis));
			}
			CustomButton button_Sure = this.Button_Sure;
			if (button_Sure != null)
			{
				button_Sure.onClick.RemoveListener(new UnityAction(this.OnApplyRace));
			}
			this.m_closeBt.onClick.RemoveListener(new UnityAction(this.ClickCloseThis));
			this.Season1.OnClick = null;
			this.Season2.OnClick = null;
			GuildRaceSeasonSelButton season = this.Season1;
			if (season != null)
			{
				season.DeInit();
			}
			GuildRaceSeasonSelButton season2 = this.Season2;
			if (season2 == null)
			{
				return;
			}
			season2.DeInit();
		}

		private void OnSelectSeason(GuildRaceSeasonSelButton button)
		{
			this.Season1.SetSelect(button == this.Season1);
			this.Season2.SetSelect(button == this.Season2);
		}

		private void OnApplyRace()
		{
			int num = 0;
			if (this.Season1.Selected)
			{
				num = this.Season1.Index;
			}
			if (this.Season2.Selected)
			{
				num = this.Season2.Index;
			}
			GuildNetUtil.Guild.DoRequest_GuildRaceGuildApplyRequest(num, delegate(bool result, GuildRaceGuildApplyResponse resp)
			{
				if (result)
				{
					GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_GuildRace_GuildApply);
					if (base.CheckIsViewOpen())
					{
						this.ClickCloseThis();
						return;
					}
				}
			});
		}

		private void ClickCloseThis()
		{
			GuildProxy.UI.CloseGuildRaceSeasonSelect();
		}

		public CustomButton Button_Mask;

		public CustomButton Button_Sure;

		public CustomButton m_closeBt;

		public GuildRaceSeasonSelButton Season1;

		public GuildRaceSeasonSelButton Season2;
	}
}

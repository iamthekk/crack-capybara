using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using HotFix.GuildUI;
using UnityEngine;

namespace HotFix
{
	public abstract class UIBaseRankItemCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.ctrl_Avatar.Init();
			this.ctrl_Avatar.OnClick = new Action<UIAvatarCtrl>(this.OnClickAvatar);
			this.Button_Guild.m_onClick = new Action(this.OnClickGuild);
			this.m_IsShow = base.gameObject.activeSelf;
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.Init();
				this.text_GuildName.gameObject.SetActive(false);
				this.Text_GuildRankName.gameObject.SetActive(false);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.Hide();
			this.ctrl_Avatar.OnClick = null;
			this.ctrl_Avatar.DeInit();
			this.Button_Guild.m_onClick = null;
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.DeInit();
			}
		}

		public void Hide()
		{
			if (!this.m_IsShow)
			{
				return;
			}
			this.m_IsShow = false;
			base.gameObject.SetActiveSafe(false);
			this.OnHide();
		}

		protected virtual void OnHide()
		{
		}

		public void Show(bool force = false)
		{
			if (force)
			{
				this.Hide();
			}
			if (this.m_IsShow)
			{
				return;
			}
			this.m_IsShow = true;
			base.gameObject.SetActiveSafe(true);
			this.OnShow();
		}

		protected virtual void OnShow()
		{
		}

		private void OnClickGuild()
		{
			if (this.m_RankType != RankType.GuildBossRank)
			{
				return;
			}
			if (this.m_RankData == null)
			{
				return;
			}
			GuildProxy.UI.OpenUIGuildCheckPop("", this.m_RankData.GuildId);
		}

		private void OnClickAvatar(UIAvatarCtrl obj)
		{
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.m_RankData.UserId);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		public void SetFresh(RankType rankType, BaseRankData rankData, int index)
		{
			this.m_RankType = rankType;
			this.m_RankData = rankData;
			this.m_Index = index;
			if (this.m_RankData == null)
			{
				this.Hide();
				return;
			}
			this.Show(false);
			this.FreshUI();
		}

		protected virtual void FreshUI()
		{
			this.FreshBase();
			this.FreshCustom();
		}

		protected virtual void FreshBase()
		{
			if (this.m_RankType == RankType.GuildBossRank)
			{
				this.text_UserName.gameObject.SetActiveSafe(false);
				this.text_GuildName.gameObject.SetActiveSafe(false);
				this.Text_GuildRankName.gameObject.SetActiveSafe(true);
				if (this.ctrl_guildIcon != null)
				{
					this.ctrl_guildIcon.gameObject.SetActiveSafe(true);
					this.ctrl_guildIcon.SetIcon(this.m_RankData.GuildIcon);
				}
				if (this.ctrl_Avatar != null)
				{
					this.ctrl_Avatar.gameObject.SetActiveSafe(false);
				}
				DxxTools.UI.SetTextWithEllipsis(this.Text_GuildRankName, this.m_RankData.GuildName);
			}
			else
			{
				this.Text_GuildRankName.gameObject.SetActiveSafe(false);
				this.text_UserName.gameObject.SetActiveSafe(true);
				if (this.ctrl_guildIcon != null)
				{
					this.ctrl_guildIcon.gameObject.SetActiveSafe(false);
				}
				if (this.ctrl_Avatar != null)
				{
					this.ctrl_Avatar.gameObject.SetActiveSafe(true);
					this.ctrl_Avatar.RefreshData(this.m_RankData.AvatarId, this.m_RankData.AvatarFrameId);
				}
				string text = (string.IsNullOrEmpty(this.m_RankData.NickName) ? DxxTools.GetDefaultNick(this.m_RankData.UserId) : this.m_RankData.NickName);
				DxxTools.UI.SetTextWithEllipsis(this.text_UserName, text);
				if (this.TitleCtrl != null)
				{
					this.TitleCtrl.SetAndFresh(this.m_RankData.TitleId);
					this.text_GuildName.gameObject.SetActiveSafe(false);
					this.text_GuildName.gameObject.SetActiveSafe(false);
				}
				else if (string.IsNullOrEmpty(this.m_RankData.GuildName))
				{
					this.text_GuildName.gameObject.SetActiveSafe(false);
				}
				else
				{
					this.text_GuildName.gameObject.SetActiveSafe(true);
					DxxTools.UI.SetTextWithEllipsis(this.text_GuildName, this.m_RankData.GuildName);
				}
			}
			if (this.image_Rank != null)
			{
				if (this.m_RankData.Rank < 1)
				{
					this.text_Rank.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_norank");
				}
				else if (this.m_RankData.Rank > 3)
				{
					this.text_Rank.text = this.m_RankData.Rank.ToString();
				}
				else if (this.showThreeRank)
				{
					this.text_Rank.text = this.m_RankData.Rank.ToString();
				}
				else
				{
					this.text_Rank.text = "";
				}
				if (this.m_RankData.Rank <= 0 || this.m_RankData.Rank >= 4)
				{
					this.image_Rank.enabled = false;
					return;
				}
				this.image_Rank.enabled = true;
				if (this.changeRankImage)
				{
					this.image_Rank.SetImage(GameApp.Table.GetAtlasPath(128), "rank_" + this.m_RankData.Rank.ToString());
					return;
				}
			}
			else
			{
				if (this.m_RankData.Rank < 1)
				{
					this.text_Rank.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_norank");
					return;
				}
				this.text_Rank.text = this.m_RankData.Rank.ToString();
			}
		}

		protected virtual void FreshCustom()
		{
			this.obj_WorldBoss.SetActiveSafe(false);
			this.obj_RogueDungeon.SetActiveSafe(false);
			this.obj_Power.SetActiveSafe(false);
			if (this.m_RankType == RankType.WorldBoss)
			{
				this.obj_WorldBoss.SetActiveSafe(true);
				this.text_Score_WorldBoss.text = DxxTools.FormatNumber(this.m_RankData.RankScore);
				return;
			}
			if (this.m_RankType == RankType.RogueDungeon)
			{
				this.obj_RogueDungeon.SetActiveSafe(true);
				this.text_Score_RogueDungeon.text = this.m_RankData.RankScore.ToString();
				return;
			}
			if (this.m_RankType == RankType.GuildBossRank || this.m_RankType == RankType.GuildBossSelfRank)
			{
				this.obj_WorldBoss.SetActiveSafe(true);
				this.text_Score_WorldBoss.text = DxxTools.FormatNumber(this.m_RankData.GuildDamage);
				return;
			}
			if (this.m_RankType == RankType.NewWorld)
			{
				this.obj_Power.SetActiveSafe(true);
				this.text_Score_Power.text = DxxTools.FormatNumber((long)this.m_RankData.Power);
			}
		}

		public void PlayAni(float endPosX, float duration, float delayTime)
		{
			if (this.m_tweener != null)
			{
				TweenExtensions.Kill(this.m_tweener, true);
			}
			base.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(endPosX - 2000f, base.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y);
			this.m_tweener = TweenSettingsExtensions.SetDelay<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions46.DOAnchorPosX(base.transform.GetChild(0).GetComponent<RectTransform>(), endPosX, duration, false), 1), delayTime);
		}

		[Header("排名设置")]
		public RankType m_RankType;

		public bool showThreeRank;

		public bool changeRankImage = true;

		[Header("排行基础控件")]
		public RectTransform node_InitAni;

		public UIAvatarCtrl ctrl_Avatar;

		public UITitleCtrl TitleCtrl;

		public CustomText text_UserName;

		public CustomText text_GuildName;

		public CustomText Text_GuildRankName;

		public CustomImage image_Rank;

		public CustomText text_Rank;

		public UIGuildIcon ctrl_guildIcon;

		public CustomButton Button_Guild;

		[Header("WorldBoss")]
		public GameObject obj_WorldBoss;

		public CustomText text_Score_WorldBoss;

		[Header("RogueDungeon")]
		public GameObject obj_RogueDungeon;

		public CustomText text_Score_RogueDungeon;

		[Header("Power")]
		public GameObject obj_Power;

		public CustomText text_Score_Power;

		private bool m_IsShow;

		private BaseRankData m_RankData;

		private int m_Index;

		private Tweener m_tweener;
	}
}

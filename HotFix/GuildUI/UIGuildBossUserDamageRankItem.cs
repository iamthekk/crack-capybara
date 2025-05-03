using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildBossUserDamageRankItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.ButtonItem.onClick.AddListener(new UnityAction(this.OnClickItem));
		}

		protected override void GuildUI_OnUnInit()
		{
			this.ButtonItem.onClick.RemoveListener(new UnityAction(this.OnClickItem));
		}

		public void SetData(GuildBossRankData data)
		{
			this.mData = data;
		}

		public void RefreshUI()
		{
			if (this.mData == null || this.mData.UserData == null || this.mData.Rank <= 0)
			{
				this.RefreshUIAsEmpty();
				return;
			}
			this.TextRank.text = this.mData.Rank.ToString();
			this.TextNick.text = this.mData.UserData.GetNick();
			this.TextDamage.text = DxxTools.FormatNumber(this.mData.Damage);
			this.TextPower.text = DxxTools.FormatNumber((long)this.mData.UserData.Power);
			this.UserIcon.Refresh(this.mData.UserData.Avatar, this.mData.UserData.AvatarFrame);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.DemageRectTransform);
		}

		public void RefreshUIAsEmpty()
		{
			if (this.mData == null || this.mData.UserData == null)
			{
				HLog.LogError("UIGuildBossUserDamageRankItem mData is null!");
				return;
			}
			GuildUserShareData userData = this.mData.UserData;
			this.TextRank.text = "-";
			this.TextNick.text = userData.GetNick();
			this.TextDamage.text = DxxTools.FormatNumber(0L);
			this.TextPower.text = DxxTools.FormatNumber((long)userData.Power);
			this.UserIcon.Refresh(userData.Avatar, userData.AvatarFrame);
		}

		private void OnClickItem()
		{
			Action<UIGuildBossUserDamageRankItem> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public CustomButton ButtonItem;

		public CustomText TextRank;

		public CustomText TextNick;

		public CustomText TextDamage;

		public CustomText TextPower;

		public UIGuildHead UserIcon;

		public RectTransform DemageRectTransform;

		public GuildBossRankData mData;

		public Action<UIGuildBossUserDamageRankItem> OnClick;
	}
}

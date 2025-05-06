using System;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageBattleItem_Doing_VS : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void SetTime(float lefttime, float totaltime)
		{
			if (totaltime < 1f)
			{
				totaltime = 1f;
			}
			if (lefttime > totaltime)
			{
				lefttime = totaltime;
			}
			float num = Mathf.Clamp01(lefttime / totaltime);
			if (this.Image_Progress != null)
			{
				this.Image_Progress.fillAmount = num;
			}
			TimeSpan timeSpan = new TimeSpan(0, 0, (int)lefttime);
			this.Text_Time.text = string.Concat(new string[]
			{
				timeSpan.Hours.ToString("D2"),
				":",
				timeSpan.Minutes.ToString("D2"),
				":",
				timeSpan.Seconds.ToString("D2")
			});
		}

		public void SetStateBattle()
		{
			this.Text_Tips.ChangeLanguageID("400423");
			Animator animBattle = this.AnimBattle;
			if (animBattle == null)
			{
				return;
			}
			animBattle.Play("battle");
		}

		public void SetStateWait()
		{
			this.Text_Tips.ChangeLanguageID("400432");
			Animator animBattle = this.AnimBattle;
			if (animBattle == null)
			{
				return;
			}
			animBattle.Play("idle");
		}

		public CustomLanguageText Text_Tips;

		public CustomText Text_Time;

		public Animator AnimBattle;

		public Image Image_Progress;
	}
}

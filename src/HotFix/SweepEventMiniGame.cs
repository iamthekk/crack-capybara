using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace HotFix
{
	public class SweepEventMiniGame : ChapterEventMiniGame
	{
		public override void OnInit()
		{
			GameEventDataMiniGame gameEventDataMiniGame = this.currentData as GameEventDataMiniGame;
			if (gameEventDataMiniGame != null)
			{
				if (gameEventDataMiniGame.miniGameType == MiniGameType.Turntable || gameEventDataMiniGame.miniGameType == MiniGameType.PaySlot)
				{
					this.isShowResult = false;
					this.ShowUI();
					this.DelayShowNext();
					return;
				}
				this.isShowResult = true;
				base.OpenMiniGameUI(gameEventDataMiniGame);
			}
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			base.OnEvent(pushType, param);
			if (this.isShowUI)
			{
				if (this.isAsyncScore)
				{
					this.HangUp();
				}
				else
				{
					this.DelayShowNext();
				}
			}
		}

		protected override void ShowUI()
		{
			if (this.isShowResult)
			{
				base.ShowUI();
			}
			this.isShowUI = true;
		}

		private void DelayShowNext()
		{
			if (this.isHangUp)
			{
				return;
			}
			Sequence sequence = DOTween.Sequence();
			float num = 1.3f / Time.timeScale;
			if (num < 0.1f)
			{
				num = 0.1f;
			}
			TweenSettingsExtensions.AppendInterval(sequence, num);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.OnClickButton(0);
			});
		}

		public override void ResumeHangUp()
		{
			base.ResumeHangUp();
			this.OnClickButton(0);
		}

		private bool isShowUI;

		private bool isShowResult;
	}
}

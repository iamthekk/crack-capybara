using System;
using DG.Tweening;
using UnityEngine;

namespace HotFix
{
	public class UISelfRankItemCtrl : UIBaseRankItemCtrl
	{
		protected override void OnInit()
		{
			base.OnInit();
			if (this.firstInit)
			{
				this.firstInit = false;
				this._initY = base.transform.localPosition.y;
			}
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
			this.KillTween();
		}

		protected override void OnHide()
		{
			base.OnHide();
			Vector3 localPosition = base.transform.localPosition;
			localPosition.y = this._initY - this.tweenMoveY;
			base.transform.localPosition = localPosition;
		}

		protected override void OnShow()
		{
			base.OnShow();
			this.KillTween();
			this._tweener = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(base.transform, this._initY, this.tweenTime, false), 27);
		}

		public void SetFreshMy(RankType rankType)
		{
			BaseRankData baseRankData = new BaseRankData();
			baseRankData.SetMyRank(rankType);
			base.SetFresh(rankType, baseRankData, 0);
		}

		private void KillTween()
		{
			if (this._tweener != null)
			{
				TweenExtensions.Kill(this._tweener, false);
				this._tweener = null;
			}
		}

		private float _initY;

		private Tweener _tweener;

		public float tweenMoveY = 100f;

		public float tweenTime = 0.15f;

		private bool firstInit = true;
	}
}

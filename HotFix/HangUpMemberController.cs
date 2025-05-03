using System;
using DG.Tweening;
using UnityEngine;

namespace HotFix
{
	public class HangUpMemberController : UnBattleMemberController
	{
		public void SetReadyPosition()
		{
			this.playerMove.localPosition = new Vector3(-10f, this.playerMove.localPosition.y, this.playerMove.localPosition.z);
		}

		public void FirstEnter(Action onEnterFinish)
		{
			Sequence sequence = this.sequencePool.Get();
			base.ChangeFastMoveAni();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalMoveX(this.playerMove, 0f, 1f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				Action onEnterFinish2 = onEnterFinish;
				if (onEnterFinish2 == null)
				{
					return;
				}
				onEnterFinish2();
			});
		}

		private SequencePool sequencePool = new SequencePool();
	}
}
